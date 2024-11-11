using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public GameObject panel; // Reference to the panel GameObject
    private bool isPanelActive = false; // Track panel state

    void Start()
    {
        // Ensure the panel starts inactive
        panel.SetActive(false);
    }

    // Method to toggle the panel's active state
    public void TogglePanel()
    {
        isPanelActive = !isPanelActive;
        panel.SetActive(isPanelActive);
    }

    // Method to disable the panel when clicking outside of it
    void Update()
    {
        if (isPanelActive && Input.GetMouseButtonDown(0))
        {
            // Check if the mouse click is not over the panel
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
