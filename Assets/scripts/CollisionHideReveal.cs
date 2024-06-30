using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHideReveal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
