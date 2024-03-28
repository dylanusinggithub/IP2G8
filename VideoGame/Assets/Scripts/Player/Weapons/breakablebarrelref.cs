using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakablebarrelref : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("breakablebarrel"))
        {
            other.GetComponent<breakable_barrel>().Smash();
        }
    }
}
