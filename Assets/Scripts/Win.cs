using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Class responsible for the Win screen
public class Win : MonoBehaviour
{
    public Text timerText;
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button quitButton;

    private Button[] buttons;
    private int selectedIndex = 0;

    private void Start()
    {
        buttons = new Button[] { playAgainButton, mainMenuButton, quitButton };
        UpdateSelection();

        playAgainButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(QuitGame);

        int minutes = GameManager.instance.minutesTaken;
        int seconds = GameManager.instance.secondsTaken;

        timerText.text = $"You completed the level in: {minutes:D2}:{seconds:D2}"; // Display the time taken to complete the level
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
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectedIndex)
            {
                case 0:
                    playAgainButton.onClick.AddListener(PlayAgain);
                    break;
                case 1:
                    mainMenuButton.onClick.AddListener(MainMenu);
                    break;
                case 2:
                    quitButton.onClick.AddListener(QuitGame);
                    break;
            }
        }
    }

    private void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
    }

    public void PlayAgain()
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
