using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency_collection : MonoBehaviour
{
    private GameManager gameManager;
    private Transform playerTransform;
    public float moveSpeed = 2f;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, playerTransform.position) <= 5f)
        {
            transform.position = Vector2.Lerp(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            gameManager.AddGems(1);
            Destroy(gameObject);
            //    audioSource.Play();
        }
    }
}
