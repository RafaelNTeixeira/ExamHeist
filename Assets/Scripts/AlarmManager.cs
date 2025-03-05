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

    public void RequestAlarm()
    {
        activeCameras++;

        if (activeCameras == 1) // First camera detected player
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
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeAudio(maxVolume, 0f, fadeDuration));
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
}
