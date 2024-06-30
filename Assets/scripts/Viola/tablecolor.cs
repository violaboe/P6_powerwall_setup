using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tablecolor : MonoBehaviour
{
    public Material color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hit");
        if (other.tag == "Ball") 
        {
            Debug.Log("hit ball");
            this.gameObject.GetComponent<MeshRenderer>().material = other.transform.gameObject.GetComponent<MeshRenderer>().material;
        }
    }
}
