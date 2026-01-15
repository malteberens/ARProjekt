using UnityEngine;
using TMPro;
using System.Collections;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Einstellungen")]
    [SerializeField] private float gameDuration = 120f; // 2 Minuten
    
    [Header("Referenzen")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private GameObject endCanvas;
    [SerializeField] private AutomaticSpawning spawner;

    [SerializeField] private HealthManager healthManager; // NEU
    [SerializeField] private TextMeshProUGUI heartsRemainingText;
    
    private float timeRemaining;
    private bool timerIsRunning = false;
    private Coroutine timerCoroutine;
    
    void Start()
    {
        // Timer startet NICHT automatisch
        timeRemaining = gameDuration;
        UpdateTimerDisplay();
    }
    
    void Update()
    {
        if (timerIsRunning && timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerDisplay();
        }
    }
    
    // Diese Funktion mit Button verknüpfen
    public void StartTimer()
    {
        if (!timerIsRunning)
        {
            timeRemaining = gameDuration;
            timerIsRunning = true;
            
            // Starte Coroutine
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
            }
            timerCoroutine = StartCoroutine(GameTimerCoroutine());
            
            Debug.Log("Timer gestartet!");
        }
    }
    
    // Diese Funktion aus HealthManager aufrufen
    public void StopTimer()
    {
        timerIsRunning = false;
        
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }
        
        Debug.Log("Timer gestoppt!");
    }
    
    IEnumerator GameTimerCoroutine()
    {
        yield return new WaitForSeconds(gameDuration);
        
        // Zeit abgelaufen!
        if (timerIsRunning) // Nur wenn nicht vorher gestoppt
        {
            TimeUp();
        }
    }
    
    void TimeUp()
    {
        timerIsRunning = false;
        timeRemaining = 0;
        
        Debug.Log("Zeit abgelaufen!");
        
        // Zerstöre alle Objekte
        if (spawner != null)
        {
            spawner.UpdateUIForEndScreen();
            spawner.DestroyAllSpawnedObjects();
        }

        if (healthManager != null && heartsRemainingText != null)
        {
            int remainingHearts = healthManager.GetCurrentHealth();
            heartsRemainingText.SetText("{0} Herzen verloren", 3-remainingHearts);
            Debug.Log($"Verbleibende Herzen: {remainingHearts}");
        }
        
        // Wechsle Canvas
        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(false);
        
        if (endCanvas != null)
            endCanvas.SetActive(true);
    }
    
    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.SetText("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
    // Optional: Timer zurücksetzen
    public void ResetTimer()
    {
        StopTimer();
        timeRemaining = gameDuration;
        UpdateTimerDisplay();
        Debug.Log("Timer zurückgesetzt!");
    }
}
