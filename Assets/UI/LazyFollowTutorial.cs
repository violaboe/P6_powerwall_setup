using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazyFollowTutorial : MonoBehaviour
{
    public Transform target; // The object to follow
    public float followSpeed = 2.0f; // The speed at which the object follows
    public float rotationSpeed = 2.0f; // The speed at which the object rotates to face the target
    public Vector3 offset = new Vector3(0, 0, 2); // Offset in front of the target

    void Update()
    {
        if (target != null)
        {
            // Calculate the target position with offset
            Vector3 targetPosition = target.position + target.forward * offset.z + target.up * offset.y + target.right * offset.x;

            // Smoothly interpolate the position of this object towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Smoothly interpolate the rotation of this object to look in the same direction as the target
            Quaternion targetRotation = Quaternion.LookRotation(target.forward, target.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}


//public Transform target; // The object to follow
//public float followSpeed = 2.0f; // The speed at which the object follows
//public float rotationSpeed = 2.0f; // The speed at which the object rotates to face the target
//public Vector3 offset; // Offset from the target object

//void Update()
//{
//    if (target != null)
//    {
//        // Calculate the target position with offset
//        Vector3 targetPosition = target.position + offset;

//        // Smoothly interpolate the position of this object towards the target position
//        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

//        // Calculate the direction to the target
//        Vector3 directionToTarget = (target.position - transform.position).normalized;

//        // Calculate the target rotation to face the target
//        Quaternion targetRotation = Quaternion.LookRotation(-directionToTarget);

//        // Smoothly interpolate the rotation of this object to look at the target
//        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
//    }
//}