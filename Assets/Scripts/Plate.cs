using UnityEngine;
using UnityEngine.UI;

public class Plate : MonoBehaviour
{
    public GameObject uiText;  // Assign the UI Text GameObject in the Inspector
    public Text plateMessage;  // Assign a UI Text element (instead of a string)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && uiText != null && plateMessage != null)
        {
            uiText.SetActive(true); // Show the text UI
            uiText.GetComponent<Text>().text = plateMessage.text; // Set the text
        }
    }
}
