using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Required for scene management

// Class responsible for the countdown timer on the screen
public class TimerText : MonoBehaviour
{
    public static TimerText instance;
    public Text text;
    public int minutes = 0; // Set the countdown start time
    public int seconds = 10;
    private float gameTimer;
    private bool isTimerRunning = true;

    public int minutesLeft = 0;
    public int secondsLeft = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        text = GetComponent<Text>();
        gameTimer = minutes * 60 + seconds;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            gameTimer -= Time.deltaTime;
            gameTimer = Mathf.Max(gameTimer, 0); // Prevent timer from going below 0

            int minutes = Mathf.FloorToInt(gameTimer / 60);
            int seconds = Mathf.FloorToInt(gameTimer % 60);

            text.text = $"{minutes:D2}:{seconds:D2}"; // Display time in MM:SS format

            if (gameTimer <= 0)
            {
                gameTimer = 0;
                isTimerRunning = false;
                TriggerGameOver();
            }

            minutesLeft = minutes;
            secondsLeft = seconds;
        }
    }

    // Function to stop the game when the timer ends
    private void TriggerGameOver()
    {
        Debug.Log("Game Over - Timer Ended");
        if (GameManager.instance.currentState == GameManager.GameState.Playing)
        {
            GameManager.instance.SetGameState(GameManager.GameState.GameOver);
        }
    }
}