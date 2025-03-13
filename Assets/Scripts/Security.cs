using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Security : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    
    [Header("Cone Raycast Settings")]
    private readonly float fovAngle = 45f;
    private readonly float viewDistance = 3f;
    private readonly float detectedDistance = 5f;
    private readonly int rayCount = 10;
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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        securityPatrol = GetComponentInParent<SecurityPatrol>();
        securityLight = GetComponentInChildren<Light2D>();
    }
    
    private void Update()
    {
        if (PlayerInSight())
        {
            anim.SetTrigger("catch");

            if (Time.time >= nextSoundTime)
            {
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                nextSoundTime = Time.time + soundCooldown;
            }

            if (GameManager.instance.currentState == GameManager.GameState.Playing)
            {
                GameManager.instance.SetGameState(GameManager.GameState.GameOver);
            }

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
            securityPatrol.enabled = !PlayerInSight();


            if (playerDetected != PlayerDetected() && !playerDetected)
            {
                securityPatrol.SpeedUp(2f);
                playerDetected = true;
            }
            if (securityPatrol.IsAtEdge() && playerDetected)
            {
                securityPatrol.SpeedDown(2f);
                playerDetected = false;
            }
        }
    }

    private bool PlayerInSight()
    {
        return PerformRaycast(viewDistance);
    }
    
    private bool PlayerDetected()
    {
        return PerformRaycast(detectedDistance);
    }

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


    private void OnDrawGizmos()
    {
        if (securityPatrol != null && securityPatrol.IsAtEdge())
            return;


        float securityHeight = boxCollider.bounds.size.y;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + securityHeight/2);
        Vector2 forward = transform.right * Mathf.Sign(transform.localScale.x);
        float halfFOV = fovAngle / 2f;
        float angleStep = fovAngle / (rayCount - 1);

        Gizmos.color = Color.cyan;
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * forward;
            Gizmos.DrawLine(origin, origin + rayDirection * viewDistance);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * forward;
            Gizmos.DrawLine(origin, origin + rayDirection * detectedDistance);
        }
    }
}
