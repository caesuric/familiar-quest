//using UnityEngine;
//using System.Collections;
//using UnityEngine.Networking;
//using System.Collections.Generic;
//using System;

//public class TrapBuilder {
//    public static void AddTrap(GraphNode room, Map map, InitializeLevel il, int currentFloor) {
//        var posX = UnityEngine.Random.Range(room.left, room.right);
//        var posY = UnityEngine.Random.Range(room.top, room.bottom);
//        var obj = GameObject.Instantiate(il.prefabs["Trap Trigger"], new Vector3((posX * 2) - 120, 145, (posY * 2) - 120), new Quaternion());
//        NetworkServer.Spawn(obj);
//        var tt = obj.GetComponent<TrapTrigger>();
//        int trapTypeRoll = UnityEngine.Random.Range(0, 12);
//        var data = TextReader.ReadSets("TrapTypes");
//        var info = new Dictionary<int, string[]>();
//        foreach (var item in data) info.Add(int.Parse(item[0]), new string[] { item[1], item[2], item[3], item[4], item[5] });
//        SetTrapDetails(tt, trapTypeRoll, info, il, room, currentFloor);
//    }

//    private static void SetTrapDetails(TrapTrigger tt, int trapTypeRoll, Dictionary<int, string[]> info, InitializeLevel il, GraphNode room, int currentFloor) {
//        if (!info.ContainsKey(trapTypeRoll) || !il.prefabs.ContainsKey(info[trapTypeRoll][0])) return;
//        tt.trap = il.prefabs[info[trapTypeRoll][0]];
//        tt.trapType = info[trapTypeRoll][1];
//        float damage = 0;
//        switch (info[trapTypeRoll][1]) {
//            case "spike":
//                damage = 25;
//                break;
//            case "arrow":
//                damage = 100;
//                break;
//            case "fireball":
//                damage = 100;
//                break;
//            case "arrowSpread":
//                damage = 50;
//                break;
//            default:
//                break;
//        }
//        for (int i = 1; i < InitializeLevel.targetLevel; i++) damage *= 1.1f;
//        tt.damage = (int)damage;
//        if (info[trapTypeRoll][2] == "wall") AddTrapToWall(tt, room);
//        tt.repeatingTrigger = (info[trapTypeRoll][3] == "repeated");
//        if (info[trapTypeRoll][4] == "moveDown") TargetTrapCoordinates(tt, currentFloor, offset: 1);
//        else if (info[trapTypeRoll][4] == "teleport") TargetTrapCoordinates(tt, currentFloor);
//    }

//    private static void TargetTrapCoordinates(TrapTrigger tt, int currentFloor, int offset=0) {
//        if (InitializeLevel.maps.Count - 1 < currentFloor - 1 + offset) return;
//        var roomNumber = UnityEngine.Random.Range(0, InitializeLevel.maps[currentFloor - 1 + offset].ng.members.Count);
//        var lowerRoom = InitializeLevel.maps[currentFloor - 1 + offset].ng.members[roomNumber];
//        var lowerPosX = UnityEngine.Random.Range(lowerRoom.left, lowerRoom.right-1);
//        var lowerPosY = UnityEngine.Random.Range(lowerRoom.top, lowerRoom.bottom-1);
//        tt.trapPosition = new Vector3((lowerPosX * 2) - 120, 145f, (lowerPosY * 2) - 120);
//    }

//    private static void AddTrapToWall(TrapTrigger tt, GraphNode room) {
//        float posX = room.left;
//        float posY = room.top;
//        int leftRightTopBottom = UnityEngine.Random.Range(0, 4);
//        var trapWallTable = new Dictionary<int, Action>() {
//            {0, () => SetLeftPos(room, tt) },
//            {1, () => SetRightPos(room, tt) },
//            {2, () => SetTopPos(room, tt) },
//            {3, () => SetBottomPos(room, tt) }
//        };
//        trapWallTable[leftRightTopBottom]();
//    }

//    private static void SetLeftPos(GraphNode room, TrapTrigger tt) {
//        var posX = room.left;
//        var posY = UnityEngine.Random.Range(room.top, room.bottom);
//        tt.trapPosition = new Vector3((posX * 2) - 120, 145.5f, (posY * 2) - 120);
//    }

//    private static void SetRightPos(GraphNode room, TrapTrigger tt) {
//        var posX = room.right;
//        var posY = UnityEngine.Random.Range(room.top, room.bottom);
//        tt.trapPosition = new Vector3((posX * 2) - 120, 145.5f, (posY * 2) - 120);
//    }

//    private static void SetTopPos(GraphNode room, TrapTrigger tt) {
//        var posX = UnityEngine.Random.Range(room.left, room.right);
//        var posY = room.top;
//        tt.trapPosition = new Vector3((posX * 2) - 120, 145.5f, (posY * 2) - 120);
//    }

//    private static void SetBottomPos(GraphNode room, TrapTrigger tt) {
//        var posX = UnityEngine.Random.Range(room.left, room.right);
//        var posY = room.bottom;
//        tt.trapPosition = new Vector3((posX * 2) - 120, 145.5f, (posY * 2) - 120);
//    }
//}
