using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyItem : MonoBehaviour
{
    [Header("Passed In Variables")]
    private Sprite chosenItem;

    private Transform player;

    public float itemSpeed = 5f;


    public void InitializeItem(Sprite chosenItem, Sprite[] itemsArray)
    {
        this.chosenItem = chosenItem;
    }

    private void Start()
    {
        //Get the player's transform
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //Move towards to the player when close to them
        if (Vector2.Distance(transform.position, player.position) < 3f)
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calculate the direction to move towards the player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        //Move towards the player with the new vector
        transform.position = Vector3.MoveTowards(transform.position, player.position, itemSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager gameManager = Object.FindAnyObjectByType<GameManager>();

        if (other.CompareTag("Player"))
        {
            switch (chosenItem.name)
            {
                case "gems_0":
                    //Gems Item
                    Debug.Log("You Picked Up Gems");
                    gameManager.AddGems(1);
                    break;

                case "heart_full_0":
                    //Heart Item
                    Debug.Log("You Picked Up a Heart");
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
