using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverButton : MonoBehaviour
{
    Animator feverButton;

    private void Awake()
    {
        feverButton = GetComponent<Animator>();
    }

    public void OnFever(bool status)
    {
        if (status == true)
        {
            feverButton.SetBool("OnFever", true);
        }
        else
        {
            feverButton.SetBool("OnFever", false);
        }
    }
}
