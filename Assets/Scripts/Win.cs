using UnityEngine;
using UnityEngine.UI;

// Class responsible for the Win screen
public class Win : MonoBehaviour
{
    public Text timerText;
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button quitButton;

    private void Start()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(QuitGame);

        int minutes = GameManager.instance.minutesTaken;
        int seconds = GameManager.instance.secondsTaken;

        timerText.text = $"You completed the level in: {minutes:D2}:{seconds:D2}"; // Display the time taken to complete the level
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
