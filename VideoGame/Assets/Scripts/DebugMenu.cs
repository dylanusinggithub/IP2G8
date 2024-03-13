using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenu : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerHealth playerHealth;
    private PlayerAttack playerAttack;
    private PlayerWeapons playerWeapons;
    private bool isDebugVisible = true;
    private float debugMenuYOffset = 130f;

    void Start()
    {
        playerControls = FindFirstObjectByType<PlayerControls>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerAttack = FindFirstObjectByType<PlayerAttack>();
        playerWeapons = FindFirstObjectByType<PlayerWeapons>();
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
        //Only display debug menu if it's visible
        if (isDebugVisible)
        {
            float yPosition = debugMenuYOffset;

            //Draw the debug menu entries with dynamic Y position
            GUI.Label(new Rect(10, yPosition, 200, 20), "DEBUG MENU (F1 TOGGLE)"); yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "HEALTH: " + playerHealth.health.ToString()); yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "MOVESPEED: " + playerControls.moveSpeed.ToString()); yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "DAMAGE: " + playerWeapons.damage.ToString());  yPosition += 20;
            GUI.Label(new Rect(10, yPosition, 200, 20), "ATTACK SPD: " + playerAttack.timeToAttack.ToString()); yPosition += 20;
        }
    }
}
