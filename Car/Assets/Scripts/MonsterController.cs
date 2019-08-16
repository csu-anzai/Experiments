﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : Character
{
    int front;
    int me;

    MonsterController frontMonsterCtrl;
    PlayerController playerCtrl;

    List<Transform> line;
    bool isConneted = false;

    private void Update()
    {
        if (queueSign != CheckQueueSign())
        {
            anim.SetTrigger("Dancing");
            queueSign++;
        }

        if (line != null && front == 0 && isConneted)
        {
            previousPos = transform.position;
            transform.position = playerCtrl.previousPos;
        }
        else if(line != null && front != -1 && isConneted)
        {
            if(frontMonsterCtrl.previousPos != line[front].position)
            {
                previousPos = transform.position;
                transform.position = frontMonsterCtrl.previousPos;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isConneted)
        {
            line = collision.GetComponent<PlayerController>().line;
            me = line.Count;
            front = me - 1;

            if (front == 0)
            {
                playerCtrl = line[front].GetComponent<PlayerController>();
            }
            else
            {
                frontMonsterCtrl = line[front].GetComponent<MonsterController>();
            }

            line.Add(transform);
            isConneted = true;

        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.transform.tag == "Player" && !isConneted)
    //    {
    //        line = collision.transform.GetComponent<PlayerController>().line;
    //        me = line.Count;
    //        front = me - 1;

    //        if (front == 0)
    //        {
    //            playerCtrl = line[front].GetComponent<PlayerController>();
    //        }
    //        else
    //        {
    //            frontMonsterCtrl = line[front].GetComponent<MonsterController>();
    //        }

    //        line.Add(transform);
    //        isConneted = true;

    //    }
    //}
}