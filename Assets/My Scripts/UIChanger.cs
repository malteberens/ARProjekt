using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas[] allCanvases;

    void Start()
    {
        // findet alle Canvas Objekte
        allCanvases = FindObjectsOfType<Canvas>(true);
        Debug.Log("Gefundene Canvas: " + allCanvases.Length);
        
        foreach (Canvas canvas in allCanvases)
        {
            Debug.Log("- " + canvas.gameObject.name);
        }
    }

    public void ShowCanvasByName(string canvasName)
    {
        Debug.Log("Wechsel zu Canvas: " + canvasName);
        
        foreach (Canvas canvas in allCanvases)
        {
            // übergebenes Canvas aktivieren
            bool shouldBeActive = (canvas.gameObject.name == canvasName);
            canvas.gameObject.SetActive(shouldBeActive);
            
            Debug.Log(canvas.gameObject.name + " ist jetzt: " + (shouldBeActive ? "aktiv" : "inaktiv"));
        }
    }
}
