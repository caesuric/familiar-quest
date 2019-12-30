using System.Collections.Generic;
using UnityEngine;

public class DesignedBuildingLayout : BuildingLayout {

    public DesignedBuildingLayout(DesignedBuilding building) {
        this.building = building;
    }

    public override void LayoutRooms() {
        LevelGenGridUtils.FillGrid(building.grid, building.numFloors, 120, 120);
        var roomsBySize = LevelGenRoomUtils.SortRoomsBySize(building.rooms);
        building.numFloors = Random.Range(1, 6);
        var roomsByFloor = ChunkRoomsByFloors(roomsBySize);
        for (int i = 0; i < building.numFloors; i++) LayoutFloor(roomsByFloor[i], i);
    }

    private void LayoutFloor(List<Room> floorRooms, int floor) {
        building.maxDimensions = LevelGenRoomUtils.CalculateMaxDimensions(floorRooms, building.numFloors);
        floorRooms = LevelGenRoomUtils.RandomizeOrder(floorRooms);
        AddStartingCorridors(floor, building.maxDimensions, floorRooms.Count);
        Vector2 entranceLocation = new Vector2(0,0);
        Vector2 exitLocation = new Vector2(0, 0);
        entranceLocation = GetEntranceLocation(floor, building.rooms, building.grid);
        exitLocation = GetExitLocation(floor, building.rooms, building.grid, entranceLocation);
        if (floor == 0) building.grid[floor, (int)entranceLocation.x, (int)entranceLocation.y] = "E";
        else building.grid[floor, (int)entranceLocation.x, (int)entranceLocation.y] = "<";
        if (floor < building.numFloors - 1) building.grid[floor, (int)exitLocation.x, (int)exitLocation.y] = ">";
        foreach (var room in floorRooms) LevelGenRoomUtils.PackRoom(room, floor, building);
        LevelGenConnectorUtils.ConnectRooms(floor, floorRooms, building);
    }

    private void AddStartingCorridors(int floor, int maxDimensions, int numRooms) {
        int xRoll = Random.Range(1, numRooms / 8);
        int yRoll = Random.Range(1, numRooms / 8);
        for (int i = 0; i < xRoll; i++) {
            int xPos = (building.maxDimensions * (i + 1) / (xRoll + 1));
            var vertCorridor = new Corridor();
            building.rooms.Add(vertCorridor);
            vertCorridor.floor = floor;
            vertCorridor.x = xPos;
            vertCorridor.y = 0;
            vertCorridor.xSize = 1;
            vertCorridor.ySize = building.maxDimensions;
            for (int y = 0; y < building.maxDimensions; y++) building.grid[floor, xPos, y] = "x";
        }
        for (int i = 0; i < yRoll; i++) {
            int yPos = (building.maxDimensions * (i + 1) / (yRoll + 1));
            var horizCorridor = new Corridor();
            building.rooms.Add(horizCorridor);
            horizCorridor.floor = floor;
            horizCorridor.x = 0;
            horizCorridor.y = yPos;
            horizCorridor.xSize = building.maxDimensions;
            horizCorridor.ySize = 1;
            for (int x = 0; x < building.maxDimensions; x++) building.grid[floor, x, yPos] = "x";
        }
    }

    private Vector2 GetEntranceLocation(int floor, List<Room> rooms, string[,,] grid) {
        return GetRandomCorridorEnd(floor, rooms, grid);
    }

    private Vector2 GetExitLocation(int floor, List<Room> rooms, string[,,] grid, Vector2 entranceLocation) {
        Vector2 output = GetRandomCorridorEnd(floor, rooms, grid);
        while (output == entranceLocation) output = GetRandomCorridorEnd(floor, rooms, grid);
        return output;
    }

    private Vector2 GetRandomCorridorEnd(int floor, List<Room> rooms, string[,,] grid) {
        var corridors = new List<Room>();
        foreach (var room in rooms) if (room is Corridor && room.floor == floor) corridors.Add(room);
        int corridorRoll = Random.Range(0, corridors.Count);
        var corridor = corridors[corridorRoll];
        int xRoll = Random.Range(0, 2);
        int yRoll = Random.Range(0, 2);
        int x, y;
        if (xRoll == 0) x = corridor.x;
        else x = corridor.x + corridor.xSize - 1;
        if (yRoll == 0) y = corridor.y;
        else y = corridor.y + corridor.ySize - 1;
        return new Vector2(x, y);
    }
}