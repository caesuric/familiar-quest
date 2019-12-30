//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//class RoomFeatureBuilder {
//    public static void AddRoomFeatures(Map map, RoomData roomData, GraphNode room) {
//        if (room.stairsDownRoom) AddStairsDown(map, roomData);
//        else if (room.treasureRoom) AddTreasure(map.blocks, roomData);
//        else if (room.bossRoom) AddBoss(map.blocks, roomData);
//        else AddFountainOrStatue(map.blocks, roomData);
//        AddCosmeticFeatures(room);
//        PossiblyAddDoor(map.blocks, roomData, room);
//    }

//    private static void AddStairsDown(Map map, RoomData roomData) {
//        map.blocks[roomData.startingX + (roomData.xSize / 2), roomData.startingY + (roomData.ySize / 2)] = ">";
//        map.endingX = roomData.startingX + (roomData.xSize / 2) + 1;
//        map.endingY = roomData.startingY + (roomData.ySize / 2) + 1;
//    }

//    private static void AddTreasure(string[,] blocks, RoomData roomData) {
//        int roll = UnityEngine.Random.Range(0, 2);
//        if (roll == 0) blocks[roomData.startingX + (roomData.xSize / 2), roomData.startingY + (roomData.ySize / 2)] = "A";
//        else {
//            roll = UnityEngine.Random.Range(0, 4);
//            if (roll < 3) blocks[roomData.startingX + (roomData.xSize / 2), roomData.startingY + (roomData.ySize / 2)] = "C";
//            else blocks[roomData.startingX + (roomData.xSize / 2), roomData.startingY + (roomData.ySize / 2)] = "LC";
//        }
//    }

//    private static void AddBoss(string[,] blocks, RoomData roomData) {
//        blocks[roomData.startingX + (roomData.xSize / 2), roomData.startingY + (roomData.ySize / 2)] = "B";
//    }

//    private static void AddFountainOrStatue(string[,] blocks, RoomData roomData) {
//        int fountainRoll = UnityEngine.Random.Range(0, 10);
//        if (fountainRoll == 9) blocks[roomData.startingX + (roomData.xSize / 2), roomData.startingY + (roomData.ySize / 2)] = "F";
//        else {
//            int statueRoll = UnityEngine.Random.Range(0, 10);
//            if (statueRoll == 9) blocks[roomData.startingX + (roomData.xSize / 2), roomData.startingY + (roomData.ySize / 2)] = "@";
//        }
//    }

//    private static void PossiblyAddDoor(string[,] blocks, RoomData roomData, GraphNode room) {
//        int doorChance = UnityEngine.Random.Range(0, 3);
//        foreach (var connection in room.connections) {
//            if (connection.startingRoom) doorChance = 2;
//        }
//        if (doorChance == 0) blocks[roomData.doorX, roomData.doorY] = " ";
//        else AddDoor(blocks, roomData, room);
//    }

//    private static void AddDoor(string[,] blocks, RoomData roomData, GraphNode room) {
//        int secretChance = UnityEngine.Random.Range(0, 10);
//        int lockedChance = UnityEngine.Random.Range(0, 10);
//        if (secretChance == 9 && !room.stairsDownPath) blocks[roomData.doorX, roomData.doorY] = "S";
//        else if (lockedChance == 9 && !room.stairsDownPath) {
//            if (roomData.side < 2) blocks[roomData.doorX, roomData.doorY] = "RLD";
//            else blocks[roomData.doorX, roomData.doorY] = "LD";
//        }
//        else {
//            if (roomData.side < 2) blocks[roomData.doorX, roomData.doorY] = "RD";
//            else blocks[roomData.doorX, roomData.doorY] = "D";
//        }
//    }

//    public static void AddTraps(Map map, InitializeLevel il, int currentFloor) {
//        foreach (var room in map.ng.members) {
//            if (room.startingRoom) continue;
//            int trapRoll = UnityEngine.Random.Range(0, 10);
//            if (trapRoll>=9) TrapBuilder.AddTrap(room, map, il, currentFloor);
//        }
//    }

//    public static void AddCosmeticFeatures(GraphNode room) {
//        for (int i = 0; i < 4; i++) {
//            int addDressingRoll = Random.Range(0, 16);
//            addDressingRoll = 0; //temporarily disabling
//            if (addDressingRoll >= 13 && !room.isHallway) AddRoomDressing(room, i);
//            else if (addDressingRoll == 15 && room.isHallway) AddHallwayDressing(room, i);
//        }
//    }

//    public static void AddRoomDressing(GraphNode room, int i) {
//        var dressing = TextReader.RandomSet("RoomDressing");
//        if ((room.right - room.left <= 2 || room.bottom - room.top <= 2) && dressing[1] == "middleEven") return;
//        room.dressing[i] = dressing[0];
//        room.dressingPlacement[i] = dressing[1];
//        room.dressingCount[i] = Random.Range(int.Parse(dressing[2]), int.Parse(dressing[3]) + 1);
//    }

//    public static void AddHallwayDressing(GraphNode room, int i) {
//        var dressing = TextReader.RandomSet("HallwayDressing");
//        room.dressing[i] = dressing[0];
//        room.dressingPlacement[i] = dressing[1];
//        room.dressingCount[i] = Random.Range(int.Parse(dressing[2]), int.Parse(dressing[3]) + 1);
//    }
//}
