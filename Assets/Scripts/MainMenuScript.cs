using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{

    public AudioSource buttonPress;
    public AudioClip buttonPressSFX;
    public AudioSource soundEffects;
    public AudioSource sound;
    public void playGame()
    {
        StartCoroutine(PlayButton());
        buttonPress.clip = buttonPressSFX;
        buttonPress.Play();

    }
    public void quitGame()
    {
        Application.Quit();
       buttonPress.clip = buttonPressSFX;
        buttonPress.Play();

    }

    public void startGame()
    {
        SceneManager.LoadScene("NewScene");
        buttonPress.clip = buttonPressSFX;
        buttonPress.Play();

    }

    public void pauseGame()
    {
        SceneManager.LoadScene("MenuScene");
        soundEffects.clip = buttonPressSFX;
        soundEffects.Play();

    }

    private IEnumerator PlayButton() 
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("videoScene");
    }

}




