using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Objects
{
    Vector3 Target;

    private void Start()
    {
        Target = transform.position + Vector3.up * 4;
    }

    public override void Action()
    {
        StartCoroutine(ElevatorAction());
    }

    IEnumerator ElevatorAction()
    {
        while (transform.position != Target)
        {
            transform.position = Vector3.Lerp(transform.position, Target, Time.deltaTime * 10);
            yield return null;
        }
    }
}
