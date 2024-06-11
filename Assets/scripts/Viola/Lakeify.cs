using UnityEngine;
using System.Collections.Generic;

public class Lakeify : MonoBehaviour
{
    public List<GameObject> wallPrefabs; // List of prefabs to spawn for walls
    public List<GameObject> floorPrefabs; // List of prefabs to spawn for floors
    public float spawnRadius = 5.0f; // Radius within which the prefabs will be spawned
    public int numberOfPrefabs = 10; // Number of prefabs to spawn
    public float minDistanceBetweenSpawns = 1.0f; // Minimum distance between spawn points

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
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            spawnCenter = new Vector3(spawnCenter.x, floorY, spawnCenter.z);
            // Generate a random point within the spawn radius, keeping the y-coordinate the same
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                0, // y-coordinate should remain the same
                Random.Range(-spawnRadius, spawnRadius)
            );

            Vector3 spawnPoint = spawnCenter + randomOffset;

            // Check if the position is too close to previously spawned positions
            if (IsPositionValid(spawnPoint))
            {
                // Randomly select a prefab from the list
                GameObject selectedPrefab = floorPrefabs[Random.Range(0, floorPrefabs.Count)];
                // Instantiate the prefab at the spawn point
                Instantiate(selectedPrefab, spawnPoint, Quaternion.identity);
                // Add the new spawn position to the set
                spawnedPositions.Add(spawnPoint);
            }
        }
    }

    private void SpawnWallPrefabs(Vector3 spawnCenter, Transform wallTransform, float X, float Z)
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            // Generate a random point within the spawn radius
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius)
            );

            // Adjust randomOffset based on wall orientation
            if (Mathf.Abs(wallTransform.forward.z) > Mathf.Abs(wallTransform.forward.x))
            {
                // Wall is aligned along the z-axis, affecting x/y coordinates
                randomOffset.x = Random.Range(-spawnRadius, spawnRadius);
                randomOffset.z = 0; // z-coordinate should be zero if the wall is aligned along the x-axis
                spawnCenter.z = Z;
            }
            else if (Mathf.Abs(wallTransform.forward.x) > Mathf.Abs(wallTransform.forward.z))
            {
                // Wall is aligned along the x-axis, affecting z/y coordinates
                randomOffset.z = Random.Range(-spawnRadius, spawnRadius);
                randomOffset.x = 0; // x-coordinate should be zero if the wall is aligned along the z-axis
                spawnCenter.x = X;
            }

            Vector3 spawnPoint = spawnCenter + randomOffset;

            // Check if the position is too close to previously spawned positions
            if (IsPositionValid(spawnPoint))
            {
                // Randomly select a prefab from the list
                GameObject selectedPrefab = wallPrefabs[Random.Range(0, wallPrefabs.Count)];

                // Calculate the rotation to align the prefab's up direction with the wall's forward direction
                Quaternion alignmentRotation = Quaternion.LookRotation(wallTransform.forward, Vector3.up);

                // Add a random rotation around the wall's forward axis
                Quaternion randomRotation = Quaternion.AngleAxis(Random.Range(0f, 360f), wallTransform.forward);

                // Combine the rotations
                Quaternion finalRotation = alignmentRotation * randomRotation;

                // Instantiate the prefab at the spawn point with the calculated rotation
                Instantiate(selectedPrefab, spawnPoint, finalRotation);
                // Add the new spawn position to the set
                spawnedPositions.Add(spawnPoint);
            }
        }
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPosition) < minDistanceBetweenSpawns)
            {
                return false;
            }
        }
        return true;
    }
}
