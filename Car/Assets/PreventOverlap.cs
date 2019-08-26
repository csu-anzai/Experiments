using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PreventOverlap : MonoBehaviour
{
    public int priorityOrder;
    public bool waiting = false;

    bool infrontHero= false;

    private void Update()
    {
        transform.position = GetComponentInParent<AIPath>().steeringTarget;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Test"))
        {
            waiting = true;
            infrontHero= true;
            print("preventing");
        }
        else if(collision.gameObject.layer == LayerMask.NameToLayer("Preventer"))   // Tag로 해도 될듯함
        {
            if (collision.GetComponent<PreventOverlap>().priorityOrder > priorityOrder)
                waiting = true;
            print("preventing");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Test"))
        {
            waiting = false;
            infrontHero= false;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Preventer") && !infrontHero)
        {
            waiting = false;
        }
    }
}
