using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject SelectMenu;
    public GameObject SettingMenu;
    public GameObject InputIPAddressMenuCustomMap;
    public GameObject SelecteLoadMap;

    public InputField IPAddressCustomMap;

    public void CreateGame()
    {
        SceneManager.LoadScene("CustomMap");
        DataScenes.client = false;
    }
    public void OpenEditorMap()
    {
        DataScenes.isEditor = true;
        SceneManager.LoadScene("Editor");
    }
    public void OpenListCustomMap(GameObject scrollView)
    {
        SelectMenu.SetActive(false);
        SelecteLoadMap.SetActive(true);
        SaveLoadMap script = scrollView.GetComponent<SaveLoadMap>();
        script.GetExistingMap(scrollView);
    }
    public void ConnectGame()
    {
        DataScenes.client = true; 
        InputIPAddressMenuCustomMap.SetActive(true);  
        SelectMenu.SetActive(false);
    }
    public void GetIPAddress()
    {        
        DataScenes.IPAddress = IPAddressCustomMap.text;
        SceneManager.LoadScene("CustomMap");
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
    }
    public void BackSelectGame()
    {
        SelectMenu.SetActive(true);
        InputIPAddressMenuCustomMap.SetActive(false);
        SelecteLoadMap.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
