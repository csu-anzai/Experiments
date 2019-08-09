using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.Lerp(transform.position, player.position, 0.25f);
        newPos.z = -10f;
        transform.position = newPos;
    }
}
