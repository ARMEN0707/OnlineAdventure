﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerClient")
        {
            collision.gameObject.SetActive(false);
        }else
        {
            Destroy(collision.gameObject);
        }
        
    }



}
