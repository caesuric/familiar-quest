using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureVaultLayout : BuildingLayout {
    public TreasureVaultLayout(Vault treasureVault) {
        building = treasureVault;
    }

    public override void LayoutRooms() {
        building.numFloors = 1;
        var worked = SetupVault();
        while (!worked) worked = SetupVault();
    }

    public bool TryLayoutRooms() {
        building.numFloors = 1;
        var worked = SetupVault();
        return worked;
    }

    public bool SetupVault() {
        LevelGenGridUtils.FillGrid(building.grid, building.numFloors, 120, 120);
        var success = true;
        var rooms = new List<Room>();
        foreach (var room in building.rooms) rooms.Add(room);
        success = LevelGenRoomUtils.PackPreConnectedRooms(building, rooms);
        return success;
    }
}

//public class TreasureVaultLayout : BuildingLayout {

//    public TreasureVaultLayout(Vault treasureVault) {
//        building = treasureVault;
//    }

//    public override void LayoutRooms() {
//        building.numFloors = Random.Range(1, 6);
//        LevelGenGridUtils.FillGrid(building.grid, building.numFloors, 120, 120);
//        var roomsBySize = LevelGenRoomUtils.SortRoomsBySize(building.rooms);
//        var roomsByFloor = ChunkRoomsByFloors(roomsBySize);
//        for (int i = 0; i < building.numFloors; i++) LayoutFloor(roomsByFloor[i], i);
//    }

//    private void LayoutFloor(List<Room> floorRooms, int floor) {
//        building.maxDimensions = LevelGenRoomUtils.CalculateMaxDimensions(floorRooms, building.numFloors);
//        floorRooms = LevelGenRoomUtils.RandomizeOrder(floorRooms);
//        foreach (var room in floorRooms) LevelGenRoomUtils.PackRoom(room, floor, building);
//        LevelGenConnectorUtils.ConnectRoomsToOneAnother(floor, floorRooms, building);

//        Vector2 entranceLocation = GetEntranceLocation(floor, building.rooms, building.grid);
//        Vector2 exitLocation = GetExitLocation(floor, building.rooms, building.grid, entranceLocation);
//        if (floor == 0) building.grid[floor, (int)entranceLocation.x, (int)entranceLocation.y] = "E";
//        else building.grid[floor, (int)entranceLocation.x, (int)entranceLocation.y] = "<";
//        if (floor < building.numFloors - 1) building.grid[floor, (int)exitLocation.x, (int)exitLocation.y] = ">";
//    }

//    private Vector2 GetEntranceLocation(int floor, List<Room> rooms, string[,,] grid) {
//        return GetRandomRoomEdge(floor, rooms, grid);
//    }

//    private Vector2 GetExitLocation(int floor, List<Room> rooms, string[,,] grid, Vector2 entranceLocation) {
//        Vector2 output = GetRandomRoomEdge(floor, rooms, grid);
//        while (output == entranceLocation) output = GetRandomRoomEdge(floor, rooms, grid);
//        return output;
//    }

//    private Vector2 GetRandomRoomEdge(int floor, List<Room> rooms, string[,,] grid) {
//        int i = Random.Range(0, rooms.Count);
//        var room = rooms[i];
//        int side = Random.Range(0, 4);
//        Vector2 output;
//        switch (side) {
//            case 0:
//                output = new Vector2(room.x + Random.Range(0, room.xSize), room.y);
//                if (HasWallAdjacent(output, floor, grid)) return output;
//                else return GetRandomRoomEdge(floor, rooms, grid);
//            case 1:
//                output = new Vector2(room.x + Random.Range(0, room.xSize), room.y + room.ySize - 1);
//                if (HasWallAdjacent(output, floor, grid)) return output;
//                else return GetRandomRoomEdge(floor, rooms, grid);
//            case 2:
//                output = new Vector2(room.x, room.y + Random.Range(0, room.ySize));
//                if (HasWallAdjacent(output, floor, grid)) return output;
//                else return GetRandomRoomEdge(floor, rooms, grid);
//            case 3:
//            default:
//                output = new Vector2(room.x + room.xSize - 1, room.y + Random.Range(0, room.ySize));
//                if (HasWallAdjacent(output, floor, grid)) return output;
//                else return GetRandomRoomEdge(floor, rooms, grid);
//        }
//    }

//    private bool HasWallAdjacent(Vector2 coordinates, int floor, string[,,] grid) {
//        if (LevelGenGridUtils.IsWallEast((int)coordinates.x, (int)coordinates.y, floor, grid)) return true;
//        if (LevelGenGridUtils.IsWallSouth((int)coordinates.x, (int)coordinates.y, floor, grid)) return true;
//        if (LevelGenGridUtils.IsWallWest((int)coordinates.x, (int)coordinates.y, floor, grid)) return true;
//        if (LevelGenGridUtils.IsWallNorth((int)coordinates.x, (int)coordinates.y, floor, grid)) return true;
//        return false;
//    }
//}
