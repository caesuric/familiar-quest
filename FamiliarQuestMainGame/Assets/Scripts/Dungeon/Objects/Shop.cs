using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    List<Item> goods = new List<Item>();

    // Use this for initialization
    void Start() {
        InitializeShopGoods();
    }

    private void InitializeShopGoods() {
        int numGoods = Random.Range(8, 20);
        for (int i = 0; i < numGoods; i++) AddItem();
    }

    private void AddItem() {
        var item = RewardGiver.GenerateItem(PlayerCharacter.GetAverageLevel());
        goods.Add(item);
    }

    public void Use() {

    }
}
