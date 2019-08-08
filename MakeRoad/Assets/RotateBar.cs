using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBar : Objects
{
    Quaternion Target;

    private void Start()
    {
        Target = Quaternion.Euler(-37f, 0f, 0f);
    }

    public override void Action()
    {
        StartCoroutine(RotateBarAction());
    }

    IEnumerator RotateBarAction()
    {
        while (transform.rotation != Target)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Target, Time.deltaTime * 10);
            yield return null;
        }
    }
}
