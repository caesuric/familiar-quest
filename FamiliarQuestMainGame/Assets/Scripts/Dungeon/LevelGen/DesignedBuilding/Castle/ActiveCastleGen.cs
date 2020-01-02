using System.Collections.Generic;
using UnityEngine;

class ActiveCastleGen : MonoBehaviour {
    private static readonly Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> prefabs = LevelGenPrefabs.prefabs;
    private static readonly Dictionary<string, Dictionary<string, Dictionary<string, float>>> prefabProbability = LevelGenPrefabs.prefabProbability;

    public static void InstantiateLayout(Dungeon layout, int floor) {
        for (int x = 0; x < layout.grid.GetLength(1); x++) {
            for (int y = 0; y < layout.grid.GetLength(2); y++) {
                InstantiateCastleBlock(x, y, layout.grid[floor, x, y]);
                InstantiateCastleWall(x, y, floor, layout.grid[floor, x, y], layout.grid);
                InstantiateRoomWall(x, y, floor, layout.grid[floor, x, y], layout);
                InstantiateRoomDoor(x, y, floor, layout.grid[floor, x, y], layout);
            }
        }
    }

    public static void InstantiateMonsters(int floor, DesignedBuilding layout, SocialStructure socialStructure, LevelGen levelGen) {
        //if (!NetworkServer.active) return;
        foreach (var monster in socialStructure.population) {
            int roll = Random.Range(0, monster.associatedRooms.Count);
            var room = monster.associatedRooms[roll];
            if (room.floor != floor) continue;
            float xRoll = Random.Range(5f * room.x, 5f * (room.x + room.xSize - 1));
            float yRoll = Random.Range(5f * room.y, 5f * (room.y + room.ySize - 1));
            if (monster.quality == 4) {
                xRoll = (room.x + (room.xSize / 2)) * 4;
                yRoll = (room.y + (room.ySize / 2)) * 4;
            }
            InstantiateMonster(monster, xRoll, yRoll, levelGen);
        }
    }

