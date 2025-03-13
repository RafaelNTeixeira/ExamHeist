using UnityEngine;

// Class responsible for the security patrol
// This class is responsible for the security patrol behaviour
public class SecurityPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Security")]
    [SerializeField] private Transform security;

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Security Animator")]
    [SerializeField] private Animator anim;

    public bool playerDetected = false;


    private void Awake()
    {
        initScale = security.localScale;
    }
    private void OnDisable()
    {
        anim.SetBool("isRunning", false);
    }

    private void Update()
    {
        // Update security movement
        if (movingLeft)
        {
            if (security.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (security.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }
    
    // Function to check if the security is at the edge of its path
    public bool IsAtEdge()
    {
        return security.position.x <= leftEdge.position.x || security.position.x >= rightEdge.position.x;
    }

    // Function to change the security direction
    private void DirectionChange()
    {
        anim.SetBool("isRunning", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    // Function to move the security in a certain direction
    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("isRunning", true);

        // Make security face direction
        security.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        // Move in that direction
        security.position = new Vector3(security.position.x + Time.deltaTime * _direction * speed,
            security.position.y, security.position.z);
    }

    // Function to detect if the player is detected. If so, adjust the security speed
    public void PlayerDetected(bool detected, float speedMultiplier)
    {
        playerDetected = detected;
        AdjustSpeed(detected ? speedMultiplier : 1 / speedMultiplier);
    }

    // Function to speed up the security speed
    public void SpeedUp(float multiplier)
    {
        AdjustSpeed(multiplier);
    }

    // Function to slow down the security speed
    public void SpeedDown(float divisor)
    {
        AdjustSpeed(1 / divisor);
    }

    // Function to adjust the security speed
    private void AdjustSpeed(float factor)
    {
        speed *= factor;
    }

}
