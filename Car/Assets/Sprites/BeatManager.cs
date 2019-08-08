using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class BeatManager : MonoBehaviour
{
    public int bpm;
    public Transform player;
    public Transform pointer;
    public AudioSource clap;
    [Range(0f, 100f)]
    public double timeOffset;
    public bool movable = false;
    public Tile targetTile;
    public Tilemap tileMap;

    double beatTerm;
    double nextTime;
    double offset;
    double lastBeat;
    bool onStart = false;
    bool bgmOn = false;

    Vector3Int currentCell;

    PlayerController playerManager;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        playerManager = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        beatTerm = 60 / (double)bpm;
        nextTime = AudioSettings.dspTime + beatTerm;
        
    }

    private void Update()
    {
        Movement();
    }

    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !onStart)
        {
            onStart = true;
        }

        if (onStart)
        {
            //deltaTime = GetComponent<AudioSource>().time - lastTime;
            //print("deltaTime: " + deltaTime);
            //timer += deltaTime;
            //print("timer: " + timer);

            //if (timer >= beatTerm)
            //{
            //    perfectTime = true;
            //    movable = true;
            //    clap.Play();
            //    //StartCoroutine(beatAnim());
            //    remainTime = beatTerm;
            //    StartCoroutine(ResetFX());

            //    offset = timer - beatTerm;

            //    timer -= beatTerm;
            //}

            if (AudioSettings.dspTime >= nextTime && bgmOn)
            {
                print("Clap");
                clap.Play();
                nextTime += beatTerm;
                lastBeat = AudioSettings.dspTime;
                currentCell = tileMap.WorldToCell(pointer.position);
                Tiling(currentCell);
                playerManager.targetCells.Enqueue(currentCell);
                playerManager.Following();
                playerManager.MakeDirection();
                StartCoroutine(ResetFX());
            }
            else if (AudioSettings.dspTime >= nextTime && !bgmOn)
            {
                bgmOn = true;
                clap.Play();
                StartCoroutine(Playing());
                //nextTime += beatTerm;
                nextTime = AudioSettings.dspTime + beatTerm;
            }

            //lastTime = GetComponent<AudioSource>().time + offset;
            //print("offset: " + offset);
            //offset = 0;

            var judgeTime = Math.Abs((AudioSettings.dspTime - lastBeat) * bpm);

            if (judgeTime >= 10 && judgeTime <= 35)
            {
                print("On: " + judgeTime);
                if (!movable)
                {
                    movable = true;
                }
            }
            else
            {
                print("Off: " + judgeTime);
                movable = false;
            }

            //if (Input.GetKeyDown(KeyCode.Space) && (nextTime * 0.95) <= AudioSettings.dspTime && AudioSettings.dspTime < nextTime)
            //{
            //    player.Translate(Vector3.left);
            //    movable = false;
            //    print("Audio: " + AudioSettings.dspTime);
            //    print("nextTime: " + nextTime * 0.95);
            //}

            //if (Input.GetKeyDown(KeyCode.Space) && (nextTime * 1.05) >= AudioSettings.dspTime && AudioSettings.dspTime > nextTime)
            //{
            //    player.Translate(Vector3.right);
            //    movable = false;
            //    print("Audio: " + AudioSettings.dspTime);
            //    print("nextTime: " + nextTime * 1.05);
            //}

            //if (Input.GetKeyDown(KeyCode.Space) && AudioSettings.dspTime == nextTime)
            //{
            //    player.Translate(Vector3.up);
            //    movable = false;
            //    print("Audio: " + AudioSettings.dspTime);
            //    print("nextTime: " + nextTime);
            //}
        }
    }

    private void Movement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && movable)
        {
            pointer.Translate(Vector3.up * 2);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && movable)
        {
            pointer.Translate(Vector3.down * 2);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && movable)
        {
            pointer.Translate(Vector3.left * 2);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && movable)
        {
            pointer.Translate(Vector3.right * 2);
        }
    }

    private void Tiling(Vector3Int position)
    {
        tileMap.SetTile(position + new Vector3Int(-1, 0, 0), targetTile);
        tileMap.SetTile(position + new Vector3Int(0, 0, 0), targetTile);
        tileMap.SetTile(position + new Vector3Int(-1, -1, 0), targetTile);
        tileMap.SetTile(position + new Vector3Int(0, -1, 0), targetTile);
    }

    IEnumerator Playing()
    {
        yield return new WaitForSeconds((float)timeOffset);
        GetComponent<AudioSource>().Play();
    }

    IEnumerator ResetFX()
    {
        //yield return new WaitForSeconds(0.18f);
        //yield return new WaitForSeconds((float)timeOffset);
        yield return null;

        pointer.GetChild(0).localScale *= 40f; // BeatFX
        pointer.GetChild(0).DOScale(Vector3.one, (float)beatTerm - 0.1f);
        pointer.GetChild(1).localScale *= 1.5f; // CrossHair
        pointer.GetChild(1).DOScale(Vector3.one, (float)beatTerm - 0.1f);
    }
}
