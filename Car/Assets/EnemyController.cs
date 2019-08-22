using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    public float attackRange;
    public float detectRange;

    public bool tracking = false;
    public bool onPatrol = false;

    protected Transform target;
    

    protected void Finding(List<Transform> foundMonsters)
    {
        RaycastHit2D hit;       

        foreach(var m in foundMonsters)
        {            
            hit = Physics2D.Raycast(transform.position, (m.position - transform.position).normalized);
            if (hit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Monsters"))
            {
                tracking = true;
                anim.SetBool("OnTracking", true);
                onPatrol = false;
                target = m;
                break;
            }
            else if (hit && hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {                
                //continue;
            }
            target = null;
            tracking = false;
            anim.SetBool("OnTracking", false);
            onPatrol = true;
        }

        //hit = Physics2D.Raycast(transform.position, (target.position - transform.position).normalized);
    }

}
