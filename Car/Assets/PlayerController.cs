using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Queue<Vector3Int> targetCells;
    public Transform pointer;
    public Vector3Int beforePos;

    bool overlap = false;

    Vector3 dir;
    BeatManager beatManager;
    GameObject w;

    private void Awake()
    {
        beatManager = GameObject.FindObjectOfType<BeatManager>();
    }

    private void Start()
    {
        targetCells = new Queue<Vector3Int>();
        dir = transform.position;
    }

    private void Update()
    {
        CheckPointerPosition();
    }

    public void Following()
    {
        beforePos = Vector3Int.FloorToInt(transform.position);
        transform.position = dir;
        if (w != null)
        {
            Destroy(w);
        }

        if (beatManager.fool.Count != 0)
        {
            //Destroy(beatManager.fool.Dequeue());
            w = beatManager.fool.Dequeue();
            dir = w.transform.position;
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
