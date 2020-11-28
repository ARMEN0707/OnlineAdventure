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
    public GameObject InputIPAddressMenuCustomMap;
    public GameObject SelecteTypeMap;
    public GameObject SelecteClientTypeMap;
    public GameObject SelecteLoadMap;

    public InputField IPAddress;
    public InputField IPAddressCustomMap;

    public void CreateGame()
    {
        SceneManager.LoadScene(1);
        DataScenes.client = false;
    }
    public void OpenEditorMap()
    {
        DataScenes.isEditor = true;
        SceneManager.LoadScene(3);
    }
    public void OpenSelecteTypeMap()
    {
        SelectMenu.SetActive(false);
        SelecteTypeMap.SetActive(true);
    }
    public void OpenSelecteClientTypeMap()
    {
        SelectMenu.SetActive(false);
        SelecteClientTypeMap.SetActive(true);
    }
    public void OpenListCustomMap(GameObject scrollView)
    {
        SelecteTypeMap.SetActive(false);
        SelecteLoadMap.SetActive(true);
        SaveLoadMap script = scrollView.GetComponent<SaveLoadMap>();
        script.GetExistingMap(scrollView);
    }
    public void ConnectGame(bool custom)
    {
        DataScenes.client = true; 
        if(custom)
        {
            InputIPAddressMenuCustomMap.SetActive(true);
        }else
        {
            InputIPAddressMenu.SetActive(true);
        }    
        SelecteClientTypeMap.SetActive(false);
        SelectMenu.SetActive(false);
    }
    public void GetIPAddress(bool custom)
    {        
        if(custom)
        {
            DataScenes.IPAddress = IPAddressCustomMap.text;
            SceneManager.LoadScene(4);
        }else
        {
            DataScenes.IPAddress = IPAddress.text;
            SceneManager.LoadScene(1);
        }
        Debug.Log(DataScenes.IPAddress);
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
        SelecteTypeMap.SetActive(false);
        SelecteClientTypeMap.SetActive(false);
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
