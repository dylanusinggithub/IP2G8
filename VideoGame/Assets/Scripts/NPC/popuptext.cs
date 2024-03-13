using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextController : MonoBehaviour
{
    public Text textToShow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the entering object is the player
        {
            textToShow.gameObject.SetActive(true); // Show the text
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the exiting object is the player
        {
            textToShow.gameObject.SetActive(false); // Hide the text
        }
    }
}
