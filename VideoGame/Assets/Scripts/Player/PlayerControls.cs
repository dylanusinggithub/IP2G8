using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float moveSpeed = 4f;
    public float friction = 1.5f;
    public bool hitFlash = false;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    public Transform Aim;
    private bool isWalking;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    private bool inDialouge = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update()
    {
        if (inDialouge)
            return;
        //store last move direction
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (hitFlash)
        {
            StartCoroutine(HitFlash());
        }

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

    IEnumerator HitFlash()
    {
        Material hitFlashMaterial = gameManager.hitFlashMaterial;

        if (hitFlashMaterial != null)
        {
            MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(materialPropertyBlock);
            materialPropertyBlock.SetColor("_Color", Color.red);
            spriteRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        yield return new WaitForSeconds(0.25f);
        hitFlash = false;

        // Reset material property block to original material
        spriteRenderer.SetPropertyBlock(null);
    }

    //Item Functions
    public void ItemSpeedIncrease()
    {
        moveSpeed = moveSpeed + 2;
    }
}
