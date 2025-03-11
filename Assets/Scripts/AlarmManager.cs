using UnityEngine;
using System.Collections;

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

    public bool IsAlarmActive()
    {
        return activeCameras > 0;
    }

    public void RequestAlarm()
    {
        // Debug.Log("Called requestAlarm");
        activeCameras++;
        cooldownTimer = alarmCooldown; // Reset cooldown every time a camera detects the player
        isAlarmActive = true;

        // Only start the fade-in if the alarm is completely off
        if (activeCameras == 1 && audioSource.volume == 0f)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(0f, maxVolume, fadeDuration));
        }
    }

    public void ReleaseAlarm()
    {
        activeCameras--;

        if (activeCameras <= 0)
        {
            activeCameras = 0;

            if (cooldownTimer <= 0)
            {
                isAlarmActive = false;
                if (fadeCoroutine != null)
                    StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeAudio(maxVolume, 0f, fadeDuration));
            }
        }
    }

    private IEnumerator FadeAudio(float startVolume, float targetVolume, float duration)
    {
        float timer = 0f;

        if (targetVolume > 0 && !audioSource.isPlaying)
            audioSource.Play();

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

    public void UpdateAlarmCooldown(float deltaTime)
    {
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

    public void StopAllSounds()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        BackgroundMusic.instance.StopMusic();

        // // Also stop all other audio sources in the scene
        // AudioSource[] allAudioSources = Object.FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        // foreach (AudioSource source in allAudioSources)
        // {
        //     source.Stop();
        // }
    }

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
