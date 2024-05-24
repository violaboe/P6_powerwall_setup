using UnityEngine;

public class BouncingSpheres : MonoBehaviour
{
    public float speed = 2f; // Base speed of the bubble
    public float changeDirectionInterval = 2f; // Time interval to change direction
    public float roomSize = 20f; // Size of the room

    private Rigidbody rb;
    private Vector3 currentDirection;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        RandomizeDirection();
        timer = changeDirectionInterval;
    }

    void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;

        if (timer <= 0)
        {
            RandomizeDirection();
            timer = changeDirectionInterval;
        }

        // Ensure the speed is constant
        rb.velocity = currentDirection * speed;

        // Check if the bubble is out of bounds and reflect its direction
        Vector3 newPosition = transform.position;
        if (Mathf.Abs(newPosition.x) > roomSize / 2 || Mathf.Abs(newPosition.y) > roomSize / 2 || Mathf.Abs(newPosition.z) > roomSize / 2)
        {
            if (Mathf.Abs(newPosition.x) > roomSize / 2)
            {
                newPosition.x = Mathf.Sign(newPosition.x) * (roomSize / 2);
                currentDirection.x = -currentDirection.x;
            }
            if (Mathf.Abs(newPosition.y) > roomSize / 2)
            {
                newPosition.y = Mathf.Sign(newPosition.y) * (roomSize / 2);
                currentDirection.y = -currentDirection.y;
            }
            //if (Mathf.Abs(newPosition.z) > roomSize / 2)
            //{
            //    newPosition.z = Mathf.Sign(newPosition.z) * (roomSize / 2);
            //    currentDirection.z = -currentDirection.z;
            //}
            transform.position = newPosition;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Vector3 reflectDir = Vector3.Reflect(currentDirection, collision.contacts[0].normal);
        currentDirection = reflectDir.normalized;
    }

    void RandomizeDirection()
    {
        currentDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f,1f)).normalized;
    }
}
