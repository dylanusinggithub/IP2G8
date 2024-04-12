using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafStormLeaf : MonoBehaviour
{
    public float speed = 4f;

    private void Start()
    {
        bool flipSprite = Random.value > 0.5f;
        if (flipSprite)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
        Destroy(gameObject, 8f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LeafStorm"))
        {
            Destroy(gameObject);
        }
    }
}
