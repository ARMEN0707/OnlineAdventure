using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCamera : MonoBehaviour
{
    private GameObject character;
    private Transform playerTransform;
    private RaycastHit2D cameraRayRight;
    private RaycastHit2D cameraRayLeft;
    private RaycastHit2D cameraRayUp;
    private RaycastHit2D cameraRayDown;
    private float distanceX;
    private float distanceY;
    private bool swapCharacter=false;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindGameObjectWithTag("Player");
        playerTransform = character.GetComponent<Transform>();
        distanceX = Vector3.Distance(Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0.5f)), transform.position);
        distanceY = Vector3.Distance(Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0)), transform.position);
    }

    //проверка на конец карты
    bool checkRight()
    {
        if(playerTransform.position.x> transform.position.x + 0.5f)
        {
            return true;
        }
        return false;
    }
    bool checkLeft()
    {
        if (playerTransform.position.x < transform.position.x - 0.5f)
        {
            return true;
        }
        return false;
    }
    bool checkDown()
    {
        if(playerTransform.position.y < transform.position.y -0.25f)
        {
            return true;
        }
        return false;
    }
    bool checkUp()
    {
        if (playerTransform.position.y > transform.position.y + 0.25f)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        try
        {
            //когда герой умирает переводим камеру на другого игрока
            if (!swapCharacter && !Character.isLife)
            {
                character = GameObject.FindGameObjectWithTag("PlayerClient");
                playerTransform = character.GetComponent<Transform>();
                swapCharacter = true;
            }
            //лучи камеры 
            cameraRayRight = Physics2D.Raycast(transform.position, transform.right, distanceX, whatIsGround);
            cameraRayLeft = Physics2D.Raycast(transform.position, -transform.right, distanceX, whatIsGround);
            cameraRayUp = Physics2D.Raycast(transform.position, transform.up, distanceY, whatIsGround);
            cameraRayDown = Physics2D.Raycast(transform.position, -transform.up, distanceY, whatIsGround);

            Debug.DrawRay(transform.position, -transform.right * distanceX, Color.yellow);
            Debug.DrawRay(transform.position, transform.right * distanceX, Color.yellow);
            Debug.DrawRay(transform.position, -transform.up * distanceY, Color.yellow);
            Debug.DrawRay(transform.position, transform.up * distanceY, Color.yellow);

            Debug.DrawRay(new Vector3(transform.position.x + 0.5f, transform.position.y - 0.25f, transform.position.z), transform.up * 0.5f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x - 0.5f, transform.position.y - 0.25f, transform.position.z), transform.up * 0.5f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x - 0.5f, transform.position.y - 0.25f, transform.position.z), transform.right * 1f, Color.red);
            Debug.DrawRay(new Vector3(transform.position.x - 0.5f, transform.position.y + 0.25f, transform.position.z), transform.right * 1f, Color.red);

            //движение камеры по горизонтали
            if (cameraRayRight.collider == null && cameraRayLeft.collider == null)
            {
                if (checkRight() || checkLeft())
                {
                    //transform.position = Vector3.Lerp(transform.position, playerTransform.position, Time.deltaTime);
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerTransform.position.x, Time.deltaTime), transform.position.y, transform.position.z);
                }
            }
            else
            {
                if ((cameraRayRight.collider != null && checkLeft()) || (cameraRayLeft.collider != null && checkRight()))
                {
                    transform.position = new Vector3(Mathf.Lerp(transform.position.x, playerTransform.position.x, Time.deltaTime), transform.position.y, transform.position.z);
                }

            }

            //движение камеры по вертикали
            if (cameraRayUp.collider == null && cameraRayDown.collider == null)
            {

                if (checkUp() || checkDown())
                {
                    transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, playerTransform.position.y, Time.deltaTime), transform.position.z);
                }

            }
            else
            {
                if ((cameraRayDown.collider != null && checkUp()) || (cameraRayUp.collider != null && checkDown()))
                {
                    transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, playerTransform.position.y, Time.deltaTime), transform.position.z);
                }
            }
        }
        catch(Exception){}

    }
}
