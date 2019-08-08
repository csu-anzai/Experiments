using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    SphereCollider sc;
    Rigidbody playerRB;
    Vector3 dir;
    Ray ray;
    RaycastHit hit;
    float moveSpeed;

    private void Awake()
    {
        sc = GetComponent<SphereCollider>();
        playerRB = GetComponent<Rigidbody>();
        dir = transform.forward;
    }

    void Start()
    {
        moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        playerRB.MovePosition(playerRB.position + dir * moveSpeed * Time.deltaTime);
        moveSpeed += 0.01f;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ButtonDown());
        }

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButton(0))
            {
                //hit.collider.GetComponent<Objects>()?.Action();
            }
        }
    }

    IEnumerator ButtonDown()
    {
        sc.enabled = true;
        yield return null;
        yield return null;
        sc.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Objects>()?.Action();
    }
}
