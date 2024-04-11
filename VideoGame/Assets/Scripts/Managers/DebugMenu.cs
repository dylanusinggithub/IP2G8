using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private PlayerControls playerControls;
    private HealthSystem healthSystem;
    private WeaponAim weaponAim;
    private GameManager gameManager;
    public bool isDebugVisible = false;
    private float debugMenuYOffset = 130f;

    void Start()
    {
        isDebugVisible = false;
        playerControls = FindFirstObjectByType<PlayerControls>();
        healthSystem = FindFirstObjectByType<HealthSystem>();
        weaponAim = FindFirstObjectByType<WeaponAim>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void Update()
    {
        //Toggle visibility
        if (Input.GetKeyDown(KeyCode.F1))
        {
            isDebugVisible = !isDebugVisible;
        }
    }

    void OnGUI()
    {
        // Only display debug menu if it's visible
        if (isDebugVisible)
        {
            float yPosition = debugMenuYOffset;

            // Draw the debug menu entries with dynamic Y position
            GUI.Label(new Rect(10, yPosition, 200, 20), "DEBUG MENU (F1 TOGGLE)");
            yPosition += 20;
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "HEALTH: " + healthSystem.currentHealth.ToString());
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "MAX HEALTH: " + healthSystem.maxHealth.ToString());
            yPosition += 20;
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "MOVESPEED: " + playerControls.moveSpeed.ToString());
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "DAMAGE: " + weaponAim.meleeDamage.ToString());
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "RNG ATTACK SPD: " + weaponAim.rangedAttackSpeed.ToString());
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "MELEE ATTACK SPD: " + weaponAim.meleeAttackSpeed.ToString());
            yPosition += 20;
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "CRITICAL CHANCE: " + weaponAim.criticalChance.ToString());
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "MULTIPLIER: " + weaponAim.criticalDamageMultiplier.ToString());
            yPosition += 20;
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "ENEMY DROPRATE: " + gameManager.enemyDropChance.ToString());
            yPosition += 20;
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "FROZEN: " + gameManager.frozenSphere.ToString());
            yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "MULTI: " + gameManager.frozenMultiplier.ToString());
            yPosition += 20;
        }
    }
}


