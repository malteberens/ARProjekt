using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Bewegungseinstellungen")]
    [SerializeField] private float bewegungsGeschwindigkeit = 2f;
    [SerializeField] private float bewegungsBereich = 3f; // Wie weit nach links/rechts
    
    [Header("Kamera Referenz")]
    [SerializeField] private Camera arKamera; // AR-Kamera (meist die Hauptkamera)
    
    private Vector3 startPosition;
    private float bewegungsOffset = 0f;
    private bool bewegtNachRechts = true;

    void Start()
    {
        // Falls keine Kamera zugewiesen, nutze die Hauptkamera
        if (arKamera == null)
        {
            arKamera = Camera.main;
        }
        
        // Speichere die Startposition (Weltkoordinaten)
        startPosition = transform.position;
    }

    void Update()
    {
        // Berechne die Rechts-Richtung relativ zur Kamera
        Vector3 kameraRechts = arKamera.transform.right;
        kameraRechts.y = 0; // Halte die Bewegung horizontal (keine vertikale Bewegung)
        kameraRechts.Normalize();
        
        // Bewege den Offset
        if (bewegtNachRechts)
        {
            bewegungsOffset += bewegungsGeschwindigkeit * Time.deltaTime;
            
            if (bewegungsOffset >= bewegungsBereich)
            {
                bewegtNachRechts = false;
            }
        }
        else
        {
            bewegungsOffset -= bewegungsGeschwindigkeit * Time.deltaTime;
            
            if (bewegungsOffset <= -bewegungsBereich)
            {
                bewegtNachRechts = true;
            }
        }
        
        // Setze die Position relativ zur Kamera-Ausrichtung
        transform.position = startPosition + (kameraRechts * bewegungsOffset);
    }
}