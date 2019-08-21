using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;
using Cinemachine;

public class HeroController : EnemyController
{
    public List<Transform> patrolPoints;
    public List<Transform> foundMonsters;
    public LayerMask mask;
    public int damage;
    public List<GameObject> emoteList;
    public ParticleSystem hitFX;

    AIDestinationSetter aiTarget;
    AIPath ai;
    CinemachineImpulseSource impulseManager;

    RaycastHit2D hit;
    Vector2 enemyDir;
    Vector2 targetVector;
    int moveCount = 0;
    int dest = 0;
    bool detectEnemy = false;

    void Start()
    {
        ai = GetComponent<AIPath>();
        aiTarget = GetComponent<AIDestinationSetter>();
        impulseManager = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        if (foundMonsters.Count == 0)
        {
            tracking = false;
            anim.SetBool("OnTracking", false);
            onPatrol = true;
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

            if (!detectEnemy && !onPatrol)
            {
                moveCount = 0;
                Tracking();
            }
            else if (!detectEnemy && onPatrol)
            {
                moveCount = 0;
                Patrolling();
            }
            else
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

                tracking = true;
                anim.SetBool("OnTracking", true);
                onPatrol = false;
            }
            //targetVector = collision.transform.position;            
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 0.7f);
    //    Gizmos.color = Color.magenta;

    //    Gizmos.DrawRay(transform.position, targetVector - (Vector2)transform.position);
    //}

    private void Attack(Vector2 enemyDir)
    {
        Vector2 dir = enemyDir - (Vector2)transform.position;
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.y);
        anim.SetTrigger("Attack");

        impulseManager.GenerateImpulse();
        hitFX.transform.position = enemyDir;
        hitFX.Play();

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

    private void Tracking()
    {
        Vector2 dir = ai.steeringTarget - transform.position;
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.y);
        if (tracking)
        {
            transform.localPosition = ai.steeringTarget;
        }
    }

    private void Patrolling()
    {
        if (aiTarget.target.tag != "Waypoint")
        {
            dest = 0;
            aiTarget.target = patrolPoints[dest];
        }
        
        if (transform.localPosition == patrolPoints[dest].position)
        {
            if (dest + 1 != patrolPoints.Count)
            {
                dest += 1;
                aiTarget.target = patrolPoints[dest];
            }
            else
            {
                dest = 0;
                aiTarget.target = patrolPoints[dest];
            }
        }

        Vector2 dir = ai.steeringTarget - transform.position;
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.y);
        transform.localPosition = ai.steeringTarget;
    }

    internal void ActivateEmote(int id) => StartCoroutine(Emote(id));

    IEnumerator Emote(int id)
    {
        if (id == 0)
        {
            GetComponent<AudioSource>().Play();
        }
        emoteList[id].SetActive(true);
        yield return new WaitForSeconds(1f);
        emoteList[id].SetActive(false);
    }
}
