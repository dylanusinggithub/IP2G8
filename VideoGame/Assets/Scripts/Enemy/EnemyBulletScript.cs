using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float force;
    private float timer;

    public AudioSource audioPlayer;

    public int damage = 1;
    public GameObject destructionEffect; // Particle effect prefab

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - rb.transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 10)
        {
            DestroyBullet();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = other.gameObject.GetComponent<HealthSystem>();

            if (player)
            {
                player.TakeDamage(damage);
                player.GetComponent<PlayerControls>().hitFlash = true;
            }
            DestroyBullet();
        }
        else if (other.gameObject.CompareTag("Walls"))
        {
            DestroyBullet();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            audioPlayer.Play();
        }
    }

    void DestroyBullet()
    {
        // Instantiate destruction effect
        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }

        // Destroy bullet object
        Destroy(gameObject);
    }
}