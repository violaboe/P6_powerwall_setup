using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using static UnityEngine.GraphicsBuffer;

public class FireflyMovement : MonoBehaviour
{
    private List<Transform> anchors;
    public AudioClip musicClip; // The music clip to play
    public float totalTimeToCompletePath = 10.0f; // Total time to complete the path in seconds

    private int currentAnchorIndex = 0;
    private bool isMoving = false;
    private bool isMusicPlaying = false;
    private Vector3 currentDirection;
    private AudioSource audioSource;

    private float totalDistance;
    private float elapsedTime = 0.0f;
    private float speed;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing. Please add an AudioSource component to the GameObject.");
        }

        if (musicClip == null)
        {
            Debug.LogError("Music clip missing. Please assign a music clip in the Inspector.");
        }
    }

    public void BeginPath(List<Transform> newAnchors)
    {
        if (newAnchors == null || newAnchors.Count == 0)
        {
            Debug.LogWarning("No anchors provided");
            return;
        }

        anchors = newAnchors;
        currentAnchorIndex = 0;
        isMoving = false;
        elapsedTime = 0.0f;

        // Initialize the current direction
        if (anchors.Count > 1)
        {
            currentDirection = (anchors[1].position - anchors[0].position).normalized;
        }

        // Calculate total path distance
        totalDistance = 0f;
        for (int i = 0; i < anchors.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(anchors[i].position, anchors[i + 1].position);
        }
        speed = totalDistance / totalTimeToCompletePath;
    }

    void Update()
    {
        // Check for the B button press on the Quest 3 controller or the space key on the keyboard
        if (IsBButtonPressed() || IsSpaceKeyPressed())
        {
            if (!isMoving)
            {
                isMoving = true;
                PlayMusic();
            }
        }

        if (!isMoving || anchors == null || anchors.Count == 0) return;

        elapsedTime += Time.deltaTime;

        if (currentAnchorIndex < anchors.Count - 1)
        {
            Transform currentAnchor;
            Transform nextAnchor;

            
                currentAnchor = anchors[currentAnchorIndex];
                nextAnchor = anchors[currentAnchorIndex + 1];
            

            float remainingDistance = Vector3.Distance(transform.position, nextAnchor.position);
            float remainingTime = totalTimeToCompletePath - elapsedTime;
            //float currentSpeed = remainingDistance / remainingTime;

            Vector3 direction = (nextAnchor.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
            transform.LookAt(transform.position - (nextAnchor.position - transform.position));

            // Check if we have reached the target anchor
            if (Vector3.Distance(transform.position, nextAnchor.position) < 0.1f)
            {
                currentAnchorIndex++;
                // Update the direction for the next anchor
                if (currentAnchorIndex < anchors.Count - 1)
                {
                    currentDirection = (anchors[currentAnchorIndex + 1].position - nextAnchor.position).normalized;
                }
                else
                {
                    isMoving = false; // Stop moving if we have reached the last anchor
                    this.gameObject.SetActive(false);
                }
            }
        }
        

        // Ensure the firefly completes its journey exactly within the specified time
        if (elapsedTime >= totalTimeToCompletePath)
        {
            transform.position = anchors[anchors.Count - 1].position;
            isMoving = false;
        }
    }

    private bool IsBButtonPressed()
    {
        // Check if the B button is pressed on the right hand controller
        bool bButtonPressed = false;
        InputDevice rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightHandDevice.isValid)
        {
            rightHandDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bButtonPressed);
        }

        return bButtonPressed;
    }

    private bool IsSpaceKeyPressed()
    {
        // Check if the space key is pressed on the keyboard
        return Input.GetKeyDown(KeyCode.Space);
    }

    private void PlayMusic()
    {
        if (audioSource != null && musicClip != null && !isMusicPlaying)
        {
            audioSource.clip = musicClip;
            audioSource.Play();
            isMusicPlaying = true;
        }
    }
}
