using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGems : MonoBehaviour
{
    public ShopCommonItem shopCommonItem;
    public ShopRareItem shopRareItem;
    public ShopEpicItem shopEpicItem;

    private void Start()
    {
        shopCommonItem = FindAnyObjectByType<ShopCommonItem>();
        shopRareItem = FindAnyObjectByType<ShopRareItem>();
        shopEpicItem = FindAnyObjectByType<ShopEpicItem>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the method in ShopCommonItem to enable collider and handle purchase
            shopCommonItem.HandlePurchase();
            shopRareItem.HandlePurchase();
            shopEpicItem.HandlePurchase();
        }
    }
}
