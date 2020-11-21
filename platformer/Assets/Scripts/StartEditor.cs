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

    private void Start()
    {
        textButton = gameObject.GetComponentInChildren<Text>();
        character.SetActive(false);
    }

    private void EnabledScript()
    {
        try {

        }
        catch(NullReferenceException){}

    }

    public void PressButton()
    {
        if(!start)
        {
            scrollView.SetActive(false);
            textButton.text = "Stop";
            start = true;
            character.transform.position = GameObject.Find("PlayerSpawn").GetComponent<Transform>().position;
            character.SetActive(true);
            moveCameraScript.enabled = false;
            playerCameraScript.enabled = true;
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
    }
}
