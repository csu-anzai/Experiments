using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatPointer : MonoBehaviour
{
    [Range(1, 50)]
    public int speed;
    public BeatManager bm;
    public Transform end;

    private void Update()
    {
        transform.localScale = end.localScale + new Vector3((float)bm.remain * speed, (float)bm.remain * speed, 0);
    }
}
