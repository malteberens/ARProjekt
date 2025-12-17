using UnityEngine;

public class CanvasSwitcher : MonoBehaviour
{
    public Canvas[] allCanvases;

    void Start()
    {
        // Findet nur AKTIVE Canvas
        allCanvases = FindObjectsOfType<Canvas>(true);  // Parameter 'true' = auch inaktive!
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
            bool shouldBeActive = (canvas.gameObject.name == canvasName);
            canvas.gameObject.SetActive(shouldBeActive);
            
            Debug.Log(canvas.gameObject.name + " ist jetzt: " + (shouldBeActive ? "aktiv" : "inaktiv"));
        }
    }
}
