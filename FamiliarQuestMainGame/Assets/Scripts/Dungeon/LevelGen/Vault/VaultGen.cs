using System.Collections.Generic;
using UnityEngine;

public class VaultGen : MonoBehaviour {
    private static readonly Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> prefabs = LevelGenPrefabs.prefabs;
    private static readonly Dictionary<string, Dictionary<string, Dictionary<string, float>>> prefabProbability = LevelGenPrefabs.prefabProbability;
    private static readonly Dictionary<Element, Element> oppositeElementTypes = new Dictionary<Element, Element>() {
        { Element.fire, Element.ice },
        { Element.ice, Element.fire },
        { Element.light, Element.dark },
        { Element.dark, Element.light },
        { Element.bashing, Element.acid },
        { Element.acid, Element.piercing },
        { Element.piercing, Element.slashing },
        { Element.slashing, Element.bashing }
    };

    public static void InstantiateLayout(Vault layout, int floor, int seed) {
        Random.InitState(seed);
        ActiveCastleGen.InstantiateLayout(layout, floor);
        foreach (var room in layout.rooms) {
            if (room is BossRoom) continue;
            if (room is VaultRoom vr && vr.hasTreasure) AddTreasure(room);
        }
    }

    public static void AddTreasure(Room room) {
        var prefab = (GameObject)(Resources.Load("Prefabs/Dungeon/Old/Chest"));
        float xRoll = 5f * (room.x + (room.xSize / 2));
        float yRoll = 5f * (room.y + (room.ySize / 2));
        var obj = GameObject.Instantiate(prefab, new Vector3(xRoll, 0, yRoll), new Quaternion());
    }

    public static void AddDressing(Room room, DesignedBuilding layout) {
        ActiveCastleGen.AddDressing(room, layout);
    }

    public static void InstantiateResettledMonsters(int floor, Vault layout, List<SocialStructure> socialStructures) {
        //if (!NetworkServer.active) return;
        foreach (var socialStructure in socialStructures) {
            foreach (var monster in socialStructure.population) {
                int roll = Random.Range(0, monster.associatedRooms.Count);
                var room = monster.associatedRooms[roll];
                if (room.floor != floor) continue;
                float xRoll = Random.Range(5f * room.x, 5f * (room.x + room.xSize - 1));
                float yRoll = Random.Range(5f * room.y, 5f * (room.y + room.ySize - 1));
                InstantiateMonster(monster, xRoll, yRoll);
            }
        }
    }

    public static void InstantiateMonsters(int floor, Vault layout, List<MonsterData> monsters) {
        //if (!NetworkServer.active) return;
        foreach (var monster in monsters) {
            int roll = Random.Range(0, monster.associatedRooms.Count);
            var room = monster.associatedRooms[roll];
            if (room.floor != floor) continue;
            float xRoll = Random.Range(5f * room.x, 5f * (room.x + room.xSize - 1));
            float yRoll = Random.Range(5f * room.y, 5f * (room.y + room.ySize - 1));
            if (monster.quality==4) {
                xRoll = (5f * room.x + (5f * (room.x + room.xSize - 1)))/2f;
                yRoll = (5f * room.y + (5f * (room.y + room.ySize - 1)))/2f;
            }
            InstantiateMonster(monster, xRoll, yRoll);
        }
    }

    private static void InstantiateMonster(MonsterData monster, float xRoll, float yRoll) {
        var prefab = GetMonsterPrefab(monster);
        if (prefab != null) {
            var obj = Instantiate(prefab, new Vector3(xRoll, 0, yRoll), new Quaternion());
            SetupMonster(obj, monster.level, monster.quality, PlayerCharacter.localPlayer.gameObject);
            if (SceneInitializer.instance != null && SceneInitializer.instance.inside && LevelGen.dungeonData!=null && LevelGen.dungeonData.dungeonData.elementalAffinity != Element.none) AddElementalAffinity(obj, LevelGen.dungeonData.dungeonData.elementalAffinity);
            if (SceneInitializer.instance != null && SceneInitializer.instance.inside && LevelGen.dungeonData != null && LevelGen.dungeonData.dungeonData.enemyStatBoosts.ContainsKey(monster.specificType)) ApplyMonsterStatBoost(obj, LevelGen.dungeonData.dungeonData.enemyStatBoosts[monster.specificType]);
            if (SceneInitializer.instance != null && SceneInitializer.instance.inside && LevelGen.dungeonData != null && LevelGen.dungeonData.dungeonData.enemyBonusAbilities.ContainsKey(monster.specificType)) ApplyMonsterBonusAbility(obj, LevelGen.dungeonData.dungeonData.enemyBonusAbilities[monster.specificType]);
        }
    }

    private static void ApplyMonsterStatBoost(GameObject obj, string stat) {
        var ma = obj.GetComponent<MonsterAttributes>();
        if (stat == "strength") ma.strength = (int)(ma.strength * 1.5f);
        if (stat == "dexterity") ma.dexterity= (int)(ma.dexterity * 1.5f);
        if (stat == "constitution") ma.constitution = (int)(ma.constitution * 1.5f);
        if (stat == "intelligence") ma.intelligence = (int)(ma.intelligence * 1.5f);
        if (stat == "wisdom") ma.wisdom = (int)(ma.wisdom * 1.5f);
        if (stat == "luck") ma.luck = (int)(ma.luck * 1.5f);
    }

    private static void ApplyMonsterBonusAbility(GameObject obj, ActiveAbility ability) {
        obj.GetComponent<MonsterBaseAbilities>().baseAbilities.Add(ability);
    }

    private static void AddElementalAffinity(GameObject obj, Element element) {
        obj.GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(element, 50));
        obj.GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(oppositeElementTypes[element], -50));
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
