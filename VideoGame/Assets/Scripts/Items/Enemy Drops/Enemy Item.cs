using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItem : MonoBehaviour
{
    [Header("Passed In Variables")]
    private Sprite chosenItem;

    private Transform player;

    private AudioManager audioManager;

    public float itemSpeed = 5f;


    public void InitializeItem(Sprite chosenItem, Sprite[] itemsArray)
    {
        this.chosenItem = chosenItem;
    }

    private void Start()
    {
        //Get the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioManager = GameObject.FindGameObjectWithTag("AudioController").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (chosenItem.name != "heart_half_0" || !IsPlayerAtMaxHealth())
        {
            //Move towards to the player when close to them
            if (Vector2.Distance(transform.position, player.position) < 3f)
            {
                MoveTowardsPlayer();
            }
        }

    }

    private bool IsPlayerAtMaxHealth()
    {
        HealthSystem healthSystem = player.GetComponent<HealthSystem>();
        bool isAtMaxHealth = healthSystem != null && healthSystem.currentHealth == healthSystem.maxHealth;
        return isAtMaxHealth;
    }

    private void MoveTowardsPlayer()
    {
        //Calculate the direction to move towards the player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        //Move towards the player with the new vector
        transform.position = Vector3.MoveTowards(transform.position, player.position, itemSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        if (other.CompareTag("Player") && (chosenItem.name != "heart_half_0" || !IsPlayerAtMaxHealth()))
        {
            switch (chosenItem.name)
            {
                case "gems_0":

                    //Gems Action
                    gameManager.AddGems(1);
                    audioManager.PlayAudio("GemPickup");
                    break;

                case "heart_half_0":

                    //Heart Action
                    HealthSystem healthSystem = FindFirstObjectByType<HealthSystem>();
                    healthSystem.Heal(1);
                    audioManager.PlayAudio("Heal");
                    break;

                //Default Action
                default:
                    Debug.LogWarning("Nothing Assigned: " + chosenItem.name);
                    break;
            }
            //Destroy object after collision with player
            Destroy(gameObject);
        }
    }
}