    private static void InstantiateMonster(MonsterData monster, float xRoll, float yRoll, LevelGen levelGen) {
        var prefab = GetMonsterPrefab(monster);
        if (prefab != null) {
            var obj = GameObject.Instantiate(prefab, new Vector3(xRoll, 0, yRoll), new Quaternion());
            obj.transform.parent = LevelGen.instance.dungeonInstance.transform;
            levelGen.instantiatedObjects.Add(obj);
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

    private static void InstantiateCastleBlock(int x, int y, string block) {
        LevelGenInstantiationUtils.InstantiateFloorBlock("castle", x, y, block);
    }

    private static void InstantiateCastleWall(int x, int y, int floor, string block, string[,,] grid) {
        LevelGenInstantiationUtils.InstantiateWall("castle", x, y, floor, block, grid);
    }

    private static void InstantiateRoomWall(int x, int y, int floor, string block, Dungeon layout) {
        if (block != "X") return;
        if (!LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 3) && LevelGenGridUtils.IsCorridor(x - 1, y, floor, layout.grid)) MakeRoomWall(x - 0.5f, y, 0);
        if (!LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 1) && LevelGenGridUtils.IsCorridor(x + 1, y, floor, layout.grid)) MakeRoomWall(x + 0.5f, y, 180);
        if (!LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 0) && LevelGenGridUtils.IsCorridor(x, y - 1, floor, layout.grid)) MakeRoomWall(x, y - 0.5f, 90);
        if (!LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 2) && LevelGenGridUtils.IsCorridor(x, y + 1, floor, layout.grid)) MakeRoomWall(x, y + 0.5f, 270);
    }

    private static void InstantiateRoomDoor(int x, int y, int floor, string block, Dungeon layout) {
        if (block != "X") return;
        var room = LevelGenRoomUtils.GetRoomFromCoords(x, y, floor, layout);
        if (room is CommonSpace) return;
        if (LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 3) && LevelGenGridUtils.IsCorridor(x - 1, y, floor, layout.grid)) MakeRoomDoor(x - 0.5f, y, 90);
        if (LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 1) && LevelGenGridUtils.IsCorridor(x + 1, y, floor, layout.grid)) MakeRoomDoor(x + 0.5f, y, 270);
        if (LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 0) && LevelGenGridUtils.IsCorridor(x, y - 1, floor, layout.grid)) MakeRoomDoor(x, y - 0.5f, 180);
        if (LevelGenRoomUtils.IsRoomEntrance(x, y, floor, layout, 2) && LevelGenGridUtils.IsCorridor(x, y + 1, floor, layout.grid)) MakeRoomDoor(x, y + 0.5f, 0);
    }

    private static void MakeRoomWall(float x, float y, float angle) {
        MakeAndRotateObject("wall", x, y, angle);
    }

    private static void MakeRoomDoor(float x, float y, float angle) {
        MakeAndRotateObject("door", x, y, angle);
    }

    private static void MakeAndRotateObject(string type, float x, float y, float angle) {
        var walls = prefabs["castle"][type];
        var wallProbs = prefabProbability["castle"][type];
        var obj = LevelGenInstantiationUtils.InstantiateBlockObject(walls, wallProbs, x, y);
        obj.transform.Rotate(new Vector3(0, angle, 0));
    }

    public static void AddDressing(Room room, DesignedBuilding layout) {
        if (room is LivingQuarters) ActiveCastleLivingQuartersGen.AddDressing((LivingQuarters)room, layout.grid);
        if (room is CommonSpace) ActiveCastleCommonSpaceGen.AddDressing((CommonSpace)room, layout.grid);
        if (room is LivingQuarters || room is CommonSpace || room is BossRoom) AddRoomLight(room);
        AddTorches(room, layout.grid);
        room.dressingLocations.Clear();
    }

    private static void AddTorches(Room room, string[,,] grid) {
        var floor = room.floor;
        int torchRoll = Random.Range(0, 2);
        bool hasTorch;
        if (torchRoll == 0) hasTorch = true;
        else hasTorch = false;
        for (int x = room.x; x < room.x + room.xSize; x++) {
            for (int y = room.y; y < room.y + room.ySize; y++) {
                hasTorch = !hasTorch;
                foreach (var location in room.dressingLocations) if (location.x == x && location.y == y) hasTorch = false;
                if (!hasTorch) continue;
                if (LevelGenGridUtils.IsFloor(x, y, floor, grid) && !LevelGenGridUtils.IsFloor(x - 1, y, floor, grid)) MakeTorch(x - 0.5f, y, 0);
                if (LevelGenGridUtils.IsFloor(x, y, floor, grid) && !LevelGenGridUtils.IsFloor(x + 1, y, floor, grid)) MakeTorch(x + 0.5f, y, 180);
                if (LevelGenGridUtils.IsFloor(x, y, floor, grid) && !LevelGenGridUtils.IsFloor(x, y - 1, floor, grid)) MakeTorch(x, y - 0.5f, 270);
                if (LevelGenGridUtils.IsFloor(x, y, floor, grid) && !LevelGenGridUtils.IsFloor(x, y + 1, floor, grid)) MakeTorch(x, y + 0.5f, 90);
            }
        }
    }

    private static void MakeTorch(float x, float y, float angle) {
        var torches = prefabs["castle"]["torch"];
        var torchProbs = prefabProbability["castle"]["torch"];
        var obj = LevelGenInstantiationUtils.InstantiateBlockObject(torches, torchProbs, x, y);
        obj.transform.position = new Vector3(obj.transform.position.x, 1, obj.transform.position.z);
        obj.transform.Rotate(new Vector3(0, angle, 0));
    }

    private static void AddRoomLight(Room room) {
        float x = (room.x + room.x + room.xSize - 1) / 2f;
        float y = (room.y + room.y + room.ySize - 1) / 2f;
        float range = Mathf.Max(room.xSize * 5, room.ySize * 5);
        var obj = Instantiate(LevelGen.instance.roomLight, new Vector3(x * 5, range / 2, y * 5), LevelGen.instance.roomLight.transform.rotation);
        obj.transform.parent = LevelGen.instance.dungeonInstance.transform;
        LevelGen.instance.instantiatedObjects.Add(obj);
        var light = obj.GetComponent<Light>();
        light.range = range;
    }
}
