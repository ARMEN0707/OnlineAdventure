using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {         
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!DataScenes.client && Server.connectClient)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
        if (DataScenes.client && Client.connect)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

    }
}
