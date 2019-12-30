using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BuildingLayout {
    public abstract void LayoutRooms();
    protected Dungeon building;

    public void ApplyRuin(float ruinLevel) {
        for (int i = 0; i < building.numFloors; i++) ApplyRuinToFloor(i, ruinLevel);
    }

    private void ApplyRuinToFloor(int floor, float ruinLevel) {
        int ruinedArea = 0;
        int areaToRuin = 0;
        foreach (var room in building.rooms) if (room.floor == floor) areaToRuin += room.xSize * room.ySize;
        areaToRuin = (int)(areaToRuin * ruinLevel);
        int maxRuinAreaSize = (int)Mathf.Sqrt(areaToRuin / 4);
        while (ruinedArea < areaToRuin) {
            int newRuinAreaSizeX = Random.Range(1, maxRuinAreaSize + 1);
            int newRuinAreaSizeY = Random.Range(1, maxRuinAreaSize + 1);
            int posX = Random.Range(0, building.maxDimensions - 1 - newRuinAreaSizeX);
            int posY = Random.Range(0, building.maxDimensions - 1 - newRuinAreaSizeY);
            for (int x = posX; x < posX + newRuinAreaSizeX; x++) {
                for (int y = posY; y < posY + newRuinAreaSizeY; y++) {
                    if (LevelGenGridUtils.IsNonStairsFloor(x, y, floor, building.grid)) {
                        building.grid[floor, x, y] = "R";
                        ruinedArea++;
                    }
                }
            }
        }
        ConnectOrphanedStairs(floor);
        EnsureStairsConnect(floor);
    }

    private void EnsureStairsConnect(int floor) {
        var startPosition = GetStartingPosition(floor);
        var endPosition = GetEndPosition(floor);
        if (startPosition == new Vector2(-1, -1) || endPosition == new Vector2(-1, -1)) return;
        if (WalkablePathBetweenPoints(floor, startPosition, endPosition)) {
            return;
        }
        var startRooms = GetRoomsConnectedToPoint(floor, startPosition);
        var endRooms = GetRoomsConnectedToPoint(floor, endPosition);
        int i = 0;
        while (i<1000) {
            i++;
            if (ConnectRoomSets(floor, startRooms, endRooms) && WalkablePathBetweenPoints(floor, startPosition, endPosition)) return;
        }
    }

    private Vector2 GetStartingPosition(int floor) {
        for (int x = 0; x < building.maxDimensions; x++) {
            for (int y = 0; y < building.maxDimensions; y++) {
                if (building.grid[floor, x, y] == "E" || building.grid[floor, x, y] == "<") return new Vector2(x, y);
            }
        }
        return new Vector2(-1, -1);
    }

    private Vector2 GetEndPosition(int floor) {
        for (int x = 0; x < building.maxDimensions; x++) {
            for (int y = 0; y < building.maxDimensions; y++) {
                if (building.grid[floor, x, y] == ">") return new Vector2(x, y);
            }
        }
        return new Vector2(-1, -1);
    }

    private bool WalkablePathBetweenPoints(int floor, Vector2 start, Vector2 end) {
        InverseGraph g = new InverseGraph(floor, building.maxDimensions, building.maxDimensions, building.grid);
        Astar astar = new Astar();
        var startCoords = new Coordinates((int)start.x, (int)start.y);
        var endCoords = new Coordinates((int)end.x, (int)end.y);
        var values = astar.SearchThroughCorridors(g, startCoords, endCoords);
        var chain = new List<Coordinates>();
        var cursor = endCoords;
        while (values.ContainsKey(cursor) && values[cursor] != cursor) {
            chain.Add(cursor);
            cursor = values[cursor];
        }
        if (values.ContainsKey(cursor)) chain.Add(values[cursor]);
        if (chain.Count > 0) chain.Reverse();
        if (chain.Count == 0 || chain[0] != startCoords) return false;
        foreach (var entry in chain) if (entry.x < 0 || entry.y < 0 || entry.x >= 120 || entry.y >= 120) return false;
        return true;
    }

    private List<Room> GetRoomsConnectedToPoint(int floor, Vector2 point) {
        var room = LevelGenRoomUtils.GetRoomFromCoords((int)point.x, (int)point.y, floor, building);
        var rooms = new List<Room> { room };
        rooms = GetConnectedRoomsRecursively(room, rooms, building.rooms);
        return rooms;
    }

    private List<Room> GetConnectedRoomsRecursively(Room room, List<Room> roomsSoFar, List<Room> allRooms) {
        var newRooms = new List<Room>();
        if (room is Corridor) {
            var corridor = (Corridor)room;
            foreach (var connectedRoom in corridor.connectedRooms) if (WalkablePathBetweenPoints(room.floor, new Vector2(room.x + (room.xSize/2), room.y + (room.ySize/2)), new Vector2(connectedRoom.x + (connectedRoom.xSize/2), connectedRoom.y + (connectedRoom.ySize/2)))) newRooms.Add(connectedRoom);
        }
        else {
            foreach (var room2 in allRooms) {
                if (room2 is Corridor) {
                    var corridor = (Corridor)room2;
                    if (corridor.connectedRooms.Contains(room)) if (WalkablePathBetweenPoints(room.floor, new Vector2(room.x + (room.xSize/2), room.y + (room.ySize/2)), new Vector2(room2.x + (room2.xSize/2), room2.y + (room2.ySize/2)))) newRooms.Add(room2);
                }
            }
        }
        var cullList = new List<Room>();
        foreach (var room2 in newRooms) if (roomsSoFar.Contains(room2)) cullList.Add(room2);
        foreach (var room2 in cullList) newRooms.Remove(room2);
        foreach (var room2 in newRooms) roomsSoFar.Add(room2);
        foreach (var room2 in newRooms) newRooms = GetConnectedRoomsRecursively(room2, roomsSoFar, allRooms);
        return allRooms;
    }

    private bool ConnectRoomSets(int floor, List<Room> set1, List<Room> set2) {
        var room1 = set1[Random.Range(0, set1.Count)];
        var room2 = set2[Random.Range(0, set2.Count)];
        var value = LevelGenConnectorUtils.ConnectTwoRooms(floor, building, room1, room2);
        return value;
    }

    private void ConnectOrphanedStairs(int floor) {
        for (int x = 0; x < building.maxDimensions; x++) {
            for (int y = 0; y < building.maxDimensions; y++) {
                if (LevelGenGridUtils.IsStairs(x, y, floor, building.grid)) ConnectOrphanedStaircase(x, y, floor);
            }
        }
    }

    private void ConnectOrphanedStaircase(int x, int y, int floor) {
        LevelGenConnectorUtils.ConnectSingleBlock(x, y, floor, building);
    }

    protected List<List<Room>> ChunkRoomsByFloors(List<Room> roomList) {
        Room bossRoom = null;
        var unassignedRooms = new List<Room>();
        foreach (var room in roomList) unassignedRooms.Add(room);
        foreach (var room in unassignedRooms) {
            if (room is BossRoom) {
                bossRoom = room;
                unassignedRooms.Remove(bossRoom);
                break;
            }
        }
        var output = new List<List<Room>>();
        var totalSizes = new List<float>();
        for (int i = 0; i < building.numFloors; i++) {
            totalSizes.Add(0);
            output.Add(new List<Room>());
        }
        output[building.numFloors - 1].Add(bossRoom);
        bossRoom.floor = building.numFloors - 1;
        totalSizes[building.numFloors - 1] += bossRoom.size;
        while (unassignedRooms.Count > 0) {
            var smallestFloor = totalSizes.IndexOf(Mathf.Min(totalSizes.ToArray()));
            var mostAppropriateRoom = GetMostAppropriateRoom(unassignedRooms, smallestFloor);
            output[smallestFloor].Add(mostAppropriateRoom);
            unassignedRooms.Remove(mostAppropriateRoom);
            mostAppropriateRoom.floor = smallestFloor;
            totalSizes[smallestFloor] += mostAppropriateRoom.size;
        }
        return output;
    }

    private Room GetMostAppropriateRoom(List<Room> roomList, int floor) {
        float lowerBand = Mathf.Floor(floor / building.numFloors);
        lowerBand = (building.maxSocialTier + 1) * lowerBand;
        float upperBand = Mathf.Floor((floor + 1) / building.numFloors);
        upperBand = (building.maxSocialTier + 1) * upperBand - 1;
        var appropriateRooms = GetRoomsInBand(roomList, (int)lowerBand, (int)upperBand);
        return roomList[0];
    }

    private List<Room> GetRoomsInBand(List<Room> roomList, int lower, int upper) {
        var output = new List<Room>();
        int tier = 0;
        foreach (var room in roomList) {
            if (room is AssignedRoom) tier = ((AssignedRoom)room).socialTier;
            else tier = 0;
            if (tier >= lower && tier <= upper) {
                output.Add(room);
            }
        }
        if (output.Count > 0) return output;
        int expander = 1;
        while (output.Count == 0 && expander <= building.maxSocialTier + 1) {
            foreach (var room in roomList) {
                if (room is AssignedRoom) tier = ((AssignedRoom)room).socialTier;
                else tier = 0;
                if (tier >= lower - expander && tier <= upper + expander) {
                    output.Add(room);
                }
            }
            expander++;
        }
        return output;
    }
}
