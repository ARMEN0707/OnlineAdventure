using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    public GameObject characterServer;
    public GameObject characterClient;
    public GameObject finish;
    public Animator finishAnimator;
    public float distance;

    void Start()
    {
        characterServer = GameObject.FindGameObjectWithTag("Player");
        characterClient = DataScenes.characterClient;
        finish.SetActive(false);
    }

    void Update()
    {
        //активация спрайта финиша
        if ((characterServer.transform.position.x>=gameObject.transform.position.x-distance)||
            (characterClient.transform.position.x >= gameObject.transform.position.x - distance))
        {
            finish.SetActive(true);
        }
        
        // При смерти двух игроков включение финиш-меню
        if (!Character.isLife && !CharacterClient.isLife)
        {
            DataScenes.place = 0;
            DataScenes.allFruits += DataScenes.collectedFruits + (DataScenes.priceWin / 10);
            StartCoroutine(LoadFinishMenu());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.GetComponent<Finish>().enabled == true)
        {
            //очки, место, и смена сццены
            if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerClient") && (collision.contacts[0].normal.y < 0))
            {
                DataScenes.place++;
                //collision.gameObject.SetActive(false);
                collision.gameObject.GetComponentInChildren<Animator>().SetTrigger("Disappearing");
                finishAnimator.SetTrigger("Press");
                if (collision.gameObject.tag == "Player")
                {
                    DataScenes.finish = true;
                    DataScenes.allFruits += DataScenes.collectedFruits + (DataScenes.priceWin / DataScenes.place);
                    StartCoroutine(LoadFinishMenu());
                }
                else if (!Character.isLife)
                {
                    DataScenes.place++;
                    DataScenes.allFruits += DataScenes.collectedFruits + (DataScenes.priceWin / DataScenes.place);
                    StartCoroutine(LoadFinishMenu());
                }
            }
        }
    }

    IEnumerator LoadFinishMenu()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("FinishMenu");
    }
}


