using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    BeatManager1 system;

    private void Awake()
    {
        system = FindObjectOfType<BeatManager1>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            print("Finish");
            system.gameObject.SetActive(false);
        }
    }
}
