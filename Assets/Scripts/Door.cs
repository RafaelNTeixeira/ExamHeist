using UnityEngine;

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
        if (playerNearby && KeyText.keyCount > 0 && Input.GetKeyUp(KeyCode.O))
        {
            OpenDoor();

            AudioSource.PlayClipAtPoint(doorSound, transform.position);

            KeyText.keyCount -= 1;
        }
    }

    public void OpenDoor()
    {
        spriteRenderer.enabled = false; 
        doorCollider.enabled = false;  
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

}
