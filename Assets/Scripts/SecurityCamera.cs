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

    [Header("Cooldown Settings")]
    [SerializeField] private float alarmCooldown = 30f; // Time before alarm turns off
    private float cooldownTimer = 0f;
    private bool alarmActive = false;

    private float startRotation;

    [Header("Audio Settings")]
    public AudioClip alarmSound;
    private AudioSource audioSource;
    [SerializeField] private float fadeDuration = 1.5f; // Time for fade in/out of alarm volume
    private Coroutine fadeCoroutine;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = alarmSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f; // Start silent
    }

    void Update()
    {
        float angle = Mathf.PingPong(Time.time * rotationSpeed, rotationAngle * 2) - rotationAngle;
        transform.rotation = Quaternion.Euler(0, 0, startRotation + angle);

        bool playerDetected = PlayerInSight();

        if (playerDetected)
        {
            cooldownTimer = alarmCooldown; // Reset cooldown
            if (!alarmActive) 
            {
                ActivateAlarm(true);
                alarmActive = true;

                // Start fade-in effect
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeAudio(0f, 0.5f, fadeDuration));
            }
        }
        else
        {
            if (alarmActive) 
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0)
                {
                    ActivateAlarm(false);
                    alarmActive = false;

                    // Start fade-out effect
                    if (fadeCoroutine != null)
                        StopCoroutine(fadeCoroutine);
                    fadeCoroutine = StartCoroutine(FadeAudio(1f, 0f, fadeDuration));
                }
            }
        }
    }

    private IEnumerator FadeAudio(float startVolume, float targetVolume, float duration)
    {
        float timer = 0f;
        
        if (targetVolume > 0 && !audioSource.isPlaying)
        {
            audioSource.Play(); // Start playing if it's not already
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0)
        {
            audioSource.Stop(); // Stop playing when fully faded out
        }
    }

    private void ActivateAlarm(bool state)
    {
        if (alarmLamps != null)
        {
            foreach (var alarmLamp in alarmLamps)
            {
                if (alarmLamp != null)
                {
                    alarmLamp.ActivateAlarm(state);
                }
            }
            foreach (var lamp in lamps)
            {
                if (lamp != null)
                {
                    lamp.enabled = !state; // Lights are shutdown
                }
            }
        }
        Debug.Log("Alarm " + (state ? "Activated" : "Deactivated"));
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

    private void OnDrawGizmos()
    {
        Vector2 origin = transform.position;
        Vector2 downward = -transform.up; 
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
