using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
public class MapPopUp : MonoBehaviour
{
    public GameObject MapImage; 
    private bool isVisible = false;

    // Reference to FirstPersonControls 
    public FirstPersonControls playerControls;

    // UI Text element to show the error message
    public Text flyerMessageText;

    public float messageDisplayTime = 3f;

    private void Start()
    {
        MapImage.SetActive(false); 
        flyerMessageText.gameObject.SetActive(false);
    }

    public void ToggleMapImage()
    {
       
        if (playerControls.FlyerCount >= 5)
        {
            isVisible = !isVisible; 
            MapImage.SetActive(isVisible);
        }
        else
        {
            // Show the error message if not enough flyers are collected
            StartCoroutine(ShowFlyerMessage("You need to collect at least 5 flyers to open the map."));
        }
    }
    void UpdateFlyerMessageUI()  //updates the UI text with the current item count as a string
    {
        flyerMessageText.text = "You need to collect at least 5 flyers to open the map.";
    }


    private IEnumerator ShowFlyerMessage(string message)
    {
        flyerMessageText.text = "You need to collect at least 5 flyers to open the map."; 
        flyerMessageText.gameObject.SetActive(true); 
        yield return new WaitForSeconds(messageDisplayTime); 
        flyerMessageText.gameObject.SetActive(false); 
    }
}
