using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Class responsible for the Game Over screen
public class GameOver : MonoBehaviour
{
    public Button playAgainButton;
    public Button mainMenuButton;
    public Button quitButton;

    private Button[] buttons;
    private int selectedIndex = 0;
    public AudioClip menuOptionSwitchSound; // Sound to play when switching between menu options

    private void Start()
    {
        buttons = new Button[] { playAgainButton, mainMenuButton, quitButton };
        UpdateSelection();

        playAgainButton.onClick.AddListener(PlayAgain);
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

    // Function to update the selected button
    private void UpdateSelection()
    {
        EventSystem.current.SetSelectedGameObject(buttons[selectedIndex].gameObject);
        AudioSource.PlayClipAtPoint(menuOptionSwitchSound, transform.position);
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
