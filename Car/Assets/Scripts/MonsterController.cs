using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : Character
{
    int front;
    int me;
    public int damage;

    MonsterController frontMonsterCtrl;
    PlayerController playerCtrl;

    List<Transform> line;
    bool isConneted = false;
    Vector2 originPos;

    private void Start()
    {
        originPos = transform.position;        
    }

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
            parentPlayer = collision.transform;
            gameObject.layer = LayerMask.NameToLayer("Monsters");
            isConneted = true;

        }

        if (collision.tag == "Hero")
        {
            line[0].GetComponent<PlayerController>().Damaged(damage);
        }
    }

    internal void ResetPosition()
    {
        line.Remove(transform);
        transform.position = originPos;
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        isConneted = false;
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
