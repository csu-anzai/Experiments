using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSight : MonoBehaviour
{
    HeroController hc;
    CircleCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        hc = GetComponentInParent<HeroController>();
        col = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        col.offset = hc.lastForwardDir * 1.5f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monsters"))
        {
            hc.foundMonsters.Remove(collision.transform);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Monsters"))
        {
            if (!hc.foundMonsters.Contains(collision.transform))
            {
                hc.foundMonsters.Add(collision.transform);
            }

            if (hc.target != null)
            {
                hc.SetTracking();
            }
        }
    }
}
