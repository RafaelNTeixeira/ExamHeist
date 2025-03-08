using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;

public class Computer : MonoBehaviour
{
    private bool playerNearby = false;
    public AudioClip hackSound;
    public AudioClip alarmSound;
    
    public GameObject hackingUIPanel; // Assign in Inspector
    public Text sequenceText; // Assign in Inspector
    public Text inputText; // Assign in Inspector
    public Text timerText; // Assign in Inspector
    public int sequenceLength = 4; // Number of letters to input
    public float hackTimeLimit = 30f; // Seconds to complete the hack

    private string correctSequence;
    private string playerInput = "";
    private float remainingTime;
    private bool isHacking = false;

    [SerializeField] private DoorExit doorExit;

    private void Update()
    {
        if (playerNearby && USBPenText.penCount > 0 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            StartHacking();
        }

        if (isHacking)
        {
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
                player.canMove = false;
            }
        }

        AudioSource.PlayClipAtPoint(hackSound, transform.position);

        correctSequence = GenerateRandomSequence(sequenceLength);
        sequenceText.text = "Enter: " + correctSequence;
        playerInput = "";
        remainingTime = hackTimeLimit;
        isHacking = true;
        hackingUIPanel.SetActive(true);

        StartCoroutine(HackingCountdown());
    }

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

    private void SuccessHack()
    {
        isHacking = false;
        hackingUIPanel.SetActive(false);
        Debug.Log("Access Granted!");
        doorExit.OpenExitDoor();
        EnablePlayerMovement();
    }

    private void FailHack()
    {
        isHacking = false;
        hackingUIPanel.SetActive(false);
        Debug.Log("Alarm Triggered!");

        EnablePlayerMovement();

        SecurityCamera[] cameras = Object.FindObjectsByType<SecurityCamera>(FindObjectsSortMode.None);
        foreach (SecurityCamera camera in cameras)
        {
            camera.ActivateAlarmLights(true);
            camera.alarmActive = true;
        }

        AlarmManager.instance.RequestAlarm();
    }

    private string GenerateRandomSequence(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger zone");
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left trigger zone");
            playerNearby = false;
        }
    }

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
