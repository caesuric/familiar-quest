//using UnityEngine;
//using UnityEngine.Networking;
//using System.Collections;
//using System.Collections.Generic;

//public class EncounterBuilder {
//    public static int AddEncounter(int mobsLeft, List<GameObject> monsterList, GameObject player, int intendedLevel, NetworkGraph ng) {
//        int difficultyRoll = UnityEngine.Random.Range(0, 100);
//        int classRoll = UnityEngine.Random.Range(0, 100);
//        int numMobs = NumberOfMonstersInEncounter(difficultyRoll, classRoll);
//        int level = LevelOfMonstersInEncounter(difficultyRoll, intendedLevel);
//        var room = FindRoomWithoutEncounter(ng);
//        if (room.hasEncounter) return mobsLeft - numMobs;
//        AddEncounterToRoom(room, level, classRoll, numMobs, monsterList, player, intendedLevel);
//        return mobsLeft - numMobs;
//    }

//    private static void AddEncounterToRoom(GraphNode room, int level, int classRoll, int numMobs, List<GameObject> monsterList, GameObject player, int intendedLevel) {
//        room.hasEncounter = true;
//        for (int i = 0; i < numMobs; i++) {
//            var prefab = GetRandomMonster(monsterList, intendedLevel);
//            int iterations = 1;
//            if (prefab == monsterList[0]) iterations = 3; //kobolds come in threes
//            for (int j = 0; j < iterations; j++) AddMonsterToRoom(room, level, classRoll, player, prefab);
//        }
//    }

//    public static GameObject GetRandomMonster(List<GameObject> monsterList, int intendedLevel) {
//        var prefab = RandomMonster(monsterList, intendedLevel);
//        if (monsterList.Count>=34 && prefab != monsterList[33]) return prefab;
//        int wispRoll = Random.Range(0, 10);
//        if (wispRoll != 0) return GetRandomMonster(monsterList, intendedLevel);
//        return prefab;
//    }

//    private static void AddMonsterToRoom(GraphNode room, int level, int classRoll, GameObject player, GameObject prefab) {
//        var posX = UnityEngine.Random.Range(room.left, room.right);
//        var posY = UnityEngine.Random.Range(room.top, room.bottom);
//        var obj = GameObject.Instantiate(prefab, new Vector3((posX * 2) - 120, 145, (posY * 2) - 120), new Quaternion());
//        NetworkServer.Spawn(obj);
//        SetupMonster(obj, player, level, classRoll);
//    }

//    public static void SetupMonster(GameObject obj, GameObject player, int level, int classRoll) {
//        var mob = obj.GetComponent<Monster>();
//        mob.GetComponent<MonsterCombatant>().player = player;
//        mob.GetComponent<MonsterScaler>().AdjustForLevel(level);
//        mob.GetComponent<MonsterScaler>().quality = GetMonsterQuality(classRoll);
//        mob.GetComponent<MonsterScaler>().numPlayers = 1;
//        mob.GetComponent<MonsterScaler>().Scale();
//        var billboard = obj.GetComponentInChildren<Billboard>();
//        if (billboard != null) billboard.mainCamera = Camera.main;
//    }

//    private static int GetMonsterQuality(int classRoll) {
//        if (classRoll < 50) return 0;
//        else if (classRoll < 80) return 1;
//        else if (classRoll < 95) return 2;
//        else return 3;
//    }

//    private static int NumberOfMonstersInEncounter(int difficultyRoll, int classRoll) {
//        if (classRoll >= 95) return 1; //minibosses come alone
//        if (difficultyRoll < 12) return UnityEngine.Random.Range(1, 3); //easy
//        else return UnityEngine.Random.Range(2, 5); //normal, hard, or extreme
//    }

//    private static int LevelOfMonstersInEncounter(int difficultyRoll, int intendedLevel) {
//        if (difficultyRoll < 12) return UnityEngine.Random.Range(1, intendedLevel); //easy
//        else if (difficultyRoll < 75) return intendedLevel; //normal
//        else if (difficultyRoll < 97) return Mathf.Min(intendedLevel + (UnityEngine.Random.Range(1, 5)), intendedLevel * 2); //hard
//        else return Mathf.Min(intendedLevel + (UnityEngine.Random.Range(5, 7)), intendedLevel * 3); //extreme
//    }

//    private static GraphNode FindRoomWithoutEncounter(NetworkGraph ng, int attempts = 10) {
//        var room = ng.members[UnityEngine.Random.Range(1, ng.members.Count)];
//        while (room.hasEncounter && attempts > 0) {
//            room = ng.members[UnityEngine.Random.Range(1, ng.members.Count)];
//            attempts -= 1;
//        }
//        return room;
//    }

//    public static GameObject RandomMonster(List<GameObject> monsterList, int intendedLevel) {
//        var mob = monsterList[UnityEngine.Random.Range(0, monsterList.Count)];
//        var mobStats = mob.GetComponent<Monster>();
//        if (mobStats.maxLevel < intendedLevel) return RandomMonster(monsterList, intendedLevel);
//        else if (mobStats.minLevel > intendedLevel) return RandomMonster(monsterList, intendedLevel);
//        else return mob;
//    }
//}
