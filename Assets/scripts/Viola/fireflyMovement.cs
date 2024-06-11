using System.Collections.Generic;
using UnityEngine;

public class fireflyMovement : MonoBehaviour
{
    private List<Transform> anchors;
    public float speed = 2.0f;
    public float perturbationStrength = 0.1f;
    public float curveStrength = 0.5f; // Strength of the curve effect

    private int currentAnchorIndex = 0;
    private bool isMoving = false;
    private Vector3 currentDirection;

    public void BeginPath(List<Transform> newAnchors)
    {
        if (newAnchors == null || newAnchors.Count == 0)
        {
            Debug.LogWarning("No anchors provided");
            return;
        }

        anchors = newAnchors;
        currentAnchorIndex = 0;
        isMoving = true;

        // Initialize the current direction
        if (anchors.Count > 1)
        {
            currentDirection = (anchors[1].position - anchors[0].position).normalized;
        }
    }

    void Update()
    {
        if (!isMoving || anchors == null || anchors.Count == 0) return;

        if (currentAnchorIndex < anchors.Count)
        {
            Transform targetAnchor = anchors[currentAnchorIndex];
            Vector3 direction = (targetAnchor.position - transform.position).normalized;

            // Calculate a curved direction by blending the current direction with the new direction
            currentDirection = Vector3.Lerp(currentDirection, direction, curveStrength * Time.deltaTime);

            // Add perturbation for fly-like movement
            Vector3 perturbation = new Vector3(
                Random.Range(-perturbationStrength, perturbationStrength),
                Random.Range(-perturbationStrength, perturbationStrength),
                Random.Range(-perturbationStrength, perturbationStrength)
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
}
