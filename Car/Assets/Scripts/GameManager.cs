using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BeatManager system;
    public Count counter;
    public List<Transform> savedMonsters;

    public int goalCount;
    public int currentCount = 0;

    private void Awake()
    {
        counter.Counting(currentCount, goalCount);
        system = FindObjectOfType<BeatManager>();
        savedMonsters = new List<Transform>();
    }

    private void Update()
    {
        if (goalCount <= currentCount)
        {
            print("Finish");
            system.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var line = collision.GetComponent<PlayerController>().line;

            for (int i = line.Count-1; i > 0; i--)
            {
                savedMonsters.Add(line[i]);
                line[i].gameObject.SetActive(false);
                line.Remove(line[i]);
            }
            currentCount = savedMonsters.Count;
            counter.Counting(currentCount, goalCount);
        }
    }
}
