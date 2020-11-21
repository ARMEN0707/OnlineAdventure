using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerClient")
        {
            if (collision.gameObject.tag == "Player")
            {
                DataScenes.collectedFruits++;
            }
            Destroy(gameObject);
        }
    }
}
