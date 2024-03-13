using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Passed In Variables")]
    private Sprite chosenItem;
    private Sprite[] itemsArray;
    private GameObject particlesObject; // Reference to the particles object

    [Header("Sending Variables")]
    public string itemName;
    public string description;
    public string rarity;
    public ItemPopup itemDisplay;

    public void Start()
    {
        itemDisplay = GameObject.FindGameObjectWithTag("ItemPopup").GetComponent<ItemPopup>();
    }
    public void InitializeItem(Sprite chosenItem, Sprite[] itemsArray)
    {
        this.chosenItem = chosenItem;
        this.itemsArray = itemsArray;
    }

    public void SetParticles(GameObject particles)
    {
        particlesObject = particles;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (chosenItem.name)
            {

                //Common Items

                case "bootsOfSwiftness":

                    //Boots Action
                    PlayerControls playerControls = other.GetComponent<PlayerControls>();
                    playerControls.StatboostSpeed();

                    //Boots Description
                    itemName = "Boots of Swiftness";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "common";

                break;

                case "gauntletsOfStrength":

                    //Gauntlets Action


                    //Gauntlets Description
                    itemName = "Gauntlet's Of Strength";
                    description = "Grants a bonus Heart Container";
                    rarity = "common";

                    break;

                case "amuletOfVitality":

                    //Amulet Action
                    PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                    playerHealth.MaxHealthIncrease();

                    //Amulet Description
                    itemName = "Amulet of Vitality";
                    description = "Increases Maximum Health";
                    rarity = "common";

                    break;

                case "steelCapBoots":

                    //Boots Action

                    //Boots Description
                    itemName = "Steelcap Boots";
                    description = "Temporary Armor Gain";
                    rarity = "common";

                break;

                case "charmOfFortune":

                    //Charm Action

                    //Charm Description
                    itemName = "Charm Of Fortune";
                    description = "Enemy Drop Rate Increase";
                    rarity = "common";

                    break;

                //Rare Items

                case "frozenSphere":
                    //Ring Action

                    itemName = "Frozen Sphere";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "rare";

                    Debug.Log("Amulet of Vitality!");
                    break;

                case "teatheredHearts":
                    //Ring Action

                    itemName = "Teathered Hearts";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "rare";

                    Debug.Log("Amulet of Vitality!");
                    break;

                //Epic Items

                case "amuletOfAscendance":
                    //Ring Action

                    itemName = "Frozen Sphere";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "epic";

                    Debug.Log("Amulet of Vitality!");
                    break;

                //Legendary Items

                //Blessed Items




                //Default Action
                default:
                    Debug.LogWarning("Nothing Assigned: " + chosenItem.name);
                    break;
            }

            //Destroy object after collision with player
            Destroy(gameObject);
            Destroy(particlesObject);
            itemDisplay.ItemDisplay(itemName, description, rarity);
        }
    }
}
 