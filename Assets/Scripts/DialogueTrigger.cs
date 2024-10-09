using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DialogueTrigger : MonoBehaviour
{
    public string dialogueLine;  // The dialogue line for this object
    public float customDuration = 5f;  // Optional custom duration for this dialogue
    private DialogueManager dialogueManager;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();  // Find the DialogueManager in the scene
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.ShowDialogue(dialogueLine, customDuration);  // Trigger with custom duration
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.HideDialogue();  // Optional: manually hide the dialogue when exiting the trigger
        }
    }
}
