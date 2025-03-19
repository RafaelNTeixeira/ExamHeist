using UnityEngine;
using UnityEngine.UI;

// Class responsible for spliting the password in half for the papers
public class Paper : MonoBehaviour
{
    private string password;

    [SerializeField] private bool isFirstHalf;
    [SerializeField] private GameObject paperUIPanel;
    [SerializeField] private Text passwordText;

    private void Awake()
    {
        HalfPassword(); // Set half of the password
        passwordText.text = password;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            paperUIPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            paperUIPanel.SetActive(false);
        }
    }

    // Function to split the password in half
    private void HalfPassword()
    {
        // first half
        if (isFirstHalf)
        {
            password = Computer.correctSequence[..(Computer.sequenceLength / 2)] + "__";
        }
        // second half
        else
        {
            password = "__" + Computer.correctSequence[(Computer.sequenceLength / 2)..];
        }
    }
}
