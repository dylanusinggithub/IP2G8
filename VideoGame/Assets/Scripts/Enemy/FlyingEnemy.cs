using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{

    [SerializeField] Transform target;
    [SerializeField] float moveSpeed = 3.5f;
    private float originalMoveSpeed;

    NavMeshAgent agent;

    public float attackRange;

    public LayerMask whatIsPlayer;

    public bool playerInAttackRange;

    public GameObject bullet;
    public Transform bulletPos;

    private float timer;

    public float health = 10f;
    bool isDead = false;
    public GameObject me;

    [Header("Enemy Drop's")]
    public Sprite[] enemyDrop;
    public GameObject spawnLocation;
    public int damage = 1;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = moveSpeed;
        originalMoveSpeed = moveSpeed;


        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);

        if (playerInAttackRange && timer > 2)
        {
            AttackPlayer();
            timer = 0;
        }
        else if(!playerInAttackRange)
        {
            ChasePlayer();
        }

        if(isDead == true)
        {
            Destroy(me);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            health -= 25;
        }

        agent.speed = moveSpeed;
    }

    void ChasePlayer()
    {
        agent.SetDestination(target.position);
    }

    void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.right = target.position - transform.position;

        Instantiate(bullet, bulletPos.position, Quaternion.identity);
    }

    void SpawnDrop(Sprite[] itemsArray)
    {
        if (itemsArray != null && itemsArray.Length > 0)
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            int randomIndex = UnityEngine.Random.Range(0, itemsArray.Length);
            Debug.Log("Random Index: " + randomIndex);
            Debug.Log("Array Length: " + itemsArray.Length);

            Sprite chosenItem = itemsArray[randomIndex];

            //Create the item above the pedastool
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

            //Attach 2d collider with trigger so it can be interacted with
            BoxCollider2D collider = newItem.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }

    public void TakeDamage(float damage)
    {
        bool isFrozen = gameManager.frozenSphere;
        if (isFrozen == true)
        {
            StartCoroutine(ApplyFreeze());
        }

        health -= damage;

        if (health <= 0)
        {
            isDead = true;

            float dropChance = gameManager.enemyDropChance;

            if (Random.value * 100 <= dropChance)
            {
                SpawnDrop(enemyDrop);
            }
        }
    }

    IEnumerator ApplyFreeze()
    {
        float frozenMultiplier = gameManager.frozenMultiplier;
        float reductionAmount = moveSpeed * frozenMultiplier; 

        moveSpeed -= reductionAmount; 

        yield return new WaitForSeconds(3f);

        moveSpeed += reductionAmount;
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
}
