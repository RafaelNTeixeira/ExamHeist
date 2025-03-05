using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayGame()
    {
        Debug.Log("Clicked on PLAY");
        GameManager.instance.SetGameState(GameManager.GameState.Playing);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
