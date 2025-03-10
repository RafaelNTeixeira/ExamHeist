using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button instructionsButton;
    public Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        instructionsButton.onClick.AddListener(InstructionsScreen);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        GameManager.instance.SetGameState(GameManager.GameState.Toturial);
    }

    public void InstructionsScreen()
    {
        Debug.Log("Clicked on Instructions");
        GameManager.instance.SetGameState(GameManager.GameState.Instructions);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
