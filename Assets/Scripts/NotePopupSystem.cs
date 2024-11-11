using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotePopupSystem : MonoBehaviour
{
    public GameObject popupPanel;       // The panel to show the note image
    public Image noteImage;             // The Image component displaying the note
    public Sprite flyerSprite;          // The sprite to display as the note content
    public TextMeshProUGUI interactText; // The "Press [E] to pick up" text

    private bool isPlayerNearby = false; // Tracks if the player is near the flyer
    private bool noteDisplayed = false;  // Tracks if the note is currently displayed

    private void Update()
    {
        // Show the note when the player presses E and is near the flyer
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (!noteDisplayed)
            {
                ShowNotePopup();
                noteDisplayed = true;
            }
            else
            {
                HideNotePopup();
                noteDisplayed = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player is close to the flyer
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            ShowInteractText();  // Show "Press [E] to pick up"
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player has left the vicinity of the flyer
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            HideInteractText();  // Hide "Press [E] to pick up" text
            HideNotePopup();     // Hide the note if it was open
            noteDisplayed = false;
        }
    }

    // Shows the note pop-up with the specified image
    public void ShowNotePopup()
    {
        noteImage.sprite = flyerSprite;    // Set the note image to the flyer sprite
        popupPanel.SetActive(true);        // Show the panel with the image
    }

    // Hides the note pop-up
    public void HideNotePopup()
    {
        popupPanel.SetActive(false);       // Hide the panel
    }

    // Shows the "Press [E] to pick up" text
    public void ShowInteractText()
    {
        interactText.gameObject.SetActive(true);  // Show "Press [E]"
    }

    // Hides the "Press [E] to pick up" text
    public void HideInteractText()
    {
        interactText.gameObject.SetActive(false); // Hide "Press [E]"
    }
}

