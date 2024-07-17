using UnityEngine;

public class RotateCameraAroundCircle : MonoBehaviour
{
    public Transform target; // The target object to rotate around
    public float radius = 10.0f; // Radius of the circle
    public float speed = 2.0f; // Speed of rotation
    public float height = 5.0f; // Height of the camera above the target

    private float angle = 0.0f;

    void Update()
    {
        // Calculate the new position of the camera
        float x = target.position.x + Mathf.Cos(angle) * radius;
        float z = target.position.z + Mathf.Sin(angle) * radius;
        float y = target.position.y + height; // Use the adjustable height

        // Set the camera's position
        transform.position = new Vector3(x, y, z);

        // Make the camera look at the target
        transform.LookAt(target);

        // Increment the angle to create continuous rotation
        angle += speed * Time.deltaTime;

        // Ensure the angle stays within 0 to 2*PI
        if (angle >= 2 * Mathf.PI)
        {
            angle -= 2 * Mathf.PI;
        }
    }
}