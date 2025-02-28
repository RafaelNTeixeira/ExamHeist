using UnityEngine;

public class Player : MonoBehaviour
{
    private float speed = 8.0f;
    private Rigidbody2D body;
    private Animator animator;
    private Door nearbyDoor;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalDirection = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalDirection * speed, body.linearVelocityY);

        // Invert the character direction when changing direction
        if (horizontalDirection > 0.01f) transform.localScale = new Vector3(4, 4, 4); 
        else if (horizontalDirection < -0.01f) transform.localScale = new Vector3(-4, 4, 4);

        animator.SetBool("isRunning", horizontalDirection != 0); // Is running when a movement key is being pressed (left or right)

        if (nearbyDoor != null && Input.GetKeyDown(KeyCode.O) && KeyText.keyCount > 0)
        {
            nearbyDoor.OpenDoor();
            KeyText.keyCount -= 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Door"))
        {
            nearbyDoor = collision.collider.GetComponent<Door>(); 
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door"))
        {
            nearbyDoor = null; 
        }
    }
}
