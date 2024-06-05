using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class FlowerPower : MonoBehaviour
{
    //Spreading the flower
    public roomManager roomMngr;
    private List<GameObject> walls;
    [SerializeField] private GameObject objectToScatter;
    [SerializeField] private int numberOfObjectsToScatter = 10;
    [SerializeField] private float derivation = 0.4f; 



    void Start()
    {
        ScatterObjects();

        walls = roomMngr.GetAllWalls();
    }

    private void ScatterObjects()
    {
        foreach (GameObject plane in walls)
        {
            Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
            Vector3 planeSize = planeMesh.bounds.size;
            Vector3 planePosition = plane.transform.position;
            Quaternion planeRotation = plane.transform.rotation;

            for (int i = 0; i < numberOfObjectsToScatter; i++)
            {
                Vector3 randomPoint = GetRandomPointOnRotatedPlane(planeSize, planePosition, planeRotation);
                GameObject go = Instantiate(objectToScatter, randomPoint, planeRotation, this.transform); 
                go.transform.localScale = transform.localScale * Random.Range(1 - derivation, 1 + derivation); 
            }
        }
    }

    private Vector3 GetRandomPointOnRotatedPlane(Vector3 planeSize, Vector3 planePosition, Quaternion planeRotation)
    {
        // Create a random point in the local space of the plane
        Vector3 randomPointLocal = new Vector3(
            Random.Range(-planeSize.x / 2, planeSize.x / 2),
            0,
            Random.Range(-planeSize.z / 2, planeSize.z / 2)
        );

        // Rotate this point 90 degrees around the plane's local x-axis to make it perpendicular
        Quaternion rotation90 = Quaternion.Euler(0, 0, 0);
        Vector3 randomPointRotatedLocal = rotation90 * randomPointLocal;

        // Convert this local point to world space
        Vector3 randomPointWorld = planePosition + planeRotation * randomPointRotatedLocal;

        return randomPointWorld;
    }
}
