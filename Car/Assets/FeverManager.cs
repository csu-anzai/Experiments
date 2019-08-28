using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverManager : MonoBehaviour
{
    public bool isAvailable;
    public ComboUI comboUI;
    public Animator feverButton;

    PlayerController pc;

    public int comboCount = 0;
    public int feverGaugeCount = 0;
    const int maxFeverGaugeCount = 100;


    private void Awake()
    {
        pc = GetComponent<PlayerController>();
    }

    internal void ResetCombo()
    {
        if (feverGaugeCount > 0)
        {
            feverGaugeCount -= 25;
            feverGaugeCount = feverGaugeCount < 0 ? 0 : feverGaugeCount;
        }
        comboCount = 0;
        comboUI.OnComboBreakAnim();
    }

    internal void IncreseCombo()
    {
        if (comboCount >= 10 && !isAvailable)
        {
            feverGaugeCount += 5 + (pc.line.Count - 1);
            if (feverGaugeCount >= 100)
            {
                isAvailable = true;
                feverButton.SetBool("OnFever", true);
                feverGaugeCount = 0;
            }
            else
            {
                feverButton.SetBool("OnFever", false);
            }
        }
        comboCount++;
        comboUI.OnIncreaseAnim();
    }

    internal void ResetFeverGauge()
    {
        feverGaugeCount = 0;
    }
}
