using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public enum GameState { MainMenu, Playing, GameOver }
    public GameState currentState;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SetGameState(GameState.MainMenu);
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.MainMenu:
                ShowMainMenu();
                break;
            case GameState.Playing:
                StartGame();
                break;
            // case GameState.GameOver:
            //     ShowGameOverScreen();
            //     break;
        }
    }

    void ShowMainMenu()
    {
        Debug.Log("Loaded Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    void StartGame()
    {
        Debug.Log("Loaded Start Game");
        SceneManager.LoadScene("GameScene");
    }

    void ShowGameOverScreen()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
