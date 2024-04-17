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

    public GameManager gameManager;

    public void Start()
    {
        itemDisplay = GameObject.FindGameObjectWithTag("ItemPopup").GetComponent<ItemPopup>();
        gameManager = FindFirstObjectByType<GameManager>();
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
                    playerControls.ItemSpeedIncrease();

                    //Boots Description
                    itemName = "Boots of Swiftness";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "common";

                break;

                case "gauntletsOfStrength":

                    //Gauntlets Action
                    WeaponAim gauntlets = FindFirstObjectByType<WeaponAim>();
                    gauntlets.ItemDamageIncrease();
               
                    //Gauntlets Description
                    itemName = "Gauntlets Of Strength";
                    description = "Increases Base Damage";
                    rarity = "common";

                    break;

                case "helmOfBerserker":

                    //Action
                    WeaponAim berserkerHelm = FindFirstObjectByType<WeaponAim>();
                    berserkerHelm.ItemCriticalChanceAdd();

                    //Description
                    itemName = "Helm of Berserker";
                    description = "Increases Critical Chance";
                    rarity = "common";

                    break;

                case "swordOfBerseker":

                    //Action
                    WeaponAim berserkerSword = FindFirstObjectByType<WeaponAim>();
                    berserkerSword.ItemCriticalChanceMulti();

                    //Description
                    itemName = "Sword of Berserker";
                    description = "Increases Critical Damage";
                    rarity = "common";

                    break;

                case "amuletOfVitality":

                    //Amulet Action
                    HealthSystem healthSystem = FindFirstObjectByType<HealthSystem>();
                    healthSystem.ItemHealthIncrease();

                    //Amulet Description
                    itemName = "Amulet of Vitality";
                    description = "Increases Maximum Health";
                    rarity = "common";

                    break;

                case "steelCapBoots":

                    //Boots Action
                    HealthSystem shieldSystem = FindFirstObjectByType<HealthSystem>();

                    if(shieldSystem.hasShield != true)
                    {
                        shieldSystem.ItemShieldAdd();
                    }
                    else
                    {
                        shieldSystem.ItemHealthIncrease();
                    }

                    //Boots Description
                    itemName = "Steelcap Boots";
                    description = "Temporary Armor Gain or Health Upgrade If Already Armoured!";
                    rarity = "common";

                break;

                case "fleetingQuiver":

                    //Boots Action
                    WeaponAim quiver = FindFirstObjectByType<WeaponAim>();
                    quiver.ItemAttackSpeedIncrease();

                    //Boots Description
                    itemName = "Fleeting Quiver";
                    description = "Increases Attack Speed";
                    rarity = "common";

                    break;

                case "charmOfFortune":

                    //Charm Action
                    GameManager dropRateIncrease = FindFirstObjectByType<GameManager>();
                    dropRateIncrease.ItemIncreaseDropRate();

                    //Charm Description
                    itemName = "Charm Of Fortune";
                    description = "Enemy Drop Rate Increase";
                    rarity = "common";

                    break;

                //Rare Items

                case "frozenSphere":

                    //Frozen Action
                    GameManager frozenSphere = FindFirstObjectByType<GameManager>();
                    frozenSphere.ItemFrozenActive();

                    //Frozen Description
                    itemName = "Frozen Sphere";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "rare";

                    Debug.Log("Amulet of Vitality!");
                    break;

                case "teatheredHearts":

                    //Action
                    GameManager teatheredHearts = FindFirstObjectByType<GameManager>();
                    teatheredHearts.ItemTeatheredHearts();

                    //Description
                    itemName = "Teathered Hearts";
                    description = "You have a chance to not be hurt by enemy attacks";
                    rarity = "rare";

                    break;

                case "keepersTimepiece":
                    
                    //Action
                    GameManager keepersTimepiece = FindFirstObjectByType<GameManager>();
                    keepersTimepiece.ItemKeepersTimepieceActive();

                    //Description
                    itemName = "Keepers Timepiece";
                    description = "Enemies are frozen at the start of combat";
                    rarity = "rare";

                    break;

                case "barbedDagger":

                    //Action
                    GameManager barbedDagger = FindFirstObjectByType<GameManager>();
                    barbedDagger.ItemBleedActive();

                    //Description
                    itemName = "Barbed Dagger";
                    description = "Your Attacks Do Bleed Damage";
                    rarity = "rare";

                    break;

                //Epic Items

                case "cosmicCompass":

                    //Action

                    //Description
                    itemName = "Cosmic Compass";
                    description = "Unveils the the whole floor";
                    rarity = "epic";

                    break;

                case "amuletOfAscendance":

                    //Action

                    //Description
                    itemName = "Amulet of Ascendance";
                    description = "+2 To All Base Stats";
                    rarity = "epic";

                    break;

                //Legendary Items

                case "divineBlade":

                    //Action
                    WeaponAim divineBlade = FindFirstObjectByType<WeaponAim>();
                    divineBlade.ItemDivineBlade();

                    //Description
                    itemName = "Divine Blade";
                    description = "+30% Damage, +15% Attack Speed, +5% Crit Chance";
                    rarity = "epic";

                    break;

                //Blessed Items
                case "forestsGift":
                    //Action

                    //Description
                    itemName = "Forest's Gift";
                    description = "You Revive Upon Death";
                    rarity = "blessed";
                    break;

                case "enduringVigor":

                    //Action
                    HealthSystem enduringVigor = FindFirstObjectByType<HealthSystem>();
                    enduringVigor.ItemHealthIncrease();
                    enduringVigor.ItemHealthIncrease();

                    //Description
                    itemName = "Enduring Vigor";
                    description = "Increase's Maximum Health by 2";
                    rarity = "blessed";

                    break;

                case "destinysHorde":

                    //Action
                    GameManager destinysHorde = FindFirstObjectByType<GameManager>();
                    destinysHorde.SpawnItemChoosers();

                    //Description
                    itemName = "Destiny's Horde";
                    description = "Recieve two random items";
                    rarity = "blessed";
                    break;

                case "fleetingGauntlets":
                    //Action

                    //Description
                    itemName = "Fleeting Gauntlets";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "blessed";
                    break;

                case "divineShield":
                    //Action

                    //Description
                    itemName = "Divine Shield";
                    description = "Gives the player a bonus movespeed of 10%";
                    rarity = "blessed";
                    break;



                //Default Action
                default:
                    Debug.LogWarning("Nothing Assigned: " + chosenItem.name);
                    break;
            }

            gameManager.currentItems.Add(this);
            gameManager.DebugLogCurrentItems();
            //Destroy object after collision with player
            Destroy(gameObject);
            Destroy(particlesObject);
            itemDisplay.ItemDisplay(itemName, description, rarity);
        }
    }
}
 