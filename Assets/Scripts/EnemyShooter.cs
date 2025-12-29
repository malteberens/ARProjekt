using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Schuss-Einstellungen")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float shootInterval = 2f; // Sekunden zwischen Schüssen
    [SerializeField] private float ballSpeed = 10f;
    [SerializeField] private float spawnDistance = 1f; // Abstand vom Gegner, wo die Kugel spawnt
    
    private Transform player; // Der Spieler/AR-Kamera
    private float nextShootTime;

    void Start()
    {
        // Finde automatisch die AR-Kamera (Hauptkamera)
        player = Camera.main.transform;
        
        if (player == null)
        {
            Debug.LogError("Keine Kamera gefunden! Stelle sicher, dass eine Kamera mit dem Tag 'MainCamera' existiert.");
        }
        
        nextShootTime = Time.time + shootInterval;
    }

    void Update()
    {
        // Prüfe, ob es Zeit für den nächsten Schuss ist
        if (Time.time >= nextShootTime)
        {
            ShootAtPlayer();
            nextShootTime = Time.time + shootInterval;
        }
    }

    void ShootAtPlayer()
    {
        if (ballPrefab == null || player == null)
        {
            Debug.LogWarning("BallPrefab oder Spieler nicht zugewiesen!");
            return;
        }

        // Berechne die Richtung zum Spieler
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        
        // Spawn-Position etwas vor dem Gegner
        Vector3 spawnPosition = transform.position + directionToPlayer * spawnDistance;
        
        // Erstelle die Kugel
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        
        // Füge Geschwindigkeit in Richtung Spieler hinzu
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = directionToPlayer * ballSpeed;
        }
        else
        {
            Debug.LogWarning("Das Ball-Prefab benötigt eine Rigidbody-Komponente!");
        }
        
        // Optional: Zerstöre die Kugel nach 5 Sekunden
        Destroy(ball, 5f);
    }

    // Optional: Visualisiere die Schussrichtung im Editor
    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
}