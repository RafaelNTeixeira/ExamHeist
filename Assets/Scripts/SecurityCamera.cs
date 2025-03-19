using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

// Class to manage the security cameras 
// This class is responsible for detecting the player and activating the alarm system
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
    [SerializeField] private Light2D[] lamps; // Game lights
    [SerializeField] private SecurityPatrol[] securityPatrols;
    [SerializeField] private Stairs[] stairs;

    public bool alarmActive = false;

    [Header("Tutorial UI Settings")]    
    [SerializeField] private bool eletricCamera = false;
    [SerializeField] private GameObject uiText;  // Assign the UI Text GameObject in the Inspector
    [SerializeField] private GameObject uiTextDelete;

    private float startRotation;
    private float timer = 0f;
    private bool reversing = false;
    private float edgeWaitTime = 2f; // Time to wait at edges
    private float edgeTimer = 0f; // Timer for edge delay
    private bool isWaiting = false; // Flag to check if we are pausing

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
    }

    void Update()
    {
        CameraRotation();

        bool playerDetected = PlayerInSight();
        if (playerDetected)
        {
            
            SetAlarmState(true); // Activate the alarm system
            AlarmManager.instance.RequestAlarm(); // Notify the global alarm system 

            if (GameManager.instance.currentState == GameManager.GameState.Tutorial)
            {
                uiTextDelete.SetActive(false); // Hide the text UI
                uiText.SetActive(true); // Show the text UI
                GameObject player = GameObject.Find("Player");
                float targetX = eletricCamera ? 7.7f : -7.3f;
                player.transform.position = new Vector2(targetX, player.transform.position.y);
            }
        }
        else
        {
            // If the player is no longer detected while the alarm is active, release the alarm
            if (alarmActive)
            {
                AlarmManager.instance.ReleaseAlarm(); // Notify the alarm system
            }

            // Deactivate all alarm lights when cooldown reaches 0
            if (AlarmManager.instance.cooldownTimer <= 0)
            {
                SetAlarmState(false); // Deactivate the alarm system
            }
        }

        // Update cooldown in AlarmManager
        AlarmManager.instance.UpdateAlarmCooldown(Time.deltaTime);
    }

    public void SetAlarmState(bool state)
    {
        ActivateAlarmLights(state);
        alarmActive = state;
        NotifySecurityPatrol(state); // Activate/deactivate security patrols
        BlockStairs(state); // Block/unblock the stairs
    }

    // Function to notify the security patrols about the player detection
    public void NotifySecurityPatrol(bool state)
    {
        foreach (var patrol in securityPatrols)
        {
            if (patrol != null && patrol.playerDetected != state)
            {
                patrol.PlayerDetected(state, 3f);
            }
        }
    }

    // Function to block/unblock the stairs
    public void BlockStairs(bool state)
    {
        foreach (var stair in stairs)
        {
            if (stair != null)
            {
                stair.BlockStairs(state);
            }
        }
    }

    // Function to activate/deactivate the alarm lights
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
    }

    // Function to reset the camera detection state
    public void ResetCameraState()
    {
        ActivateAlarmLights(false); // Turn off alarm lights
        alarmActive = false;
        NotifySecurityPatrol(false);
    }

    // Function to check if the player is in sight
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

    // Function to manage camera rotation
    private void CameraRotation()
    {
        if (isWaiting)
        {
            edgeTimer += Time.deltaTime;
            if (edgeTimer >= edgeWaitTime)
            {
                isWaiting = false;
                edgeTimer = 0f;
            }
            return; // Skip rotation update while waiting
        }

        // Update rotation timer
        timer += (reversing ? -1 : 1) * Time.deltaTime * rotationSpeed;

        // Check if we reached the edge
        if (timer >= rotationAngle || timer <= -rotationAngle)
        {
            timer = Mathf.Clamp(timer, -rotationAngle, rotationAngle); // Ensure exact edge position
            reversing = !reversing; // Flip direction
            isWaiting = true; // Start waiting
        }

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, startRotation + timer);
    }

    // Function to disable the camera
    public void DisableCamera()
    {
        gameObject.SetActive(false);
    }

    // Function to enable the camera
    public void EnableCamera()
    {
        gameObject.SetActive(true);
    }

}
