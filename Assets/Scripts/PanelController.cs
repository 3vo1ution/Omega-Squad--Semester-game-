using UnityEngine;


public class PanelController : MonoBehaviour
{
    public GameObject panel; 
    private bool isPanelActive = false;
    public AudioSource sound;
    public AudioClip buttonPressSFX;
    void Start()
    {
        panel.SetActive(false);
    }

    
    public void TogglePanel()
    {
        isPanelActive = !isPanelActive;
        panel.SetActive(isPanelActive);

    }

    // disable the panel when clicking outside of it
    void Update()
    {
        if (isPanelActive && Input.GetMouseButtonDown(0))
        {
         
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                    panel.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    Camera.main))
            {
                panel.SetActive(false);
                isPanelActive = false;
            }


        }


    }
}
