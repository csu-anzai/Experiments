﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChasePlayer : EnemyController
{
    public Transform player;

    PlayerController pc;
    CinemachineVirtualCamera cam;

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        pc = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        //Vector3 newPos = Vector3.Lerp(transform.position, player.position, 0.25f);
        //newPos.z = -10f;
        //transform.position = newPos;

        cam.m_Lens.OrthographicSize = Mathf.Lerp(cam.m_Lens.OrthographicSize, pc.line.Count + 4, 0.5f);
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize, 5, 10);
    }
}
