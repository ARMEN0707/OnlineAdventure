using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeFruits : MonoBehaviour
{
    public GameObject collectedFruits;
    public GameObject prizeFruits;
    public GameObject winText;
    // Start is called before the first frame update
    void Start()
    {
        if(DataScenes.place==1)
        {
            winText.GetComponent<Text>().text = "YOU WIN";
            prizeFruits.GetComponent<Text>().text = DataScenes.priceWin.ToString() + " Fruits for a victory";
        }else
        {
            winText.GetComponent<Text>().text = "YOU DEFEAT";
            if (DataScenes.place==0)
            {
                prizeFruits.GetComponent<Text>().text = (DataScenes.priceWin / 10).ToString() + " Fruits for defeat";
            }
            else
            {
                prizeFruits.GetComponent<Text>().text = (DataScenes.priceWin / DataScenes.place).ToString() + " Fruits for defeat";
            }
           
        }
        collectedFruits.GetComponent<Text>().text = DataScenes.collectedFruits.ToString()+ " FRUITS COLLECTED";

        
    }
}
