using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
    }

    public void RestartGame()
    {
        GameManager.instance.PlayAgain();
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
