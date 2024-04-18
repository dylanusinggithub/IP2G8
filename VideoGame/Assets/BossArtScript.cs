using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossArtScript : MonoBehaviour
{
    public SpriteRenderer[] spriteRenderers;
    public GameManager gameManager;

    public bool hitFlash = false;

    public void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    public void Update()
    {
        if (hitFlash)
        {
            StartCoroutine(HitFlash());
        }
    }

    IEnumerator HitFlash()
    {
        Material hitFlashMaterial = gameManager.hitFlashMaterial;

        if (hitFlashMaterial != null)
        {
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(materialPropertyBlock);
                materialPropertyBlock.SetColor("_Color", Color.red);
                renderer.SetPropertyBlock(materialPropertyBlock);
            }
        }

        yield return new WaitForSeconds(0.25f);
        hitFlash = false;

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.SetPropertyBlock(null);
        }
    }
}
