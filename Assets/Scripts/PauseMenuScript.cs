using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;

    public GameObject[] ObjectsToDisableWhenPauseMenuIsGone;
    public GameObject PauseMenuContainer;

    private void Awake()
    {
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false; // Unpause audio time
        GameIsPaused = false;

        // Reset Pause Menu
        ResetPauseMenu();
        
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true; // Pause audio time
        GameIsPaused = true;
    }

    void ResetPauseMenu()
    {
        for (int i = 0; i < ObjectsToDisableWhenPauseMenuIsGone.Length; i++)
        {
            ObjectsToDisableWhenPauseMenuIsGone[i].SetActive(false);
        }
        PauseMenuContainer.SetActive(true);
    }
}
