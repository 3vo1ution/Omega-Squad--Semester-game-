using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button button; 
    public float delayTime = 5.0f; 
    void Start()
    {
       
        button.gameObject.SetActive(false);
        StartCoroutine(ShowButtonAfterDelay());
    }

    IEnumerator ShowButtonAfterDelay()
    {
        // Wait for the specified amount of time and show the button after the delay
        yield return new WaitForSeconds(delayTime);
        
        button.gameObject.SetActive(true);
    }
}
