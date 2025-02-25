using UnityEngine;

public class Security : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    [Header("Cone Raycast Settings")]
    private readonly float fovAngle = 60f;
    private readonly float viewDistance = 3f; 
    private readonly int rayCount = 10; // Number of rays in cone
    [SerializeField] private LayerMask obstacleLayer;

    private Animator anim;
    private SecurityPatrol securityPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        securityPatrol = GetComponentInParent<SecurityPatrol>();
    }
    
    private void Update()
    {
        if (PlayerInSight())
            anim.SetTrigger("catch");

        if (securityPatrol != null)
            securityPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        if (securityPatrol != null && securityPatrol.IsAtEdge())
        {
            return false;
        }

        float securityHeight = boxCollider.bounds.size.y;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + securityHeight/2); // Offset so that raycasts originate at the security's eye level.
        Vector2 forward = transform.right * Mathf.Sign(transform.localScale.x);
        float halfFOV = fovAngle / 2f;
        float angleStep = fovAngle / (rayCount - 1); // Slice of the PoV angle

        // Iterate through each ray within the cone.
        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * forward;

            RaycastHit2D hitPlayer = Physics2D.Raycast(origin, rayDirection, viewDistance, playerLayer);
            if (hitPlayer.collider != null)
            {
                // Debug.Log("Player detected: " + hitPlayer.collider.name);
                Debug.DrawRay(origin, rayDirection * viewDistance, Color.green);// Encountered player
                return true;
            }
            else
            {
                Debug.DrawRay(origin, rayDirection * viewDistance, Color.yellow); // Didn't encounter anything
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (securityPatrol != null && securityPatrol.IsAtEdge())
        {
            return;
        }

        float securityHeight = boxCollider.bounds.size.y;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y + securityHeight/2);
        Vector2 forward = transform.right * Mathf.Sign(transform.localScale.x);
        float halfFOV = fovAngle / 2f;
        float angleStep = fovAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * forward;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(origin, origin + rayDirection * viewDistance);
        }
    }
}
