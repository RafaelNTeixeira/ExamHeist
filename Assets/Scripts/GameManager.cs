using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public enum GameState { MainMenu, Playing, Paused, GameOver }
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
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.GameOver:
                ShowGameOverScreen();
                break;
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
        Time.timeScale = 1; // Ensure normal game speed
        SceneManager.LoadScene("GameScene");
    }

    void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0; // Freeze game
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive); // Load pause menu without unloading the game
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1; // Unfreeze game
        SceneManager.UnloadSceneAsync("PauseMenu"); // Only remove the pause menu
        currentState = GameState.Playing;
    }

    void ShowGameOverScreen()
    {
        Debug.Log("Game Over");
        Time.timeScale = 1; // Reset time in case of pause
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Update()
    {
        // Toggle pause when pressing Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
        }
    }
}
