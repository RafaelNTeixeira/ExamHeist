using UnityEngine;

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
    
    public bool IsAtEdge()
    {
        return security.position.x <= leftEdge.position.x || security.position.x >= rightEdge.position.x;
    }

    private void DirectionChange()
    {
        anim.SetBool("isRunning", false);
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;
        anim.SetBool("isRunning", true);

        //Make security face direction
        security.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        security.position = new Vector3(security.position.x + Time.deltaTime * _direction * speed,
            security.position.y, security.position.z);
    }

    public void PlayerDetected(bool _detected)
    {
        if (_detected) speed *= 2;
        
        else speed /= 2;
    }
}
