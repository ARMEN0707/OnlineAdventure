using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruits : MonoBehaviour
{
    private AudioSource soundTakeFruit;

    private void Start()
    {
        soundTakeFruit = GameObject.Find("TakeFruits").GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerClient")
        {
            if (collision.gameObject.tag == "Player")
            {
                soundTakeFruit.Play();
                DataScenes.collectedFruits++;
            }
            Destroy(gameObject);
        }
    }
}
