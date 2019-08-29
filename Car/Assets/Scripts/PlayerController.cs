using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;
using DG.Tweening;

public class PlayerController : Character
{
    public List<Transform> line;
    public List<ParticleSystem> judgeFX;
    public List<ParticleSystem> stompFX;
    public LayerMask mask;
    public InventoryManager inven;
    public GameManager gm;
    public bool keyboardMode;

    AudioSource clapSFX;
    FeverManager fm;
    RaycastHit2D hit;
    Vector2 dir;
    Vector2 mDir;
    float dirx = 0;
    float diry = 0;
    float horizontal;
    float vertical;
    int damage;
    
    private void Start()
    {
        clapSFX = GetComponent<AudioSource>();
        fm = GetComponent<FeverManager>();
        parentPlayer = transform;
        line = new List<Transform>();
        line.Add(transform);
    }

    private void Update()
    {
        inven.ItemFollowing(transform.GetChild(1));

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
    }

    public void Movement()
    {
        if (keyboardMode)
        {
            dirx = Input.GetAxisRaw("Horizontal");
            diry = Input.GetAxisRaw("Vertical");
        }
        else
        {
            dirx = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            diry = CrossPlatformInputManager.GetAxisRaw("Vertical");
        }

        float feverSkill = CrossPlatformInputManager.GetAxisRaw("Fire1");
        
        mDir = new Vector2(dirx, diry);        

        if (beatManager.movable && mDir != Vector2.zero && mDir.magnitude == 1)
        {
            hit = Physics2D.Raycast(transform.position, mDir, 1f, mask);
            if (hit && hit.transform.tag == "Interactable")
            {
                print(hit.transform.tag);
                hit.transform.GetComponent<Interactable>().Interact();
                anim.SetTrigger("Fail");
                beatManager.isMovingCurrentBeat = true;
                return;
            }
            else if (hit)
            {
                anim.SetTrigger("Fail");
                fm.ResetCombo();
                if (!judgeFX[1].isPlaying)
                    judgeFX[1].Play();
                beatManager.isMovingCurrentBeat = true;
                return;
            }

            if (fm.comboCount >= 10)
            {
                foreach (var fx in stompFX)
                {
                    fx.Play();
                }
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
            fm.feverButton.OnFever(false);
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
            m.DOMove(transform.position, 0.2f);
        }
    }

    internal List<Transform> Damaged(int damage)
    {
        List<Transform> mobs = new List<Transform>();
        fm.ResetCombo();

        if (damage != 0 && line.Count - damage <= 0)
        {
            print("Game Over");
            gm.nextStatus = GameManager.state.Lose;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            inven.AddItem(collision.transform);
        }
    }
}
