using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField] private bool goesUp = true;
    public bool GoesUp => goesUp; // Getter
    public AudioClip stairsSound;

    private bool playerNearby = false;
    private Transform playerTransform;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            playerNearby = true;
            playerTransform = collider.transform;
        }
    }

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
        if (playerNearby && playerTransform != null)
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
    }
}
