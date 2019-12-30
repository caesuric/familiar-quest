//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;

//public class RoomBuilder : MonoBehaviour {
//    private static Dictionary<int, Action> doorwayMethods = new Dictionary<int, Action>();

//    public static void DigStartingRoom(Map map) {
//        int x = UnityEngine.Random.Range(1, map.mapSize - 9);
//        int y = UnityEngine.Random.Range(1, map.mapSize - 9);
//        SetStartingRoomData(map.ng, x, y);
//        map.startingX = x + 4;
//        map.startingY = y + 4;
//        for (int i = x; i < x + 8; i++) for (int j = y; j < y + 8; j++) map.blocks[i, j] = " ";
//        if (map.floor != 1) map.blocks[map.startingX - 1, map.startingY - 1] = "<";
//        else if (map.floor == 1) map.blocks[map.startingX - 1, map.startingY - 1] = "<<";
//    }
    
//    private static void SetStartingRoomData(NetworkGraph ng, int x, int y) {
//        ng.members[0].left = x;
//        ng.members[0].top = y;
//        ng.members[0].right = x + 7;
//        ng.members[0].bottom = y + 7;
//        ng.members[0].dug = true;
//        ng.members[0].startingRoom = true;
//    }

//    public static bool AttemptToDigRoom(Map map, GraphNode room) {
//        for (int i = 0; i < map.roomAttempts; i++) {
//            bool success = IndividualAttemptToDigRoom(map, room);
//            if (success) return success;
//        }
//        return false;
//    }

//    private static bool IndividualAttemptToDigRoom(Map map, GraphNode room) {
//        var connection = PickRandomConnection(room);
//        if (connection == null) return false;
//        var roomData = GenerateRoomData(room);
//        SetDoorwayMethods(roomData, connection);
//        doorwayMethods[roomData.side]();
//        if (!IsSpaceForRoom(map, roomData)) return false;
//        SetNodeData(roomData, room);
//        CarveSquareRoom(map.blocks, roomData);
//        RoomFeatureBuilder.AddRoomFeatures(map, roomData, room);
//        room.roomData = roomData;
//        return true;
//    }

//    public static void SetDoorwayMethods(RoomData roomData, GraphNode connection) {
//        doorwayMethods = new Dictionary<int, Action>() {
//            {0, () => SetDoorLeft(roomData, connection) },
//            {1, () => SetDoorRight(roomData, connection) },
//            {2, () => SetDoorTop(roomData, connection) },
//            {3, () => SetDoorBottom(roomData, connection) }
//        };
//    }

//    private static void SetDoorLeft(RoomData roomData, GraphNode connection) {
//        roomData.startingX = connection.left - roomData.xSize - 1;
//        roomData.startingY = UnityEngine.Random.Range(connection.top, connection.bottom);
//        roomData.doorX = connection.left - 1;
//        roomData.doorY = UnityEngine.Random.Range(Mathf.Max(connection.top, roomData.startingY), Mathf.Min(connection.bottom, roomData.startingY + roomData.ySize - 1));
//    }

//    private static void SetDoorRight(RoomData roomData, GraphNode connection) {
//        roomData.startingX = connection.right + 2;
//        roomData.startingY = UnityEngine.Random.Range(connection.top, connection.bottom);
//        roomData.doorX = connection.right + 1;
//        roomData.doorY = UnityEngine.Random.Range(Mathf.Max(connection.top, roomData.startingY), Mathf.Min(connection.bottom, roomData.startingY + roomData.ySize - 1));
//    }

//    private static void SetDoorTop(RoomData roomData, GraphNode connection) {
//        roomData.startingX = UnityEngine.Random.Range(connection.left, connection.right);
//        roomData.startingY = connection.top - roomData.ySize - 1;
//        roomData.doorX = UnityEngine.Random.Range(Mathf.Max(connection.left, roomData.startingX), Mathf.Min(connection.right, roomData.startingX + roomData.xSize - 1));
//        roomData.doorY = connection.top - 1;
//    }

//    private static void SetDoorBottom(RoomData roomData, GraphNode connection) {
//        roomData.startingX = UnityEngine.Random.Range(connection.left, connection.right);
//        roomData.startingY = connection.bottom + 2;
//        roomData.doorX = UnityEngine.Random.Range(Mathf.Max(connection.left, roomData.startingX), Mathf.Min(connection.right, roomData.startingX + roomData.xSize - 1));
//        roomData.doorY = connection.bottom + 1;
//    }

