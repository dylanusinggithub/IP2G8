using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float moveSpeed = 4f;
    public float friction = 1.5f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    public Transform Aim;
    private bool isWalking;

    void Update()
    {
        //store last move direction
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        //Times movement vector by current player movespeed
        Vector2 veloctiy = movement * moveSpeed;

        //Apply friction to reduce speed
        veloctiy *= friction;

        //Move the rigidboy using the calculation including friction
        rb.MovePosition(rb.position + veloctiy * Time.fixedDeltaTime);

    }

    public void StatboostSpeed()
    {
        moveSpeed = moveSpeed + 2;
    }
}
