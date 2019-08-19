using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class HeroController : Character
{
    AIPath ai;
    int moveCount = 0;

    void Start()
    {
        ai = GetComponent<AIPath>();        
    }

    void Update()
    {
        if (queueSign != CheckQueueSign())
        {
            moveCount++;
            queueSign++;
        }

        if (moveCount == 2)
        {
            moveCount = 0;
            Movement();
        }
    }

    private void Movement()
    {
        transform.localPosition = ai.steeringTarget;
    }
}
