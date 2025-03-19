using UnityEngine;

// Class responsible for the stairs object
// It moves the player up or down when the player is nearby
public class Stairs : MonoBehaviour
{
    [SerializeField] private bool goesUp = true;
    public AudioClip stairsSound;

    private bool playerNearby = false;
    private Transform playerTransform;
    public bool isBlocked = false;
    [SerializeField] private GameObject blockage;

    // Function to detect if the player is close enough to interact with the stairs
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerNearby = true;
            playerTransform = collider.transform;
        }
    }

    // Function to detect if the player exits the interaction range with the stairs
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerNearby = false;
            playerTransform = null;
        }
    }

    private void Update()
    {
        if (GameManager.instance.currentState == GameManager.GameState.Paused) return;

        // If the player is nearby and the door isn't locked, move the player up or down when pressing the up or down arrow key
        if (playerNearby && playerTransform != null && !isBlocked)
        {
            if (goesUp && Input.GetKeyDown(KeyCode.UpArrow))
            {
                AudioSource.PlayClipAtPoint(stairsSound, transform.position);
                playerTransform.position += new Vector3(0, 2, 0);
            }
            else if (!goesUp && Input.GetKeyDown(KeyCode.DownArrow))
            {
                AudioSource.PlayClipAtPoint(stairsSound, transform.position);
                playerTransform.position -= new Vector3(0, 2, 0);
            }
        }
        if (isBlocked)
        {
            blockage.SetActive(true);
        }
        else{
            blockage.SetActive(false);
        }
    }

    public void BlockStairs(bool state)
    {
        isBlocked = state;
    }
}
