﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseButton;
    public GameObject settingMenu;
    public GameObject blackBackground;

    public static bool gamePause=false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gamePause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        blackBackground.SetActive(true);
        gamePause = true;
    }
    public void Resume()
    {
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);
        blackBackground.SetActive(false);
        gamePause = false;
    }
    public void Setting()
    {
        settingMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }
    public void CloseSetting()
    {
        settingMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void Exit()
    {
        if(DataScenes.client)
        {
            Client.disconnect = true;
        }    
        if(!DataScenes.client)
        {
            Server.disconnectServer = true;
        }
        Time.timeScale = 1;
        DataScenes.nameMap = default;
        SceneManager.LoadScene(0);
    }
}
