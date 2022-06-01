using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject blurEffect;
    private bool pauseCR_Running;

    public void Update()
    {
        if (InputManager._instance.GetKeyDown("Pause"))
        {
            if (SCR_GameManager._gameState != GameState.PauseMenu)
                PauseGame();
            else if(SCR_GameManager._gameState == GameState.PauseMenu)
                ResumeGame();

            //pauses if gamestate is not pause and vice versa
        }
    }

    public void PauseGame()
    {
        if(!pauseCR_Running) StartCoroutine(Pause());
    }

    public void ResumeGame()
    {
        if (!pauseCR_Running) StartCoroutine(Resume());
    }

    IEnumerator Pause()
    {
        pauseCR_Running = true;

        pauseMenu.SetActive(true);
        blurEffect.SetActive(true);
        SCR_GameManager._instance.SetState(GameState.PauseMenu);

        //pause state stops time and activates cursor

        yield return null;

        pauseCR_Running = false;
    }

    IEnumerator Resume()
    {
        pauseCR_Running = true;

        pauseMenu.SetActive(false);
        blurEffect.SetActive(false);
        SCR_GameManager._instance.SetState(GameState.Gameplay);

        //gameplay state resumes time and activates cursor

        yield return null;

        pauseCR_Running = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
