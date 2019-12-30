using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsArea : MonoBehaviour {

    public static DropsArea instance = null;
    public List<GameObject> items = new List<GameObject>();
    public GameObject itemPrefab;
    public GameObject abilityPrefab;
    public Inventory inventory;
    public DuloGames.UI.UIWindow inventoryWindow;
    public DuloGames.UI.UIWindow abilitiesWindow;

    // Use this for initialization
    void Start () {
        if (instance == null) instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void AddItemDrop(Equipment item) {
        var obj = Instantiate(instance.itemPrefab, instance.transform);
        instance.items.Add(obj);
        obj.GetComponent<ItemDrop>().Initialize(item, instance.inventory);
    }

    public static void AddAbilityDrop(Ability ability) {
        var obj = Instantiate(instance.abilityPrefab, instance.transform);
        instance.items.Add(obj);
        obj.GetComponent<AbilityDrop>().Initialize(ability);
    }

    public static void ClearDrops() {
        foreach (var item in instance.items) Destroy(item);
    }

    public static void OpenInventory() {
        instance.inventoryWindow.Toggle();
    }

    public static void OpenAbilities() {
        instance.abilitiesWindow.Toggle();
    }
}
