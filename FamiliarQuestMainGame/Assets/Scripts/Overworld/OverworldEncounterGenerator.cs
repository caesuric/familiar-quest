using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class OverworldEncounterGenerator {
    private readonly static List<string> monsterTypes = new List<string>() {
        "Cyclops",
        "Dark Bishop",
        "Dark Knight",
        "Goblin Archer",
        "Goblin Rogue",
        "GOBLIN",
        "Ogre",
        "Troll",
        "Warlock",
        "Wolf"
    };
    private static List<GameObject> monsterPrefabs = new List<GameObject>();

    // Use this for initialization
    static OverworldEncounterGenerator() {
        var monsterList = Resources.LoadAll("Prefabs/Monsters");
        foreach (var monster in monsterList) monsterPrefabs.Add((GameObject)monster);
    }

    public static IEnumerator Generate() {
        LoadingProgressBar.UpdateProgressText("Adding Monsters");
        OverworldGenerator.instance.UpdateProgress(12, 0f);
        for (int i = 0; i < 40; i++) {
            AddEncounter();
            if (i % 4 == 0) {
                OverworldGenerator.instance.UpdateProgress(12, i / 20f);
                yield return null;
            }
        }
    }

    public static void AddEncounter() {
        var monsterTypes = GetMonsterTypesForEncounter();
        var numMobs = RNG.Int(3, 6);
        var position = OverworldPathfinding.GetValidRandomPosition();
        var baseLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
        var minLevel = Mathf.Max(baseLevel - 6, 1);
        var maxLevel = Mathf.Max(baseLevel - 3, 1);
        for (int i = 0; i < numMobs; i++) {
            var xRoll = RNG.Float(position.x - 3, position.x + 3);
            var yRoll = RNG.Float(position.z - 3, position.z + 3);
            var typeRoll = RNG.Int(0, monsterTypes.Count);
            var name = monsterTypes[typeRoll];
            int qualityRoll = RNG.Int(0, 100);
            int quality = 0;
            if (qualityRoll < 50) quality = 0;
            else if (qualityRoll < 80) quality = 1;
            else if (qualityRoll < 95) quality = 2;
            else quality = 3;
            var data = new MonsterData(name, name, RNG.Int(minLevel, maxLevel + 1), quality, null);
            InstantiateMonster(data, xRoll, yRoll);
        }
    }

    private static List<string> GetMonsterTypesForEncounter() {
        var numTypes = RNG.Int(1, 3);
        var output = new List<string>();
        var baseLevel = PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level;
        var minLevel = Mathf.Max(baseLevel - 6, 1);
        var maxLevel = Mathf.Max(baseLevel - 3, 1);
        var eligibleTypes = new List<string>();
        foreach (var monster in monsterPrefabs) if (monsterTypes.Contains(monster.name) && monster.GetComponent<Monster>().minLevel <= maxLevel && monster.GetComponent<Monster>().maxLevel >= minLevel) eligibleTypes.Add(monster.name);
        if (eligibleTypes.Count == 0) eligibleTypes = monsterTypes;
        for (int i = 0; i < numTypes; i++) {
            var roll = RNG.Int(0, eligibleTypes.Count);
            output.Add(eligibleTypes[roll]);
        }
        return output;
    }

    private static void InstantiateMonster(MonsterData monster, float xRoll, float yRoll) {
        var prefab = GetMonsterPrefab(monster);
        if (prefab != null) {
            var obj = Object.Instantiate(prefab, new Vector3(xRoll, 24, yRoll), new Quaternion());
            SetupMonster(obj, monster.level, monster.quality, PlayerCharacter.localPlayer.gameObject);
        }
    }

    private static GameObject GetMonsterPrefab(MonsterData monster) {
        return (GameObject)(Resources.Load("Prefabs/Monsters/" + monster.specificType));
    }

    private static void SetupMonster(GameObject obj, int level, int quality, GameObject player) {
        var mob = obj.GetComponent<Monster>();
        //mob.GetComponent<MonsterCombatant>().player = player;
        mob.GetComponent<MonsterScaler>().AdjustForLevel(level);
        mob.GetComponent<MonsterScaler>().quality = quality;
        mob.GetComponent<MonsterScaler>().numPlayers = 1;
        mob.GetComponent<MonsterScaler>().Scale();
        var billboard = obj.GetComponentInChildren<Billboard>();
        if (billboard != null) billboard.mainCamera = Camera.main;
        if (quality == 4) mob.gameObject.AddComponent<Boss>();
    }


}
