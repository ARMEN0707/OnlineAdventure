using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject SelectMenu;
    public GameObject SettingMenu;
    public GameObject InputIPAddressMenu;

    public InputField IPAddress;

    public void CreateGame()
    {
        SceneManager.LoadScene(1);
        DataScenes.client = false;
    }
    public void ConnectGame()
    {
        DataScenes.client = true;
        InputIPAddressMenu.SetActive(true);
        SelectMenu.SetActive(false);
    }
    public void GetIPAddress()
    {
        DataScenes.IPAddress = IPAddress.text;
        Debug.Log(DataScenes.IPAddress);
        SceneManager.LoadScene(1);
    }
    public void SelectGame()
    {        
        gameObject.SetActive(false);
        SelectMenu.SetActive(true);
    }
    public void OpenSettingMenu()
    {
        SettingMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void CloseSettingMenu()
    {
        SettingMenu.SetActive(false);
        gameObject.SetActive(true);
    }
    public void BackMainMenu()
    {
        gameObject.SetActive(true);
        SelectMenu.SetActive(false);
    }
    public void BackSelectGame()
    {
        SelectMenu.SetActive(true);
        InputIPAddressMenu.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
