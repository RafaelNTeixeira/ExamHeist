using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    public GameObject uiText;  // Assign the UI Text GameObject in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && uiText != null && !uiText.activeSelf)
        {
            uiText.SetActive(true); // Show the text UI
        }
    }
}
