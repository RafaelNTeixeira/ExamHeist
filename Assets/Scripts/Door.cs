using UnityEngine;

// Class to manage the door
// This class is responsible for opening the door when the player has a key
public class Door : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;
    private bool playerNearby = false;
    public AudioClip doorSound;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        // If the player is nearby, has a key and presses the up arrow key, open the door
        if (playerNearby && KeyText.keyCount > 0 && Input.GetKeyUp(KeyCode.UpArrow))
        {
            OpenDoor();

            AudioSource.PlayClipAtPoint(doorSound, transform.position);

            KeyText.keyCount -= 1;
        }
    }

    // Function to open the door
    public void OpenDoor()
    {
        spriteRenderer.enabled = false; 
        doorCollider.enabled = false;  
    }

    // Function to detect if the player is close enough to interact with the door
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    // Function to detect if the player exits the interaction range with the door
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

}
