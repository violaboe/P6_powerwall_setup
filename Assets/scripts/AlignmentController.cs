using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentController : MonoBehaviour
{
    public Vector3 VectorToScreen;

    private List<Vector3> screenCorners = new List<Vector3>();
    private List<GameObject> cornerPrefabs = new List<GameObject>(); // Track instantiated corner prefabs
    [SerializeField] private GameObject CornerPrefab;
    [SerializeField] private GameObject CornerPreviewPrefab;
    [SerializeField] private GameObject ControllerRight;
    [SerializeField] private GameObject HeadSet;
    [SerializeField] private Material planeMaterial;
    private GameObject adjustmentPlane;
    private bool isAdjustmentPlaneCreated = false;
    

    private void Start()
    {
        CornerPreviewPrefab =  Instantiate(CornerPreviewPrefab);
    }

    void Update()
    {
        //Move Preview with Controller && Lock Axis
        if (!isAdjustmentPlaneCreated)
        {
            // Let it be where the controller is
            if (screenCorners.Count == 0)
            {
                CornerPreviewPrefab.transform.position = ControllerRight.transform.position;
                CornerPreviewPrefab.transform.rotation = ControllerRight.transform.rotation;
            }
            // Lock the height to the first point
            else if (screenCorners.Count == 1)
            {
                // Lock height to the y-coordinate of the first corner point
                CornerPreviewPrefab.transform.position = new Vector3(ControllerRight.transform.position.x, screenCorners[0].y, ControllerRight.transform.position.z);
                CornerPreviewPrefab.transform.rotation = ControllerRight.transform.rotation;
            }
            // Lock the height to the second point
            else if (screenCorners.Count == 2)
            {
                // Lock height to the y-coordinate of the second corner point
                CornerPreviewPrefab.transform.position = new Vector3(screenCorners[1].x, ControllerRight.transform.position.y, screenCorners[1].z);
                CornerPreviewPrefab.transform.rotation = ControllerRight.transform.rotation;
            }
        }

        //Corner Creation
        if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger) && screenCorners.Count < 3 && !isAdjustmentPlaneCreated)
        {
            // Instantiate the CornerPrefab at the controller's position and rotation
            GameObject newCorner = Instantiate(CornerPrefab, CornerPreviewPrefab.transform.position, CornerPreviewPrefab.transform.rotation);
            // Add the position of the new corner to the list
            screenCorners.Add(newCorner.transform.position);
            // Add the instantiated corner to the list
            cornerPrefabs.Add(newCorner);
        }
        else if (screenCorners.Count >= 3 && !isAdjustmentPlaneCreated)
        {
            // Calculate the fourth point
            Vector3 point4 = screenCorners[0] + (screenCorners[2] - screenCorners[1]);

            // Create the plane using the 3 points and the calculated fourth point
            adjustmentPlane = CreatePlane(screenCorners[0], screenCorners[1], screenCorners[2], point4);

            CalculatePlaneCenter(screenCorners[0], screenCorners[1], screenCorners[2], point4);

            // Set plane to created
            isAdjustmentPlaneCreated = true;
        }

        //reset Btn
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            resetPlane();
        }

        // Draw line from the last corner to VectorToScreen
        if (isAdjustmentPlaneCreated)
        {
            if (screenCorners.Count > 0)
            {
                VectorToScreen = HeadSet.transform.position - screenCorners[screenCorners.Count - 1];
                Debug.DrawLine(screenCorners[screenCorners.Count - 1], screenCorners[screenCorners.Count - 1] + VectorToScreen, Color.red);
            }
        }
    }

    private void resetPlane()
    {
        // Destroy adjustmentPlane GameObject if it exists
        if (adjustmentPlane != null)
        {
            Destroy(adjustmentPlane);
            adjustmentPlane = null;
        }

        // Destroy all cornerPrefabs
        foreach (var cornerPrefab in cornerPrefabs)
        {
            Destroy(cornerPrefab);
        }
        cornerPrefabs.Clear();

        // Clear the screenCorners list
        screenCorners.Clear();

        // Reset the flag for plane creation
        isAdjustmentPlaneCreated = false;
    }

    public GameObject CreatePlane(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
    {
        // Create a new GameObject to hold the plane
        GameObject plane = new GameObject("Plane");

        // Add a MeshFilter and MeshRenderer component
        MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();

        // Create a new mesh and assign it to the MeshFilter
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        // Define the vertices
        Vector3[] vertices = new Vector3[4];
        vertices[0] = point1;
        vertices[1] = point2;
        vertices[2] = point3;
        vertices[3] = point4;

        // Define the triangles
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 0;
        triangles[4] = 2;
        triangles[5] = 3;

        // Define the UVs
        Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(0, 1);

        // Assign vertices, triangles, and UVs to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Recalculate normals and bounds
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Assign a material to the MeshRenderer
        meshRenderer.material = planeMaterial; // Ensure this material is assigned in the inspector

        return plane;
    }

    private Vector3 CalculatePlaneCenter(Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4)
    {
        // Calculate the center point of the plane as the average of the three points
        Vector3 planeCenter = (point1 + point2 + point3 + point4) / 4f;

        // Instantiate a prefab at the calculated center (for demonstration) and add it to the list (for reset)
        cornerPrefabs.Add(Instantiate(CornerPrefab, planeCenter, Quaternion.identity));
        screenCorners.Add(planeCenter);

        return planeCenter;
    }
}
