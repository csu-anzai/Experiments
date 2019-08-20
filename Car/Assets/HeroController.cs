using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class HeroController : EnemyController
{
    public LayerMask mask;
    public int damage;

    RaycastHit2D hit;
    Vector2 enemyDir;
    AIPath ai;
    int moveCount = 0;
    bool detectEnemy = false;
    Vector2 targetVector;

    public List<Transform> foundMonsters;

    void Start()
    {
        ai = GetComponent<AIPath>();
        foundMonsters = new List<Transform>();
    }

    void Update()
    {
        if (foundMonsters.Count == 0)
        {
            tracing = false;
        }
        else
        {
            Finding(foundMonsters);
        }

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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("Monsters"))
    //    {
    //        Finding(collision.transform);
    //        foundMonsters.Add(collision.transform);
    //    }
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monsters"))
        {
            foundMonsters.Remove(collision.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monsters"))
        {            
            if(!foundMonsters.Contains(collision.transform))
            {
                foundMonsters.Add(collision.transform);
            }

            //Finding(foundMonsters);

            if (target != null)
            {
                GetComponent<AIDestinationSetter>().target = target;
                targetVector = target.position;

                tracing = true;
            }
            //targetVector = collision.transform.position;            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 0.7f);
        Gizmos.color = Color.magenta;

        Gizmos.DrawRay(transform.position, targetVector - (Vector2)transform.position);
    }

    private void Attack(Vector2 enemyDir)
    {
        print("Attack: " + enemyDir);       
        var mobs = hit.transform.GetComponent<Character>().parentPlayer.GetComponent<PlayerController>().Damaged(damage);
        detectEnemy = false;

        if (mobs == null)
            return;
        foreach (var m in mobs)
        {
            if(foundMonsters.Contains(m))
            {
                foundMonsters.Remove(m);
            }
        }        
    }

    private void Movement()
    {
        if (tracing)
        {
            transform.localPosition = ai.steeringTarget;
        }
    }
}
