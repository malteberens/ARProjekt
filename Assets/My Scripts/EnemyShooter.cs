using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    [Header("Schuss-Einstellungen")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] public float shootInterval = 2f; // Sekunden zwischen Schüssen
    [SerializeField] private float ballSpeed = 10f;
    [SerializeField] private float spawnDistance = 1f;
    
    [Header("Animation")]
    [SerializeField] private EnemyAnimationController animationController;
    
    private Transform player;
    private float nextShootTime;
    
    void Start()
    {
        player = Camera.main.transform;
        
        if (player == null)
        {
            Debug.LogError("Keine Kamera gefunden!");
        }
        
        // Automatisch Animation Controller finden falls nicht zugewiesen
        if (animationController == null)
        {
            animationController = GetComponent<EnemyAnimationController>();
        }
        
        nextShootTime = Time.time + shootInterval;
    }
    
    void Update()
    {
        // Nur schießen wenn nicht tot
        if (animationController != null && animationController.IsDead())
        {
            return;
        }
        
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
        
        // Spiele Attack Animation ab
        if (animationController != null)
        {
            animationController.PlayAttackAnimation();
        }
        
        // Berechne Richtung zum Spieler
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        
        // Spawn-Position etwas vor dem Gegner
        Vector3 spawnPosition = transform.position + directionToPlayer * spawnDistance;
        
        // Erstelle die Kugel
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        
        // Füge Geschwindigkeit hinzu
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = directionToPlayer * ballSpeed;
        }
        else
        {
            Debug.LogWarning("Das Ball-Prefab benötigt eine Rigidbody-Komponente!");
        }
        
        // Zerstöre die Kugel nach 3 Sekunden
        Destroy(ball, 6f);
    }
    
    void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);
        }
    }
    
    /// <summary>
    /// Erhöhe die Schussrate - wird vom DifficultyManager aufgerufen
    /// </summary>
    public void IncreaseFireRate(float decreaseAmount)
    {   
        shootInterval -= decreaseAmount;
        shootInterval = Mathf.Max(0.1f, shootInterval);
        Debug.Log($"[{gameObject.name}] Neue Schussrate: {shootInterval:F2} Sekunden");
    }
}