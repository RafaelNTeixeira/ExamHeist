using UnityEngine;

public class Security : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

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
        // Check if the security is at an edge
        if (securityPatrol != null && securityPatrol.IsAtEdge())
        {
            return false; // Disable BoxCast when at the edge
        }

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right/Mathf.Abs(transform.localScale.x) * range * Mathf.Sign(transform.localScale.x) * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, playerLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        // Check if the security is at an edge
        if (securityPatrol != null && securityPatrol.IsAtEdge())
        {
            return; 
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right/Mathf.Abs(transform.localScale.x) * range * Mathf.Sign(transform.localScale.x) * colliderDistance,
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
