using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    
    [Header("Animation & Dissolve")]
    [SerializeField] private EnemyAnimationController animationController;
    
    [Header("Andere Komponenten deaktivieren beim Tod")]
    [SerializeField] private bool disableShooterOnDeath = true;
    [SerializeField] private bool disableMovementOnDeath = true;
    
    private bool isDead = false;
    
    private void Awake()
    {
        currentHealth = maxHealth;
        
        // Automatisch Animation Controller finden
        if (animationController == null)
        {
            animationController = GetComponent<EnemyAnimationController>();
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isDead) return; // Bereits tot, ignoriere weiteren Schaden
        
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        if (isDead) return; // Verhindere mehrfaches Sterben
        
        isDead = true;
        Debug.Log($"{gameObject.name} stirbt!");
        
        // Deaktiviere Shooter Script
        if (disableShooterOnDeath)
        {
            EnemyShooter shooter = GetComponent<EnemyShooter>();
            if (shooter != null)
            {
                shooter.enabled = false;
            }
        }
        
        // Deaktiviere Movement Script
        if (disableMovementOnDeath)
        {
            EnemyMovement movement = GetComponent<EnemyMovement>();
            if (movement != null)
            {
                movement.enabled = false;
            }
        }
        
        // Deaktiviere Collider (keine weiteren Treffer)
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // Starte Dissolve Animation
        if (animationController != null)
        {
            animationController.StartDissolve();
            // GameObject wird vom AnimationController zerstört
        }
        else
        {
            // Fallback: Wenn kein AnimationController, normal zerstören
            Debug.LogWarning($"{gameObject.name} hat keinen EnemyAnimationController - nutze normales Destroy");
            Destroy(gameObject, 0.5f);
        }
    }
    
    // Optional: Prüfe ob tot
    public bool IsDead()
    {
        return isDead;
    }
    
    // Optional: Hole aktuelle Health
    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    // Optional: Heile den Gegner
    public void Heal(int amount)
    {
        if (isDead) return;
        
        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth); // Nicht über Maximum
        Debug.Log($"{gameObject.name} geheilt! Health: {currentHealth}/{maxHealth}");
    }
}