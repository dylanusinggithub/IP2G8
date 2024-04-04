using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio References")]
    public AudioClip[] audioClips;
    private Dictionary<string, AudioClip> audioClipDictionary = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;

    private void Awake()
    {
        foreach (AudioClip clip in audioClips)
        {
            audioClipDictionary.Add(clip.name, clip);
        }

        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Start()
    {
        PlayAudio("BackgroundLoop");
    }

    public void PlayAudio(string fileName)
    {
        if (audioClipDictionary.ContainsKey(fileName))
        {
            audioSource.PlayOneShot(audioClipDictionary[fileName]);
        }
        else
        {
            Debug.LogError("Audio clip with file name " + fileName + " not found!");
        }
    }
}
