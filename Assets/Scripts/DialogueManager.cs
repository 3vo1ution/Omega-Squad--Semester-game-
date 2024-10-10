using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;  // Use TMP_Text for TextMeshPro
    public GameObject dialogueBox;
    public float displayDuration = 3f;  // Duration the dialogue appears on screen (in seconds)

    private Coroutine dialogueCoroutine;

    private void Start()
    {
        dialogueBox.SetActive(false);  // Hide the dialogue box at the start
    }

    // Call this to display dialogue for a specific duration
    public void ShowDialogue(string dialogue, float duration = -1)
    {
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);  // Stop any currently running coroutine
        }

        dialogueBox.SetActive(true);  // Show the dialogue box
        dialogueText.text = dialogue;  // Set the dialogue text

        // Use the default duration if none is specified
        float timeToDisplay = duration > 0 ? duration : displayDuration;
        dialogueCoroutine = StartCoroutine(HideDialogueAfterTime(timeToDisplay));  // Start the coroutine to hide it after a delay
    }

    // Coroutine to hide the dialogue after a certain amount of time
    private IEnumerator HideDialogueAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);  // Wait for the specified duration
        HideDialogue();  // Hide the dialogue after the delay
    }

    // Call this to hide the dialogue immediately
    public void HideDialogue()
    {
        dialogueBox.SetActive(false);  // Hide the dialogue box
        dialogueText.text = "";  // Clear the text
    }
}
