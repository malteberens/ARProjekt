using UnityEngine;
using TMPro;

public class TextEditor : MonoBehaviour
{
    public TMP_Text textField;  // Kann auch deaktiviertes GameObject referenzieren
    
    void Start()
    {
        // Funktioniert auch, wenn textField deaktiviert ist
        if (textField != null)
        {
            textField.gameObject.SetActive(true);  // Erst aktivieren
            textField.text = "0";          // Dann bearbeiten
        }
    }
}
