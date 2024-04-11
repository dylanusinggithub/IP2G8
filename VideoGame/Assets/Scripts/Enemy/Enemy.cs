using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public Transform target;
    public float moveSpeed = 3.5f;
    private float originalMoveSpeed;
    private NavMeshAgent agent;

    [Header("Health")]
    public float health = 10;
    public float attackRange = 2.0f;
    public int damage = 1;
    private bool canAttack = true;
    private bool isDead = false;
    public bool hitFlash = false;

    [Header("Game Objects")]
    public GameObject me;
    public GameObject player;

    [Header("Enemy Drops")]
    public Sprite[] enemyDrop;
    public GameObject spawnLocation;

    [Header("References")]
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private GameObject frozenParticleInstance;
    private GameObject bleedParticleInstance;
    private GameObject keepersParticleInstance;
    private Animator animator;

    public bool enemyFrozen = false;
    public bool isFlipped = false;

    public AudioSource audioPlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = moveSpeed;
        originalMoveSpeed = moveSpeed;

        gameManager = FindFirstObjectByType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");

        //Store the original material
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
        }

        if(gameManager.keepersTimepiece == true)
        {
            StartCoroutine(ApplyKeepersTimepiece());
        }
    }

    void Update()
    {
        agent.SetDestination(target.position);

        if (isDead)
        {
            Destroy(me);
        }

        if (hitFlash)
        {
            StartCoroutine(HitFlash());
        }

        // Check if player is in attack range
        if (canAttack && Vector3.Distance(transform.position, target.position) <= attackRange)
        {
            AttackPlayer();
            StartCoroutine(Attack());
        }

        LookAtPlayer();
        agent.speed = moveSpeed;

        isAlive();
    }

    IEnumerator Attack()
    {
        canAttack = false;
        moveSpeed = 0;
        animator.SetBool("Walker 2", false);

        yield return new WaitForSeconds(2.0f);

        canAttack = true;
        moveSpeed = originalMoveSpeed;
        animator.SetBool("Walker 2", true);
    }

    void AttackPlayer()
    {
        player.GetComponent<HealthSystem>().TakeDamage(damage);
        player.GetComponent<PlayerControls>().hitFlash = true;
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > target.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < target.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    void SpawnDrop(Sprite[] itemsArray)
    {
        if (itemsArray != null && itemsArray.Length > 0)
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            int randomIndex = Random.Range(0, itemsArray.Length);
            Debug.Log("Random Index: " + randomIndex);
            Debug.Log("Array Length: " + itemsArray.Length);

            Sprite chosenItem = itemsArray[randomIndex];

            //Create the item above the pedestal
            GameObject newItem = new GameObject("EnemyDrop");

            newItem.transform.position = spawnLocation.transform.position;

            //Add a sprite renderer with values
            SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = chosenItem;
            spriteRenderer.sortingOrder = 2;

            //Change the scale of the sprite / My references are too small can be removed later on
            float scaleMultiplier = 1f;
            newItem.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1.0f);

            //Attach item script to the object and run internal function
            EnemyItem item = newItem.AddComponent<EnemyItem>();
            item.InitializeItem(chosenItem, itemsArray);

            //Attach 2D collider with trigger so it can be interacted with
            BoxCollider2D collider = newItem.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }

    public void TakeDamage(float damage)
    {
        bool isFrozen = gameManager.frozenSphere;
        bool isBleed = gameManager.bleed;

        if (isFrozen)
        {
            StartCoroutine(ApplyFreeze());
        }

        if(isBleed)
        {
            StartCoroutine(ApplyBleed());
        }

        health -= damage;
    }

    public void isAlive()
    {
        if (health <= 0)
        {
            isDead = true;

            float dropChance = gameManager.enemyDropChance;

            if (Random.value * 100 <= dropChance)
            {
                SpawnDrop(enemyDrop);
            }

            GameObject[] bleedEffects = GameObject.FindGameObjectsWithTag("BleedEffect");
            foreach (GameObject obj in bleedEffects)
            {
                Destroy(obj);
            }
        }
    }

    IEnumerator ApplyFreeze()
    {
        if (enemyFrozen)
            yield break;

        float frozenMultiplier = gameManager.frozenMultiplier;
        float reductionAmount = moveSpeed * frozenMultiplier;

        Material frozenMaterial = gameManager.frozenMaterial;

        moveSpeed -= reductionAmount;

        GameObject frozenPrefab = gameManager.frozenParticle;

        frozenParticleInstance = Instantiate(frozenPrefab, transform.position, Quaternion.identity, transform);

        //Change the material
        if (spriteRenderer != null && frozenMaterial != null)
        {
            spriteRenderer.material = frozenMaterial;
        }

        enemyFrozen = true;

        yield return new WaitForSeconds(3f);

        moveSpeed += reductionAmount;

        enemyFrozen = false;
        Destroy(frozenParticleInstance);

        //Revert the material
        if (spriteRenderer != null && originalMaterial != null)
        {
            spriteRenderer.material = originalMaterial;
        }
    }

    IEnumerator ApplyKeepersTimepiece()
    {
        if (gameManager.keepersTimepiece)
        {
            float timepieceLength = gameManager.keepersTimepieceLength;
            Material timepieceMaterial = gameManager.keepersMaterial;
            GameObject timepieceParticle = gameManager.keepersParticle;

            if (timepieceMaterial != null)
            {
                enemyFrozen = true;
                moveSpeed = 0f;
                spriteRenderer.material = timepieceMaterial;
                keepersParticleInstance = Instantiate(timepieceParticle, transform.position, Quaternion.identity);

                yield return new WaitForSeconds(timepieceLength);

                enemyFrozen = false;
                moveSpeed = originalMoveSpeed;
                spriteRenderer.material = originalMaterial;
                Destroy(keepersParticleInstance);
            }
        }
    }

    IEnumerator ApplyBleed()
    {
        if (gameManager.bleed)
        {
            float bleedDuration = gameManager.bleedTicks;
            float bleedDamageAmount = gameManager.bleedDamage;
            Material bleedMaterial = gameManager.bleedMaterial;
            GameObject bleedParticle = gameManager.bleedParticle;
            GameObject bleedParticleInstance = null;

            if (bleedMaterial != null && bleedParticle != null)
            {
                for (int i = 0; i < bleedDuration; i++)
                {
                    health -= bleedDamageAmount;

                    bleedParticleInstance = Instantiate(bleedParticle, transform.position, Quaternion.identity);
                    spriteRenderer.material = bleedMaterial;
                    yield return new WaitForSeconds(0.5f);

                    spriteRenderer.material = originalMaterial;
                    if (health <= 0)
                    {
                        break;
                    }
                    Destroy(bleedParticleInstance);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
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

    void OnGUI()
    {
        if (target != null && !isDead)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
            screenPosition.y += 40;
            GUI.Label(new Rect(screenPosition.x, Screen.height - screenPosition.y, 100, 20), "HP: " + health);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            audioPlayer.Play();

        }



    }




}
