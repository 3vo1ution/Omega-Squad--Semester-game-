using UnityEngine;
public class DialogueTrigger : MonoBehaviour
{
    public string dialogueLine;  // The dialogue line for this object
    public float customDuration = 5f;  // Optional custom duration for this dialogue
    private DialogueManager dialogueManager;
    public AudioSource soundEffects;
    public AudioClip DialogueLine;
  

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
        {
            soundEffects.clip = DialogueLine;
            soundEffects.Play(); 
        }
       
    }


}