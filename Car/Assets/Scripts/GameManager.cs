using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public enum state {Ready, Playing, Win, Lose}
    public Count counter;
    public List<Transform> savedMonsters;
    public int goalCount;
    public int currentCount = 0;
    public ParticleSystem FinishFX;
    public state nextStatus = state.Ready;
    public Text readyText;
    public Text winText;
    public Text loseText;
    public AudioSource victorySFX;
    public AudioSource savingSFX;

    BeatManager system;
    public state gameStatus;

    private void Awake()
    {
        StartCoroutine(ReadyUI());
        gameStatus = state.Ready;
        counter.Counting(currentCount, goalCount);
        system = FindObjectOfType<BeatManager>();
        savedMonsters = new List<Transform>();
    }    

    private void Update()
    {
        if (gameStatus != nextStatus)
        {
            CheckStatus();
        }

        if (goalCount <= currentCount)
        {
            print("Finish");
            system.gameObject.SetActive(false);
        }
    }

    private void CheckStatus()
    {
        switch (nextStatus)
        {
            case state.Playing:
                gameStatus = nextStatus;
                break;
            case state.Win:
                gameStatus = nextStatus;
                StartCoroutine(WinUI());
                break;
            case state.Lose:
                gameStatus = nextStatus;
                StartCoroutine(LoseUI());
                break;
        }
    }

    IEnumerator ReadyUI()
    {
        while (gameStatus == state.Ready)
        {
            readyText.enabled = true;
            yield return new WaitForSeconds(0.4f);
            readyText.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator LoseUI()
    {
        while (gameStatus == state.Lose)
        {
            loseText.enabled = true;
            yield return new WaitForSeconds(0.4f);
            loseText.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator WinUI()
    {
        while (gameStatus == state.Win)
        {
            winText.enabled = true;
            yield return new WaitForSeconds(0.4f);
            winText.enabled = false;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var line = collision.GetComponent<PlayerController>().line;
            if (line.Count != 0)
            {
                StartCoroutine(Savemonster(line));
            }
        }
    }

    IEnumerator Savemonster(List<Transform> line)
    {
        for (int i = line.Count - 1; i > 0; i--)
        {
            savingSFX.Play();
            savedMonsters.Add(line[i]);
            line[i].DOMove(transform.position, 0.15f);
            yield return new WaitForSeconds(0.15f);

            line[i].gameObject.SetActive(false);
            line.Remove(line[i]);
            currentCount = savedMonsters.Count;
            counter.Counting(currentCount, goalCount);
        }
        if (goalCount <= currentCount)
        {
            FinishFX.Play();
            nextStatus = state.Win;
            victorySFX.Play();
        }
    }
}
