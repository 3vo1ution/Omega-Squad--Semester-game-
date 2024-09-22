using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager Instance;

    [Header("Music Clips")]
    [Tooltip("Assign the happy music clip for the Start Scene.")]
    public AudioClip happyMusic;

    [Tooltip("Assign the eerie music clip for the Main Scene.")]
    public AudioClip eerieMusic;

    [Header("Fade Settings")]
    [Tooltip("Duration for fading in and out the music.")]
    public float fadeDuration = 1.0f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes

            // Add AudioSource component if not present
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // Configure AudioSource
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return;
        }
    }

    private void Start()
    {
        // Play happy music by default
        PlayMusic(happyMusic);

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Callback method when a new scene is loaded.
    /// </summary>
    /// <param name="scene">The scene that was loaded.</param>
    /// <param name="mode">The load scene mode.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Determine which scene is loaded and play corresponding music
        switch (scene.name)
        {
            case "StartingScene": 
                PlayMusicWithFade(happyMusic);
                break;
            case "NewScene":
                PlayMusicWithFade(eerieMusic);
                break;
            default:
                Debug.LogWarning("No music assigned for this scene: " + scene.name);
                break;
        }
    }

   
    private void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioManager: Music clip is not assigned.");
            return;
        }

        if (audioSource.clip == clip)
        {
            // Music is already playing
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

   
    private void PlayMusicWithFade(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioManager: Music clip is not assigned.");
            return;
        }

        if (audioSource.clip == clip)
        {
            // Music is already playing
            return;
        }

        StartCoroutine(FadeOutIn(clip));
    }

       private IEnumerator FadeOutIn(AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        // Fade out
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade in
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
