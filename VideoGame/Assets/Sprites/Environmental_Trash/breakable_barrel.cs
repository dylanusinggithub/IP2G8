using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakable_barrel : MonoBehaviour
{
    private Animator anim;
    private AudioManager audioManager;

    [Header("Enemy Drops")]
    public Sprite[] barrelDrops;
    private GameObject spawnLocation;
    public float barrelDropChance = 30f;

    void Start()
    {
        anim = GetComponent<Animator>();
        audioManager = FindFirstObjectByType<AudioManager>();
        spawnLocation = this.gameObject;
    }

    public void Smash()
    {
        anim.SetBool("smashed", true);
        audioManager.PlayAudio("BarrelBreak");
        StartCoroutine(breakCo());
    }

    IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.3f);
        Destroy(this.gameObject);
        DropChance();
    }

    public void DropChance()
    {
        float dropChance = barrelDropChance;

        if (Random.value * 100 <= dropChance)
        {
            SpawnDrop(barrelDrops);
        }
    }


    void SpawnDrop(Sprite[] itemsArray)
    {
        if (itemsArray != null && itemsArray.Length > 0)
        {
            UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
            int randomIndex = Random.Range(0, itemsArray.Length);
            Debug.Log("Random Index: " + randomIndex);
            Debug.Log("Array Length: " + itemsArray.Length);

            Sprite chosenItem = itemsArray[randomIndex];

            //Create the item above the pedestal
            GameObject newItem = new GameObject("EnemyDrop");

            newItem.transform.position = spawnLocation.transform.position;

            //Add a sprite renderer with values
            SpriteRenderer spriteRenderer = newItem.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = chosenItem;
            spriteRenderer.sortingOrder = 2;

            //Change the scale of the sprite / My references are too small can be removed later on
            float scaleMultiplier = 1.25f;
            newItem.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier, 1.0f);

            //Attach item script to the object and run internal function
            EnemyItem item = newItem.AddComponent<EnemyItem>();
            item.InitializeItem(chosenItem, itemsArray);

            //Attach 2D collider with trigger so it can be interacted with
            BoxCollider2D collider = newItem.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
    }
}
