using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Class responsible for the Instructions screen
public class InstructionsMenu : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(GoBack);
    }

    public void GoBack()
    {
        GameManager.instance.GoBackToMenu();
    }
}
