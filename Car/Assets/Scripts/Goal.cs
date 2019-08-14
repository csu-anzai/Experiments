using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    BeatManager system;

    private void Awake()
    {
        system = FindObjectOfType<BeatManager>();
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
