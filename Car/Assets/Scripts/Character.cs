using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public BeatManager beatManager;
    public Transform parentPlayer;
    public Vector3 previousPos;

    protected Animator anim;
    protected int queueSign;

    protected void Awake()
    {
        queueSign = 0;
        anim = GetComponentInChildren<Animator>();
    }

    protected int CheckQueueSign()
    {
        return beatManager.queueSign;
    }
}
