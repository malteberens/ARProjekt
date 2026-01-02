using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    [Header("Herz UI")]
    public GameObject[] hearts;
    
    [Header("Canvas")]
    public GameObject gameplayCanvas;
    public GameObject endCanvas;

    public GameTimer gameTimer;
    
    [Header("Endscreen Text")]
    public TextMeshProUGUI endscreenText;

    [Header("Referenzen")]
    //public ARWerfenUnendlich throwingScript; // Dein Wurf-Script
    public AutomaticSpawning enemySpawner; // Dein Enemy-Spawner (falls vorhanden)
    
    private int currentHealth = 3;
    private int totalLostHearts = 0;
    private bool isSubscribed = false;
    
    void Start()
    {
        InitializeGame();
    }
    
    void OnEnable()
    {
        SubscribeToEvents();
    }
    
    void OnDisable()
    {
        UnsubscribeFromEvents();
    }
    
    void SubscribeToEvents()
    {
        if (!isSubscribed)
        {
            ARPlayerCollision.OnPlayerCollision += OnCollisionDetected;
            ARPlayerCollision.OnPlayerTriggerEnter += OnCollisionDetected;
            isSubscribed = true;
            Debug.Log("Events subscribed");
        }
    }
    
    void UnsubscribeFromEvents()
    {
        if (isSubscribed)
        {
            ARPlayerCollision.OnPlayerCollision -= OnCollisionDetected;
            ARPlayerCollision.OnPlayerTriggerEnter -= OnCollisionDetected;
            isSubscribed = false;
            Debug.Log("Events unsubscribed");
        }
    }
    
    void OnCollisionDetected(GameObject collidedObject)
    {
        if (collidedObject.CompareTag("Enemy") || collidedObject.CompareTag("enemy"))
        {
            LoseHeart();
        }
    }
    
    public void LoseHeart()
    {
        if (currentHealth <= 0) return;
        
        currentHealth--;
        totalLostHearts++;
        
        Debug.Log($"Herz verloren! Verbleibende Herzen: {currentHealth}");
        
        if (currentHealth >= 0 && currentHealth < hearts.Length)
        {
            hearts[currentHealth].SetActive(false);
        }
        
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    
    void GameOver()
    {
        Debug.Log("Game Over!");
        if (enemySpawner != null)
        {
            enemySpawner.DestroyAllSpawnedObjects();
        }
        
        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(false);
            
        if (endCanvas != null)
            endCanvas.SetActive(true);
        
        UpdateEndscreenText();
    }
    
    void UpdateEndscreenText()
    {
        if (endscreenText != null)
        {
            endscreenText.text = $"Verlorene Herzen: {totalLostHearts}";
        }
    }
    
    void InitializeGame()
    {
        currentHealth = 3;
        totalLostHearts = 0;
        
        foreach (GameObject heart in hearts)
        {
            if (heart != null)
            {
                heart.SetActive(true);
            }
        }
        
        if (endCanvas != null)
            endCanvas.SetActive(false);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    // ÖFFENTLICHE RESET-METHODE (wird von ResetManager aufgerufen)
    public void ResetGame()
    {
        Debug.Log("HealthManager: Reset wird durchgeführt");

         if (gameTimer != null)
            {
            gameTimer.StopTimer();
            Debug.Log("Timer gestoppt");
            }
        
        // Events neu subscribed
        UnsubscribeFromEvents();
        
        // Health zurücksetzen
        currentHealth = 3;
        totalLostHearts = 0;
        
        // Herzen aktivieren
        foreach (GameObject heart in hearts)
        {
            if (heart != null)
            {
                heart.SetActive(true);
            }
        }
        
        // Canvas umschalten
        if (endCanvas != null)
            endCanvas.SetActive(false);
        
        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(true);
        
        // Events wieder subscriben
        SubscribeToEvents();
        
        Debug.Log("HealthManager: Reset abgeschlossen");
    }
}
