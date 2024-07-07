using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CubegodrayScaler : MonoBehaviour
{
    public float targetYScale = 2.0f;
    public float targetXScale = 2.0f;
    public float growthSpeedY = 0.1f;
    public float growthSpeedX = 0.1f;
    public float circularMovementSpeed = 1.0f;
    public float circleRadius = 5.0f;

    private Vector3 initialScale;
    private Vector3 initialPosition;
    private float angle;

    void Start()
    {
        // Store the initial scale of the cube
        initialScale = transform.localScale;
        // Store the initial position of the cube
        initialPosition = transform.position;
        // Initialize the angle for circular movement
        angle = 0.0f;
    }

    void Update()
    {
        // Scale the cube on the y-axis
        if (transform.localScale.y < targetYScale)
        {
            float newYScale = transform.localScale.y + growthSpeedY * Time.deltaTime;
            if (newYScale > targetYScale)
            {
                newYScale = targetYScale;
            }
            transform.localScale = new Vector3(transform.localScale.x, newYScale, transform.localScale.z);
        }

        // Scale the cube on the x-axis
        if (transform.localScale.x < targetXScale)
        {
            float newXScale = transform.localScale.x + growthSpeedX * Time.deltaTime;
            if (newXScale > targetXScale)
            {
                newXScale = targetXScale;
            }
            transform.localScale = new Vector3(newXScale, transform.localScale.y, transform.localScale.z);
        }

        // Move the cube in a circular path along the x and z axis
        angle += circularMovementSpeed * Time.deltaTime;
        float x = initialPosition.x + Mathf.Cos(angle) * circleRadius;
        float z = initialPosition.z + Mathf.Sin(angle) * circleRadius;
        transform.position = new Vector3(x, transform.position.y, z);
    }
}
