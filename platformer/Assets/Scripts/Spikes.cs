using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {        
        if (col.isTrigger==false)
        {
            Character characterScript = col.gameObject.GetComponent<Character>();
            if(characterScript)
            {
                col.attachedRigidbody.velocity = new Vector2(1f, 2f);
                Character.life -= 3;
                Character.isLife = false;
            }
            else
            {
                CharacterClient.life -= 3;
                CharacterClient.isLife = false;
            }
        }
    }

}
