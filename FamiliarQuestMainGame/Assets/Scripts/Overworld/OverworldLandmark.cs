using UnityEngine;

public class OverworldLandmark {
    public Vector2 position;
    public string type;
}

public class OverworldDungeon : OverworldLandmark {
    public bool entered = false;
    public TreasureVault dungeonData = null;
}