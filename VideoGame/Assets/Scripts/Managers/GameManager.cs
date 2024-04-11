using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Gem References")]
    public int gemCount = 0;
    public TextMeshProUGUI gemCountText;

    [Header("Global References")]
    public float enemyDropChance = 40f;
    public List<Item> currentItems = new List<Item>();
    public GameObject itemChooserPrefab;

   [Header("Player Statuses")]

   [Header("Frozen Sphere")]
    public bool frozenSphere = false;
    public float frozenMultiplier = 0.150f;
    public Material frozenMaterial;
    public GameObject frozenParticle;

    [Header("Keeeper's Timepiece")]
    public bool keepersTimepiece = false;
    public float keepersTimepieceLength = 1f;
    public Material keepersMaterial;
    public GameObject keepersParticle;

    [Header("Barbed Dagger")]
    public bool bleed = false;
    public int bleedTicks = 3;
    public float bleedDamage = 1f;
    public Material bleedMaterial;
    public GameObject bleedParticle;

    [Header("Material References")]
    public Material hitFlashMaterial;

    void Start()
    {
        gemCountText.text = gemCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DebugLogCurrentItems()
    {
        string itemList = "Current Items:\n";
        foreach (Item item in currentItems)
        {
            itemList += "Item Name: " + item.itemName + "\n";
            itemList += "Description: " + item.description + "\n";
            itemList += "Rarity: " + item.rarity + "\n\n";
        }
        Debug.Log(itemList);
    }

    public void AddGems(int amount)
    {
        gemCount += amount;
        UpdateGemCountText();
    }

    public void RemoveGems(int amount)
    {
        gemCount -= amount;
        UpdateGemCountText();
    }

    private void UpdateGemCountText()
    {
        gemCountText.text = gemCount.ToString();
    }

    public void ItemIncreaseDropRate()
    {
        enemyDropChance = (float)(enemyDropChance * 1.3);
    }

    public void ItemFrozenActive()
    {
        if (!frozenSphere)
        {
            frozenSphere = true;
        }
        else if (frozenSphere)
        {
            frozenMultiplier *= 1.3f;
        }
    }

    public void ItemKeepersTimepieceActive()
    {
        if (!keepersTimepiece)
        {
            keepersTimepiece = true;
        }
        else if (keepersTimepiece)
        {
            keepersTimepieceLength += 0.25f;
        }
    }

    public void ItemBleedActive()
    {
        if (!bleed)
        {
            bleed = true;
        }
        else if (bleed)
        {
            bleedTicks += 1;
            bleedDamage += 0.5f;
        }
    }


    public void SpawnItemChoosers()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
            return;
        }

        // Calculate positions for the item choosers
        Vector3 playerPosition = player.transform.position;
        float xOffset = 3f; // Adjust as needed
        float yOffset = -1f; // Adjust as needed

        Vector3 firstChooserPosition = new Vector3(playerPosition.x - xOffset, playerPosition.y + yOffset, playerPosition.z);
        Vector3 secondChooserPosition = new Vector3(playerPosition.x + xOffset, playerPosition.y + yOffset, playerPosition.z);

        // Instantiate Item Choosers using the prefab
        GameObject firstChooser = Instantiate(itemChooserPrefab, firstChooserPosition, Quaternion.identity);
        GameObject secondChooser = Instantiate(itemChooserPrefab, secondChooserPosition, Quaternion.identity);
    }
}
