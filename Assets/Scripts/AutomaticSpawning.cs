using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using System.Linq;

public class AutomaticSpawning : MonoBehaviour
{
    [SerializeField] private ARPlaneManager planeManager;
    [SerializeField] private GameObject enemyPrefab;
    
    [Header("Spawn-Einstellungen")]
    [SerializeField] private float minDistanceFromCamera = 1f;
    [SerializeField] private float maxDistanceFromCamera = 5f;
    [SerializeField] private float minPlaneSize = 0.5f;
    [SerializeField] private float edgeMargin = 0.2f;
    [SerializeField] private float spawnHeightOffset = 0.1f;
    
    [Header("Automatisches Spawning")]
    [SerializeField] private bool autoSpawn = true;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private int maxEnemies = 5;
    [SerializeField] private float initialDelay = 2f;
    
    private float nextSpawnTime;
    private int currentEnemyCount = 0;
    private bool hasStartedSpawning = false;

    private List<GameObject> thrownObjects = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    
    void Start()
    {
        nextSpawnTime = Time.time + initialDelay;
    }
    
    void Update()
    {
        if (!autoSpawn)
            return;
        
        if (!hasStartedSpawning && planeManager.trackables.count >= 1)
        {
            hasStartedSpawning = true;
            Debug.Log("AR-Ebenen erkannt. Spawning startet.");
        }
        
        if (hasStartedSpawning && Time.time >= nextSpawnTime && currentEnemyCount < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }
    
    public void SpawnEnemy()
    {
        List<ARPlane> suitablePlanes = new List<ARPlane>();
        Camera arCamera = Camera.main;
        
        foreach (var detectedPlane in planeManager.trackables)
        {
            if (detectedPlane.alignment == PlaneAlignment.HorizontalUp &&
                IsPlaneValid(detectedPlane, arCamera))
            {
                suitablePlanes.Add(detectedPlane);
            }
        }
        
        if (suitablePlanes.Count == 0)
        {
            Debug.LogWarning("Keine geeigneten Ebenen zum Spawnen gefunden!");
            return;
        }
        
        suitablePlanes = suitablePlanes.OrderByDescending(p => p.size.x * p.size.y).ToList();
        
        int planeIndex = Random.Range(0, Mathf.Max(1, suitablePlanes.Count / 2));
        ARPlane selectedPlane = suitablePlanes[planeIndex];
        
        Vector3 spawnPosition = GetValidSpawnPosition(selectedPlane, arCamera);
        
        if (spawnPosition != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(
                (arCamera.transform.position - spawnPosition).normalized
            );
            lookRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, lookRotation);
            
            // Füge Gegner zur Liste hinzu
            spawnedEnemies.Add(enemy);
            
            EnemyTracker tracker = enemy.AddComponent<EnemyTracker>();
            tracker.spawner = this;
            
            currentEnemyCount++;
            Debug.Log($"Gegner gespawnt! Aktuelle Anzahl: {currentEnemyCount}/{maxEnemies}");
        }
        else
        {
            Debug.LogWarning("Keine gültige Spawn-Position gefunden!");
        }
    }
    
    public void OnEnemyDestroyed()
    {
        currentEnemyCount--;
        Debug.Log($"Gegner zerstört! Verbleibende Anzahl: {currentEnemyCount}");
    }
    
    private bool IsPlaneValid(ARPlane plane, Camera camera)
    {
        if (plane.size.x < minPlaneSize || plane.size.y < minPlaneSize)
            return false;
        
        float distanceToCamera = Vector3.Distance(plane.center, camera.transform.position);
        if (distanceToCamera < minDistanceFromCamera || distanceToCamera > maxDistanceFromCamera)
            return false;
        
        return true;
    }
    
    private Vector3 GetValidSpawnPosition(ARPlane plane, Camera camera)
    {
        Vector2 planeSize = plane.size;
        Vector3 planeCenter = plane.center;
        
        float usableWidth = Mathf.Max(0.1f, planeSize.x - (edgeMargin * 2));
        float usableHeight = Mathf.Max(0.1f, planeSize.y - (edgeMargin * 2));
        
        for (int attempt = 0; attempt < 10; attempt++)
        {
            float randomX = Random.Range(-usableWidth / 2f, usableWidth / 2f);
            float randomZ = Random.Range(-usableHeight / 2f, usableHeight / 2f);
            
            Vector3 localPos = planeCenter + new Vector3(randomX, 0f, randomZ);
            Vector3 worldPos = plane.transform.TransformPoint(localPos);
            worldPos.y += spawnHeightOffset;
            
            float distanceToCamera = Vector3.Distance(worldPos, camera.transform.position);
            
            if (distanceToCamera >= minDistanceFromCamera && distanceToCamera <= maxDistanceFromCamera)
            {
                return worldPos;
            }
        }
        
        return Vector3.zero;
    }
    
    public void DestroyAllSpawnedObjects()
    {
        // Deaktiviere Auto-Spawning
        autoSpawn = false;
        hasStartedSpawning = false;
        
        // Lösche alle geworfenen Objekte
        foreach (GameObject obj in thrownObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        thrownObjects.Clear();
        
        // Lösche alle Gegner
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        spawnedEnemies.Clear();
        
        // Setze Counter zurück
        currentEnemyCount = 0;
        
        Debug.Log("Alle gespawnten Objekte gelöscht und Auto-Spawning deaktiviert");
    }
    
    public void EnableAutoSpawning()
    {
        autoSpawn = true;
        hasStartedSpawning = false;
        nextSpawnTime = Time.time + initialDelay;
        Debug.Log("Auto-Spawning aktiviert");
    }
    
    private void OnDrawGizmos()
    {
        if (planeManager == null || !Application.isPlaying)
            return;
        
        Camera cam = Camera.main;
        if (cam == null)
            return;
        
        foreach (var plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp)
            {
                bool isValid = IsPlaneValid(plane, cam);
                Gizmos.color = isValid ? Color.green : Color.red;
                
                Gizmos.matrix = plane.transform.localToWorldMatrix;
                Gizmos.DrawWireCube(plane.center, new Vector3(plane.size.x, 0, plane.size.y));
            }
        }
    }
}

public class EnemyTracker : MonoBehaviour
{
    public AutomaticSpawning spawner;
    
    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.OnEnemyDestroyed();
        }
    }
}