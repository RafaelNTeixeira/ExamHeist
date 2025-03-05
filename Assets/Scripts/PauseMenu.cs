using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ResumeGame()
    {
        Debug.Log("Clicked on RESUME");
        GameManager.instance.ResumeGame();
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
