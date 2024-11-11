
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // For TextMeshPro support
using System.Collections;

public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager Instance;

    [Header("Music Clips")]
    public AudioClip happyMusic;  // Background music for the starting scene

    [Header("Sound Effects")]
    public AudioClip dialogueClip; // Audio clip for dialogue
    public AudioClip itemPickupClip; // Audio clip for item pickup
    public AudioClip doorOpenClip; // Audio clip for door opening

    [Header("Dialogue Settings")]
    public TMP_Text dialogueText; // UI Text component for dialogue display
    public GameObject dialogueBox; // Dialogue box UI element
    public float dialogueDuration = 5.0f; // Duration to display dialogue text

    [Header("Fade Settings")]
    public float fadeDuration = 1.0f;

    private AudioSource audioSource;
    private Coroutine dialogueCoroutine;

    private bool isDialogueActive = false;  // Flag to track if dialogue is active

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
        // Play happy music by default (Only in the starting scene)
        if (SceneManager.GetActiveScene().name == "StartingScene")
        {
            PlayMusic(happyMusic);
        }

        // Hide the dialogue box at the start
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from scene loaded event to prevent memory leaks
        // SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Method to play music
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

    // Method to play music with fade transition
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

    // Coroutine to handle fading out and in of audio
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

    // Coroutine to hide the dialogue after a certain amount of time
    private IEnumerator HideDialogueAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideDialogue();
    }

    // Method to hide the dialogue immediately
    public void HideDialogue()
    {
        if (dialogueBox != null)
        {
            dialogueBox.SetActive(false);
        }

        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        // Resume the background music when dialogue ends
        if (isDialogueActive)
        {
            audioSource.UnPause();  // Unpause background music
            isDialogueActive = false;  // Set dialogue flag to false
        }
    }

    // Method to play item pickup sound
    public void PlayItemPickupSound()
    {
        if (audioSource != null && itemPickupClip != null)
        {
            audioSource.PlayOneShot(itemPickupClip);
        }
    }

    // Method to play door opening sound
    public void PlayDoorOpenSound()
    {
        if (audioSource != null && doorOpenClip != null)
        {
            audioSource.PlayOneShot(doorOpenClip);
        }
    }

    // Method to play dialogue and pause music
    public void PlayDialogue(string dialogue, AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            // Ensure the game continues to run even when dialogue is active
            audioSource.Pause();  // Pause the background music
            isDialogueActive = true;

            audioSource.clip = clip;
            audioSource.Play();

            // Display the dialogue text
            if (dialogueText != null)
            {
                dialogueText.text = dialogue;
                dialogueBox.SetActive(true);
                if (dialogueCoroutine != null)
                {
                    StopCoroutine(dialogueCoroutine);
                }
                dialogueCoroutine = StartCoroutine(HideDialogueAfterTime(dialogueDuration));
            }
        }
    }
}
