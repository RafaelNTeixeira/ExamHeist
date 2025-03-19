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
    public AudioClip menuOptionSwitchSound; // Sound to play when switching between menu options
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false; // Don't play the audio on awake
        audioSource.ignoreListenerPause = true; // Don't pause the audio when the game is frozen

        buttons = new Button[] { resumeButton, restartButton, mainMenuButton, quitButton };
        UpdateSelection();

        resumeButton.onClick.AddListener(ResumeGame);
        restartButton.onClick.AddListener(RestartGame);
        mainMenuButton.onClick.AddListener(MainMenu);
        quitButton.onClick.AddListener(QuitGame);
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

    // Function to update the selected button
    private void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
        if (menuOptionSwitchSound != null)
        {
            audioSource.PlayOneShot(menuOptionSwitchSound);
        }
    }

    // Function to resume the game
    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
    }

    // Function to restart the game
    public void RestartGame()
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
