using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

// Class to manage the computer hacking system
// This class is responsible for managing the hacking minigame
public class Computer : MonoBehaviour
{
    private bool playerNearby = false;
    public AudioClip hackSound;
    public AudioClip alarmSound;
    public GameObject NoUBSUIText;
    
    public GameObject hackingUIPanel; // Assign in Inspector
    public Text sequenceText; // Assign in Inspector
    public Text inputText; // Assign in Inspector
    public Text timerText; // Assign in Inspector
    public static int sequenceLength = 4; // Number of letters to input
    public float hackTimeLimit = 30f; // Seconds to complete the hack

    public static string correctSequence;
    private string playerInput = "";
    private float remainingTime;
    private bool isHacking = false;
    private bool gotAccess = false; // Check if player already guessed the password

    [SerializeField] private DoorExit doorExit;

    private void Awake()
    {
        inputText.text = "-> ";
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.Paused) return;
        
        if (playerNearby && USBPenText.penCount > 0 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            // If player is nearby, has the USB pen and the alarm is not activated, start hacking
            if (!gotAccess && !AlarmManager.instance.isAlarmActive && !isHacking)
            {
                StartHacking();
            }
        }
        
        else if (playerNearby && USBPenText.penCount < 1 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            NoUBSUIText.SetActive(true);
        }

        if (isHacking)
        {
            // Update the player input in the canvas and check if it's correct
            foreach (char c in Input.inputString)
            {
                if (char.IsLetter(c))
                {
                    playerInput += char.ToUpper(c);
                    inputText.text = "-> " + playerInput;
                    CheckInput();
                }
            }
        }
    }

    // Function to start the hacking minigame
    private void StartHacking()
    {
        Debug.Log("Started Hacking");
        playerInput = "";
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player player = playerObj.GetComponent<Player>();
            if (player != null)
            {
                player.canMove = false; // Disable player movement while hacking
            }
        }

        AudioSource.PlayClipAtPoint(hackSound, transform.position);

        // correctSequence = GenerateRandomSequence(sequenceLength); // Generate a random sequence of letters
        sequenceText.text = "Enter PassWord";
        playerInput = "";
        remainingTime = hackTimeLimit;
        isHacking = true;
        hackingUIPanel.SetActive(true); // Show the hacking UI panel

        StartCoroutine(HackingCountdown()); // Start the hacking countdown timer
    }

    // Coroutine to countdown the hacking time
    private IEnumerator HackingCountdown()
    {
        while (remainingTime > 0)
        {
            if (!isHacking) yield break;

            timerText.text = "Time: " + remainingTime.ToString("F1");
            yield return new WaitForSeconds(0.1f);
            remainingTime -= 0.1f;
        }

        if (isHacking)
        {
            FailHack();
        }
    }

    // Function to check the player input
    private void CheckInput()
    {
        if (playerInput == correctSequence)
        {
            SuccessHack();
        }
        else if (playerInput.Length >= correctSequence.Length)
        {
            FailHack(); // If incorrect input is complete, trigger alarm
        }
    }

    // Function to handle a successful hack
    private void SuccessHack()
    {
        isHacking = false;
        gotAccess = true;
        USBPenText.penCount--;
        hackingUIPanel.SetActive(false);
        doorExit.OpenExitDoor();
        EnablePlayerMovement();
    }

    // Function to handle a failed hack
    private void FailHack()
    {
        isHacking = false;
        hackingUIPanel.SetActive(false);

        inputText.text = "-> "; // Reset the displayed input text
        playerInput = ""; // Clear the stored player input

        EnablePlayerMovement();

        SecurityCamera[] cameras = Object.FindObjectsByType<SecurityCamera>(FindObjectsSortMode.None);
        foreach (SecurityCamera camera in cameras)
        {
            camera.ActivateAlarmLights(true);
            camera.alarmActive = true;
        }

        AlarmManager.instance.RequestAlarm();
    }

    // Function to generate a random sequence of letters
    public static string GenerateRandomSequence(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }

    // Function to handle player entering the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    // Function to handle player leaving the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            NoUBSUIText.SetActive(false);
        }
    }

    // Function to enable player movement
    private void EnablePlayerMovement()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            Player player = playerObj.GetComponent<Player>();
            if (player != null)
            {
                player.canMove = true; // Enable movement
            }
        }
    }
}
