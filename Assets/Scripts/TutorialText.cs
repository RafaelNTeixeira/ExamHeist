using UnityEngine;

// Class responsible for the tutorial text
public class TutorialText : MonoBehaviour
{
    public GameObject uiText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the player enters the trigger display the tutorial text
        if (other.CompareTag("Player") && uiText != null && !uiText.activeSelf)
        {
            uiText.SetActive(true);
        }
    }
}
