using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentFruits : MonoBehaviour
{
    private Text textFruits;

    void Start()
    {
        textFruits = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        textFruits.text = DataScenes.collectedFruits.ToString();
    }
}
