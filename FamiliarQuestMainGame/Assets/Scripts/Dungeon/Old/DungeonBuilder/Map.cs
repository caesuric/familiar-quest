//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;
//using UnityEngine.Networking;

//public class Map {
//    public string[,] blocks;
//    public int mapSize;
//    public int startingX;
//    public int startingY;
//    public int endingX;
//    public int endingY;
//    public bool valid = true;
//    public int roomAttempts = 1000;
//    public NetworkGraph ng;
//    public int floor;
//    public int intendedLevel;

//    public Map(NetworkGraph ng, int mapSize, int floor) {
//        this.ng = ng;
//        this.floor = floor;
//        this.mapSize = mapSize;
//        FillMap();
//        RoomBuilder.DigStartingRoom(this);
//        foreach (var room in ng.members) if (!room.startingRoom) room.dug = false;
//        foreach (var room in ng.members) {
//            if (!room.startingRoom) {
//                bool success = RoomBuilder.AttemptToDigRoom(this, room);
//                if (!success) valid = false;
//            }
//        }
//    }

//    private void FillMap() {
//        blocks = new string[mapSize, mapSize];
//        for (int i = 0; i < mapSize; i++) for (int j = 0; j < mapSize; j++) blocks[i, j] = "W";
//    }

//    public void AddBossObject(List<GameObject> monsterList, GameObject player, float x, float y) {
//        var obj = GameObject.Instantiate(EncounterBuilder.RandomMonster(monsterList, intendedLevel), new Vector3(x, 145, y), new Quaternion());
//        obj.AddComponent<Boss>();
//        obj.GetComponent<Boss>().doorLocations = GetBossRoomDoorLocations();
//        NetworkServer.Spawn(obj);
//        var mob = obj.GetComponent<Monster>();
//        mob.GetComponent<MonsterCombatant>().player = player;
//        mob.GetComponent<MonsterScaler>().AdjustForLevel(intendedLevel);
//        mob.GetComponent<MonsterScaler>().quality = 4;
//        mob.GetComponent<MonsterScaler>().numPlayers = 1;
//        mob.GetComponent<MonsterScaler>().Scale();
//        var billboard = obj.GetComponentInChildren<Billboard>();
//        if (billboard != null) billboard.mainCamera = Camera.main;
//    }

//    private List<Vector3> GetBossRoomDoorLocations() {
//        var list = new List<Vector3>();
//        GraphNode bossRoom = GetBossRoom();
//        float x, y;
//        x = bossRoom.left;
//        for (y = bossRoom.top; y < bossRoom.bottom; y++) list.Add(mapToUnityCoords(x, y));
//        x = bossRoom.right;
//        for (y = bossRoom.top; y < bossRoom.bottom; y++) list.Add(mapToUnityCoords(x, y));
//        y = bossRoom.top;
//        for (x = bossRoom.left; x < bossRoom.right; x++) list.Add(mapToUnityCoords(x, y));
//        y = bossRoom.bottom;
//        for (x = bossRoom.left; x < bossRoom.right; x++) list.Add(mapToUnityCoords(x, y));
//        return list;
//    }

//    public static Vector3 mapToUnityCoords(float x, float y) {
//        return new Vector3((x * 2) - 120, 145, (y * 2) - 120);
//    }

//    private GraphNode GetBossRoom() {
//        foreach (var room in ng.members) if (room.bossRoom) return room;
//        return ng.members[0];
//    }

//    public int AddEncounter(int mobsLeft, List<GameObject> monsterList, GameObject player) {
//        return EncounterBuilder.AddEncounter(mobsLeft, monsterList, player, intendedLevel, ng);
//    }

//    public void Cleanup() {
//        for (int i = 0; i < mapSize; i++) {
//            for (int j = 0; j < mapSize; j++) {
//                if (i == 0 || j == 0 || i == mapSize - 1 || j == mapSize - 1) continue;
//                if (blocks[i, j] != "W") continue;
//                bool spaceFound = false;
//                for (int k = -1; k <= 1; k++) {
//                    for (int l = -1; l <= 1; l++) {
//                        if (blocks[i + k, j + l] == " ") spaceFound = true;
//                    }
//                }
//                if (!spaceFound) blocks[i, j] = "X";
//            }
//        }
//    }
//}
