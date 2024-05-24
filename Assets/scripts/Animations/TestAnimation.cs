using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeAnimation : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody rb;
 

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Anim1();
        }        
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Anim2();
        }        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Anim3();
        }
    }

    void Anim1()
    {
        Vector3 newPos = new Vector3();
        newPos.x = this.transform.position.x;
        newPos.y = this.transform.position.y;
        newPos.z = this.transform.position.z + Random.Range(0.1f,1.5f);

        Vector3 newScale = new Vector3();
        newScale.y = this.transform.localScale.y;
        newScale.x = this.transform.localScale.y;
        newScale.z = this.transform.localScale.y;
        
        
        this.Animate(this.transform, Easing.AnimationType.Position, Easing.Ease.EaseInOutQuad, transform.position, newPos, 2f);
        this.Animate(this.transform, Easing.AnimationType.Scale, Easing.Ease.EaseInOutQuad, this.transform.localScale, newScale, 2f);
    }    

    void Anim2()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
  

    }
    void Anim3()
    {
        this.GetComponent<Light>().enabled = true;
    }

}
