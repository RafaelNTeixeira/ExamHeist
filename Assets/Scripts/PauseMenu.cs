using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Class responsible for the Pause Menu screen
public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;
    public Button quitButton;

    private Button[] buttons;
    private int selectedIndex = 0;

    private void Start()
    {
        buttons = new Button[] { resumeButton, restartButton, mainMenuButton, quitButton };
        UpdateSelection();

        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            UpdateSelection();
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            switch (selectedIndex)
            {
                case 0:
                    resumeButton.onClick.AddListener(ResumeGame);
                    break;
                case 1:
                    restartButton.onClick.AddListener(RestartGame);
                    break;
                case 2:
                    mainMenuButton.onClick.AddListener(MainMenu);
                    break;
                case 3:
                    quitButton.onClick.AddListener(QuitGame);
                    break;
            }
        }
    }

    private void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
    }

    public void RestartGame()
    {
        GameManager.instance.PlayAgain();
    }

    public void MainMenu()
    {
        GameManager.instance.SetGameState(GameManager.GameState.MainMenu);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
