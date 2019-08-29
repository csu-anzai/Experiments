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
    bool bgmOn = false;
    
    Vector3Int currentCell;
    PlayerController playerManager;
    AudioSource mySong;

    public double remain;

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
            clap.Play();
            queueSign++;
            term += beatTerm;
            lastBeat = songTime;            
        }

        judgeTime = (songTime - lastBeat) * 100;

        if(judgeTime >= 0 && judgeTime <= 20 && bgmOn)
        {
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

        if (judgeTime > 20)
        {
            timeOver = true;
        }
        else
        {
            timeOver = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("judgeTime: " + judgeTime);
        }
    }
}
