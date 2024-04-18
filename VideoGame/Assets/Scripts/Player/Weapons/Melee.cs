using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    [Header("Melee Variables")]
    private WeaponAim weaponScript;
    private float meleeDamage;
    public float knockbackDuration = 0.2f; //Adjust here on in inspector
    public GameObject bloodEffectPrefab;

    private float baseMeleeDamage;
    private float criticalChance;
    private float criticalDamageMultiplier;

    public AudioManager audioManager;
    public string[] hitSoundOptions = { "HitSoundOne", "HitSoundTwo", "HitSoundThree" };

    //Track which enemies have already been hit
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    void Start()
    {
        weaponScript = FindFirstObjectByType<WeaponAim>();
        baseMeleeDamage = weaponScript.meleeDamage;
        criticalChance = weaponScript.criticalChance;
        criticalDamageMultiplier = weaponScript.criticalDamageMultiplier;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                baseMeleeDamage = weaponScript.meleeDamage;
                float damage = CalculateDamage(baseMeleeDamage);

                enemy.TakeDamage(damage);
                enemy.hitFlash = true;
                hitEnemies.Add(collision);
                ApplyKnockbackToEnemy(enemy, transform.position);

                Instantiate(bloodEffectPrefab, enemy.transform.position, Quaternion.identity);

                //string randomSound = hitSoundOptions[Random.Range(0, hitSoundOptions.Length)];
                //audioManager.PlayAudio(randomSound);
            }
            
        }

        if (collision.CompareTag("Enemy2") && !hitEnemies.Contains(collision))
        {
            FlyingEnemy flyingEnemy = collision.GetComponent<FlyingEnemy>();
            if (flyingEnemy != null)
            {
                baseMeleeDamage = weaponScript.meleeDamage;
                float damage = CalculateDamage(baseMeleeDamage);

                flyingEnemy.TakeDamage(damage);
                flyingEnemy.hitFlash = true;
                hitEnemies.Add(collision);
                ApplyKnockbackToFlyingEnemy(flyingEnemy, transform.position);

                Instantiate(bloodEffectPrefab, flyingEnemy.transform.position, Quaternion.identity);

                //string randomSound = hitSoundOptions[Random.Range(0, hitSoundOptions.Length)];
                //audioManager.PlayAudio(randomSound);
            }
        }

        if (collision.CompareTag("Boss") && !hitEnemies.Contains(collision))
        {
            BossScript bossEnemy = collision.GetComponent<BossScript>();
            BossArtScript bossArtEnemy = FindFirstObjectByType<BossArtScript>();
            if (bossEnemy != null)
            {
                baseMeleeDamage = weaponScript.meleeDamage;
                float damage = CalculateDamage(baseMeleeDamage);
                bossEnemy.TakeDamage(damage);
                bossArtEnemy.hitFlash = true;
                hitEnemies.Add(collision);
                Instantiate(bloodEffectPrefab, bossEnemy.transform.position, Quaternion.identity);
            }
        }

        if (collision.CompareTag("breakable"))
        {
            collision.GetComponent<breakable_pot>().Smash();
        }

        if (collision.CompareTag("breakablebarrel"))
        {
            collision.GetComponent<breakable_barrel>().Smash();
        }
    }
    private float CalculateDamage(float baseDamage)
    {
        float critChance = criticalChance / 100f;
        float damage = baseDamage;
        if (Random.Range(0f, 1f) <= critChance)
        {
            Debug.Log("Critical Strike!");
            audioManager.PlayAudio("CriticalHitSound");
            damage *= criticalDamageMultiplier;
        }
        else
        {
            audioManager.PlayAudio("RegularHitSound");
        }
        return damage;
    }

    //Ground Enemy Knockback
    private void ApplyKnockbackToEnemy(Enemy enemy, Vector3 origin)
    {
        StartCoroutine(LerpKnockback(enemy.transform, origin, knockbackDuration));
    }

    //Flying Enemy Knockback
    private void ApplyKnockbackToFlyingEnemy(FlyingEnemy flyingEnemy, Vector3 origin)
    {
        StartCoroutine(LerpKnockback(flyingEnemy.transform, origin, knockbackDuration));
    }

    //Lerping knockack coroutine
    private IEnumerator LerpKnockback(Transform targetTransform, Vector3 origin, float duration)
    {
        if (targetTransform == null)
            yield break;

        float elapsedTime = 0f;
        Vector3 startPosition = targetTransform.position;
        Vector3 knockbackDirection = (targetTransform.position - origin).normalized;

        while (elapsedTime < duration)
        {
            if (targetTransform == null)
                yield break;

            float t = elapsedTime / duration;
            targetTransform.position = Vector3.Lerp(startPosition, startPosition + knockbackDirection, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (targetTransform != null)
            targetTransform.position = startPosition + knockbackDirection;
    }


    public void OnAttackAnimationFinished()
    {
        //Clear hash list and deactivate attack
        hitEnemies.Clear();
        weaponScript.meleeSlash.SetActive(false);
    }
}
