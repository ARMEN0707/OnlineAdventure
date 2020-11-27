using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StartEditor : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject character;
    public MoveCamera moveCameraScript;
    public PlayerCamera playerCameraScript;

    private Text textButton;
    private bool start = false;
    public bool IsStart
    {
        get
        {
            return start;
        }
    }


    private void Start()
    {
        textButton = gameObject.GetComponentInChildren<Text>();
        character.SetActive(false);
    }

    public  void ActivateAll()
    {
        ActivateBird();
        ActivateChicken();
    }

    private  void ActivateBird()
    {
        Bird[] birdScripts = FindObjectsOfType<Bird>();
        foreach (Bird item in birdScripts)
        {
            item.enabled = !item.enabled;
            item.transform.position = item.startPoint;
        }
    }

    private  void ActivateKillEnemies()
    {
        KillEnemies[] killEnemiesScript = FindObjectsOfType<KillEnemies>();
        foreach(KillEnemies item in killEnemiesScript)
        {
            item.colliderEnemies.isTrigger = false;
            item.rbEnemies.bodyType = RigidbodyType2D.Kinematic;
        }
    }

    private  void ActivateChicken()
    {
        Chicken[] chickenScripts = FindObjectsOfType<Chicken>();
        foreach (Chicken item in chickenScripts)
        {
            item.enabled = !item.enabled;
            item.transform.position = item.startPoint;
        }
    }

    //запуск или остановка воспроизведения карты
    public void PressButton()
    {
        if(!start)
        {
            scrollView.SetActive(false);
            textButton.text = "Stop";
            start = true;
            if(GameObject.Find("PlayerSpawn")!=null)
            {
                character.transform.position = GameObject.Find("PlayerSpawn").GetComponent<Transform>().position;
                character.SetActive(true);
                moveCameraScript.enabled = false;
                playerCameraScript.enabled = true;
            }
        }
        else
        {
            scrollView.SetActive(true);
            textButton.text = "Start";
            character.SetActive(false);
            start = false;
            moveCameraScript.enabled = true;
            playerCameraScript.enabled = false;
        }
        ActivateKillEnemies();
        ActivateAll();
    }
}
