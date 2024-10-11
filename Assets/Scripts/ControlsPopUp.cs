using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPopUp : MonoBehaviour
{
    public GameObject controlsImage;
    private bool isVisible = false;

    private void Start()
    {
        controlsImage.SetActive(false);//image is hidden at start
       

    }
    public void ToggleControlsImage()
    {
        isVisible = !isVisible;// image is visible when button is pressed
        controlsImage.SetActive(isVisible);    
    }
}