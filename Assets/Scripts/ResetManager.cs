using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [Header("Manager Referenzen")]
    public HealthManager healthManager;
    
    [Header("Optionale Objekte zum Zurücksetzen")]
    public GameObject[] objectsToReset; // Enemies, Collectibles, etc.
    
    // Wird vom Button aufgerufen
    public void ResetGame()
    {
        Debug.Log("=== ResetManager: Reset wird gestartet ===");
        
        // 1. Health Manager zurücksetzen
        if (healthManager != null)
        {
            ResetHealthSystem();
        }
        else
        {
            Debug.LogError("HealthManager nicht zugewiesen!");
        }
        
        // 2. Optional: Andere Objekte zurücksetzen
        ResetGameObjects();
        
        Debug.Log("=== ResetManager: Reset abgeschlossen ===");
    }
    
    void ResetHealthSystem()
    {
        Debug.Log("Health System wird zurückgesetzt...");
        
        // Rufe die öffentliche Reset-Methode auf
        healthManager.ResetGame();
    }
    
    void ResetGameObjects()
    {
        // Setze alle markierten Objekte zurück
        foreach (GameObject obj in objectsToReset)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                Debug.Log($"Objekt zurückgesetzt: {obj.name}");
            }
        }
    }
}
