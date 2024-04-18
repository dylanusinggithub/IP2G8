using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    private bool isActive = false;
    private float timer = 0f;
    private float cooldownTimer = 0f;
    private bool isOnCooldown = false;

    [Header("Root Erupt Attack")]
    public GameObject[] rootEruptObjects;
    public int numberOfRootEruptAttacks = 3;
    public float rootEruptAttackInterval = 5f;
    public float timeBeforeReset = 3f;

    public float rootEruptCooldown = 20f;

    [Header("Seed Shot Attack")]
    public GameObject bossSeedPrefab;
    public float horizontalOffset = 3.25f;
    public int numberOfVerticalRows = 5;
    public int numberOfSeedsInRow = 15;
    public float delayBetweenSeeds = 0.6f;
    public float horizontalRowOffset = 0.5f;

    public float seedShotCooldown = 10f;

    [Header("Leaf Storm Attack")]
    public GameObject warningAreaPrefab;
    public int numberOfWarningAreas = 3;
    public float delayBetweenStorms = 1f;
    public float leafStormWidth = 41.53f;
    public float leafStormHeight = 13.94f;
    public float leafStormYOffset = -10.69f;
    public float minSpawnDistance = 5f;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    public float leafStormCooldown = 20f;

    [Header("Boss Health")]
    public float bossHealth;
    public float maxHealth = 500f;
    public Image healthBar;
    private bool hasTakenDamage = false;
    public GameObject objectToEnableOnDamage;

    [Header("References")]
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    public bool enemyFrozen = false;

    public GameObject victoryScreen;

    public enum BossState
    {
        Idle,
        RootErupt,
        LeafStorm,
        SeedShot
    }

    private BossState currentState;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentState = BossState.Idle;
        bossHealth = maxHealth;
        objectToEnableOnDamage.SetActive(false);

        //Store the original material
        if (spriteRenderer != null)
        {
            originalMaterial = spriteRenderer.material;
        }
    }

    private void Update()
    {
        if (isActive && hasTakenDamage)
        {
            if (!isOnCooldown)
            {
                timer -= Time.deltaTime;

                if (timer <= 0f)
                {
                    ChangeState();
                }
            }
            else
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer <= 0f)
                {
                    isOnCooldown = false;
                }
            }

        }
        ////DEBUG
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    StartCoroutine(RootEruptCoroutine());
        //    ForceState(BossState.RootErupt);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    StartCoroutine(LeafStormCoroutine());
        //    ForceState(BossState.LeafStorm);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    StartCoroutine(SpawnBossSeedCoroutine());
        //    ForceState(BossState.SeedShot);
        //}

        if (!hasTakenDamage && bossHealth < maxHealth)
        {
            hasTakenDamage = true;
            objectToEnableOnDamage.SetActive(true);
        }
    }

    public void ActivateBoss()
    {
        isActive = true;
        timer = 2f;
    }

    private void ChangeState()
    {
        int nextState = Random.Range(0, 4);

        currentState = (BossState)nextState;

        timer = 2f;

        switch (currentState)
        {
            case BossState.Idle:
                break;
            case BossState.RootErupt:
                Debug.Log("Boss performs Root Erupt!");
                StartCoroutine(RootEruptCoroutine());
                ForceState(BossState.RootErupt);

                StartCooldown(rootEruptCooldown);
                break;
            case BossState.LeafStorm:
                Debug.Log("Boss performs Leaf Storm!");
                StartCoroutine(LeafStormCoroutine());
                ForceState(BossState.LeafStorm);

                StartCooldown(leafStormCooldown);
                break;
            case BossState.SeedShot:
                Debug.Log("Boss performs Seed Shot!");
                StartCoroutine(SpawnBossSeedCoroutine());
                ForceState(BossState.SeedShot);

                StartCooldown(seedShotCooldown);
                break;
        }
    }

    private void StartCooldown(float cooldown)
    {
        isOnCooldown = true;
        cooldownTimer = cooldown;
    }

    //DEBUG
    public void ForceState(BossState state)
    {
        currentState = state;
        Debug.Log("Forcing boss into state: " + state);
    }

    //Seed Shot Attack
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

    //Root Erupt Attack
    private IEnumerator RootEruptCoroutine()
    {
        for (int attackCount = 0; attackCount < numberOfRootEruptAttacks; attackCount++)
        {
            yield return new WaitForSeconds(rootEruptAttackInterval);

            List<GameObject> shuffledObjects = new List<GameObject>(rootEruptObjects);
            Shuffle(shuffledObjects);

            int numObjectsToEnable = Random.Range(1, 4);

            for (int i = 0; i < numObjectsToEnable; i++)
            {
                if (i < shuffledObjects.Count)
                {
                    shuffledObjects[i].SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Not enough objects to enable.");
                    break;
                }
            }

            yield return new WaitForSeconds(timeBeforeReset);

            foreach (var obj in shuffledObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    //Health
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float fillAmount = (float)bossHealth / maxHealth;
            healthBar.fillAmount = fillAmount;
        }
    }

    public void TakeDamage(float damage)
    {
        bossHealth -= damage;

        if (!hasTakenDamage && objectToEnableOnDamage != null)
        {
            objectToEnableOnDamage.SetActive(true);
            hasTakenDamage = true;
            ActivateBoss();
        }

        bossHealth -= damage;

        UpdateHealthBar();

        if (bossHealth <= 0)
        {
            victoryScreen.SetActive(true);
            objectToEnableOnDamage.SetActive(false);
        }
    }
}
