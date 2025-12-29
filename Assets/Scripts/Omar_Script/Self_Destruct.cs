using UnityEngine;

public class BallZerstörer : MonoBehaviour
{
    [Header("Einstellungen")]
    [Tooltip("Zeit in Sekunden, bis der Ball verschwindet")]
    [SerializeField] private float lebensdauer = 5f; 

    void Start()
    {
        // Diese Funktion löscht das Objekt automatisch nach X Sekunden aus dem Speicher
        Destroy(gameObject, lebensdauer);
    }
}