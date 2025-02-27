using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [Header("Rotação")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationAngle = 45f;

    [Header("Configuração da Luz")]
    [SerializeField] private float fovAngle = 30f;  // Menor campo de visão (cone menor)
    [SerializeField] private float viewDistance = 1.5f; // Luz com menor alcance
    [SerializeField] private int rayCount = 10; // Menos raios para um cone menor
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private float startRotation;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        float angle = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, startRotation + angle);

        if (PlayerInSight())
        {
            Debug.Log("Jogador encontrado");
        }
    }

    private bool PlayerInSight()
    {
        Vector2 origin = transform.position;
        Vector2 downward = -transform.up; // Agora aponta para baixo
        float halfFOV = fovAngle / 2f;
        float angleStep = fovAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * downward;

            RaycastHit2D hitPlayer = Physics2D.Raycast(origin, rayDirection, viewDistance, playerLayer);
            if (hitPlayer.collider != null)
            {
                Debug.DrawRay(origin, rayDirection * viewDistance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(origin, rayDirection * viewDistance, Color.yellow);
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 downward = -transform.up; // Aponta para baixo
        float halfFOV = fovAngle / 2f;
        float angleStep = fovAngle / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector2 rayDirection = Quaternion.Euler(0, 0, currentAngle) * downward;
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(origin, origin + rayDirection * viewDistance);
        }
    }
}
