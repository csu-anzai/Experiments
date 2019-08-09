using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Queue<Vector3Int> targetCells;
    public Transform pointer;

    bool overlap = false;

    Vector3 dir;

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
        transform.position = dir;
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
