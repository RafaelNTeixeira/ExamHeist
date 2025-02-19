using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 8.0f;
    private Rigidbody2D body;
    private Animator animator;

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
        if (horizontalDirection > 0.01f) transform.localScale = new Vector3(5, 5, 5); 
        else if (horizontalDirection < -0.01f) transform.localScale = new Vector3(-5, 5, 5);

        animator.SetBool("isRunning", horizontalDirection != 0); // Is running when a movement key is being pressed (left or right)
    }
}
