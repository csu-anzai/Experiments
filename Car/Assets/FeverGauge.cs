using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeverGauge : MonoBehaviour
{
    public FeverManager fm;

    Image gauge;

    private void Awake()
    {
        gauge = GetComponent<Image>();
    }

    void Update()
    {
        gauge.fillAmount = Mathf.Lerp(gauge.fillAmount, fm.feverGaugeCount * 0.01f, Time.deltaTime * 10);
    }
}
