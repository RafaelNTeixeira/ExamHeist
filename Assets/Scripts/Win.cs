using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public Button playAgainButton;
    public Button quitButton;

    private void Start()
    {
        playAgainButton.onClick.AddListener(PlayAgain);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void PlayAgain()
    {
        GameManager.instance.PlayAgain();
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
