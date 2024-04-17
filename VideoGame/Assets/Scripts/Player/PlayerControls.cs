using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    public float moveSpeed = 4f;
    public float friction = 1.5f;
    public bool hitFlash = false;
    public bool canMove = true;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    public Transform Aim;
    private bool isWalking;

    private SpriteRenderer spriteRenderer;
    private GameManager gameManager;

    private bool inDialouge = false;

    public AudioManager audioManager;
    public float stepCooldown = 0.5f;

    private Material originalMaterial;

    bool canPlayStep = true;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        //Store the original material
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
        }
    }

    void Update()
    {
        if (canMove)
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

            if (hitFlash)
            {
                StartCoroutine(HitFlash());
            }

            if (movement.magnitude > 0 && canPlayStep)
            {
                StartCoroutine(PlayFootstep());
            }
        }
    }

    IEnumerator PlayFootstep()
    {
            audioManager.PlayAudio("GrassWalk");
            canPlayStep = false;
            yield return new WaitForSeconds(stepCooldown);
            canPlayStep = true;
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

    public void TeatheredFlashStart()
    {
        StartCoroutine (TeatheredFlash());
    }

     IEnumerator TeatheredFlash()
    {
        Material teatheredMaterial = gameManager.teatheredHeartsMaterial;
        GameObject teatheredParticle = gameManager.teatheredHeartParticle;
        GameObject teatheredInstance = null;

        if (teatheredMaterial != null)
        {
            spriteRenderer.material = teatheredMaterial;
            teatheredInstance = Instantiate(teatheredParticle, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(1f);

            //Revert the material
            if (spriteRenderer != null && originalMaterial != null)
            {
                spriteRenderer.material = originalMaterial;
            }

            Destroy(teatheredInstance);
        }
    }

    //Item Functions
    public void ItemSpeedIncrease()
    {
        moveSpeed = moveSpeed + 2;
    }
}
