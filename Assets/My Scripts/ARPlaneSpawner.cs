using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARPlaneSpawner : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject enemyPrefab;

   public void SpawnEnemy()
{
    List<ARPlane> planes = new List<ARPlane>();

    foreach (var detectedPlane in planeManager.trackables)
    {
        if (detectedPlane.alignment == PlaneAlignment.HorizontalUp)
        {
            planes.Add(detectedPlane);
        }
    }

    if (planes.Count == 0)
        return;

    ARPlane plane = planes[Random.Range(0, planes.Count)];

    Vector2 size = plane.size;
    Vector3 center = plane.center;

    float randomX = Random.Range(-size.x / 2f, size.x / 2f);
    float randomZ = Random.Range(-size.y / 2f, size.y / 2f);

    Vector3 localPos = center + new Vector3(randomX, 0f, randomZ);
    Vector3 worldPos = plane.transform.TransformPoint(localPos);

    Instantiate(enemyPrefab, worldPos, Quaternion.identity);
}
}