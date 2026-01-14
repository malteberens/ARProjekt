using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [Header("Referenzen")]
    [SerializeField] private AutomaticSpawning spawningScript;
    
    [Header("Bewegungs-Aktivierung")]
    [SerializeField] private int enemiesNeededForMovement = 5; // Nach wie vielen Abschüssen
    [SerializeField] private bool showDebugMessages = true;
    
    private bool movementActivated = false;
    private int lastCheckedCount = 0;
    GameObject enemy;

    [Header("Schussfrequenz-Erhöhen")]
    [SerializeField] private int enemiesNeededForFasterShooting = 10; // Nach wie vielen Abschüssen
    [SerializeField] private float NewshootInterval = 1f; // neue cooldown zwischen Schüssen

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
    
        
        // Prüfe nur wenn Bewegung noch nicht aktiviert wurde
        if (!movementActivated && spawningScript != null)
        {
            // Greife auf die Anzahl zerstörter Gegner zu
            int destroyedEnemies = GetDestroyedEnemiesCount();
            Debug.Log($"Aktuelle Gegner-Anzahl: {destroyedEnemies}");
            // Debug-Ausgabe bei Änderung
            if (showDebugMessages && destroyedEnemies != lastCheckedCount)
            {
                Debug.Log($"Zerstörte Gegner: {destroyedEnemies}/{enemiesNeededForMovement}");
                lastCheckedCount = destroyedEnemies;
            }
            
            // Prüfe ob Schwellenwert erreicht
            if (destroyedEnemies >= enemiesNeededForMovement)
            {
                ActivateEnemyMovement();
            }
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
            Debug.Log($"BEWEGUNG AKTIVIERT! {enemiesNeededForMovement} Gegner wurden zerstört.");
        }
        
        // Aktiviere Bewegung für alle bereits existierenden Gegner
        EnemyMovement[] existingEnemies = FindObjectsOfType<EnemyMovement>();
        foreach (EnemyMovement enemy in existingEnemies)
        {
            enemy.enabled = true;
        }
        
        // Optional: Zeige Nachricht im Spiel
        // Du kannst hier UI-Text oder andere Effekte hinzufügen
    }
    
    // Diese Methode kann von außen aufgerufen werden um den Status zu prüfen
    public bool IsMovementActive()
    {
        return movementActivated;
    }
    
    // Methode zum Zurücksetzen (z.B. bei neuem Spiel)
    public void ResetDifficulty()
    {
        movementActivated = false;
        lastCheckedCount = 0;
        
        if (showDebugMessages)
        {
            Debug.Log("Difficulty Manager zurückgesetzt");
        }
    }
}