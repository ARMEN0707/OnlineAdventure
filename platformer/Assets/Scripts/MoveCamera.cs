using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Horizontal"))
        {
            Vector3 move = Vector3.right * Input.GetAxis("Horizontal");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + move, Time.deltaTime*speed);
        }
        if(Input.GetButton("Vertical"))
        {
            Vector3 move = Vector3.up * Input.GetAxis("Vertical");
            transform.position = Vector3.MoveTowards(transform.position, transform.position + move, Time.deltaTime*speed);
        }
    }
}
