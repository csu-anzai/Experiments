using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;

public class PlayerController : Character
{
    public List<Transform> line;
    public List<ParticleSystem> judgeFX;
    public LayerMask mask;

    AudioSource clapSFX;
    FeverManager fm;
    RaycastHit2D hit;
    Vector2 dir;
    Vector2 mDir;
    float horizontal;
    float vertical;
    int damage;

    private void Start()
    {
        fm = GetComponent<FeverManager>();
        clapSFX = GetComponent<AudioSource>();
        parentPlayer = transform;
        line = new List<Transform>();
        line.Add(transform);
    }

    private void Update()
    {
        if (CheckTimeOver() && !beatManager.isMovingCurrentBeat)
        {
            fm.ResetCombo();
            if (!judgeFX[1].isPlaying)
                judgeFX[1].Play();
            beatManager.isMovingCurrentBeat = true;
        }

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

    public void Movement()
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
                fm.ResetCombo();
                if(!judgeFX[1].isPlaying)
                    judgeFX[1].Play();
                beatManager.isMovingCurrentBeat = true;
                return;
            }

            fm.IncreseCombo();
            clapSFX.Play();
            judgeFX[0].Play();
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(dir);
            return;
        }
        else if(!beatManager.movable && dir != Vector2.zero && dir.magnitude == 1)
        {
            if (!judgeFX[1].isPlaying)
                judgeFX[1].Play();
            beatManager.isMovingCurrentBeat = true;
        }


        float dirx = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float diry = CrossPlatformInputManager.GetAxisRaw("Vertical");
        float feverSkill = CrossPlatformInputManager.GetAxisRaw("Fire1");
        
        mDir = new Vector2(dirx, diry);        

        if (beatManager.movable && mDir != Vector2.zero && mDir.magnitude == 1)
        {
            hit = Physics2D.Raycast(transform.position, mDir, 1f, mask);
            if (hit)
            {
                anim.SetTrigger("Fail");
                fm.ResetCombo();
                if (!judgeFX[1].isPlaying)
                    judgeFX[1].Play();
                beatManager.isMovingCurrentBeat = true;
                return;
            }

            fm.IncreseCombo();
            clapSFX.Play();
            judgeFX[0].Play();
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(mDir);
            CrossPlatformInputManager.SetAxisZero("Horizontal");
            CrossPlatformInputManager.SetAxisZero("Vertical");
            return;
        }
        else if(!beatManager.movable && mDir != Vector2.zero && mDir.magnitude == 1)
        {
            if(!judgeFX[1].isPlaying)
                judgeFX[1].Play();
            beatManager.isMovingCurrentBeat = true;
            CrossPlatformInputManager.SetAxisZero("Horizontal");
            CrossPlatformInputManager.SetAxisZero("Vertical");
        }

        if (beatManager.movable && feverSkill == 1 && fm.isAvailable)
        {
            clapSFX.Play();
            judgeFX[0].Play();
            fm.ResetFeverGauge();
            fm.isAvailable = false;
            beatManager.isMovingCurrentBeat = true;
            CrossPlatformInputManager.SetAxisZero("Fire1");
            print("Fire");
            ActiveSkill();
        }
    }

    private void ActiveSkill()
    {
        foreach(var m in line)
        {
            m.GetComponent<Character>().previousPos = transform.position;
            //m.position = transform.position;
            m.position = Vector3.Lerp(m.position, transform.position, 0.9f);
        }
    }

    internal List<Transform> Damaged(int damage)
    {
        List<Transform> mobs = new List<Transform>();
        fm.ResetCombo();

        if (damage != 0 && line.Count - damage <= 0)
        {
            print("Game Over");
            beatManager.gameObject.SetActive(false);
            return mobs;
        }

        for (int i = 0; i < damage; i++)
        {
            mobs.Add(line[line.Count - 1]);
            StartCoroutine(line[line.Count - 1].GetComponent<MonsterController>().ResetPosition());
        }
        return mobs;
    }
}
