using System.Collections;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    private GameObject attackArea;

    [Header("Melee Settings")]
    public GameObject meleeSlash;
    public float meleeDamage = 2.5f;
    public float meleeAttackSpeed = 0.5f;
    public float meleeTimer = 0f;
    public bool isAttacking = false;
    public float knockbackForce = 6f;


    [Header("Ranged Settings")]
    public Transform Aim;
    public GameObject bullet;
    public float rangedDamage = 2.5f;
    public float fireForce = 10f;
    public float rangedAttackSpeed = 1f;
    public float shootTimer = 0f;
    public bool canShoot = true;

    [Header("Critical Hit Settings")]
    public float criticalChance = 10f;
    public float criticalDamageMultiplier = 1.5f;

    [Header("Weapon Aim Settings")]
    public SpriteRenderer weaponSpriteRenderer;
    public SpriteRenderer playerSpriteRenderer;

    // Camera reference
    private Camera mainCam;
    private Vector3 mousePosition;
    public Camera bossCam;

    // Other References
    public Melee meleeScript;
    public AudioManager audioManager;
    public BossRoom room;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        meleeSlash.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckMeleeTimer();
        CheckCanShoot();

        if (!meleeSlash.activeSelf)
        {
            AimWeapon();
        }

        if (Input.GetMouseButtonDown(0) && !isAttacking && canShoot)
        {
            Melee();
        }

        if (Input.GetMouseButtonDown(1) && canShoot && !isAttacking)
        {
            Shoot();
        }
    }

    public void Melee()
    {
        isAttacking = true;
        meleeSlash.SetActive(true);
        audioManager.PlayAudio("AttackSlash");
        StartCoroutine(HideMelee());
    }

    private IEnumerator HideMelee()
    {
        yield return new WaitForSeconds(0.25f);
        meleeSlash.SetActive(false);
        meleeScript.OnAttackAnimationFinished();
    }

    void Shoot()
    {
        canShoot = false;
        GameObject intBullet = Instantiate(bullet, Aim.position, Aim.rotation);
        intBullet.GetComponent<Rigidbody2D>().AddForce(-Aim.up * fireForce, ForceMode2D.Impulse);
        audioManager.PlayAudio("ThrowingSound");
    }

    void CheckMeleeTimer()
    {
        //Check if is attacking
        if (isAttacking)
        {
            meleeTimer += Time.deltaTime;

            if (meleeTimer >= meleeAttackSpeed)
            {
                meleeTimer = 0f;
                isAttacking = false;

            }
        }
    }

    void CheckCanShoot()
    {
        //Check if can't shoot
        if (!canShoot)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > rangedAttackSpeed)
            {
                canShoot = true;
                shootTimer = 0f;
            }
        }
    }

    void AimWeapon()
    {
        //Get the mouse position
        mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        //Calculate the direction through a vector
        Vector3 direction = (mousePosition - transform.position).normalized;

        //Calculate angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //Add a offset cause it's offset for some reason?
        angle += 90f;

        //Apply the rotation
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Adjust the sorting order based on rotation
        if (angle < 90 && angle > -90)
        {
            //Put it under player
            weaponSpriteRenderer.sortingLayerName = playerSpriteRenderer.sortingLayerName;
            weaponSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder - 1;
        }
        else
        {
            //Put it over player
            weaponSpriteRenderer.sortingLayerName = playerSpriteRenderer.sortingLayerName;
            weaponSpriteRenderer.sortingOrder = playerSpriteRenderer.sortingOrder + 1;
        }
    }



    //Item Functions
    public void ItemDamageIncrease()
    {
        //Increase damage from item pick ups
        meleeDamage = (float)(meleeDamage * 1.3);
        rangedDamage = (float)(meleeDamage * 1.3);
    }

    public void ItemAttackSpeedIncrease()
    {
        meleeAttackSpeed = (float)(meleeAttackSpeed - 0.125f);
        rangedAttackSpeed = (float)(rangedAttackSpeed - 0.125f);
    }

    public void ItemCriticalChanceAdd()
    {
        criticalChance += 5f;
    }

    public void ItemCriticalChanceMulti()
    {
        criticalDamageMultiplier += 0.5f;
    }
}
