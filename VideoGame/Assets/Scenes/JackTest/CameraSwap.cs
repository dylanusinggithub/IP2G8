using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwap : MonoBehaviour
{
    public Transform newTarget;
    public Vector3 playerChange;

    private CameraNew cam;

    [SerializeField]
    private GameObject[] objectsToEnable;

    void Start()
    {
        //Finds the main camera in the game
        cam = Camera.main.GetComponent<CameraNew>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            //Moves camera
            cam.SetTarget(newTarget);

            //Player Offset
            other.transform.position += playerChange;

            EnemyEnable();

        }
    }

    void EnemyEnable()
    {
        //Enables enemys when entering the room they are assigned too
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}
