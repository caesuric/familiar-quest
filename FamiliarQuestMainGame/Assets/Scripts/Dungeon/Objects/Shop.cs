using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    public List<Item> goods = new List<Item>();
    public static Dictionary<Item, int> costs = new Dictionary<Item, int>();
    private DuloGames.UI.UIWindow shopPane = null;
    private ShopPaneController shopPaneController = null;
    private DuloGames.UI.UIWindow inventory = null;

    // Use this for initialization
    void Start() {
        InitializeShopGoods();
        shopPane = GameObject.FindGameObjectWithTag("ShopPane").GetComponent<DuloGames.UI.UIWindow>();
        shopPaneController = shopPane.GetComponent<ShopPaneController>();
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<DuloGames.UI.UIWindow>();
    }

    private void InitializeShopGoods() {
        int numGoods = Random.Range(8, 20);
        for (int i = 0; i < numGoods; i++) AddItem();
    }

    private void AddItem() {
        var item = RewardGiver.GenerateItem(PlayerCharacter.GetAverageLevel());
        if (item is Equipment) goods.Add(item);
    }

    public void Use() {
        shopPane.Show();
        inventory.Show();
        shopPaneController.PopulateShop(goods, this);
        PlayerCharacter.localPlayer.inventory.Refresh();
    }

    public void Buy(int cost, Item item) {
        if (PlayerCharacter.localPlayer.gold >= cost) {
            goods.Remove(item);
            shopPaneController.PopulateShop(goods, this);
            PlayerCharacter.localPlayer.inventory.items.Add(item);
            PlayerCharacter.localPlayer.inventory.Refresh();
            PlayerCharacter.localPlayer.gold -= cost;
        }
    }

    public void Sell(Item item) {
        goods.Add(item);
        shopPaneController.PopulateShop(goods, this);
        PlayerCharacter.localPlayer.inventory.items.Remove(item);
        PlayerCharacter.localPlayer.inventory.Refresh();
        PlayerCharacter.localPlayer.gold += Appraise(item);
    }

    public static int Appraise(Item item) {
        if (costs.ContainsKey(item)) return costs[item];
        var costLookups = new List<float>() { 10, 20, 40, 80, 160, 320, 640, 1280 };
        var statLookups = new List<float>() { 0, 2, 3, 5, 7, 9, 12, 15 };
        var highestStat = ((Equipment)item).GetHighestStat();
        float highestStatValue;
        if (((Equipment)item).stats.ContainsKey(highestStat)) highestStatValue = ((Equipment)item).stats[highestStat];
        else highestStatValue = 0;
        float baseline = statLookups[((Equipment)item).quality];
        var level = 1;
        float cost = costLookups[((Equipment)item).quality];
        while (highestStatValue > baseline) {
            highestStatValue /= 1.1f;
            cost *= 1.1f;
            level++;
        }
        cost *= Random.Range(0.8f, 1.2f);
        costs[item] = (int)cost;
        return (int)cost;
    }
}
