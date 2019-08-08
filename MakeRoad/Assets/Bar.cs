using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : Objects
{
    Vector3 Target;
    Vector3 screenPoint;

    private void Start()
    {
        Target = transform.position - Vector3.right * 2;
    }

    private void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        //Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //point.z = GameObject.FindObjectOfType<Bar>().transform.position.z;
        //point.y = GameObject.FindObjectOfType<Bar>().transform.position.y;
        //GameObject.FindObjectOfType<Bar>().transform.position = point;

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
        curPosition.z = transform.position.z;
        curPosition.y = transform.position.y;

        transform.position = curPosition;
    }

    public override void Action()
    {
        StartCoroutine(BarAction());
    }

    IEnumerator BarAction()
    {
        while (transform.position != Target)
        {
            transform.position = Vector3.Lerp(transform.position, Target, Time.deltaTime * 3);
            yield return null;
        }
    }
}