//    private static bool IsSpaceForRoom(Map map, RoomData roomData) {
//        var noGoX1 = map.ng.members[0].left;
//        var noGoX2 = map.ng.members[0].right;
//        var noGoY1 = map.ng.members[0].top;
//        var noGoY2 = map.ng.members[0].bottom;
//        for (int i = roomData.startingX + SideModX(roomData.side); i < roomData.startingX + SideModX(roomData.side) + roomData.xSize; i++) {
//            for (int j = roomData.startingY + SideModY(roomData.side); j < roomData.startingY + SideModY(roomData.side) + roomData.ySize; j++) {
//                if (i < 1 || j < 1 || i >= map.mapSize - 1 || j >= map.mapSize - 1 || map.blocks[i, j] != "W" || TouchingStartingRoom(i, j, noGoX1, noGoX2, noGoY1, noGoY2)) {
//                    return false;
//                }
//            }
//        }
//        return true;
//    }

//    private static bool TouchingStartingRoom(int x, int y, int x1, int x2, int y1, int y2) {
//        x1 -= 1;
//        x2 += 1;
//        y1 -= 1;
//        y2 += 1;
//        if (x >= x1 && x <= x2 && y >= y1 && y <= y2) return true;
//        return false;
//    }

//    private static void SetNodeData(RoomData roomData, GraphNode room) {
//        room.left = roomData.startingX;
//        room.top = roomData.startingY;
//        room.right = roomData.startingX + roomData.xSize - 1;
//        room.bottom = roomData.startingY + roomData.ySize - 1;
//        room.dug = true;
//    }

//    private static void CarveSquareRoom(string[,] blocks, RoomData roomData) {
//        for (int i = roomData.startingX; i < roomData.startingX + roomData.xSize; i++) {
//            for (int j = roomData.startingY; j < roomData.startingY + roomData.ySize; j++) {
//                blocks[i, j] = " ";
//            }
//        }
//    }

//    private static RoomData GenerateRoomData(GraphNode room) {
//        var roomData = new RoomData();
//        int roomType = UnityEngine.Random.Range(0, 5); //0 for hallway, 1-4 for room
//        if (room.stairsDownRoom) roomType = 4;
//        if (room.bossRoom) roomType = -1; //boss room
//        if (roomType==-1) {
//            GenerateBossRoomData(roomData);
//        }
//        else if (roomType == 0) {
//            GenerateHallwayData(roomData);
//            room.isHallway = true;
//        }
//        else {
//            GenerateSquareRoomData(roomData);
//            room.isHallway = false;
//        }
//        return roomData;
//    }

//    private static void GenerateBossRoomData(RoomData roomData) {
//        roomData.xSize = 15;
//        roomData.ySize = 15;
//        roomData.side = UnityEngine.Random.Range(0, 4);
//    }

//    private static void GenerateHallwayData(RoomData roomData) {
//        roomData.side = UnityEngine.Random.Range(0, 4);
//        if (roomData.side == 0 || roomData.side == 1) {
//            roomData.xSize = UnityEngine.Random.Range(4, 16);
//            roomData.ySize = UnityEngine.Random.Range(2, 3);
//        }
//        else {
//            roomData.xSize = UnityEngine.Random.Range(2, 3);
//            roomData.ySize = UnityEngine.Random.Range(4, 16);
//        }
//    }

//    private static void GenerateSquareRoomData(RoomData roomData) {
//        roomData.xSize = UnityEngine.Random.Range(4, 16);
//        roomData.ySize = UnityEngine.Random.Range(4, 16);
//        roomData.side = UnityEngine.Random.Range(0, 4);
//    }

//    private static GraphNode PickRandomConnection(GraphNode room) {
//        var index = UnityEngine.Random.Range(0, room.connections.Count);
//        if (room.connections[index].dug) {
//            return room.connections[index];
//        }
//        else {
//            return null;
//        }
//    }
//    private static int SideModX(int side) {
//        switch (side) {
//            case 0:
//                return -1;
//            case 1:
//                return 1;
//            case 2:
//                return 0;
//            case 3:
//            default:
//                return 0;
//        }
//    }
//    private static int SideModY(int side) {
//        switch (side) {
//            case 0:
//                return 0;
//            case 1:
//                return 0;
//            case 2:
//                return -1;
//            case 3:
//            default:
//                return 1;
//        }
//    }
//}
