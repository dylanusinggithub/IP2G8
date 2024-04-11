

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakable_pot : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {

    }

    public void Smash()
    {
        anim.SetBool("smashed", true);
        StartCoroutine(breakCo());
    }

    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
}