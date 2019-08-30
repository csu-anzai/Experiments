using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MonsterController : Character
{
    public int damage;
    public Vector2 originPos;
    public ParticleSystem stompFX;

    int front;
    int me;

    MonsterController frontMonsterCtrl;
    PlayerController playerCtrl;
    AIPath ai;

    List<Transform> line;
    bool isConneted = false;
    bool untouchable = false;

    private void Start()
    {
        originPos = transform.position;
        ai = GetComponent<AIPath>();
    }

    private void Update()
    {
        if (queueSign != CheckQueueSign())
        {
            anim.SetTrigger("Dancing");
            queueSign++;
        }

        if (line != null && front == 0 && isConneted && !untouchable)
        {
            previousPos = transform.position;
            transform.position = playerCtrl.previousPos;
        }
        else if(line != null && front != -1 && isConneted && !untouchable)
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
        if (collision.tag == "Player" && !isConneted && !untouchable)
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

            playerCtrl = collision.GetComponent<PlayerController>();
            playerCtrl.stompFX.Add(stompFX);
            line.Add(transform);
            parentPlayer = collision.transform;
            gameObject.layer = LayerMask.NameToLayer("Monsters");
            isConneted = true;

        }
    }

    internal IEnumerator ResetPosition()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        untouchable = true;
        isConneted = false;
        line.Remove(transform);
        playerCtrl.stompFX.Remove(stompFX);
        ai.canSearch = true;
        ai.canMove = true;        
        yield return new WaitForSeconds(3f);
        untouchable = false;
        transform.position = originPos;
        ai.canSearch = false;
        ai.canMove = false;
    }
}
