using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : Character
{
    public List<Transform> line;
    public LayerMask mask;

    RaycastHit2D hit;
    Ray2D upRay;
    Ray2D downRay;
    Ray2D leftRay;
    Ray2D rightRay;

    Vector2 dir;
    Vector2 mDir;
    float horizontal;
    float vertical;
    int damage;

    private void Start()
    {
        line = new List<Transform>();
        line.Add(transform);
    }

    private void Update()
    {
        if(queueSign != CheckQueueSign())
        {
            anim.SetTrigger("Dancing");
            queueSign++;
        }

        if(!beatManager.isMovingCurrentBeat)
        {
            Movement();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Damaged(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Damaged(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Damaged(3);
        }
    }

    private void Movement()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        dir = new Vector2(horizontal, vertical);

        if (beatManager.movable && dir != Vector2.zero && dir.magnitude == 1)
        {
            hit = Physics2D.Raycast(transform.position, dir, 1f, mask);
            if (hit)
            {
                anim.SetTrigger("Fail");
                return;
            }

            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(dir);
            return;
        }

        //if (Input.GetKeyDown(KeyCode.UpArrow) && beatManager.movable)
        //{
        //    previousPos = transform.position;
        //    beatManager.isMovingCurrentBeat = true;
        //    transform.Translate(Vector3.up * 1);
        //    return;
        //}

        //if (Input.GetKeyDown(KeyCode.DownArrow) && beatManager.movable)
        //{
        //    previousPos = transform.position;
        //    beatManager.isMovingCurrentBeat = true;
        //    transform.Translate(Vector3.down * 1);
        //    return;
        //}

        //if (Input.GetKeyDown(KeyCode.LeftArrow) && beatManager.movable)
        //{
        //    previousPos = transform.position;
        //    beatManager.isMovingCurrentBeat = true;
        //    transform.Translate(Vector3.left * 1);
        //    return;
        //}

        //if (Input.GetKeyDown(KeyCode.RightArrow) && beatManager.movable)
        //{
            
        //    previousPos = transform.position;
        //    beatManager.isMovingCurrentBeat = true;
        //    transform.Translate(Vector3.right * 1);
        //    return;
        //}

        //if (Input.touchCount == 1 && beatManager.movable)
        //{
        //    previousPos = transform.position;
        //    beatManager.isMovingCurrentBeat = true;
        //    transform.Translate(Vector3.up * 1);
        //    return;
        //}

        // Mobile ------------------------------------------------------

        float dirx = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float diry = CrossPlatformInputManager.GetAxisRaw("Vertical");
        
        mDir = new Vector2(dirx, diry);        

        if (beatManager.movable && mDir != Vector2.zero && mDir.magnitude == 1)
        {
            print("pass");
            hit = Physics2D.Raycast(transform.position, mDir, 1f, mask);
            if (hit)
            {
                anim.SetTrigger("Fail");
                return;
            }
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(mDir);
            CrossPlatformInputManager.SetAxisZero("Horizontal");
            CrossPlatformInputManager.SetAxisZero("Vertical");
            return;

        }

        // Mobile move Legacy------------------------------------

        //if (dirx != 0 && beatManager.movable)
        //{
        //    previousPos = transform.position;
        //    beatManager.isMovingCurrentBeat = true;
        //    transform.Translate(new Vector3(dirx, 0f, 0f));
        //    CrossPlatformInputManager.SetAxisZero("Horizontal");
        //    return;
        //}

        //if (diry != 0 && beatManager.movable)
        //{
        //    previousPos = transform.position;
        //    beatManager.isMovingCurrentBeat = true;
        //    transform.Translate(new Vector3(0f, diry, 0f));
        //    CrossPlatformInputManager.SetAxisZero("Vertical");
        //    return;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Hero")
        {
            Damaged(damage);
        }
    }

    internal void Damaged(int damage)
    {
        if (damage != 0 && line.Count - damage <= 0)
        {
            print("Game Over");
            beatManager.gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < damage; i++)
        {
            line[line.Count - 1].GetComponent<MonsterController>().ResetPosition();
        }
    }
}
