using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class HeroController : Character
{
    public LayerMask mask;
    public int damage;

    RaycastHit2D hit;
    Vector2 enemyDir;
    AIPath ai;
    int moveCount = 0;
    bool detectEnemy = false;    

    void Start()
    {
        ai = GetComponent<AIPath>();        
    }

    void Update()
    {
        if (queueSign != CheckQueueSign())
        {
            moveCount++;
            queueSign++;
        }

        if (moveCount == 2)
        {
            hit = Physics2D.CircleCast(transform.position, 0.7f, Vector2.zero, 0f, mask);
            if (hit)
            {
                detectEnemy = true;
                enemyDir = hit.transform.position;
            }

            if (!detectEnemy)
            {
                moveCount = 0;
                Movement();
            } else
            {
                moveCount = 0;
                Attack(enemyDir);
            }   
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.7f);
        Gizmos.color = Color.magenta;
    }

    private void Attack(Vector2 enemyDir)
    {
        print("Attack: " + enemyDir);
        hit.transform.GetComponent<Character>().parentPlayer.GetComponent<PlayerController>().Damaged(damage);
        detectEnemy = false;
    }

    private void Movement()
    {
        transform.localPosition = ai.steeringTarget;
    }
}
