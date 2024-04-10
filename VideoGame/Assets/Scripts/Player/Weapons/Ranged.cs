using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    [Header("Ranged Variables")]
    private WeaponAim weaponScript;
    private float rangedDamage;
    public float bulletLife = .75f;

    private void Start()
    {
        weaponScript = FindFirstObjectByType<WeaponAim>();
        rangedDamage = weaponScript.rangedDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(rangedDamage);
            Destroy(gameObject);
            enemy.hitFlash = true;
        }

        FlyingEnemy flyingEnemy = collision.GetComponent<FlyingEnemy>();
        if (flyingEnemy != null)
        {
            flyingEnemy.gameObject.GetComponent<FlyingEnemy>().TakeDamage(rangedDamage);
            Destroy(gameObject);
            flyingEnemy.hitFlash = true;
        }

        else if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Destroy(this.gameObject, bulletLife);
    }
}
