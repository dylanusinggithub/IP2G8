using System.Collections;
using UnityEngine;

public class LeafStorm : MonoBehaviour
{
    [Header("Sprite Settings")]
    public SpriteRenderer spriteRenderer;

    [Header("Prefab Settings")]
    public GameObject leafPrefab;

    [Header("Timing Settings")]
    public float fadeInSpeed = 0.5f;
    public float startDelay = 0f;
    public float damageZoneDuration = 2.5f;
    public float flashSpeed = 0.1f;

    [Header("Leaf Spawn Settings")]
    public float leafSpawnHeight = 8f;
    public int numLeavesToSpawn = 20;
    public float spawnDelay = 0.4f;

    private bool isActive = false;
    private bool canDealDamage = true;
    private bool playerInTrigger = false;

    void Start()
    {
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(startDelay);

        Color originalColor = spriteRenderer.color;
        originalColor.a = 0f;
        spriteRenderer.color = originalColor;

        while (spriteRenderer.color.a < 1f)
        {
            originalColor.a += Time.deltaTime * fadeInSpeed;
            spriteRenderer.color = originalColor;
            yield return null;
        }

        if (spriteRenderer.color.a >= 1f)
        {
            isActive = true;

            for (int i = 0; i < numLeavesToSpawn; i++)
            {
                float xPos = Random.Range(transform.position.x - spriteRenderer.bounds.size.x / 2f - 0.25f, transform.position.x + spriteRenderer.bounds.size.x / 2f - 0.25f);
                Vector3 leafPosition = new Vector3(xPos, transform.position.y + leafSpawnHeight, transform.position.z);
                Instantiate(leafPrefab, leafPosition, Quaternion.identity);
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        yield return new WaitForSeconds(damageZoneDuration);

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashSpeed);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isActive && playerInTrigger && canDealDamage && other.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();

            PlayerControls playerControls = other.GetComponent<PlayerControls>();

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(1);
                playerControls.hitFlash = true;
                canDealDamage = false;
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(1.25f);
        canDealDamage = true;
    }
}
