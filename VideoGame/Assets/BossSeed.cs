using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSeed : MonoBehaviour
{
    public int damageAmount = 1;
    public float speed = 5f;

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        Destroy(gameObject, 8f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<HealthSystem>().TakeDamage(damageAmount);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }
}
