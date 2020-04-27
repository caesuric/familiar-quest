using System.Collections.Generic;
using UnityEngine;

public class OverworldLandmark {
    public Vector2 position;
    public string type;
    public string uuid = System.Guid.NewGuid().ToString();
}

public class OverworldDungeon : OverworldLandmark {
    public bool entered = false;
    public TreasureVault dungeonData = null;
    public List<int> seeds = new List<int>();

    public OverworldDungeon() {
        type = "dungeon";
    }
}