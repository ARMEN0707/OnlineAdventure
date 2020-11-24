using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemies : MonoBehaviour
{
    private Animator anim;
    public Rigidbody2D rbEnemies;
    public BoxCollider2D colliderEnemies;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        colliderEnemies = GetComponent<BoxCollider2D>();
        rbEnemies = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //убийстово врага
        if ((col.gameObject.tag == "Player" || col.gameObject.tag == "PlayerClient") && (col.contacts[0].normal.y<0))
        {
            col.rigidbody.velocity = new Vector2(0, 2f);
            colliderEnemies.isTrigger = true;
            rbEnemies.bodyType = RigidbodyType2D.Dynamic;
            rbEnemies.velocity = new Vector2(1f, 2f);
            anim.SetTrigger("isLife");
        }
        // убийство игрока
        else if (col.gameObject.tag == "Player")
        {
            Character.life--;
            if (Character.life==0)
            {
                Debug.Log("fsd");
                Character.isLife = false;
            }else
            {
                Character characterScript = col.gameObject.GetComponent<Character>();
                characterScript.charAnimator.SetTrigger("Hit");
            }
            col.rigidbody.velocity = new Vector2(1.5f, 2f);
        }
        //удийство клиента
        else if(col.gameObject.tag == "PlayerClient")
        {
            CharacterClient.life--;
            if (CharacterClient.life == 0)
            {
                Debug.Log("rwer");
                CharacterClient.isLife = false;
            }
            else
            {
                CharacterClient characterClientScript = col.gameObject.GetComponent<CharacterClient>();
                characterClientScript.charAnimator.SetTrigger("Hit");
            }
        }
    }

}   

