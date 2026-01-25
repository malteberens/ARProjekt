using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [Header("Manager Referenzen")]
    public HealthManager healthManager;
    
    [Header("Optionale Objekte zum Zurücksetzen")]
    public GameObject[] objectsToReset;
    
    public void ResetGame()
    {
        Debug.Log("Reset wird gestartet");
        
        // Health Manager zurücksetzen
        if (healthManager != null)
        {
            ResetHealthSystem();
        }
        else
        {
            Debug.LogError("HealthManager nicht zugewiesen!");
        }
        
        // Andere Objekte zurücksetzen
        ResetGameObjects();
        
        Debug.Log("Reset abgeschlossen");
    }
    
    void ResetHealthSystem()
    {
        Debug.Log("Health System wird zurückgesetzt");
        
        // Methode im HealthManager
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
