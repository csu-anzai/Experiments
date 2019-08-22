using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFunctionCall : MonoBehaviour
{
    HeroController hc;

    private void Awake()
    {
        hc = GetComponentInParent<HeroController>();
    }

    public void Attack()
    {
        hc.Attack();
    }
}
