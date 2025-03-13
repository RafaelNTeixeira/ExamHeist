using UnityEngine;
using System.Collections;

// Class to manage the alarm system
// This class is a singleton to allow managing the alarm system across different scenes
public class AlarmManager : MonoBehaviour
{
    public static AlarmManager instance;

    private AudioSource audioSource;
    public AudioClip alarmSound;
    private Coroutine fadeCoroutine;
    private int activeCameras = 0; // Track number of cameras detecting player

    [SerializeField] private float fadeDuration = 1.5f; // Fade in/out duration
    [SerializeField] private float maxVolume = 0.5f; // Max alarm volume
    [SerializeField] private float alarmCooldown = 50f; // Global cooldown
    public float cooldownTimer = 0f;
    public bool isAlarmActive = false; // Check if alarm is active

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = alarmSound;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0f;
    }

    // Function to request the alarm activation from a camera
    public void RequestAlarm()
    {
        // Debug.Log("Called requestAlarm");
        activeCameras++; // Camera detected the player
        cooldownTimer = alarmCooldown; // Reset cooldown every time a camera detects the player
        isAlarmActive = true;

        // Start the alarm sound if it's not already playing
        if (activeCameras == 1 && audioSource.volume == 0f)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(0f, maxVolume, fadeDuration)); // Fade in the alarm sound
        }
    }

    // Function to release the alarm activation from a camera
    public void ReleaseAlarm()
    {
        activeCameras--; // Camera stopped detecting the player

        if (activeCameras <= 0) 
        {
            activeCameras = 0;

            // Start the cooldown timer to deactivate the alarm system
            if (cooldownTimer <= 0)
            {
                isAlarmActive = false;
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeAudio(maxVolume, 0f, fadeDuration)); // Fade out the alarm sound
            }
        }
    }

    // Function to fade the audio source volume
    private IEnumerator FadeAudio(float startVolume, float targetVolume, float duration)
    {
        float timer = 0f;

        if (targetVolume > 0 && !audioSource.isPlaying)
            audioSource.Play();

        // Fade the audio source volume
        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, targetVolume, timer / duration);
            yield return null;
        }

        audioSource.volume = targetVolume;

        if (targetVolume == 0)
            audioSource.Stop();
    }

    // Function to update the alarm cooldown
    public void UpdateAlarmCooldown(float deltaTime)
    {
        // If no cameras are detecting the player and the alarm timer ended, deactivate the alarm sound
        if (activeCameras == 0 && cooldownTimer > 0)
        {
            cooldownTimer -= deltaTime;
            if (cooldownTimer <= 0)
            {
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeAudio(maxVolume, 0f, fadeDuration));
            }
        }
    }

    // Stop alarm sound when game is paused
    public void StopAlarm()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause(); // Pause the audio when the game is paused
        }
    }

    // Resume alarm sound when game is unpaused
    public void ResumeAlarm()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.UnPause(); // Resume the audio if it was paused
        }
    }

    // Function to stop all audio sources
    public void StopAllSounds()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // BackgroundMusic.instance.StopMusic();
    }

    // Function to reset the alarm state
    public void ResetAlarmState()
    {
        StopAllSounds();  // Stop all audio sources
        activeCameras = 0; 
        cooldownTimer = 0f;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
