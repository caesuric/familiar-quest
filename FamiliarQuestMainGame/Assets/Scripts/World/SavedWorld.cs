using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedWorld {
    public List<SavedItem> inventory = new List<SavedItem>();
    public string name = "";

    public static SavedWorld BrandNewWorld(string name) {
        var obj = new SavedWorld {
            name = name
        };
        return obj;
    }

    public static SavedWorld ConvertFrom(GameObject go) {
        var obj = new SavedWorld();
        var sharedInventory = go.GetComponent<SharedInventory>();
        var worldAutoSaver = go.GetComponent<WorldAutoSaver>();
        foreach (var item in sharedInventory.inventory) obj.inventory.Add(SavedItem.ConvertFrom(item));
        obj.name = worldAutoSaver.worldName;
        return obj;
    }

    public void ConvertTo(GameObject go) {
        var sharedInventory = go.GetComponent<SharedInventory>();
        var worldAutoSaver = go.GetComponent<WorldAutoSaver>();
        foreach (var item in inventory) sharedInventory.inventory.Add(item.ConvertTo());
        sharedInventory.CmdRefresh();
        worldAutoSaver.worldName = name;
    }
}
