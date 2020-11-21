using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformBrown : MonoBehaviour
{
    public float speed;

    public Transform[] pathElements;
    int currPath, size;

    public bool right = true,move=false;

    // Start is called before the first frame update
    void Start()
    {
        size = pathElements.Length;
        currPath = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (move)
        {            
            if ((Vector2)transform.position != (Vector2)pathElements[currPath - 1].position)
            {
                Vector2 tempVector = Vector2.MoveTowards(transform.position, pathElements[currPath - 1].position, Time.fixedDeltaTime * speed);
                transform.position = new Vector3(tempVector.x, tempVector.y, transform.position.z);
            }
            else if (right)
            {
                if (currPath <= size) currPath++;
            }
            else
            {
                if (currPath > 1) currPath--;
            }
        }
        if((Vector2)transform.position == (Vector2)pathElements[size-1].position && right)
        {
            move = false;
        }
        if ((Vector2)transform.position == (Vector2)pathElements[0].position && !right)
        {
            move = false;
        }

    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enter");
        if((collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerClient") && collision.rigidbody.velocity.y <= 0)
        {
            collision.transform.parent = transform;
            move = true;
            right = true;
            if (currPath <= size) currPath++;

        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("Exit");
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerClient")
        {
            move = true;
            right = false;
            if (currPath > 1)  currPath--;
            collision.transform.parent = null;
        }     
    }
 
}
