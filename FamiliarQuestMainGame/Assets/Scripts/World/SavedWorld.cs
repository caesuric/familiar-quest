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
        var worldAutoSaver = go.GetComponent<WorldAutoSaver>();
        foreach (var item in PlayerCharacter.localPlayer.inventory.items) obj.inventory.Add(SavedItem.ConvertFrom(item));
        obj.name = worldAutoSaver.worldName;
        return obj;
    }

    public void ConvertTo(GameObject go) {
        var worldAutoSaver = go.GetComponent<WorldAutoSaver>();
        foreach (var item in inventory) if (item!=null) PlayerCharacter.localPlayer.inventory.items.Add(item.ConvertTo());
        worldAutoSaver.worldName = name;
    }
}
