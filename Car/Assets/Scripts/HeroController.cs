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
    public LayerMask heroMask;

    AIDestinationSetter aiTarget;
    AIPath ai;
    CinemachineImpulseSource impulseManager;
    PreventOverlap preventer;

    public RaycastHit2D hit;
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
        preventer = GetComponentInChildren<PreventOverlap>();
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
                print("hit");
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
                AttackReady(enemyDir);
            }   
        }
    }

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

            if (target != null)
            {
                GetComponent<AIDestinationSetter>().target = target;
                targetVector = target.position;

                tracking = true;
                anim.SetBool("OnTracking", true);
                onPatrol = false;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, 0.7f);
    //    Gizmos.color = Color.magenta;

    //    Gizmos.DrawRay(transform.position, targetVector - (Vector2)transform.position);
    //}

    private void AttackReady(Vector2 enemyDir)
    {
        Vector2 dir = enemyDir - (Vector2)transform.position;
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.y);
        anim.SetTrigger("Attack");
        detectEnemy = false;
    }

    private void Tracking()
    {
        //LayerMask heroMask = LayerMask.NameToLayer("Test");
        RaycastHit2D heroHit;
        //RaycastHit2D[] hits;

        //heroHit = Physics2D.Raycast(transform.position, ai.steeringTarget - transform.position, 2f, heroMask);
        heroHit = Physics2D.CircleCast(ai.steeringTarget, 0.4f, Vector2.zero, 0f, heroMask);        

        if (heroHit)
            print("heroHIt");

        Vector2 dir = ai.steeringTarget - transform.position;
        anim.SetFloat("X", dir.x);
        anim.SetFloat("Y", dir.y);
        if (tracking && /*!heroHit*/ !preventer.waiting)
        {
            transform.position = (Vector2)ai.steeringTarget;
        }
    }

    private void Patrolling()
    {
        //LayerMask heroMask = LayerMask.NameToLayer("Test");
        RaycastHit2D heroHit;
        //List<RaycastHit2D> hits;
        //hits = new List<RaycastHit2D>();

        //heroHit = Physics2D.Raycast(transform.position, ai.steeringTarget - transform.position, new ContactFilter2D().SetLayerMask(heroMask), hits);

        heroHit = Physics2D.CircleCast(ai.steeringTarget, 0.4f, Vector2.zero, 0f, heroMask);

        if (heroHit)
            print("heroHIt");

        if (aiTarget.target.tag != "Waypoint")
        {
            dest = 0;
            aiTarget.target = patrolPoints[dest];
        }

        if (Vector2.Distance(transform.position, patrolPoints[dest].position) <= 0.2)
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

        if (/*!heroHit*/!preventer.waiting)
        {
            transform.position = (Vector2)ai.steeringTarget;
        }
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

    public void Attack()
    {
        RaycastHit2D hit2 = Physics2D.CircleCast(transform.position, 0.4f, new Vector2(anim.GetFloat("X"), anim.GetFloat("Y")), 1f, mask);
        if (hit2 && hit2.transform.gameObject.layer == LayerMask.NameToLayer("Monsters"))
        {            
            impulseManager.GenerateImpulse();
            hitFX.transform.position = hit2.transform.position;
            hitFX.Play();

            var mobs = hit2.transform.GetComponent<Character>().parentPlayer.GetComponent<PlayerController>().Damaged(damage);
            if (mobs == null)
                return;
            foreach (var m in mobs)
            {
                if (foundMonsters.Contains(m))
                {
                    foundMonsters.Remove(m);
                }
            }
        }
    }
}
