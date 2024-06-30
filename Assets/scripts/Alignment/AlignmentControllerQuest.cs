using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignmentControllerQuest : MonoBehaviour
{

    [Header("RootOffsetAlign")]
    [SerializeField] private GameObject questSceneParent;
    private Vector3 AlignmentPlaneCenter;

    [Header("PlaneOrientation Vectors")]
    public Vector3 VectorToScreen;
    public Vector3 planeNormal;
    public bool isSceneAligned = false;

    [Header("ScreenDimension")]
    public Vector2 screenDimensions;

    [Header("Controller Pointer Offset")]
    [SerializeField] private float CornerControllerOffset = 0.10f;

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
        if (CornerPrefab != null) CornerPreviewPrefab = Instantiate(CornerPreviewPrefab);
        else Debug.LogError("No Prefab in Inspector");
    }

    void Update()
    {
        // Move Preview with Controller && Lock Axis
        if (!isAdjustmentPlaneCreated)
        {

            Vector3 previewPos = ControllerRight.transform.position + CornerControllerOffset * ControllerRight.transform.forward;

            // Let it be where the controller is
            if (screenCorners.Count == 0)
            {
                CornerPreviewPrefab.transform.position = previewPos;
                CornerPreviewPrefab.transform.rotation = ControllerRight.transform.rotation;
            }
            // Lock the height to the first point
            else if (screenCorners.Count == 1)
            {
                // Lock height to the y-coordinate of the first corner point
                CornerPreviewPrefab.transform.position = new Vector3(previewPos.x, screenCorners[0].y, previewPos.z);
                CornerPreviewPrefab.transform.rotation = ControllerRight.transform.rotation;
            }
            // Lock the height to the second point
            else if (screenCorners.Count == 2)
            {
                // Lock height to the y-coordinate of the second corner point
                CornerPreviewPrefab.transform.position = new Vector3(screenCorners[1].x, previewPos.y, screenCorners[1].z);
                CornerPreviewPrefab.transform.rotation = ControllerRight.transform.rotation;
            }
        }

        // Corner Creation
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
            //Save the Dimensions of the Screen for later use in the Cave/Screen Scene to make it adaptable
            GetScreenDimensions(screenCorners[0], screenCorners[2]);

            // Calculate the fourth point
            Vector3 point4 = screenCorners[0] + (screenCorners[2] - screenCorners[1]);

            // Create the plane using the 3 points and the calculated fourth point
            adjustmentPlane = CreatePlane(screenCorners[0], screenCorners[1], screenCorners[2], point4);

            AlignmentPlaneCenter = CalculatePlaneCenter(screenCorners[0], screenCorners[1], screenCorners[2], point4);

            // Use the points directly to find the normal pointing to the player
            planeNormal = FindNormalPointingToPlayer(screenCorners[0], screenCorners[1], screenCorners[2]);

            // Set plane to created
            isAdjustmentPlaneCreated = true;

            // Align the quest world on the reference plane creation
            AlignQuestWorldOnReferencePlaneCreation();

            //bool server is checking to know if scene is aligned 
            isSceneAligned = true;
        }

        // Reset button
        if (OVRInput.GetUp(OVRInput.RawButton.B))
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

        //// Create a player visualization in relation to a Mockup plane 
        //if (OVRInput.GetUp(OVRInput.RawButton.B))
        //{
        //    // SendAlignmentToserver();
        //    // CreateMockupPlayer();
        //    isSceneAligned = true;
        //}
        // if (isSceneAligned)
        // {
        //     SendAlignmentToserver();
        //     // RepositionMockupPlayer();
        // }
    }

    private void AlignQuestWorldOnReferencePlaneCreation()
    {
        Vector3 cavePodestHeight = new Vector3(0f, 0.23f, 0f);
        //Move Scene to new centerpos
        questSceneParent.transform.position = AlignmentPlaneCenter + cavePodestHeight; //if this ever gives errror delete the offset 
        //Rotate scene mathcing to rotation of plane 
        questSceneParent.transform.rotation = Quaternion.LookRotation(planeNormal);
    }

    private void GetScreenDimensions(Vector3 point1, Vector3 point3)
    {
        float screenHeight = Mathf.Abs(point1.y - point3.y);
        float screenWidth = Mathf.Abs(Vector3.Distance(point3, point1));

        screenDimensions = new Vector2(screenWidth, screenHeight);
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

        // Define the triangles to invert normals
        int[] triangles = new int[6];
        triangles[0] = 0;
        triangles[1] = 2;
        triangles[2] = 1;
        triangles[3] = 0;
        triangles[4] = 3;
        triangles[5] = 2;

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

    private Vector3 FindNormalPointingToPlayer(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 AB = pointB - pointA;
        Vector3 AC = pointC - pointA;

        // Calculate the normal using the cross product
        Vector3 normal = Vector3.Cross(AC, AB); // Reverse the order 

        // Normalize the resulting vector
        normal.Normalize();

        // Check which normal direction is pointing at the player
        if (Vector3.Dot(normal, AlignmentPlaneCenter - HeadSet.transform.position) < 0)
        {
            normal = -normal;
        }

        return normal;
    }



    //This functon SendAlignmentToserver not needed exept we decide to make variables (VectorToScreen, planeNormal) private 
    //private (Vector3, Vector3) SendAlignmentToserver()
    //{
    //    //return Aligning Info
    //    return (VectorToScreen, planeNormal);
    //}


    //Was replaced by newer version of the function not using a mesh but the corner points 
    //private Vector3 FindNormalPointingToPlayer(GameObject go)
    //{
    //    MeshFilter meshFilter = go.GetComponent<MeshFilter>();
    //    if (meshFilter == null || meshFilter.sharedMesh == null)
    //    {
    //        Debug.LogWarning("Object does not have a mesh or MeshFilter component.");
    //        return Vector3.zero;
    //    }

    //    Mesh mesh = meshFilter.sharedMesh;
    //    Vector3[] normals = mesh.normals;

    //    if (normals.Length > 0)
    //    {
    //        // Get the normal of the first vertex
    //        return go.transform.TransformDirection(normals[0]);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Object mesh does not have normals.");
    //        return Vector3.zero;
    //    }

    //}

    //This function doesnt need to acces any mesh, more cost efficient, might also be more accureate
}
