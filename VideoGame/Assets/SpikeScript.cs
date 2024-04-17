using System.Collections;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    private Animator animator;
    private bool playerOnSpikes = false;

    public int damage = 1;
    public bool canDealDamage = true;

    private GameObject player;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (animator != null && playerOnSpikes)
        {
            if (canDealDamage == true)
            {
                canDealDamage = false;
                player.GetComponent<HealthSystem>().TakeDamage(damage);
                player.GetComponent<PlayerControls>().hitFlash = true;
                StartCoroutine(DamageCooldown());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("PlayerOn");
            StartCoroutine(Wait());

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("PlayerOff");
            playerOnSpikes = false;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
        playerOnSpikes = true;
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(1.25f);
        canDealDamage = true;
    }
}
