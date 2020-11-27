using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Begin : MonoBehaviour
{

    private Animator anim;
    //private ParticleSystem particle;
    private bool start;
    private GameObject characterServer;
    private GameObject characterClient;

    public Transform playerSpawn;
    private GameObject server;
    private GameObject client;
    public GameObject[] players;


    // Start is called before the first frame update
    void Awake()
    {
        start = false;
        DataScenes.collectedFruits = 0;
        client = GameObject.Find("Client");
        server = GameObject.Find("Server");

        if (DataScenes.client)
        {
            //инициализация клиента
            characterServer = Instantiate(players[1], playerSpawn.position, Quaternion.identity);
            characterClient = Instantiate(players[0], playerSpawn.position, Quaternion.identity);
            server.SetActive(false);
            DataScenes.numberSkinCharacter = 1;
        }
        else
        {
            //игициализация сервера
            characterServer = Instantiate(players[0], playerSpawn.position, Quaternion.identity);
            characterClient = Instantiate(players[1], playerSpawn.position, Quaternion.identity);
            client.SetActive(false);
            DataScenes.numberSkinCharacter = 0;

        }
        //настройка игроков
        characterServer.name = characterServer.name.Replace("(Clone)", "");
        characterClient.name = characterClient.name.Replace("(Clone)", "");
        DataScenes.characterClient = characterClient;
        characterServer.tag = "Player";
        characterClient.tag = "PlayerClient";        
        characterClient.SetActive(false);
        characterServer.GetComponent<Character>().enabled = true;
        characterClient.GetComponent<CharacterClient>().enabled = true;
        Destroy(characterServer.GetComponent<CharacterClient>());
        Destroy(characterClient.GetComponent<Character>());

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Анимация старта
        if ((collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerClient") && start==false)
        {
            anim = GetComponentInChildren<Animator>();
            start = true;
            anim.SetTrigger("Start");
        }
    }
}
