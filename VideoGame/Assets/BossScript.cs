using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float minInterval = 2f;
    public float maxInterval = 5f;
    public float attackCooldown = 1f;

    private bool isActive = false;
    private float timer = 0f;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    [Header("Seed Shot Attack")]
    public GameObject bossSeedPrefab;
    public float horizontalOffset = 3.25f;
    public int numberOfVerticalRows = 5;
    public int numberOfSeedsInRow = 15;
    public float delayBetweenSeeds = 0.6f;
    public float horizontalRowOffset = 0.5f;


    public enum BossState
    {
        Idle,
        BranchSwipe,
        RootErupt,
        LeafStorm,
        SeedShot
    }

    private BossState currentState;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = BossState.Idle;
    }

    private void Update()
    {
        //if (isActive)
        //{
        //    if (!isOnCooldown)
        //    {
        //        timer -= Time.deltaTime;

        //        if (timer <= 0f)
        //        {
        //            ChangeState();
        //        }
        //    }
        //    else
        //    {
        //        cooldownTimer -= Time.deltaTime;
        //        if (cooldownTimer <= 0f)
        //        {
        //            isOnCooldown = false;
        //        }
        //    }

            //DEBUG
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ForceState(BossState.BranchSwipe);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ForceState(BossState.RootErupt);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ForceState(BossState.LeafStorm);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
            StartCoroutine(SpawnBossSeedCoroutine());
            ForceState(BossState.SeedShot);
            }
    }

    public void ActivateBoss()
    {
        isActive = true;
        timer = Random.Range(minInterval, maxInterval);
    }

    private void ChangeState()
    {
        int nextState = Random.Range(0, 5);

        currentState = (BossState)nextState;

        timer = Random.Range(minInterval, maxInterval);

        switch (currentState)
        {
            case BossState.Idle:
                break;
            case BossState.BranchSwipe:
                Debug.Log("Boss performs Branch Swipe!");
                StartCooldown();
                break;
            case BossState.RootErupt:
                Debug.Log("Boss performs Root Erupt!");
                StartCooldown();
                break;
            case BossState.LeafStorm:
                Debug.Log("Boss performs Leaf Storm!");
                StartCooldown();
                break;
            case BossState.SeedShot:
                StartCoroutine(SpawnBossSeedCoroutine());
                StartCooldown();
                break;
        }
    }

    private void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = attackCooldown;
    }

    //DEBUG
    public void ForceState(BossState state)
    {
        currentState = state;
        Debug.Log("Forcing boss into state: " + state);
        StartCooldown();
    }

    // Seed Shot Attack
    private IEnumerator SpawnBossSeedCoroutine()
    {
        for (int i = 0; i < numberOfVerticalRows; i++)
        {
            float rowOffset = i % 2 == 0 ? 0 : horizontalRowOffset;
            for (int j = 0; j < numberOfSeedsInRow; j++)
            {
                float offset = horizontalOffset * (j - (numberOfSeedsInRow - 1) / 2f) + rowOffset;
                Vector3 spawnPosition = CalculateSpawnPosition(offset);
                SpawnBossSeed(spawnPosition);
            }
            yield return new WaitForSeconds(delayBetweenSeeds);
        }
    }

    private Vector3 CalculateSpawnPosition(float offset)
    {
        Vector3 bossPosition = transform.position;

        float bossHeight = GetComponent<SpriteRenderer>().bounds.size.y;

        Vector3 spawnPosition = new Vector3(bossPosition.x + offset, bossPosition.y - (bossHeight / 2), bossPosition.z);

        return spawnPosition;
    }


    private void SpawnBossSeed(Vector3 position)
    {
        Instantiate(bossSeedPrefab, position, Quaternion.identity);
    }
}
