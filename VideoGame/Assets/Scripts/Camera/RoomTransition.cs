using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public CameraController cam;

    public Vector3 cameraChange;
    public Vector3 playerChange;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (cam != null)
            {
                // Moves camera target
                cam.target.position += cameraChange;

                // Player offset
                other.transform.position += playerChange;
            }
            else
            {
                Debug.LogError("CameraController not assigned to RoomTransition!");
            }
        }
    }
}
