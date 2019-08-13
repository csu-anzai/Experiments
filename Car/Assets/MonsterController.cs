using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public Vector3 previousPos;
    int front;
    int me;
    List<Transform> line;
    bool isConneted = false;

    private void Update()
    {
        if(line != null && front != -1 && isConneted)
        {
            transform.position = line[front].GetComponent<PlayerController>().previousPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isConneted)
        {
            line = collision.GetComponent<PlayerController>().line;
            me = line.Count;
            front = me - 1;
            line.Add(transform);
            isConneted = true;

        }
    }
}
