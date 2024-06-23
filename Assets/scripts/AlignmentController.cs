using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentController : MonoBehaviour
{
    [Header("PlaneOrientation Vectors")]
    public Vector3 VectorToScreen;
    public Vector3 planeNormal;

    [Header("Orientat other plane")]
    [SerializeField] private GameObject planeInOtherOrientation;
    [SerializeField] private GameObject reorientedPlayerVisualizer;
    bool isMockupPlayerCreated = false;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject CornerPrefab;
    [SerializeField] private GameObject CornerPreviewPrefab;
    [SerializeField] private GameObject ControllerRight;
    [SerializeField] private GameObject HeadSet;
    [SerializeField] private Material planeMaterial;

    private List<Vector3> screenCorners = new List<Vector3>();
    private List<GameObject> cornerPrefabs = new List<GameObject>(); // Track instantiated corner prefabs

    private GameObject adjustmentPlane;
    private bool isAdjustmentPlaneCreated = false;
    

  

  private void Start()
    {
        CornerPreviewPrefab =  Instantiate(CornerPreviewPrefab);

        Vector3 x = new Vector3(0, 2, 2);
        Vector3 normal1 = new Vector3(0, 1, 0); // Plane normal
        Vector3 normal2 = new Vector3(1, 0, 0); // Other plane normal

        Vector3 y = TransformPositionAroundCoordinateSystem(x, normal1, normal2);
        Debug.Log("Transformed Vector: " + y);
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

            planeNormal = FindNormalPointingToPlayer(adjustmentPlane);

            // Set plane to created
            isAdjustmentPlaneCreated = true;
        }

        //reset Btn
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            resetPlane();
        }

        //Create a playervisualization in relation to a Mockup plane 
        if (OVRInput.GetUp(OVRInput.RawButton.B))
        {
            CreateMockupPlayer();
            isMockupPlayerCreated = true;
        }
        if (isMockupPlayerCreated)
        {
            RepositionMockupPlayer();
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

    private void CreateMockupPlayer()
    {
        //Creates a mockup player , at point calculated by taking the normal of the current plane, the vector to the camera and fitting it to a new normal of another gameobject
        reorientedPlayerVisualizer = Instantiate(reorientedPlayerVisualizer, TransformPositionAroundCoordinateSystem(VectorToScreen, planeNormal, FindNormalPointingToPlayer(planeInOtherOrientation)), Quaternion.identity);
    }

    private void RepositionMockupPlayer()
    {
        if(reorientedPlayerVisualizer != null)
        {
            reorientedPlayerVisualizer.transform.position = TransformPositionAroundCoordinateSystem(VectorToScreen, planeNormal, FindNormalPointingToPlayer(planeInOtherOrientation));
        }
    }

    private Vector3 TransformPositionAroundCoordinateSystem(Vector3 VectorToImmitate, Vector3 normal1, Vector3 normal2)
    {
        //Calculate angle between the two normals
        float angle = Vector3.Angle(normal1, normal2);
        
        //Find Rot Axis
        Vector3 rotationAxis = Vector3.Cross(normal1, normal2).normalized;

        //Combine rotation with axis in a quaternion
        Quaternion rotation = Quaternion.AngleAxis(angle, rotationAxis);

        Vector3 RotatedVector = rotation * VectorToImmitate;

        return (RotatedVector + planeInOtherOrientation.transform.position);

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

    private Vector3 FindNormalPointingToPlayer(GameObject go)
    {
            MeshFilter meshFilter = go.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
            {
                Debug.LogWarning("Object does not have a mesh or MeshFilter component.");
                return Vector3.zero;
            }

            Mesh mesh = meshFilter.sharedMesh;
            Vector3[] normals = mesh.normals;

            if (normals.Length > 0)
            {
                // Get the normal of the first vertex
                return go.transform.TransformDirection(normals[0]);
            }
            else
            {
                Debug.LogWarning("Object mesh does not have normals.");
                return Vector3.zero;
            }
        
    }
}
