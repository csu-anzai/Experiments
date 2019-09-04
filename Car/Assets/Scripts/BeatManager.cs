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
    public int queueSign;
    public bool isMovingCurrentBeat = false;
    public bool preMiss = false;
    public Queue<Vector3> fool;
    public bool timeOver;
    public GameManager gm;

    double beatTerm;
    double lastBeat;
    double judgeTime;
    double previousFrameTime;
    double lastReportedPlayheadPosition;
    double songTime;
    double term;
    double copyTerm;
    bool bgmOn = false;
    
    Vector3Int currentCell;
    PlayerController playerManager;
    AudioSource mySong;

    public double remain;
    public double judge2;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        playerManager = player.GetComponent<PlayerController>();
    }

    void Start()
    {
        beatTerm = 60 / (double)bpm;
        fool = new Queue<Vector3>();
        mySong = GetComponent<AudioSource>();
        previousFrameTime = GetTimer();
        lastReportedPlayheadPosition = 0;
        term = mySong.time + beatTerm + timeOffset;
        copyTerm = term;
        queueSign = 0;
    }

    private double GetTimer()
    {
        return mySong.time;
    }

    void Update()
    {
        remain = term - songTime;

        if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount >= 1) && !bgmOn)
        {
            gm.nextStatus = GameManager.state.Playing;
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
            //print("judgeTime: " + judge2);
            clap.Play();
            queueSign++;
            term += beatTerm;
            lastBeat = songTime; 
        }

        judgeTime = (songTime - lastBeat) * 100;
        //print((copyTerm - songTime) * 100);
        judge2 = (copyTerm - songTime) * 100;

        if (judge2 <= 15 && judge2 >= -15 && bgmOn)
        {
            if (!movable && !preMiss)
            {
                movable = true;
                isMovingCurrentBeat = false;
            }
        }
        else if (bgmOn)
        {
            movable = false;
        }

        //if (judgeTime >= 0 && judgeTime <= 20 && bgmOn)
        //{
        //    if (!movable)
        //    {
        //        movable = true;
        //        isMovingCurrentBeat = false;
        //    }
        //}
        //else if (bgmOn)
        //{
        //    movable = false;
        //}

        if (judge2 < -30)
        {
            copyTerm = term;
            preMiss = false;
            timeOver = true;
        }
        else
        {
            timeOver = false;
        }

        //if (judgeTime > 40)
        //{
        //    copyTerm = term;
        //    timeOver = true;
        //}
        //else
        //{
        //    timeOver = false;
        //}

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("judgeTime: " + judge2);
        }
    }
}
