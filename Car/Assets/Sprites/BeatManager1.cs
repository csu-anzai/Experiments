using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;

public class BeatManager1 : MonoBehaviour
{
    public int bpm;
    public Transform player;
    public Transform pointer;
    public AudioSource clap;
    [Range(0f, 100f)]
    public double timeOffset;
    public bool movable = false;
    public Tile targetTile;
    public Tile originTile;
    public Tilemap tileMap;
    public GameObject water;
    public Queue<GameObject> fool;

    double beatTerm;
    double nextTime;
    double offset;
    double lastBeat;
    double judgeTime;

    int countL = 0;
    int countR = 0;

    bool perfectTime = false;
    bool onStart = false;
    bool bgmOn = false;
    bool isMovingCurrentBeat = false;

    Vector3Int currentCell;

    PlayerController playerManager;

    // Test
    double previousFrameTime;
    double lastReportedPlayheadPosition;
    double songTime;
    double term;
    AudioSource mySong;
    // Test

    private void Awake()
    {
        Application.targetFrameRate = 60;
        playerManager = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        beatTerm = 60 / (double)bpm;
        nextTime = AudioSettings.dspTime + beatTerm;
        fool = new Queue<GameObject>();

        // Test
        mySong = GetComponent<AudioSource>();
        previousFrameTime = GetTimer();
        lastReportedPlayheadPosition = 0;
        term = mySong.time + beatTerm + timeOffset;
        // Test
    }

    private double GetTimer()
    {
        return mySong.time;
    }

    void Update()
    {
        //print(GetTimer());

        if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount >= 2) && !bgmOn)
        {
            mySong.Play();
            bgmOn = true;
        }

        songTime += GetTimer() - previousFrameTime;
        previousFrameTime = GetTimer();

        if (mySong.time != lastReportedPlayheadPosition)
        {
            songTime = (songTime + mySong.time) / 2;
            lastReportedPlayheadPosition = mySong.time;
        }

        if (songTime >= term && bgmOn)
        {
            clap.Play();
            playerManager.Following();
            StartCoroutine(ResetFX());

            term += beatTerm;
            lastBeat = songTime;            
        }

        judgeTime = (songTime - lastBeat) * 100;

        if(judgeTime >= 0 && judgeTime <= 20)
        {
            if (!isMovingCurrentBeat)
            {
                Movement();
            }
            if (!movable)
            {
                movable = true;
                isMovingCurrentBeat = false;
            }
        }
        else
        {
            movable = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("judgeTime: " + judgeTime);
        }

        //if (onStart)
        //{
        //    if (AudioSettings.dspTime >= nextTime && bgmOn)
        //    {
        //        clap.Play();
        //        nextTime += beatTerm;
        //        lastBeat = AudioSettings.dspTime;
        //        currentCell = tileMap.WorldToCell(pointer.position);
        //        //Tiling(currentCell);
        //        playerManager.targetCells.Enqueue(currentCell);
        //        playerManager.Following();
        //        //playerManager.MakeDirection();                
        //        StartCoroutine(ResetFX());
        //    }
        //    else if (AudioSettings.dspTime >= nextTime && !bgmOn)
        //    {
        //        bgmOn = true;
        //        clap.Play();
        //        StartCoroutine(Playing());
        //        //nextTime += beatTerm;
        //        nextTime = AudioSettings.dspTime + beatTerm;
        //    }

        //    judgeTime = (AudioSettings.dspTime - lastBeat) * bpm;

        //    //print("No Abs: " + judgeTime);
        //    //if ((judgeTime >= 0 && judgeTime <= 25) || (judgeTime >= 50))
        //    if (judgeTime >= 55 || judgeTime <= 20)      // center = 10
        //    {
        //        if (!isMovingCurrentBeat)
        //        {
        //            Movement();
        //        }
        //        if (!movable)
        //        {
        //            movable = true;
        //            isMovingCurrentBeat = false;
        //        }
        //    }
        //    else
        //    {
        //        movable = false;
        //    }

    }
    

    private void Movement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && movable)
        {
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.up * 2);
            SpriteTiling();
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && movable)
        {
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.down * 2);
            SpriteTiling();
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && movable)
        {
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.left * 2);
            SpriteTiling();
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && movable)
        {
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.right * 2);
            SpriteTiling();
            return;
        }

        if (Input.touchCount == 1 && movable)
        {
            isMovingCurrentBeat = true;

            pointer.Translate(Vector3.up * 2);
            SpriteTiling();
            return;
        }
    }

    private void Tiling(Vector3Int position)
    {
        tileMap.SetTile(position + new Vector3Int(-1, 0, 0), targetTile);
        tileMap.SetTile(position + new Vector3Int(0, 0, 0), targetTile);
        tileMap.SetTile(position + new Vector3Int(-1, -1, 0), targetTile);
        tileMap.SetTile(position + new Vector3Int(0, -1, 0), targetTile);
        tileMap.SetTile(playerManager.beforePos + new Vector3Int(-1, 0, 0), originTile);
    }

    private void SpriteTiling()
    {
        var w = Instantiate(water, pointer.position, Quaternion.identity);
        fool.Enqueue(w);
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
