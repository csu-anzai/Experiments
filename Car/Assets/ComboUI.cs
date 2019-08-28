using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
    public FeverManager fm;

    Text comboText;
    Animator anim;

    private void Awake()
    {
        comboText = GetComponent<Text>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        comboText.text = fm.comboCount.ToString();
    }

    public void OnIncreaseAnim()
    {
        anim.SetTrigger("OnIncrease");
    }

    public void OnComboBreakAnim()
    {
        anim.SetTrigger("OnComboBreak");
    }
}
