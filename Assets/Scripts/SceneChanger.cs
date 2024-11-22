using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string EndScreen; // The name of the scene to load
    public float interactionRange = 3f; // The range for interacting with the object
    public Transform playerCamera; // Assign the player's camera

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Change "E" to your desired interaction key
        {
            CheckForInteraction();
        }
    }

    void CheckForInteraction()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (hit.collider != null && hit.collider.CompareTag("Interactable")) // Check for the correct tag
            {
                LoadTargetScene();
            }
        }
    }

    void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(EndScreen))
        {
            SceneManager.LoadScene(EndScreen);
        }
        else
        {
            Debug.LogError("Target scene name is not set in the inspector!");
        }
    }
}
