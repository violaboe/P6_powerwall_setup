using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dancing : MonoBehaviour
{
    public GameObject reality;
    public GameObject office;
    public Material fakematerial;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            reality.gameObject.SetActive(false);
            foreach(MeshRenderer mr in office.GetComponentsInChildren<MeshRenderer>())
            {
                Material[] mats = mr.materials;
                for(int i = 0; i< mr.materials.Length; i++)
                {
                    mats[i] = fakematerial;
                }
                mr.materials = mats;
                //mats = new List<Material>();

                //mr.SetMaterials(fakematerial);
                ////for(int i = 0; i < mr.materials.Length; i++)
                ////{
                ////    mr.materials[i]. = fakematerial;
                ////    mr.SetMaterials()
                ////}               
                //Material[] mats = renderer.materials;
                //mats[0] = someMaterial;
                //mats[1] = someOtherMaterial;
                //mats[2] = yetAnotherMaterial;
                //renderer.materials = mats;

            }
        }

    }
}
