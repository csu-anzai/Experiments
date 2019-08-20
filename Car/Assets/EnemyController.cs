using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    public float attackRange;
    public float detectRange;

    public bool tracing = false;
        

    protected Transform target;
    

    protected void Finding(List<Transform> foundMonsters)
    {
        RaycastHit2D hit;
        print("finding");

        foreach(var m in foundMonsters)
        {
            hit = Physics2D.Raycast(transform.position, (m.position - transform.position).normalized);
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Monsters"))
            {
                print("hit");
                tracing = true;
                target = m;
                break;
            }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                print(hit.transform.name);
                //continue;
            }
            target = null;
            tracing = false;
        }

        //hit = Physics2D.Raycast(transform.position, (target.position - transform.position).normalized);
    }

}
