using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
    public Text costText;
    public InventoryItemUpdater iiu;
    public int cost = 100;
    public Shop shop;

    void Start() {
        iiu = GetComponent<InventoryItemUpdater>();
    }

    public void Initialize(Shop shop, int cost) {
        this.shop = shop;
        this.cost = cost;
        costText.text = cost.ToString();
    }

    public void Buy() {
        shop.Buy(cost, iiu.item);
    }
}
