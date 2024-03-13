using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    public float damage = 2.5f;

    public Enemy enemy;
    public FlyingEnemy flyingEnemy;
    public enum WeaponType { Melee, Bullet}
    public WeaponType weaponType;

    public float bulletLife = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy2")) 
        {
            collision.gameObject.GetComponent<FlyingEnemy>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
            
    }

    private void Update()
    {
        if (weaponType == WeaponType.Bullet)
        {
            Destroy(this.gameObject, bulletLife);

        }
    }

}
