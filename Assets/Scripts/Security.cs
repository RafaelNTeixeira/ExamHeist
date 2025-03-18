using UnityEngine;
using UnityEngine.Rendering.Universal;

// Class responsible for the security guard
// It manages the detection of the player and the security light
public class Security : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Cone Raycast Settings")]
    private readonly float fovAngle = 45f;
    private readonly float detectedDistance = 1f;
    private readonly float viewDistance = 5f;
    private readonly int rayCount = 15;
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Audio Settings")]
    public AudioClip hitSound;
    private readonly float soundCooldown = 2f;
    private float nextSoundTime = 0f;
    
    [Header("Tutorial UI Settings")]
    public GameObject uiText;
    public GameObject uiTextDelete;

    private Animator anim;
    private SecurityPatrol securityPatrol;
    private Light2D securityLight;
    private bool playerDetected = false;
    private bool securityTouchPlayer = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        securityPatrol = GetComponentInParent<SecurityPatrol>();
        securityLight = GetComponentInChildren<Light2D>();
    }
    
    private void Update()
    {
        if (PlayerDetected() || securityTouchPlayer)
        {
            anim.SetTrigger("catch");

            if (Time.time >= nextSoundTime)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                nextSoundTime = Time.time + soundCooldown;
            }

            // If the player is detected, the game is over
            if (GameManager.instance.currentState == GameManager.GameState.Playing)
            {
                GameManager.instance.SetGameState(GameManager.GameState.GameOver);
            }

            // If the player is detected in the tutorial, reset the player position
            if (GameManager.instance.currentState == GameManager.GameState.Tutorial)
            {
                uiTextDelete.SetActive(false);
                uiText.SetActive(true);
                GameObject player = GameObject.Find("Player");
                player.transform.position = new Vector2(7.7f, player.transform.position.y);
            }
        }

        if (securityPatrol != null)
        {
            securityPatrol.enabled = !PlayerDetected();

            // If the player is detected, speed up the security guard
            if (!playerDetected && PlayerInSight())
            {
                print("Player in sight");
                securityPatrol.SpeedUp(2.2f);
                playerDetected = true;
            }
            // If the player is not detected, slow down the security guard
            else if (securityPatrol.IsAtEdge() && playerDetected && !AlarmManager.instance.isAlarmActive)
            {
                securityPatrol.SpeedDown(2.2f);
                playerDetected = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            securityTouchPlayer = true;
        }
    }

    // Function to detect if the player is detected
    private bool PlayerDetected()
    {
        return PerformRaycast(detectedDistance);
    }
    
    // Function to detect if the player is in sight
    private bool PlayerInSight()
    {
        return PerformRaycast(viewDistance);
    }

    // Function to perform a raycast to detect the player
    private bool PerformRaycast(float distance)
    {
        if (securityPatrol != null && securityPatrol.IsAtEdge())
        {
            securityLight.enabled = false;
            return false;
        }

        securityLight.enabled = true;
        float securityHeight = boxCollider.bounds.size.y;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + securityHeight / 2);
        Vector2 forward = transform.right * Mathf.Sign(transform.localScale.x);
        float halfFOV = fovAngle / 2f;
        float angleStep = fovAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * forward;

            // Check if the ray hits any obstacles
            RaycastHit2D hitObstacle = Physics2D.Raycast(origin, rayDirection, distance, obstacleLayer);
            if (hitObstacle.collider != null)
            {
                Debug.DrawRay(origin, rayDirection * distance, Color.red); // Obstacle blocks vision
                continue; // Skip player detection if an obstacle is in the way
            }

            // Check if the ray hits the player
            RaycastHit2D hitPlayer = Physics2D.Raycast(origin, rayDirection, distance, playerLayer);
            if (hitPlayer.collider != null)
            {
                Debug.DrawRay(origin, rayDirection * distance, Color.green); // Player detected
                return true;
            }
            else
            {
                Debug.DrawRay(origin, rayDirection * distance, Color.yellow); // Empty space
            }
        }
        return false;
    }

    // Function to draw the field of view in the Scene view
    private void OnDrawGizmos()
    {
        if (securityPatrol != null && securityPatrol.IsAtEdge())
            return;

        float securityHeight = boxCollider.bounds.size.y;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + securityHeight / 2);
        Vector2 forward = transform.right * Mathf.Sign(transform.localScale.x);
        float halfFOV = fovAngle / 2f;

        int rayCountCyan = rayCount;  // More cyan rays
        int rayCountRed = Mathf.Max(1, rayCount / 2);  // Fewer red rays
        float angleStepCyan = fovAngle / (rayCountCyan - 1);
        float angleStepRed = fovAngle / (rayCountRed - 1);

        // Draw cyan rays (full count)
        Gizmos.color = Color.cyan;
        for (int i = 0; i < rayCountCyan; i++)
        {
            float currentAngle = -halfFOV + angleStepCyan * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * forward;
            Gizmos.DrawLine(origin, origin + rayDirection * detectedDistance);
        }

        // Draw red rays (half count)
        Gizmos.color = Color.red;
        for (int i = 0; i < rayCountRed; i++)
        {
            float currentAngle = -halfFOV + angleStepRed * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * forward;
            Gizmos.DrawLine(origin, origin + rayDirection * viewDistance);
        }
    }

}
