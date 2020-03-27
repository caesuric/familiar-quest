using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPaneController : MonoBehaviour {

    public GameObject objectContainer;
    public GameObject objectPrefab;
    private List<GameObject> shopObjects = new List<GameObject>();
    private Shop shop;

    public void PopulateShop(List<Item> items, Shop shop) {
        this.shop = shop;
        foreach (var obj in shopObjects) Destroy(obj);
        foreach (var item in items) AddItem(item);
    }

    private void AddItem(Item item) {
        var go = Instantiate(objectPrefab, objectContainer.transform);
        shopObjects.Add(go);
        var sic = go.GetComponent<ShopItemController>();
        sic.Initialize(shop, Shop.Appraise(item));
        var iiu = go.GetComponent<InventoryItemUpdater>();
        iiu.Initialize(item, PlayerCharacter.localPlayer.inventory);
    }

    public void SellItem(Item item) {
        shop.Sell(item);
    }
}
