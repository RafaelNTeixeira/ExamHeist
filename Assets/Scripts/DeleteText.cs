using UnityEngine;
using UnityEngine.UI;

// Class to delete the text UI
// This class is responsible for deleting the text UI when the player interacts with it
public class DeleteText : MonoBehaviour
{
    public GameObject uiText; // Assign the UI Text GameObject in the Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && uiText != null)
        {
            uiText.SetActive(false); // Hide the text UI
        }
    }
}
