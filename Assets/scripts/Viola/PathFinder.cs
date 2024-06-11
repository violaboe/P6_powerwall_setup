using UnityEngine;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using System.Collections;
using UnityEngine.UIElements;

public class PathFinder : MonoBehaviour
{
    // List of GameObjects to search through
    private List<GameObject> ceilingAnchors = new List<GameObject>();
    private List<GameObject> floorAnchors = new List<GameObject>();
    private List<GameObject> wallAnchors = new List<GameObject>();

    public List<string> orderOfPath = new List<string>();
    public List<Transform> fireflyPath = new List<Transform>();

    public fireflyMovement fireflyMovement;
    // The goal distance to find the closest GameObject to
    public float goalDistance;
    // The reference point from which distances are calculated
    public Transform referencePoint;
    // How often to check for spawned objects
    public float checkInterval = 0.5f;

    public Material closestPoint;

    private HashSet<GameObject> usedAnchors = new HashSet<GameObject>();

    void Start()
    {
        // Start the coroutine to wait for the ceiling anchors to be spawned
        StartCoroutine(WaitForAnchors());
    }

    private IEnumerator WaitForAnchors()
    {
        while (GameObject.FindGameObjectsWithTag("ceiling anchor").Length == 0)
        {
            // Wait for a specified interval before checking again
            yield return new WaitForSeconds(checkInterval);
        }

        // Once the objects are spawned, call AnchorsLoaded
        AnchorsLoaded();
    }

    public void AnchorsLoaded()
    {
        ceilingAnchors.AddRange(GameObject.FindGameObjectsWithTag("ceiling anchor"));
        wallAnchors.AddRange(GameObject.FindGameObjectsWithTag("wall anchor"));
        floorAnchors.AddRange(GameObject.FindGameObjectsWithTag("floor anchor"));

        if (ceilingAnchors == null || ceilingAnchors.Count == 0)
        {
            Debug.LogError("The list of ceiling anchors is empty.");
            return;
        }

        if (wallAnchors == null || wallAnchors.Count == 0)
        {
            Debug.LogError("The list of wall anchors is empty.");
            return;
        }

        if (floorAnchors == null || floorAnchors.Count == 0)
        {
            Debug.LogError("The list of floor anchors is empty.");
            return;
        }

        GameObject closestAnchor = null;

        for (int i = 0; i < orderOfPath.Count; i++)
        {
            if (orderOfPath[i].ToString() == "wall")
            {
                closestAnchor = FindClosestGameObject(wallAnchors);
            }
            else if (orderOfPath[i].ToString() == "ceiling")
            {
                closestAnchor = FindClosestGameObject(ceilingAnchors);
            }
            else if (orderOfPath[i].ToString() == "floor")
            {
                closestAnchor = FindClosestGameObject(floorAnchors);
            }

            if (closestAnchor != null)
            {
                Debug.Log("The closest anchor is: " + closestAnchor.name);
                closestAnchor.GetComponent<MeshRenderer>().material = closestPoint;
                referencePoint.transform.position = closestAnchor.transform.position;
                fireflyPath.Add(closestAnchor.transform);
                usedAnchors.Add(closestAnchor);
            }
            else
            {
                Debug.Log("No GameObject found close to the goal distance.");
            }
        }
        fireflyMovement.BeginPath(fireflyPath);
    }

    GameObject FindClosestGameObject(List<GameObject> anchors)
    {
        GameObject closestGameObject = null;
        float closestDistanceDifference = Mathf.Infinity;

        foreach (GameObject obj in anchors)
        {
            if (usedAnchors.Contains(obj))
            {
                continue;
            }

            float distance = Vector3.Distance(referencePoint.position, obj.transform.position);
            float distanceDifference = Mathf.Abs(distance - goalDistance);

            if (distanceDifference < closestDistanceDifference)
            {
                closestDistanceDifference = distanceDifference;
                closestGameObject = obj;
            }
        }

        return closestGameObject;
    }
}
