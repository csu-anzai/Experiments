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
    }

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
                clap.Play();
                nextTime += beatTerm;
                lastBeat = AudioSettings.dspTime;
                currentCell = tileMap.WorldToCell(pointer.position);
                //Tiling(currentCell);
                playerManager.targetCells.Enqueue(currentCell);
                playerManager.Following();
                //playerManager.MakeDirection();                
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

            judgeTime =(AudioSettings.dspTime - lastBeat) * bpm;

            //print("No Abs: " + judgeTime);
            //if ((judgeTime >= 0 && judgeTime <= 25) || (judgeTime >= 50))
            if(judgeTime >= 55 || judgeTime <= 20)      // center = 10
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
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.up * 2);
            SpriteTiling();
            print("No Abs: " + judgeTime);
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && movable)
        {
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.down * 2);
            SpriteTiling();
            print("No Abs: " + judgeTime);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && movable)
        {
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.left * 2);
            SpriteTiling();
            print("No Abs: " + judgeTime);
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && movable)
        {
            isMovingCurrentBeat = true;
            
            pointer.Translate(Vector3.right * 2);
            SpriteTiling();
            print("No Abs: " + judgeTime);
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
