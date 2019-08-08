using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BeatManager : MonoBehaviour
{
    public int bpm;
    public Transform player;
    public Transform beatFX;
    public AudioSource clap;
    [Range(0f, 100f)]
    public double timeOffset;

    double beatTerm;
    double remainTime;

    double timer;
    double lastTime;
    double deltaTime;
    double nextTime;
    double offset;
    double lastBeat;

    int countL = 0;
    int countR = 0;

    bool perfectTime = false;
    public bool movable = false;
    bool onStart = false;
    bool bgmOn = false;

    //AudioSource source;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        timer = 0d;
        lastTime = 0d;
        deltaTime = 0d;

        beatTerm = 60 / (double)bpm;
        remainTime = beatTerm;
        nextTime = AudioSettings.dspTime + beatTerm;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !onStart)
        {
            onStart = true;
        }

        if (onStart)
        {
            if (AudioSettings.dspTime >= nextTime && bgmOn)
            {
                perfectTime = true;
                print("Clap");
                clap.Play();
                nextTime += beatTerm;
                lastBeat = AudioSettings.dspTime;
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

            var judgeTime =(AudioSettings.dspTime - lastBeat) * bpm;

            //print("No Abs: " + judgeTime);
            //if ((judgeTime >= 0 && judgeTime <= 25) || (judgeTime >= 50))
            if(judgeTime >= 55 || judgeTime <= 20)      // center = 10
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

            if (Input.GetKeyDown(KeyCode.RightArrow) && movable)
            {
                movable = false;
                //print(judgeTime);
                player.Translate(Vector3.right);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && movable)
            {
                movable = false;
                //print(judgeTime);
                player.Translate(Vector3.left);
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
    

    IEnumerator Playing()
    {
        yield return new WaitForSeconds((float)timeOffset);
        GetComponent<AudioSource>().Play();
    }

    IEnumerator ResetFX()
    {
        //yield return new WaitForSeconds(0.18f);
        yield return new WaitForSeconds((float)timeOffset);
        perfectTime = false;

        beatFX.localScale *= 20f;
        beatFX.DOScale(Vector3.one, (float)beatTerm - 0.1f);
    }
}
