using System.Collections.Generic;
using UnityEngine;

public static class LevelGenRoomUtils {

    public static List<Room> GetUnconnectedRooms(List<Room> floorRooms, int floor) {
        var output = new List<Room>();
        foreach (var room in floorRooms) {
            if (room is Corridor) continue;
            if (RoomUnconnected(floorRooms, room, floor)) output.Add(room);
        }
        return output;
    }

    public static bool RoomUnconnected(List<Room> rooms, Room room, int floor) {
        foreach (var room2 in rooms) if (room2 is Corridor corridor && corridor.connectedRooms.Contains(room)) return false;
        return true;
    }

    public static List<Corridor> FetchCorridorsForFloor(int floor, List<Room> rooms) {
        var output = new List<Corridor>();
        foreach (var room in rooms) if (room is Corridor && room.floor == floor) output.Add((Corridor)room);
        return output;
    }

    public static Corridor FindClosestCorridor(Room room, List<Corridor> corridors) {
        Corridor closest = null;
        int closestDistance = int.MaxValue;
        foreach (var corridor in corridors) {
            var distance = GetRoomDistance(room, corridor);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = corridor;
            }
        }
        return closest;
    }

    public static Room FindClosestRoom(Room room, List<Room> rooms) {
        Room closest = null;
        int closestDistance = int.MaxValue;
        foreach (var room2 in rooms) {
            if (room2 == room) continue;
            var distance = GetRoomDistance(room, room2);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = room2;
            }
        }
        return closest;
    }

    public static Corridor FindClosestCorridorToBlock(int x, int y, int floor, Dungeon building, List<Corridor> corridors) {
        Corridor closest = null;
        int closestDistance = int.MaxValue;
        foreach (var corridor in corridors) {
            var distance = GetRoomDistanceFromBlock(x, y, corridor);
            if (distance < closestDistance) {
                closestDistance = distance;
                closest = corridor;
            }
        }
        return closest;
    }

    private static int GetRoomDistance(Room room1, Room room2) {
        var x1 = room1.x;
        var y1 = room1.y;
        var x1b = room1.x + room1.xSize - 1;
        var y1b = room1.y + room1.ySize - 1;
        var x2 = room2.x;
        var y2 = room2.y;
        var x2b = room2.x + room2.xSize - 1;
        var y2b = room2.y + room2.ySize - 1;
        var left = x2b < x1;
        var right = x1b < x2;
        var bottom = y2b < y1;
        var top = y1b < y2;
        if (top && left) return RectDistance(x1, y1b, x2b, y2);
        else if (left && bottom) return RectDistance(x1, y1, x2b, y2b);
        else if (bottom && right) return RectDistance(x1b, y1, x2, y2b);
        else if (right && top) return RectDistance(x1b, y1b, x2, y2);
        else if (left) return x1 - x2b;
        else if (right) return x2 - x1b;
        else if (bottom) return y1 - y2b;
        else if (top) return y2 - y1b;
        else return 0;
    }

    private static int GetRoomDistanceFromBlock(int x, int y, Room room) {
        var x1 = x;
        var y1 = x;
        var x1b = x;
        var y1b = x;
        var x2 = room.x;
        var y2 = room.y;
        var x2b = room.x + room.xSize - 1;
        var y2b = room.y + room.ySize - 1;
        var left = x2b < x1;
        var right = x1b < x2;
        var bottom = y2b < y1;
        var top = y1b < y2;
        if (top && left) return RectDistance(x1, y1b, x2b, y2);
        else if (left && bottom) return RectDistance(x1, y1, x2b, y2b);
        else if (bottom && right) return RectDistance(x1b, y1, x2, y2b);
        else if (right && top) return RectDistance(x1b, y1b, x2, y2);
        else if (left) return x1 - x2b;
        else if (right) return x2 - x1b;
        else if (bottom) return y1 - y2b;
        else if (top) return y2 - y1b;
        else return 0;
    }

    private static int RectDistance(int x1, int y1, int x2, int y2) {
        return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    }

    public static List<Room> RandomizeOrder(List<Room> floorRooms) {
        var output = new List<Room>();
        while (floorRooms.Count > 0) {
            int roll = Random.Range(0, floorRooms.Count);
            output.Add(floorRooms[roll]);
            floorRooms.RemoveAt(roll);
        }
        return output;
    }

    public static int CalculateMaxDimensions(List<Room> floorRooms, int numFloors) {
        int totalSize = 0;
        foreach (var room in floorRooms) totalSize += (int)room.size;
        return (int)Mathf.Sqrt(totalSize) + DesignedBuilding.floorPadding / numFloors;
    }

    public static void PackRoom(Room room, int floor, Dungeon building) {
        room.xSize = (int)Mathf.Sqrt(room.size);
        room.ySize = (int)(room.size / (float)room.xSize);
        int swapRoll = Random.Range(0, 2);
        if (swapRoll == 0) {
            var temp = room.xSize;
            room.xSize = room.ySize;
            room.ySize = temp;
        }
        for (int x = 0; x < building.maxDimensions - room.xSize; x++) {
            for (int y = 0; y < building.maxDimensions - room.ySize; y++) {
                if (LevelGenValidationUtils.RoomFits(room, floor, x, y, building.grid)) {
                    room.x = x;
                    room.y = y;
                    room.floor = floor;
                    LevelGenGridUtils.FillRoom(room, floor, x, y, building.grid);
                    return;
                }
            }
        }
        building.maxDimensions = (int)(building.maxDimensions * 1.2);
        PackRoom(room, floor, building);
    }

    public static List<Room> SortRoomsBySize(List<Room> rooms) {
        var roomsBySize = new List<Room>();
        foreach (var room in rooms) roomsBySize.Add(room);
        roomsBySize.Sort((a, b) => -a.size.CompareTo(b.size));
        Room bossRoom = null;
        foreach (var room in rooms) {
            if (room is BossRoom) {
                bossRoom = room;
                break;
            }
        }
        if (bossRoom != null) {
            roomsBySize.Remove(bossRoom);
            roomsBySize.Add(bossRoom);
        }
        return roomsBySize;
    }

    public static List<Vector2> GetPotentialSideDressingSpots(Room room, string[,,] grid) {
        var output = new List<Vector2>();
        for (int x = room.x; x < room.x + room.xSize; x++) {
            for (int y = room.y; y < room.y + room.ySize; y++) {
                if (LevelGenGridUtils.IsWallEast(x, y, room.floor, grid) || LevelGenGridUtils.IsWallNorth(x, y, room.floor, grid) || LevelGenGridUtils.IsWallSouth(x, y, room.floor, grid) || LevelGenGridUtils.IsWallWest(x, y, room.floor, grid)) output.Add(new Vector2(x, y));
            }
        }
        return output;
    }

    public static bool IsRoomEntrance(int x, int y, int floor, Dungeon layout, int direction) {
        var room = GetRoomFromCoords(x, y, floor, layout);
        if (room == null) return false;
        foreach (var entrance in room.entrances) {
            if (entrance.x == x && entrance.y == y && entrance.z == direction) return true;
        }
        return false;
    }

    public static Room GetRoomFromCoords(int x, int y, int floor, Dungeon layout) {
        foreach (var room in layout.rooms) if (room.floor == floor && room.x <= x && room.y <= y && room.x + room.xSize > x && room.y + room.ySize > y) return room;
        return null;
    }

    public static bool PackPreConnectedRooms(Dungeon building, List<Room> rooms) {
        PlaceStartingRoom(building, rooms[0]);
        var connectedRooms = new List<Room> {
            rooms[0]
        };
        rooms.Remove(rooms[0]);
        int overallTries = 100000;
        while (rooms.Count > 0 && overallTries > 0) {
            overallTries--;
            int retriesLeft = 1000;
            Room room = null;
            while (retriesLeft > 0) {
                retriesLeft--;
                room = FindConnectedRoom(connectedRooms, rooms);
                if (room == null) return false;
                var success = AttemptToConnectPreConnectedRoom(building, room, connectedRooms);
                if (success) {
                    retriesLeft = 1000;
                    rooms.Remove(room);
                    connectedRooms.Add(room);
                }
            }
            if (retriesLeft == 0) {
                JustConnectRoomRandomly(building, room, connectedRooms);
                rooms.Remove(room);
                connectedRooms.Add(room);
            }
        }
        return true;
    }

    private static bool PlaceStartingRoom(Dungeon building, Room room) {
        room.size = 25;
        room.xSize = 5;
        room.ySize = 5;
        room.x = (building.maxDimensions / 2) - 2; // 58;
        room.y = (building.maxDimensions / 2) - 2; // 58;
        room.SetEntrance((building.maxDimensions / 2) - 2, (building.maxDimensions / 2) - 2, 3); //room.SetEntrance(58, 58, 3);
        LevelGenGridUtils.FillRoom(room, 0, room.x, room.y, building.grid);
        building.grid[0, (building.maxDimensions / 2) - 3, (building.maxDimensions / 2) - 2] = "x";
        building.grid[0, (building.maxDimensions / 2) - 4, (building.maxDimensions / 2) - 2] = "E";
        return true;
    }

    private static Room FindConnectedRoom(List<Room> connectedRooms, List<Room> unconnectedRooms) {
        var possibleRooms = new List<Room>();
        foreach (var room in unconnectedRooms) if (IsConnectedToSet(room, connectedRooms)) possibleRooms.Add(room);
        if (possibleRooms.Count == 0) return null;
        int roll = RNG.Int(0, possibleRooms.Count);
        return possibleRooms[roll];
    }

    private static bool IsConnectedToSet(Room room, List<Room> connectedRooms) {
        foreach (var room2 in connectedRooms) if (AreRoomsConnected(room, room2)) return true;
        return false;
    }

    private static bool AreRoomsConnected(Room room, Room room2) {
        if (room2 is Corridor corridor2 && corridor2.connectedRooms.Contains(room)) return true;
        else if (room is Corridor corridor && corridor.connectedRooms.Contains(room2)) return true;
        return false;
    }

    private static void JustConnectRoomRandomly(Dungeon building, Room room, List<Room> connectedRooms) {
        int roll = RNG.Int(0, connectedRooms.Count);
        var room2 = connectedRooms[roll];
        var result = AttemptToConnectPreConnectedRoomSpecifically(building, room, room2);
        if (!result) JustConnectRoomRandomly(building, room, connectedRooms);
    }

    private static bool AttemptToConnectPreConnectedRoom(Dungeon building, Room room, List<Room> connectedRooms) {
        var validRooms = new List<Room>();
        foreach (var room2 in connectedRooms) {
            if (AreRoomsConnected(room, room2)) {
                validRooms.Add(room2);
            }
        }
        if (validRooms.Count == 0) return false;
        else {
            int roll = RNG.Int(0, validRooms.Count);
            return AttemptToConnectPreConnectedRoomSpecifically(building, room, validRooms[roll]);
        }
    }

    private static bool AttemptToConnectPreConnectedRoomSpecifically(Dungeon building, Room room, Room room2) {
        if (room is Corridor) {
            int directionRoll = RNG.Int(0, 2);
            if (directionRoll == 0) {
                room.xSize = RNG.Int(2, 7);
                room.ySize = RNG.Int(1, 3);
            }
            else {
                room.xSize = RNG.Int(1, 3);
                room.ySize = RNG.Int(2, 7);
            }
        }
        else if (room is BossRoom) {
            room.xSize = 10;
            room.ySize = 10;
        }
        else {
            room.xSize = RNG.Int(2, 5);
            room.ySize = RNG.Int(2, 5);
        }
        room.size = room.xSize * room.ySize;
        var coords = SelectRandomBorderingLocation(room2, room);
        if (LevelGenValidationUtils.RoomFitsNoLeeway(building, room, 0, (int)coords.x, (int)coords.y, building.grid)) {
            room.x = (int)coords.x;
            room.y = (int)coords.y;
            if (room is Corridor) {
                LevelGenGridUtils.FillRoom(room, 0, room.x, room.y, building.grid);
                //LevelGenGridUtils.FillCorridor(room, 0, room.x, room.y, building.grid);
                //AddEntrance(room2, room, FlipSide((int)coords.z));
            }
            else {
                LevelGenGridUtils.FillRoom(room, 0, room.x, room.y, building.grid);
                //AddEntrance(room, room2, (int)coords.z);
            }
            return true;
        }
        return false;
    }

    private static int FlipSide(int side) {
        switch (side) {
            case 0:
            default:
                return 2;
            case 1:
                return 3;
            case 2:
                return 0;
            case 3:
                return 1;
        }
    }

    private static Vector3 SelectRandomBorderingLocation(Room room, Room room2) {
        int side = RNG.Int(0, 4);
        int x = 0;
        int y = 0;
        switch (side) {
            case 0:
            default:
                // top
                x = RNG.Int(room.x - room2.xSize + 1, room.x + room.xSize - 1);
                y = room.y - room2.ySize;
                break;
            case 1:
                //right
                x = room.x + room.xSize;
                y = RNG.Int(room.y - room2.ySize + 1, room.y + room.ySize - 1);
                break;
            case 2:
                //bottom
                x = RNG.Int(room.x - room2.xSize + 1, room.x + room.xSize - 1);
                y = room.y + room.ySize;
                break;
            case 3:
                //left
                x = room.x - room2.xSize;
                y = RNG.Int(room.y - room2.ySize + 1, room.y + room.ySize - 1);
                break;
        }
        return new Vector3(x, y, side);
    }

    private static void AddEntrance(Room room, Room room2, int side) {
        int xMin = 0;
        int xMax = 0;
        int yMin = 0;
        int yMax = 0;
        switch (side) {
            case 0:
            default:
                // top
                xMin = Mathf.Max(room.x, room2.x);
                xMax = Mathf.Min(room.x + room.xSize - 1, room2.x + room2.xSize - 1);
                yMin = room.y;
                yMax = room.y;
                side = 0;
                break;
            case 1:
                //right
                xMin = room.x + room.xSize - 1;
                xMax = room.x + room.xSize - 1;
                yMin = Mathf.Max(room.y, room2.y);
                yMax = Mathf.Min(room.y + room2.ySize - 1, room2.y + room2.ySize - 1);
                side = 1;
                break;
            case 2:
                //bottom
                xMin = Mathf.Max(room.x, room2.x);
                xMax = Mathf.Min(room.x + room.xSize, room2.x + room2.xSize);
                yMin = room.y + room.ySize - 1;
                yMax = room.y + room.ySize - 1;
                side = 2;
                break;
            case 3:
                //left
                xMin = room.x;
                xMax = room.x;
                yMin = Mathf.Max(room.y, room2.y);
                yMax = Mathf.Min(room.y + room2.ySize - 1, room2.y + room2.ySize - 1);
                side = 3;
                break;
        }
        room.SetEntrance(RNG.Int(xMin, xMax), RNG.Int(yMin, yMax), side);
    }
}
 