using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakable_barrel : MonoBehaviour
{
    private Animator anim;
    private AudioManager audioManager;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    void Update()
    {

    }

    public void Smash()
    {
        anim.SetBool("smashed", true);
        audioManager.PlayAudio("BarrelBreak");
        StartCoroutine(breakCo());
    }

    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
}
