using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

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
        bool isInputDetected = touchAction.WasPressedThisFrame() || mouseAction.WasPressedThisFrame();

if (isInputDetected && readyToThrow)
{
    // Prüfe ob Touch auf UI ist (nur bei Touch-Eingaben)
    bool isTouchOnUI = false;
    if (Input.touchCount > 0)
    {
        Touch touch = Input.GetTouch(0);
        isTouchOnUI = EventSystem.current.IsPointerOverGameObject(touch.fingerId);
    }
    // Prüfe ob Maus auf UI ist (nur bei Maus-Eingaben)
    else if (mouseAction.WasPressedThisFrame())
    {
        isTouchOnUI = EventSystem.current.IsPointerOverGameObject();
    }

    if (!isTouchOnUI)
    {
        ThrowObject();
    }
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