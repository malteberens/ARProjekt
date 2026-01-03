using UnityEngine;

public class DisableCanvasOnStart : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
}
