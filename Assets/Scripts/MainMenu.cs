using UnityEngine;
using UnityEngine.UI;

// Class responsible for the Main Menu screen
public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button tutorialButton;
    public Button instructionsButton;
    public Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        tutorialButton.onClick.AddListener(Tutorial);
        instructionsButton.onClick.AddListener(InstructionsScreen);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Playing);
    }

    public void Tutorial()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Tutorial);
    }

    public void InstructionsScreen()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Instructions);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
