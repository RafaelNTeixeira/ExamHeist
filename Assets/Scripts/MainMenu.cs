using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button tutorialButton;
    public Button instructionsButton;
    public Button quitButton;

    private Button[] buttons;
    private int selectedIndex = 0;
    public AudioClip menuOptionSwitchSound; // Sound to play when switching between menu options

    private void Start()
    {
        buttons = new Button[] { playButton, tutorialButton, instructionsButton, quitButton };
        UpdateSelection();

        playButton.onClick.AddListener(PlayGame);
        tutorialButton.onClick.AddListener(Tutorial);
        instructionsButton.onClick.AddListener(InstructionsScreen);
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
                    playButton.onClick.AddListener(PlayGame);
                    break;
                case 1:
                    tutorialButton.onClick.AddListener(Tutorial);
                    break;
                case 2:
                    instructionsButton.onClick.AddListener(InstructionsScreen);
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

    // Function to play the game
    public void PlayGame()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Playing);
    }

    // Function to go to the tutorial
    public void Tutorial()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Tutorial);
    }

    // Function to go to the instructions screen
    public void InstructionsScreen()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Instructions);
    }

    // Function to quit the game
    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
