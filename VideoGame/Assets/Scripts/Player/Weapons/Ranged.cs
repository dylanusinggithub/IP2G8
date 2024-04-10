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

    public GameObject bloodEffectPrefab;
    public GameObject shurikenDestroyPrefab;

    private void Start()
    {
        weaponScript = FindFirstObjectByType<WeaponAim>();
        rangedDamage = weaponScript.rangedDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(rangedDamage);
                enemy.hitFlash = true;
                Instantiate(bloodEffectPrefab, enemy.transform.position, Quaternion.identity);
            }
        }

        if (collision.CompareTag("Enemy2"))
        {
            FlyingEnemy flyingEnemy = collision.GetComponent<FlyingEnemy>();
            if (flyingEnemy != null)
            {
                flyingEnemy.gameObject.GetComponent<FlyingEnemy>().TakeDamage(rangedDamage);
                Destroy(gameObject);
                flyingEnemy.hitFlash = true;
                Instantiate(bloodEffectPrefab, flyingEnemy.transform.position, Quaternion.identity);
            }
        }

        else if (collision.gameObject.CompareTag("Walls"))
        {
            Instantiate(shurikenDestroyPrefab, this.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        Destroy(this.gameObject, bulletLife);
    }
}
