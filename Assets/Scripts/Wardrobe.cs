using System.ComponentModel.Design.Serialization;
using UnityEngine;

// Class responsible for the wardrobe object
// It allows the player to hide inside it
public class Wardrobe : MonoBehaviour
{
    private bool isPlayerInside = false;  
    private bool isHiding = false;        
    private GameObject player;
    public AudioClip wardrobeSound;

    // Check if the player enters the wardrobe trigger area 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = true;
            player = collision.gameObject; 
        }
    }

    // Check if the player exits the wardrobe trigger area 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = false;
        }
    }

    private void Update()
    {
        // If the player is near the wardrobe and presses the up/down arrow keys, hide/unhide the player
        if (Input.GetKeyDown(KeyCode.UpArrow) && isPlayerInside || 
            Input.GetKeyDown(KeyCode.DownArrow) && isHiding)
        {
            ToggleHide();
        }

    }

    // Function to hide/unhide the player
    private void ToggleHide()
    {
        if (player == null) return;

        isHiding = !isHiding;
        ApplyHideState(isHiding);

        AudioSource.PlayClipAtPoint(wardrobeSound, transform.position);
    }

    // Function to apply the hide state to the player
    private void ApplyHideState(bool hide)
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        BoxCollider2D boxCollider = player.GetComponent<BoxCollider2D>();
        Rigidbody2D body = player.GetComponent<Rigidbody2D>();
        Player playerScript = player.GetComponent<Player>();

        spriteRenderer.color = new Color(1, 1, 1, hide ? 0.6f : 1f);
        boxCollider.enabled = !hide;
        body.bodyType = hide ? RigidbodyType2D.Static : RigidbodyType2D.Dynamic;
        playerScript.enabled = !hide;

       playerScript.animator.SetBool("isRunning", false); // Stop the running animation of the player when hiding
    }
}
