using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedWorld {
    public List<SavedItem> inventory = new List<SavedItem>();
    public string name = "";
    public float[,] elevation = new float[1024, 1024];
    public SavedVector2 baseCoords = new SavedVector2();
    public List<SavedVector2> dungeonCoords = new List<SavedVector2>();
    public bool savedOverworld = false;

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
        if (OverworldGenerator.instance != null) {
            obj.savedOverworld = true;
            obj.elevation = OverworldGenerator.instance.elevation;
            foreach (var landmark in OverworldLandmarkGenerator.landmarks) {
                if (landmark.type == "base") obj.baseCoords = SavedVector2.ConvertFrom(landmark.position);
                else if (landmark.type == "dungeon") obj.dungeonCoords.Add(SavedVector2.ConvertFrom(landmark.position));
            }
        }
        return obj;
    }

    public void ConvertTo(GameObject go) {
        var worldAutoSaver = go.GetComponent<WorldAutoSaver>();
        foreach (var item in inventory) if (item!=null) PlayerCharacter.localPlayer.inventory.items.Add(item.ConvertTo());
        worldAutoSaver.worldName = name;
        if (savedOverworld) {
            OverworldGenerator.loadedPreviouslyMadeWorld = true;
            OverworldGenerator.loadedElevation = elevation;
            OverworldGenerator.loadedBaseCoords = baseCoords.ConvertTo();
            OverworldGenerator.loadedDungeonCoords = new List<Vector2>();
            foreach (var coord in dungeonCoords) OverworldGenerator.loadedDungeonCoords.Add(coord.ConvertTo());
        }
    }
}

[System.Serializable]
public class SavedVector2 {
    public float x;
    public float y;

    public static SavedVector2 ConvertFrom(Vector2 vector2) {
        return new SavedVector2() {
            x = vector2.x,
            y = vector2.y
        };
    }

    public Vector2 ConvertTo() {
        return new Vector2(x, y);
    }
}
