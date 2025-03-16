using UnityEngine;
using UnityEngine.UI;

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

    private void HalfPassword()
    {
        if (isFirstHalf)
        {
            password = Computer.correctSequence[..(Computer.sequenceLength / 2)] + "__";
        }
        else
        {
            password = "__" + Computer.correctSequence[(Computer.sequenceLength / 2)..];
        }
    }
}
