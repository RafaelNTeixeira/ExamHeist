using UnityEngine;

using UnityEngine.SceneManagement;

// Class to manage the game state
// This class is a singleton to allow managing the game state across different scenes
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public enum GameState { MainMenu, Instructions, Tutorial, Playing, Paused, GameOver, Win}
    public GameState currentState;
    private SecurityPatrol[] securityPatrols;
    private bool cutscenePlayed = false;
    public bool canPauseGame = false;
    public int minutesTaken = 0;
    public int secondsTaken = 0;

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

    // Function to set the game state
    public void SetGameState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Current State: " + currentState);

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

    // Function to show the main menu
    void ShowMainMenu()
    {
        Debug.Log("Loaded Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

    // Function to start the game
    void StartGame()
    {
        Debug.Log("Loaded Start Game");
        Time.timeScale = 1; // Ensure normal game speed

        // Rebuild music object to restart music due to game state changes
        if (BackgroundMusic.instance != null)
        {
            BackgroundMusic.instance.DestroyMusic();
        }

        GameObject room = GameObject.Find("Room");
        if (room != null)
        {
            Destroy(room); // Rebuild the room to not overlap with the tutorial one
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

    // Function to show the instructions menu
    void ShowInstructionsMenu()
    {
        Debug.Log("Loaded Instructions");
        SceneManager.LoadScene("Instructions"); // Load instructions menu without unloading the game
    }

    // Function to start the tutorial
    void StartTutorial()
    {
        Debug.Log("Loaded Tutorial");
        Time.timeScale = 1;

        // Rebuild music object to restart music due to game state changes
        if (BackgroundMusic.instance != null)
        {
            BackgroundMusic.instance.DestroyMusic();
        }

        GameObject room = GameObject.Find("Room");
        if (room != null)
        {
            Destroy(room); // Rebuild the room to not overlap with the game scene one
        }

        SceneManager.LoadScene("Tutorial");
    }

    // Function to go back to the main menu from the instructions menu
    public void GoBackToMenu()
    {
        Debug.Log("Game Resumed");
        SceneManager.LoadScene("MainMenu"); // Only remove the instructions menu
    }

    // Function to pause the game
    void PauseGame()
    {
        Debug.Log("Game Paused");
        Time.timeScale = 0; // Freeze game
        AlarmManager.instance.StopAlarm();
        SceneManager.LoadSceneAsync("PauseMenu", LoadSceneMode.Additive); // Load pause menu without unloading the game
    }

    // Function to resume the game
    public void ResumeGame()
    {
        Debug.Log("Game Resumed");
        BackgroundMusic.instance.PlayMusic();
        Time.timeScale = 1; // Unfreeze game
        SceneManager.UnloadSceneAsync("PauseMenu"); // Only remove the pause menu
        AlarmManager.instance.ResumeAlarm(); // Resume alarm sound if it was active before pausing
        currentState = GameState.Playing; // Can't call SetGameState() otherwise it will load the scene from scratch because of the state machine
    }

    // Function to restart the game
    public void PlayAgain()
    {
        Debug.Log("Game Restarting");
        Time.timeScale = 1;
        ResetGameState();
        SetGameState(GameState.Playing);
    }

    // Function to reset the game state
    public void ResetGameState() 
    {
        AlarmManager.instance.ResetAlarmState();
        // BackgroundMusic.instance.PlayMusic();

        foreach (var camera in Object.FindObjectsByType<SecurityCamera>(FindObjectsSortMode.None))
        {
            camera.ResetCameraState();
        }

        USBPenText.penCount = 0;
        KeyText.keyCount = 0;
    }

    // Function to show the game over screen
    void ShowGameOverScreen()
    {
        Debug.Log("Game Over");
        AlarmManager.instance.StopAllSounds();

        if (BackgroundMusic.instance != null)
        {
            BackgroundMusic.instance.DestroyMusic();
        }

        SceneManager.LoadScene("GameOver");
    }

    // Function to show the win screen
    void ShowWinScreen()
    {
        Debug.Log("Win");

        TimerText timer = Object.FindFirstObjectByType<TimerText>();
        if (timer != null)
        {
            minutesTaken = timer.minutes;
            secondsTaken = timer.seconds;
        }
        else
        {
            Debug.LogError("TimerText instance not found!");
        }

        AlarmManager.instance.StopAllSounds();
        SceneManager.LoadScene("Win");
    }

    // Function to quit the game
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
                BackgroundMusic.instance.StopMusic();
                SetGameState(GameState.MainMenu);
            }
            else if (currentState == GameState.Instructions){
                SetGameState(GameState.MainMenu);
            }
            else if (currentState == GameState.Playing && canPauseGame)
            {
                BackgroundMusic.instance.StopMusic();
                SetGameState(GameState.Paused);
            }
            else if (currentState == GameState.Paused)
            {
                BackgroundMusic.instance.PlayMusic();
                ResumeGame();
            }
        }
    }
}
