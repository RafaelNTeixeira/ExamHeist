using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance; // Singleton instance
    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep playing across scenes
            audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.loop = true;
                audioSource.mute = true; // Mute initially
                audioSource.Play();
            }
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        // PLay the music on the following scenes
        if (scene.name == "GameScene" || scene.name == "Tutorial")
        {
            audioSource.mute = false;
        }

        // Only allow to play music on the following scenes (pause menu in case of resume option)
        if (scene.name != "GameScene" && scene.name != "Tutorial" && scene.name != "PauseMenu")
        {
            DestroyMusic();
        }
    }

    public void PlayMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void DestroyMusic()
    {
        Debug.Log("Destroying BackgroundMusic");
        instance = null; // Clear the singleton reference
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe from scene changes
    }

    // Function to change music dynamically
    public void ChangeMusic(AudioClip newClip)
    {
        if (audioSource.clip != newClip)
        {
            audioSource.Stop();
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
}
