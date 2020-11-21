using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour
{
    public GameObject[] characterSkin;
    public GameObject ButtonHome;
    public GameObject CollectedText;
    public GameObject FinishText;
    public GameObject WinText;
    public GameObject Character;
    public GameObject Cup;

    // Start is called before the first frame update
    void Start()
    {
        Character = Instantiate(characterSkin[DataScenes.numberSkinCharacter]);
        ButtonHome.SetActive(false);
        CollectedText.SetActive(false);
        FinishText.SetActive(false);
        WinText.SetActive(true);
        Character.SetActive(false);
        Cup.SetActive(true);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ActiveAll()
    {
        ButtonHome.SetActive(true);
        CollectedText.SetActive(true);
        FinishText.SetActive(true);
        Character.SetActive(true);
    }
}
