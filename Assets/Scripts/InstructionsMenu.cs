using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InstructionsMenu : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
    }

    public void GoBack()
    {
        Debug.Log("Clicked GO BACK");
        GameManager.instance.GoBackToMenu();
    }
}
