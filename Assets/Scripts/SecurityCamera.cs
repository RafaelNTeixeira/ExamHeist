using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class SecurityCamera : MonoBehaviour
{
    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float rotationAngle = 45f;

    [Header("Light")]
    [SerializeField] private float fovAngle = 30f;  
    [SerializeField] private float viewDistance = 1.5f; 
    [SerializeField] private int rayCount = 10;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private AlarmLamp[] alarmLamps;
    [SerializeField] private Light2D[] lamps;
    [SerializeField] private SecurityPatrol[] securityPatrol;

    private bool alarmActive = false;

    private float startRotation;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        float angle = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, startRotation + angle);

        bool playerDetected = PlayerInSight();

        if (playerDetected)
        {
            ActivateAlarmLights(true);
            alarmActive = true;
            AlarmManager.instance.RequestAlarm(); // Notify the global alarm system
            NotifySecurityPatrol(true);      
        }
        else
        {
            if (alarmActive)
            {
                AlarmManager.instance.ReleaseAlarm(); // Notify the alarm system
            }

            // Only deactivate lights when cooldown reaches 0
            if (AlarmManager.instance.cooldownTimer <= 0)
            {
                ActivateAlarmLights(false);
                alarmActive = false;
            }
        }

        // Update cooldown in AlarmManager
        AlarmManager.instance.UpdateAlarmCooldown(Time.deltaTime);
    }

    public void NotifySecurityPatrol(bool state)
    {
        foreach (var patrol in securityPatrol)
        {
            if (patrol != null)
            {
                patrol.PlayerDetected(state);
            }
        }
    }

    public void ActivateAlarmLights(bool state)
    {
        foreach (var alarmLamp in alarmLamps)
        {
            if (alarmLamp != null)
            {
                alarmLamp.ActivateAlarmLight(state);
            }
        }
        foreach (var lamp in lamps)
        {
            if (lamp != null)
            {
                lamp.enabled = !state; // Ensure lights only turn on when no alarm is active
            }
        }

        //Debug.Log("Alarm " + (state ? "Activated" : "Deactivated"));
    }

    public void ResetCameraState()
    {
        ActivateAlarmLights(false); // Turn off alarm lights
        alarmActive = false;
    }

    private bool PlayerInSight()
    {
        Vector2 origin = transform.position;
        Vector2 downward = -transform.up;
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

    public void DisableCamera()
    {
        gameObject.SetActive(false);
    }

    public void EnableCamera()
    {
        gameObject.SetActive(true);
    }

}
