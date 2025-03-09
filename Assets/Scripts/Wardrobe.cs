using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    private bool isPlayerInside = false;  
    private bool isHiding = false;        
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = true;
            player = collision.gameObject; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            isPlayerInside = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && isPlayerInside || 
            Input.GetKeyDown(KeyCode.DownArrow) && isHiding)
        {
            ToggleHide();
        }

    }

     private void ToggleHide()
    {
        if (player == null) return;

        isHiding = !isHiding;
        ApplyHideState(isHiding);
    }

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
    }
}
