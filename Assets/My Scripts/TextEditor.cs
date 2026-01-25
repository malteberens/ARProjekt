using UnityEngine;
using TMPro;

public class TextEditor : MonoBehaviour
{
    public TMP_Text textField;
    
    void Start()
    {
        if (textField != null)
        {
            textField.gameObject.SetActive(true);  // Erst aktivieren
            textField.text = "0";          // Dann bearbeiten
        }
    }
}
