using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public float speed;

    public GameObject[] pathElements=null;
    private int currPath=1,size;

    bool right=true;

    // Start is called before the first frame update
    void Start()
    {
        size=pathElements.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //движение объекта
        if((Vector2)transform.position!=(Vector2)pathElements[currPath-1].transform.position)
        {
            Vector2 tempVector = Vector2.MoveTowards(transform.position, pathElements[currPath - 1].transform.position, Time.fixedDeltaTime * speed);
            transform.position = new Vector3(tempVector.x, tempVector.y,transform.position.z);
        }
        else if(right)
        {
            currPath++;
        }
        else
        {
            currPath--;
        }

        if(currPath==1)
        {
            right = true;
        }
        if(currPath==size)
        {
            right = false;
        }        
    }

    //прикрепление объекта 
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "PlayerClient")
        {
            col.transform.parent = transform;
        }
    }

    //открепление объекта
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player" || col.gameObject.tag == "PlayerClient")
        {
            col.transform.parent = null;
        }
    }
}
