using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedState : State
{
    public GameObject seed;
    public GameObject player;
    public bool seedover;
    public SlapState slapState;

    public override State RunCurrentState()
    {
        seedtime();
        if (seedover == true)
        {
            return slapState;
        }
        else
        {
            return this;
        }
    }

    public void seedtime()
    {
        GameObject seedobject = Instantiate(seed);
        seedobject.transform.position = player.transform.position;
        seedover = true;
        //lemme change this rq
    }
}
