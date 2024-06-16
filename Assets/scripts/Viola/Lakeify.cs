using UnityEngine;
using System.Collections.Generic;

public class Lakeify : MonoBehaviour
{
    public List<GameObject> wallPrefabs; // List of prefabs to spawn for walls
    public List<GameObject> floorPrefabs; // List of prefabs to spawn for floors
    public float innerSpawnRadius = 3.0f; // Inner radius with higher spawn density
    public float outerSpawnRadius = 5.0f; // Outer radius with lower spawn density
    public int innerNumberOfPrefabs = 15; // Number of prefabs to spawn in the inner radius
    public int outerNumberOfPrefabs = 5; // Number of prefabs to spawn in the outer radius
    public float innerMinDistanceBetweenSpawns = 0.5f; // Minimum distance between spawn points in the inner radius
    public float outerMinDistanceBetweenSpawns = 1.0f; // Minimum distance between spawn points in the outer radius

    private HashSet<Vector3> spawnedPositions = new HashSet<Vector3>();

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "WALL_FACE_EffectMesh")
        {
            // Get the point of contact
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            float wallX = other.transform.position.x;
            float wallZ = other.transform.position.z;
            // Spawn the prefabs around the contact point
            SpawnWallPrefabs(contactPoint, other.transform, wallX, wallZ);
        }
        else if (other.gameObject.name == "FLOOR_EffectMesh")
        {
            // Get the point of contact
            Vector3 contactPoint = other.ClosestPoint(transform.position);

            float floorY = other.transform.position.y;

            // Spawn the prefabs around the contact point
            SpawnFloorPrefabs(contactPoint, floorY);
        }
        else if (other.gameObject.name == "CEILING_EffectMesh")
        {
            // Disable the MeshRenderer of the ceiling
            MeshRenderer meshRenderer = other.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.enabled = false;
            }
        }
    }

    private void SpawnFloorPrefabs(Vector3 spawnCenter, float floorY)
    {
        spawnCenter = new Vector3(spawnCenter.x, 0, spawnCenter.z);
        SpawnPrefabsInRadius(spawnCenter, floorY, floorPrefabs, innerNumberOfPrefabs, innerSpawnRadius, innerMinDistanceBetweenSpawns);
        SpawnPrefabsInRadius(spawnCenter, floorY, floorPrefabs, outerNumberOfPrefabs, outerSpawnRadius, outerMinDistanceBetweenSpawns, innerSpawnRadius);
    }

    private void SpawnWallPrefabs(Vector3 spawnCenter, Transform wallTransform, float X, float Z)
    {
        SpawnWallPrefabsInRadius(spawnCenter, wallTransform, X, Z, innerNumberOfPrefabs, innerSpawnRadius, innerMinDistanceBetweenSpawns);
        SpawnWallPrefabsInRadius(spawnCenter, wallTransform, X, Z, outerNumberOfPrefabs, outerSpawnRadius, outerMinDistanceBetweenSpawns, innerSpawnRadius);
    }

    private void SpawnPrefabsInRadius(Vector3 spawnCenter, float floorY, List<GameObject> prefabs, int numberOfPrefabs, float radius, float minDistance, float innerRadius = 0f)
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector3 randomOffset;
            float distanceFromCenter;
            do
            {
                randomOffset = new Vector3(
                    Random.Range(-radius, radius),
                    0, // y-coordinate should remain the same
                    Random.Range(-radius, radius)
                );
                distanceFromCenter = randomOffset.magnitude;
            } while (distanceFromCenter < innerRadius || distanceFromCenter > radius);

            Vector3 spawnPoint = spawnCenter + randomOffset;
            spawnPoint = new Vector3(spawnPoint.x, floorY, spawnPoint.z);

            if (IsPositionValid(spawnPoint, minDistance))
            {
                GameObject selectedPrefab = prefabs[Random.Range(0, prefabs.Count)];
                Instantiate(selectedPrefab, spawnPoint, Quaternion.identity);
                spawnedPositions.Add(spawnPoint);
            }
        }
    }

    private void SpawnWallPrefabsInRadius(Vector3 spawnCenter, Transform wallTransform, float X, float Z, int numberOfPrefabs, float radius, float minDistance, float innerRadius = 0f)
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector3 randomOffset;
            float distanceFromCenter;
            do
            {
                randomOffset = new Vector3(
                    Random.Range(-radius, radius),
                    Random.Range(-radius, radius),
                    Random.Range(-radius, radius)
                );

                // Adjust randomOffset based on wall orientation
                if (Mathf.Abs(wallTransform.forward.z) > Mathf.Abs(wallTransform.forward.x))
                {
                    randomOffset.x = Random.Range(-radius, radius);
                    randomOffset.z = 0;
                }
                else if (Mathf.Abs(wallTransform.forward.x) > Mathf.Abs(wallTransform.forward.z))
                {
                    randomOffset.z = Random.Range(-radius, radius);
                    randomOffset.x = 0;
                }

                distanceFromCenter = randomOffset.magnitude;
            } while (distanceFromCenter < innerRadius || distanceFromCenter > radius);

            Vector3 spawnPoint = spawnCenter + randomOffset;
            if (Mathf.Abs(wallTransform.forward.z) > Mathf.Abs(wallTransform.forward.x))
            {
                spawnPoint.z = Z;
            }
            else if (Mathf.Abs(wallTransform.forward.x) > Mathf.Abs(wallTransform.forward.z))
            {
                spawnPoint.x = X;
            }

            if (IsPositionValid(spawnPoint, minDistance))
            {
                GameObject selectedPrefab = wallPrefabs[Random.Range(0, wallPrefabs.Count)];
                Quaternion alignmentRotation = Quaternion.LookRotation(wallTransform.forward, Vector3.up);
                Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), wallTransform.forward);
                Quaternion finalRotation = alignmentRotation * randomRotation;

                Instantiate(selectedPrefab, spawnPoint, finalRotation);
                spawnedPositions.Add(spawnPoint);
            }
        }
    }

    private bool IsPositionValid(Vector3 position, float minDistance)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}
