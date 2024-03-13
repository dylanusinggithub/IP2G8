using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemChooser : MonoBehaviour
{
    [Header("Item Arrays")]
    public Sprite[] commonItems;
    public Sprite[] rareItems;
    public Sprite[] epicItems;
    public Sprite[] legendaryItems;

    [Header("Particle Prefab Array")]
    public GameObject commonParticle;
    public GameObject rareParticle;
    public GameObject epicParticle;
    public GameObject legendaryParticle;
    public GameObject blessedParticle;

    [Header("References")]
    public GameObject spawnLocation;
    public GameObject player;
    public GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnRandomItem();
        }
    }

    public void SpawnRandomItem()
    {
        //Create new seed based on game tick so it's random each time
        Random.InitState(System.Environment.TickCount);

        //Generate a random value between 0 -> 100
        int randomValue = Random.Range(0, 100);

        GameObject particlesPrefab = null;

        if (randomValue <= 69)
        {
            particlesPrefab = commonParticle;
            SpawnItem(commonItems, particlesPrefab);
        }
        else if (randomValue >= 70 && randomValue <= 85)
        {
            particlesPrefab = rareParticle;
            SpawnItem(rareItems, particlesPrefab);
        }
        else if (randomValue >= 86 && randomValue <= 95)
        {
            particlesPrefab = epicParticle;
            SpawnItem(epicItems, particlesPrefab);
        }
        else
        {
            particlesPrefab = legendaryParticle;
            SpawnItem(legendaryItems, particlesPrefab);
        }

        Debug.Log(randomValue);
    }

    void SpawnItem(Sprite[] itemsArray, GameObject particlesPrefab)
    {
        if (itemsArray != null && itemsArray.Length > 0)
        {
            int randomIndex = Random.Range(0, itemsArray.Length);
            Sprite chosenItem = itemsArray[randomIndex];

            //Create the item above the pedestal
            GameObject newItem = new GameObject("NewItem");

            Vector3 offset = new Vector3(0, 1.3f, 0);

            newItem.transform.position = spawnLocation.transform.position;
            newItem.transform.position += offset;

            // Instantiate particles behind the item
            GameObject particles = Instantiate(particlesPrefab, newItem.transform.position, Quaternion.identity);

            //Add a sprite renderer with values
            SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = chosenItem;
            spriteRenderer.sortingOrder = 2;

            //Change the scale of the sprite / My references are too small can be removed later on
            float scaleMultiplier = 0.35f;
            newItem.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1.0f);

            //Attach item script to the object and run internal function
            Item item = newItem.AddComponent<Item>();
            ItemHover itemHover = newItem.AddComponent<ItemHover>();
            item.InitializeItem(chosenItem, itemsArray);
            item.SetParticles(particles);

            //Attach 2d collider with trigger so it can be interacted with
            BoxCollider2D collider = newItem.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;



            //Debug log item
            Debug.Log("Item: " + chosenItem.name);
        }
        else
        {
            Debug.Log("No item array is empty");
        }
    }
}
