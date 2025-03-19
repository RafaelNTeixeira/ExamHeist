using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading;

// Class responsible for the Win screen
public class Win : MonoBehaviour
{
    public Text timerText;
    public Button playCutsceneButton;
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button quitButton;

    private Button[] buttons;
    private int selectedIndex = 0;
    public AudioClip menuOptionSwitchSound; // Sound to play when switching between menu options
    public AudioClip winSound; // Sound to play when switching between menu options

    private void Start()
    {
        AudioSource.PlayClipAtPoint(winSound, transform.position);
        buttons = new Button[] { playCutsceneButton, playAgainButton, mainMenuButton, quitButton };
        UpdateSelection();

    
        playCutsceneButton.onClick.AddListener(PlayFinalCutscene);
        playAgainButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(QuitGame);

        int minutes = TimerText.instance.minutesLeft; // Get the minutes left from the TimerText script
        int seconds = TimerText.instance.secondsLeft; // Get the seconds left from the TimerText script

        int score = minutes * 100 + seconds; // Calculate the score based on the time taken to complete the level
       
        if (!AlarmManager.instance.wasPlayerDetected)
        {
            score += 1000; // Add bonus points if the player was not detected
            timerText.text = $"You completed the level with {score} points.\nBonus: 1000 points for not being detected by the cameras.";
        }
        else
        {
            timerText.text = $"You completed the level with {score} points.";
        }

    }

    private void Update()
    {
        // Check if the player is pressing the arrow keys to switch between menu options
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
                    playCutsceneButton.onClick.AddListener(PlayFinalCutscene);
                    break;
                case 1:
                    playAgainButton.onClick.AddListener(PlayAgain);
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

    // Function to update the selected button
    private void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
        AudioSource.PlayClipAtPoint(menuOptionSwitchSound, transform.position);
    }

    // Function to play the final cutscene
    public void PlayFinalCutscene()
    {
        GameManager.instance.PlayFinalCutscene();
    }

    // Function to play again
    public void PlayAgain()
    {
        GameManager.instance.PlayAgain();
    }

    // Function to go back to the main menu
    public void MainMenu()
    {
        GameManager.instance.SetGameState(GameManager.GameState.MainMenu);
    }

    // Function to quit the game
    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
