using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    private LayerMask playerMask;
    private RaycastHit2D ray;
    private bool run;
    private Animator chickenAnim;

    public Transform distance;
    public float speed;

    public Vector3 startPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerMask = LayerMask.GetMask("Player");
        chickenAnim = GetComponentInChildren<Animator>();
        startPoint = new Vector3(transform.position.x,transform.position.y,transform.position.z);
    }
    
    void FixedUpdate()
    {
        //движение объекта
        ray = Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y-0.05f), transform.right, Vector2.Distance(transform.position, distance.position),playerMask);
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y - 0.05f), transform.right * Vector2.Distance(transform.position, distance.position), Color.red);
        if(ray.collider!=null)
        {
            transform.position = Vector3.MoveTowards(transform.position, distance.position, Time.fixedDeltaTime * speed);
            run = true;
        }
        else
        {
            run = false;
        }
        chickenAnim.SetBool("Run", run);

    }

}
