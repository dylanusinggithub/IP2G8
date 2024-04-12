using System.Collections;
using System.Collections.Generic;
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

    [Header("Leaf Storm Attack")]
    public GameObject warningAreaPrefab;
    public int numberOfWarningAreas = 3;
    public float delayBetweenStorms = 1f;
    public float leafStormWidth = 41.53f;
    public float leafStormHeight = 13.94f;
    public float leafStormYOffset = -10.69f;
    public float minSpawnDistance = 5f;
    private List<Vector3> spawnedPositions = new List<Vector3>();

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
                StartCoroutine(LeafStormCoroutine());
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

    //Leaf Storm Attack
    private IEnumerator LeafStormCoroutine()
    {
        spawnedPositions.Clear();
        int seed = Random.Range(0, int.MaxValue); 
        Random.InitState(seed); 

        for (int i = 0; i < numberOfWarningAreas; i++)
        {
            Vector3 randomPosition = GetRandomPositionInBounds();
            while (IsTooCloseToSpawnedPosition(randomPosition))
            {
                Random.InitState(++seed);
                randomPosition = GetRandomPositionInBounds();
            }
            spawnedPositions.Add(randomPosition); 
            SpawnWarningArea(randomPosition);
            yield return new WaitForSeconds(delayBetweenStorms);
        }
    }

    private bool IsTooCloseToSpawnedPosition(Vector3 newPosition)
    {
        foreach (Vector3 position in spawnedPositions)
        {
            if (Vector3.Distance(newPosition, position) < minSpawnDistance)
            {
                return true;
            }
        }
        return false;
    }

    private Vector3 GetRandomPositionInBounds()
    {
        Vector3 center = transform.position;
        float minX = center.x - leafStormWidth / 2f + 1.5f;
        float maxX = center.x + leafStormWidth / 2f - 1.5f;
        float minY = center.y - leafStormHeight / 2f + leafStormYOffset + 1f;
        float maxY = center.y + leafStormHeight / 2f + leafStormYOffset - 1f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector3(randomX, randomY, center.z);
    }

    private void SpawnWarningArea(Vector3 position)
    {
        Instantiate(warningAreaPrefab, position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = transform.position;
        Gizmos.DrawWireCube(center + new Vector3(0f, leafStormYOffset, 0f), new Vector3(leafStormWidth, leafStormHeight, 0f));
    }
}
