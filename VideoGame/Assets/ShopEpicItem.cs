using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEpicItem : MonoBehaviour
{
    [Header("Item Arrays")]
    public Sprite[] epicItems;

    [Header("Particle Prefab Array")]
    public GameObject epicParticle;

    [Header("References")]
    public GameObject spawnLocation;
    public GameObject player;
    public GameObject healthBar;
    public GameManager gameManager;

    [Header("Bools")]
    public bool itemChooserDestroy = false;

    private BoxCollider2D itemCollider;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEpicItem();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEpicItem()
    {
        //Create new seed based on game tick so it's random each time
        Random.InitState(System.Environment.TickCount);

        GameObject particlesPrefab;

        particlesPrefab = epicParticle;
        SpawnItem(epicItems, particlesPrefab);
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

            //Create particles behind the item
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
            itemCollider = newItem.AddComponent<BoxCollider2D>();
            itemCollider.isTrigger = true;
            itemCollider.enabled = false;


            // Destroy the ItemChooser if itemChooserDestroy is true
            if (itemChooserDestroy)
            {

                Destroy(gameObject); // Destroy the ItemChooser itself

            }
        }
        else
        {
            Debug.Log("No item array is empty");
        }
    }

    // Method to enable collider and handle purchase
    public void HandlePurchase()
    {
        if (gameManager != null && gameManager.gemCount >= 10)
        {
            itemCollider.enabled = true;
           // gameManager.RemoveGems(10); // Remove gems from player's count
        }
    }
}
