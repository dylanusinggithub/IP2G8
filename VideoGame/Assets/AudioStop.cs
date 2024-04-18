using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStop : MonoBehaviour
{

    private AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioManager>();
        StopAudio();
    }

    private void StopAudio()
    {
        audioManager.StopAllMusic();
    }
}
