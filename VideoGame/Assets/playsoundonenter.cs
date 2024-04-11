using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundOnEnter : MonoBehaviour
{
    AudioSource source;
    Collider2D soundTrigger;
    bool playerInside = false;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        soundTrigger = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            source.Play();
            playerInside = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            source.Stop();
            playerInside = false;
        }
    }

  
}
