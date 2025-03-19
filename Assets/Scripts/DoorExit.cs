using UnityEngine;

// Class to manage the exit door
// This class is responsible for opening the exit door when the player hacks the computer
public class DoorExit : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool doorOpened = false;
    public GameObject DoorClosedText;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Function to open the exit door
    public void OpenExitDoor()
    {
        spriteRenderer.enabled = false; 
        doorOpened = true;
    }

    // Function to detect if the player collides with the exit door
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (doorOpened)
            {
                // When player wins, stop the background music and change the game state to Win
                if (GameManager.instance.currentState == GameManager.GameState.Playing)
                {
                    BackgroundMusic.instance.StopMusic();
                    GameManager.instance.SetGameState(GameManager.GameState.Win);
                }
                // When player wins in the tutorial, stop the background music and change the game state to MainMenu
                else if (GameManager.instance.currentState == GameManager.GameState.Tutorial)
                {
                    BackgroundMusic.instance.StopMusic();
                    GameManager.instance.SetGameState(GameManager.GameState.MainMenu);
                }
            }
            else{
                // If the door is closed, show a warning message
                DoorClosedText.SetActive(true);
            }
        }
    }

    // Function to detect if the player exits the exit door
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !doorOpened)
        {
            DoorClosedText.SetActive(false);
        }
    }
}
