using UnityEngine;
using System;

public class ARPlayerCollision : MonoBehaviour
{
    public static event Action<GameObject> OnPlayerCollision;
    public static event Action<GameObject> OnPlayerTriggerEnter;
    
    // KORRIGIERT: Mit Großbuchstaben
    public string[] allowedTags = { "Enemy", "enemy" };
    
    void Start()
    {
        Debug.Log("ARPlayerCollision Script gestartet");
        EnsureColliderSetup();
    }
    
    void EnsureColliderSetup()
    {
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            CapsuleCollider capsule = gameObject.AddComponent<CapsuleCollider>();
            capsule.radius = 0.3f;
            capsule.height = 0f;
            capsule.center = new Vector3(0, 0, 0);
            capsule.isTrigger = true;
            Debug.Log("Collider hinzugefügt (als Trigger)");
        }
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            rb.useGravity = false;
            Debug.Log("Rigidbody hinzugefügt");
        }
    }
    
    void OnTriggerEnter(Collider other)
{
    // Ignoriere AR Planes und andere AR-Objekte
    if (other.gameObject.name.Contains("Plane") || 
        other.gameObject.name.Contains("AR"))
    {
        return; // Ignorieren
    }
    
   // Debug.Log("=== TRIGGER ERKANNT ===");
   // Debug.Log("Objekt Name: " + other.gameObject.name);
   // Debug.Log("Tag des Objekts: " + other.gameObject.tag);
    
    if (IsAllowedTag(other.gameObject.tag))
    {
        Debug.Log("Tag ist erlaubt! Event wird ausgelöst...");
        OnPlayerTriggerEnter?.Invoke(other.gameObject);
    }
    else
    {
        Debug.LogWarning("Tag ist NICHT erlaubt. Objekt: " + other.gameObject.name + 
                        ", Tag: " + other.gameObject.tag + 
                        ", Erlaubte Tags: " + string.Join(", ", allowedTags));
    }
}

    
    bool IsAllowedTag(string tag)
    {
        if (allowedTags.Length == 0) return true;
        
        foreach (string allowedTag in allowedTags)
        {
            if (tag == allowedTag) return true;
        }
        return false;
    }
}
