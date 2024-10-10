using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("NewScene");

    }
    public void quitGame()
    {
        Application.Quit();
    }

    public void pauseGame()

    {
        SceneManager.LoadScene("StartingScene");
    }

}




