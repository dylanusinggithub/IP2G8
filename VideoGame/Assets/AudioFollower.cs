using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFollower : MonoBehaviour
{
    private Transform playerTransform;

    //Example Comment for Kaz

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
        }
    }

    private void Update()
    {
        //Follow the player
        if (playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
    }
}
