using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public enum GameState { MainMenu, Instructions, Tutorial, Playing, Paused, GameOver, Win}
    public GameState currentState;
    private SecurityPatrol[] securityPatrols;
    private bool cutscenePlayed = false;
    public bool canPauseGame = false;
    private bool tutorialPlayed = false;


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
            case GameState.Instructions:
                ShowInstructionsMenu();
                break;
            case GameState.Tutorial:
                StartTutorial();
                break;
            case GameState.Paused:
                PauseGame();
                break;
            case GameState.GameOver:
                ShowGameOverScreen();
                break;
            case GameState.Win:
                ShowWinScreen();
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

        if (tutorialPlayed)
        {
            GameObject room = GameObject.Find("Room");
            if (room != null)
            {
                Destroy(room); // Destroy the Room from the tutorial scene
            }
        }

        if (!cutscenePlayed)
        {
            SceneManager.LoadScene("CutScene");
        }
        else 
        {
            SceneManager.LoadScene("GameScene");
        }

        cutscenePlayed = true;
    }

    void ShowInstructionsMenu()
    {
        Debug.Log("Loaded Instructions");
        SceneManager.LoadSceneAsync("Instructions", LoadSceneMode.Additive); // Load instructions menu without unloading the game
    }

    void StartTutorial()
    {
        Debug.Log("Loaded Tutorial");
        tutorialPlayed = true;
        SceneManager.LoadScene("Tutorial");
    }

    public void GoBackToMenu()
    {
        Debug.Log("Game Resumed");
        SceneManager.UnloadSceneAsync("Instructions"); // Only remove the instructions menu
        currentState = GameState.MainMenu;
    }

    void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0; // Freeze game
        AlarmManager.instance.StopAlarm();
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive); // Load pause menu without unloading the game
    }

    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        Time.timeScale = 1; // Unfreeze game
        SceneManager.UnloadSceneAsync("PauseMenu"); // Only remove the pause menu
        AlarmManager.instance.ResumeAlarm();
        currentState = GameState.Playing; // Can't call SetGameState() otherwise it will load the scene from scratch because of the state machine
    }

    public void PlayAgain()
    {
        Debug.Log("Game Restarting");
        Time.timeScale = 1;
        ResetGameState();
        
        SetGameState(GameState.Playing);
    }

    public void ResetGameState() 
    {
        AlarmManager.instance.ResetAlarmState();
        BackgroundMusic.instance.PlayMusic();

        foreach (var camera in Object.FindObjectsByType<SecurityCamera>(FindObjectsSortMode.None))
        {
            camera.ResetCameraState();
        }

        USBPenText.penCount = 0;
        KeyText.keyCount = 0;
    }


    void ShowGameOverScreen()
    {
        Debug.Log("Game Over");
        AlarmManager.instance.StopAllSounds();
        //Time.timeScale = 0;
        SceneManager.LoadScene("GameOver");
    }

    void ShowWinScreen()
    {
        Debug.Log("Win");
        AlarmManager.instance.StopAllSounds();
        //Time.timeScale = 0;
        SceneManager.LoadScene("Win");
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
            if (currentState == GameState.Tutorial) {
                SetGameState(GameState.MainMenu);
            }
            if (currentState == GameState.Playing && canPauseGame)
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
