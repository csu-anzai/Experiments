using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Queue<Vector3Int> targetCells;
    public List<Transform> line;
    public Transform pointer;
    public Vector3Int beforePos;
    public Vector3 previousPos;

    bool overlap = false;

    Vector3 dir;
    Vector3 w;
    BeatManager1 beatManager;
    
    private void Awake()
    {
        beatManager = GameObject.FindObjectOfType<BeatManager1>();
    }

    private void Start()
    {
        line = new List<Transform>();
        line.Add(transform);
        targetCells = new Queue<Vector3Int>();
        dir = transform.position;
    }

    private void Update()
    {
        CheckPointerPosition();
        if(!beatManager.isMovingCurrentBeat)
        {
            Movement();
        }
    }

    private void Movement()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && beatManager.movable)
        {
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(Vector3.up * 2);
            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && beatManager.movable)
        {
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(Vector3.down * 2);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && beatManager.movable)
        {
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(Vector3.left * 2);
            return;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && beatManager.movable)
        {
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(Vector3.right * 2);
            return;
        }

        if (Input.touchCount == 1 && beatManager.movable)
        {
            previousPos = transform.position;
            beatManager.isMovingCurrentBeat = true;
            transform.Translate(Vector3.up * 2);
            return;
        }
    }


    public void MakeDirection()
    {
        if (targetCells.Count != 0)
        {
            dir = targetCells.Dequeue();
        }
    }

    private void CheckPointerPosition()
    {
        overlap = (transform.position == pointer.position) ? true : false;
    }
}
