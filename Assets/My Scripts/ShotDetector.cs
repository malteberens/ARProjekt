using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TouchTextEditor : MonoBehaviour
{
    public GameObject activeCanvas;
    public TMP_Text textField;
    
    private int touchCount = 0;
    private bool wasCanvasActive = false;
    
    void Update()
    {
        // Prüft Canvas-Status
        bool isCanvasActive = (activeCanvas != null && activeCanvas.activeSelf);
        
        // Reset bei Canvas-Aktivierung
        if (isCanvasActive && !wasCanvasActive)
        {
            touchCount = 0;
            Debug.Log("Canvas aktiviert - Counter zurückgesetzt");
        }
        
        wasCanvasActive = isCanvasActive;
        
        // Stoppt, wenn Canvas inaktiv
        if (!isCanvasActive)
            return;
        
        // Touch-Abfrage
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                // zählt nur, wenn der Touch nicht auf einem UI Objekt ist
                if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    HandleTouch(touch.position);
                }
            }
        }
        // Maus
        if (Input.GetMouseButtonDown(0))
        {
            // zählt nur, wenn der Touch nicht auf einem UI Objekt ist
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                HandleTouch(Input.mousePosition);
            }
        }
    }
    // Counter und Textfeldaktualisierung
    void HandleTouch(Vector2 position)
    {
        touchCount++;
        Debug.Log("Touch #" + touchCount);
        
        if (textField != null)
        {
            textField.text = touchCount + " Schüsse";
        }
    }
}
