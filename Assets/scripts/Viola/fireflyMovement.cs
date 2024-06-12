using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class fireflyMovement : MonoBehaviour
{
    private List<Transform> anchors;
    public float speed = 2.0f;
    public float perturbationStrength = 0.1f;
    public float curveStrength = 0.5f; // Strength of the curve effect
    public AudioClip musicClip; // The music clip to play

    private int currentAnchorIndex = 0;
    private bool isMoving = false;
    private bool isMusicPlaying = false;
    private Vector3 currentDirection;
    private AudioSource audioSource;

    private float noiseOffsetX;
    private float noiseOffsetY;
    private float noiseOffsetZ;

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

        // Initialize noise offsets for smoother perturbation
        noiseOffsetX = Random.Range(0f, 100f);
        noiseOffsetY = Random.Range(0f, 100f);
        noiseOffsetZ = Random.Range(0f, 100f);
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

        // Initialize the current direction
        if (anchors.Count > 1)
        {
            currentDirection = (anchors[1].position - anchors[0].position).normalized;
        }
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

        if (currentAnchorIndex < anchors.Count)
        {
            Transform targetAnchor = anchors[currentAnchorIndex];
            Vector3 direction = (targetAnchor.position - transform.position).normalized;

            // Calculate a curved direction by blending the current direction with the new direction
            currentDirection = Vector3.Lerp(currentDirection, direction, curveStrength * Time.deltaTime);

            // Add smoother perturbation for fly-like movement using Perlin noise
            Vector3 perturbation = new Vector3(
                (Mathf.PerlinNoise(Time.time * speed + noiseOffsetX, 0f) - 0.5f) * perturbationStrength,
                (Mathf.PerlinNoise(Time.time * speed + noiseOffsetY, 1f) - 0.5f) * perturbationStrength,
                (Mathf.PerlinNoise(Time.time * speed + noiseOffsetZ, 2f) - 0.5f) * perturbationStrength
            );

            Vector3 moveDirection = currentDirection + perturbation;
            transform.position += moveDirection * speed * Time.deltaTime;

            // Check if we have reached the target anchor
            if (Vector3.Distance(transform.position, targetAnchor.position) < 0.1f)
            {
                currentAnchorIndex++;

                // Update the direction for the next anchor
                if (currentAnchorIndex < anchors.Count)
                {
                    int nextAnchorIndex = (currentAnchorIndex + 1) % anchors.Count;
                    currentDirection = (anchors[nextAnchorIndex].position - targetAnchor.position).normalized;
                }
                else
                {
                    isMoving = false; // Stop moving if we have reached the last anchor
                }
            }
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
