using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5f; // Adjust this to change the speed of movement
    public float rotationSpeed = 100f; // Adjust this to change the speed of rotation

    void Update()
    {
        // Get the user input for movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction based on the input
        Vector3 movementDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Calculate the movement amount based on the speed and time
        Vector3 movementAmount = movementDirection * movementSpeed * Time.deltaTime;

        // Move the GameObject
        transform.Translate(movementAmount, Space.World);

        // Rotate the GameObject based on input for rotation
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}