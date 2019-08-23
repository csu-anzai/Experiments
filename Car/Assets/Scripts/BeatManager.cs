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
        if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount >= 1) && !bgmOn)
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
            queueSign++;

            StartCoroutine(ResetFX());

            term += beatTerm;
            lastBeat = songTime;            
        }

        judgeTime = (songTime - lastBeat) * 100;

        if(judgeTime >= 0 && judgeTime <= 20)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("judgeTime: " + judgeTime);
        }
    }

    IEnumerator ResetFX()
    {
        yield return null;

        pointer.GetChild(0).localScale *= 40f; // BeatFX
        pointer.GetChild(0).DOScale(Vector3.one, (float)beatTerm - 0.1f);
        pointer.GetChild(1).localScale *= 1.5f; // CrossHair
        pointer.GetChild(1).DOScale(Vector3.one, (float)beatTerm - 0.1f);
    }
}
