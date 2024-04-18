using System.Collections;
using UnityEngine;

public class BossRoot : MonoBehaviour
{
    [Header("Sprite Settings")]
    public SpriteRenderer spriteRenderer;

    [Header("Timing Settings")]
    public float fadeInSpeed = 0.5f;
    public float startDelay = 0f;
    public float damageZoneDuration = 2.5f;
    public float flashSpeed = 0.1f;

    private bool isActive = false;
    private bool canDealDamage = true;
    private bool playerInTrigger = false;
    private Coroutine sequenceCoroutine;

    void OnEnable()
    {
        sequenceCoroutine = StartCoroutine(StartSequence());
    }

    void OnDisable()
    {
        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);
    }

    IEnumerator StartSequence()
    {
        Color originalColor = spriteRenderer.color;
        originalColor.a = 0f;
        spriteRenderer.color = originalColor;

        yield return new WaitForSeconds(startDelay);

        while (spriteRenderer.color.a < 1f)
        {
            originalColor.a += Time.deltaTime * fadeInSpeed;
            spriteRenderer.color = originalColor;
            yield return null;
        }

        if (spriteRenderer.color.a >= 1f)
        {
            isActive = true;
        }

        yield return new WaitForSeconds(damageZoneDuration);

        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(flashSpeed);
        }

        gameObject.SetActive(false);
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
        if (spriteRenderer.color.a >= 1f && playerInTrigger && canDealDamage && other.CompareTag("Player"))
        {
            HealthSystem healthSystem = other.GetComponent<HealthSystem>();

            PlayerControls playerControls = other.GetComponent<PlayerControls>();

            if (healthSystem != null)
            {
                healthSystem.TakeDamage(2);
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
