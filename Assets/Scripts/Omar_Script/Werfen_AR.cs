using UnityEngine;
using UnityEngine.InputSystem;

public class ARWerfenUnendlich : MonoBehaviour
{
    [Header("Referenzen")]
    [Tooltip("Die AR-Kamera zur Bestimmung der Wurfrichtung")]
    [SerializeField] private Transform arCamera;

    [Tooltip("Ziehe hier ein leeres Objekt hin, um den Spawn-Punkt festzulegen. Bleibt es leer, wird die Kamera genutzt.")]
    [SerializeField] private Transform individuellerSpawnPunkt;

    [SerializeField] private GameObject objectToThrow;

    [Header("Einstellungen")]
    [SerializeField] private float throwCooldown = 0.2f; // Kleinerer Cooldown für mehr Action

    [Header("Wurf-Physik")]
    [SerializeField] private float throwForce = 15f;
    [SerializeField] private float throwUpwardForce = 2f;

    private bool readyToThrow = true;
    private InputAction touchAction;
    private InputAction mouseAction;

    private void Awake()
    {
        // Setup der Input Actions für PC-Simulator und iPhone
        touchAction = new InputAction(type: InputActionType.Button, binding: "<Touchscreen>/primaryTouch/press");
        mouseAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
    }

    private void OnEnable()
    {
        touchAction.Enable();
        mouseAction.Enable();
    }

    private void OnDisable()
    {
        touchAction.Disable();
        mouseAction.Disable();
    }

    private void Update()
    {
        // Wir prüfen nur noch auf Cooldown, nicht mehr auf die Anzahl der Bälle
        bool isInputDetected = touchAction.WasPressedThisFrame() || mouseAction.WasPressedThisFrame();

        if (isInputDetected && readyToThrow)
        {
            ThrowObject();
        }
    }

    private void ThrowObject()
    {
        readyToThrow = false;

        // Logik für den Spawn-Punkt:
        // Wenn ein individueller Punkt zugewiesen wurde, nimm diesen. 
        // Ansonsten nimm die Kamera-Position + einen kleinen Offset nach vorne.
        Vector3 spawnPos;
        if (individuellerSpawnPunkt != null)
        {
            spawnPos = individuellerSpawnPunkt.position;
        }
        else
        {
            spawnPos = arCamera.position + arCamera.forward * 0.2f;
        }

        // Objekt instanziieren
        GameObject projectile = Instantiate(objectToThrow, spawnPos, arCamera.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Die Kraft wird weiterhin in Blickrichtung der Kamera ausgeübt
            Vector3 forceToAdd = arCamera.forward * throwForce + arCamera.up * throwUpwardForce;
            rb.AddForce(forceToAdd, ForceMode.Impulse);
        }

        // Cooldown zurücksetzen
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }
}