using UnityEngine;
using System.Collections;

// Class responsible for the player mechanics
// It manages the player movement and speed
public class Player : MonoBehaviour
{
    private float speed = 4.0f;
    private Rigidbody2D body;
    public Animator animator;
    public bool canMove = true;
    public bool isBoosted = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!canMove) // Prevent movement when hacking
        {
            body.linearVelocity = Vector2.zero; // Stop the player
            animator.SetBool("isRunning", false);
            return;
        }

        float horizontalDirection = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalDirection * speed, body.linearVelocityY);

        // Invert the character direction when changing direction
        if (horizontalDirection > 0.01f) transform.localScale = new Vector3(4, 4, 4); 
        else if (horizontalDirection < -0.01f) transform.localScale = new Vector3(-4, 4, 4);

        animator.SetBool("isRunning", horizontalDirection != 0); // Is running when a movement key is being pressed (left or right)
    }


    // Speed boost player movement speed during a certain amount of time
    public void BoostSpeed(float boostSpeed)
    {
        ApplySpeed(boostSpeed);
        isBoosted = true;
        StartCoroutine(RestoreSpeed(boostSpeed));
    }

    // Coroutine to restore the player speed after a certain amount of time
    private IEnumerator RestoreSpeed(float boostSpeed)
    {
        yield return new WaitForSeconds(20);
        ApplySpeed(1 / boostSpeed);
        isBoosted = false;
    }

    public void ApplySpeed(float _speed)
    {
        speed *= _speed;
    }

}
