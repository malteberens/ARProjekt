using UnityEngine;
using System.Collections;

public class CanvasAutoSwitch : MonoBehaviour
{
    public GameObject canvasToHide;     // Das Canvas, das ausgeblendet werden soll
    public GameObject canvasToShow;     // Das Canvas, das eingeblendet werden soll
    public float delayInSeconds = 3f;   // Wartezeit in Sekunden

    // Diese Methode wird vom Button aufgerufen
    public void StartCanvasSwitch()
    {
        StartCoroutine(SwitchCanvasAfterDelay());
    }

    IEnumerator SwitchCanvasAfterDelay()
    {
        Debug.Log("Warte " + delayInSeconds + " Sekunden...");
        
        yield return new WaitForSeconds(delayInSeconds);
        
        if (canvasToHide != null)
        {
            Debug.Log("Blende Canvas aus: " + canvasToHide.name);
            canvasToHide.SetActive(false);
        }
        
        if (canvasToShow != null)
        {
            Debug.Log("Blende Canvas ein: " + canvasToShow.name);
            canvasToShow.SetActive(true);
        }
    }
}
