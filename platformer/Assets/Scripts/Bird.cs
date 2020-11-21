using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public bool right = true;
    public float distance, speed;
    private SpriteRenderer enemiesSprite;
    private Animator enemiesAnimator;
    private Vector3 tempPointRight, tempPointLeft;

    // Start is called before the first frame update
    void Start()
    {
        enemiesAnimator = GetComponentInChildren<Animator>();
        enemiesSprite = GetComponentInChildren<SpriteRenderer>();
        tempPointRight = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        tempPointLeft = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (right)
        {
            transform.position = Vector3.MoveTowards(transform.position, tempPointRight, Time.fixedDeltaTime * speed);
            enemiesSprite.flipX = false;
            if (transform.position == tempPointRight)
            {
                right = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, tempPointLeft, Time.fixedDeltaTime * speed);
            enemiesSprite.flipX = true;
            if (transform.position == tempPointLeft)
            {
                right = true;
            }
        }           
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        right = !right;
    }
}


