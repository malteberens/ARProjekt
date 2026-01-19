using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Referenzen")]
    [SerializeField] private AutomaticSpawning spawningScript;
    
    [Header("Bewegungs-Aktivierung")]
    [SerializeField] private int enemiesNeededForMovement = 5;
    [SerializeField] private bool showDebugMessages = true;
    
    [Header("Schussfrequenz-Erhöhung")]
    [SerializeField] private int enemiesNeededForFasterShooting = 10;
    [SerializeField] private float shootIntervalDecrease = 0.5f; // Um wie viel schneller (in Sekunden)
    
    private bool movementActivated = false;
    private bool shootingSpeedIncreased = false; // NEU: Tracking für Schussrate
    private int lastCheckedCount = 0;

    void Start()
    {
        // Automatisch Spawning Script finden falls nicht zugewiesen
        if (spawningScript == null)
        {
            spawningScript = GetComponent<AutomaticSpawning>();
            
            if (spawningScript == null)
            {
                Debug.LogError("DifficultyManager: Kein AutomaticSpawning Script gefunden!");
            }
            else if (showDebugMessages)
            {
                Debug.Log("DifficultyManager: AutomaticSpawning Script automatisch gefunden");
            }
        }
    }
    
    void Update()
    {
        if (spawningScript == null) return;
        
        // Greife auf die Anzahl zerstörter Gegner zu
        int destroyedEnemies = GetDestroyedEnemiesCount();
        
        // Debug-Ausgabe bei Änderung
        if (showDebugMessages && destroyedEnemies != lastCheckedCount)
        {
            Debug.Log($"Zerstörte Gegner: {destroyedEnemies}");
            lastCheckedCount = destroyedEnemies;
        }
        
        // Prüfe Bewegungs-Aktivierung
        if (!movementActivated && destroyedEnemies >= enemiesNeededForMovement)
        {
            ActivateEnemyMovement();
        }
        
        // NEU: Prüfe Schussraten-Erhöhung
        if (!shootingSpeedIncreased && destroyedEnemies >= enemiesNeededForFasterShooting)
        {
            IncreaseAllEnemyShootingSpeed();
        }
    }
    
    private int GetDestroyedEnemiesCount()
    {
        // Greife auf das private Feld über Reflection zu
        var field = typeof(AutomaticSpawning).GetField("totalDestroyedEnemies", 
            System.Reflection.BindingFlags.NonPublic | 
            System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            return (int)field.GetValue(spawningScript);
        }
        
        return 0;
    }
    
    private void ActivateEnemyMovement()
    {
        movementActivated = true;
        
        if (showDebugMessages)
        {
            Debug.Log($"🎯 BEWEGUNG AKTIVIERT! ({enemiesNeededForMovement} Gegner zerstört)");
        }
        
        // Aktiviere Bewegung für alle bereits existierenden Gegner
        EnemyMovement[] existingEnemies = FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemy in existingEnemies)
        {
            enemy.enabled = true;
        }
    }
    
    // NEU: Diese Methode erhöht die Schussgeschwindigkeit aller Gegner
    private void IncreaseAllEnemyShootingSpeed()
    {
        shootingSpeedIncreased = true;
        
        if (showDebugMessages)
        {
            Debug.Log($"⚡ SCHUSSRATE ERHÖHT! ({enemiesNeededForFasterShooting} Gegner zerstört)");
        }
        
        // Finde alle Gegner mit EnemyShooter Script
        EnemyShooter[] allShooters = FindObjectsOfType<EnemyShooter>();
        
        if (allShooters.Length == 0)
        {
            Debug.LogWarning("Keine EnemyShooter gefunden!");
            return;
        }
        
        // Erhöhe die Schussrate für jeden Gegner
        foreach (EnemyShooter shooter in allShooters)
        {
            shooter.IncreaseFireRate(shootIntervalDecrease);
        }
        
        if (showDebugMessages)
        {
            Debug.Log($"Schussrate von {allShooters.Length} Gegnern erhöht!");
        }
    }
    
    // Diese Methode kann von außen aufgerufen werden um den Status zu prüfen
    public bool IsMovementActive()
    {
        return movementActivated;
    }
    
    // NEU: Prüfe ob Schussrate bereits erhöht wurde
    public bool IsShootingSpeedIncreased()
    {
        return shootingSpeedIncreased;
    }
    
    // Methode zum Zurücksetzen (z.B. bei neuem Spiel)
    public void ResetDifficulty()
    {
        movementActivated = false;
        shootingSpeedIncreased = false;
        lastCheckedCount = 0;
        
        if (showDebugMessages)
        {
            Debug.Log("Difficulty Manager zurückgesetzt");
        }
    }
}