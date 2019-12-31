using System.Collections.Generic;
using UnityEngine;

public static class LevelGenConnectorUtils {

    public static void ConnectRooms(int floor, List<Room> floorRooms, Dungeon building) {
        foreach (var room in floorRooms) ConnectRoom(floor, room, building);
        var unconnected = LevelGenRoomUtils.GetUnconnectedRooms(floorRooms, floor);
        foreach (var room in unconnected) ConnectRoomCrookedly(floor, room, building);
    }

    public static void ConnectRoomsToOneAnother(int floor, List<Room> floorRooms, Dungeon building) {
        var connectedRooms = new List<Room>();
        var unconnectedRooms = floorRooms;
        connectedRooms.Add(unconnectedRooms[0]);
        unconnectedRooms.RemoveAt(0);
        var attempts = 0;
        while (unconnectedRooms.Count > 0 && attempts < 10) {
            foreach (var room in unconnectedRooms) {
                ConnectRoomToOtherRooms(floor, room, building, connectedRooms);
                if (!LevelGenRoomUtils.RoomUnconnected(building.rooms, room, floor)) {
                    connectedRooms.Add(room);
                    unconnectedRooms.Remove(room);
                    break;
                }
            }
            attempts++;
        }
        attempts = 0;
        while (unconnectedRooms.Count > 0 && attempts < 10) {
            foreach (var room in unconnectedRooms) {
                ConnectRoomToRoomsCrookedly(floor, room, building, connectedRooms);
                if (!LevelGenRoomUtils.RoomUnconnected(building.rooms, room, floor)) {
                    connectedRooms.Add(room);
                    unconnectedRooms.Remove(room);
                    break;
                }
            }
            attempts++;
        }
    }

    private static void ConnectRoom(int floor, Room room, Dungeon building) {
        var corridors = LevelGenRoomUtils.FetchCorridorsForFloor(floor, building.rooms);
        var corridor = LevelGenRoomUtils.FindClosestCorridor(room, corridors);
        if (LevelGenValidationUtils.IsStraightConnectingLine(room, corridor, building.grid)) AddStraightConnectingLine(room, corridor, building);
    }

    private static void ConnectRoomToOtherRooms(int floor, Room room, Dungeon building, List<Room> floorRooms) {
        var room2 = LevelGenRoomUtils.FindClosestRoom(room, floorRooms);
        if (LevelGenValidationUtils.IsStraightConnectingLine(room, room2, building.grid)) AddStraightConnectingLine(room, room2, building);
    }


    private static void ConnectRoomCrookedly(int floor, Room room, Dungeon building) {
        var corridors = LevelGenRoomUtils.FetchCorridorsForFloor(floor, building.rooms);
        int count = 0;
        while (corridors.Count > 0 && count < 1000) {
            var corridor = LevelGenRoomUtils.FindClosestCorridor(room, corridors);
            foreach (var start in GetSurrounding(corridor)) {
                foreach (var end in GetSurrounding(room)) {
                    var _start = new Coordinates(start.x, start.y);
                    var _end = new Coordinates(end.x, end.y);
                    var result = AddCrookedConnector(room, corridor, _start, _end, building);
                    if (result) return;
                }
            }
            count++;
            corridors.Remove(corridor);
        }
    }

    private static void ConnectRoomToRoomsCrookedly(int floor, Room room, Dungeon building, List<Room> rooms) {
        int count = 0;
        while (rooms.Count > 0 && count < 1000) {
            var room2 = LevelGenRoomUtils.FindClosestRoom(room, rooms);
            foreach (var start in GetSurrounding(room2)) {
                foreach (var end in GetSurrounding(room)) {
                    var _start = new Coordinates(start.x, start.y);
                    var _end = new Coordinates(end.x, end.y);
                    var result = AddCrookedConnector(room, room2, _start, _end, building);
                    if (result) return;
                }
            }
            count++;
            rooms.Remove(room2);
        }
    }

    private static List<Coordinates> GetSurrounding(Room room) {
        var output = new List<Coordinates>();
        for (int x = room.x; x < room.x + room.xSize; x++) {
            output.Add(new Coordinates(x, room.y - 1));
            output.Add(new Coordinates(x, room.y + room.ySize));
        }
        for (int y = room.y; y < room.y + room.ySize; y++) {
            output.Add(new Coordinates(room.x - 1, y));
            output.Add(new Coordinates(room.x + room.xSize, y));
        }
        return output;
    }

    private static bool AddStraightConnectingLine(Room room, Room room2, Dungeon building) {
        var right = LevelGenValidationUtils.IsStraightConnectingLineRight(room, room2, building.grid);
        var bottom = LevelGenValidationUtils.IsStraightConnectingLineBottom(room, room2, building.grid);
        var top = LevelGenValidationUtils.IsStraightConnectingLineTop(room, room2, building.grid);
        var left = LevelGenValidationUtils.IsStraightConnectingLineLeft(room, room2, building.grid);
        bool success = false;
        if (right) {
            success = DrawHorizontalCorridor(room.floor, room.x + room.xSize, room2.x, room, room2, building);
        }
        else if (left) {
            success = DrawHorizontalCorridor(room.floor, room2.x + room2.xSize, room.x, room, room2, building);
        }
        else if (bottom) {
            success = DrawVerticalCorridor(room.floor, room.y + room.ySize, room2.y, room, room2, building);
        }
        else if (top) {
            success = DrawVerticalCorridor(room.floor, room2.y + room2.ySize, room.y, room, room2, building);
        }
        return success;
    }

    internal static bool ConnectTwoRooms(int floor, Dungeon building, Room room1, Room room2) {
        Graph g = new Graph(floor, building.maxDimensions, building.maxDimensions, building.grid);
        Astar astar = new Astar();
        var startPositions = GetSurrounding(room1);
        var endPositions = GetSurrounding(room2);
        var start = startPositions[Random.Range(0, startPositions.Count)];
        var end = endPositions[Random.Range(0, endPositions.Count)];
        var values = astar.SearchUnfilled(g, start, end);
        var chain = new List<Coordinates>();
        var cursor = end;
        while (values.ContainsKey(cursor) && values[cursor] != cursor) {
            chain.Add(cursor);
            cursor = values[cursor];
        }
        if (values.ContainsKey(cursor)) chain.Add(values[cursor]);
        if (chain.Count > 0) chain.Reverse();
        if (chain.Count == 0 || chain[0] != start) return false;
        foreach (var entry in chain) if (entry.x < 0 || entry.y < 0 || entry.x >= 120 || entry.y >= 120) return false;
        Corridor newCorridor = null;
        foreach (var entry in chain) {
            bool first = true;
            if (LevelGenGridUtils.IsFloor(entry.x, entry.y, room1.floor, building.grid)) continue;
            building.grid[floor, entry.x, entry.y] = "x";
            newCorridor = new Corridor();
            if (first) {
                newCorridor.connectedRooms.Add(room1);
                int entranceDirection;
                int entranceX = room1.ClampEntranceX(end.x);
                int entranceY = room1.ClampEntranceY(end.y);
                if (end.y < entranceY) entranceDirection = 0;
                else if (end.x > entranceX) entranceDirection = 1;
                else if (end.y > entranceY) entranceDirection = 2;
                else entranceDirection = 3;
                room1.SetEntrance(end.x, end.y, entranceDirection);
            }
            newCorridor.x = entry.x;
            newCorridor.y = entry.y;
            newCorridor.xSize = 1;
            newCorridor.ySize = 1;
            newCorridor.floor = floor;
            building.rooms.Add(newCorridor);
            first = false;
        }
        if (newCorridor != null) newCorridor.connectedRooms.Add(room2);
        return true;
    }

    private static bool AddCrookedConnector(Room room, Room room2, Coordinates start, Coordinates end, Dungeon building) {
        Graph g = new Graph(room.floor, building.maxDimensions, building.maxDimensions, building.grid);
        Astar astar = new Astar();
        var values = astar.SearchUnfilled(g, start, end);
        var chain = new List<Coordinates>();
        var cursor = end;
        while (values.ContainsKey(cursor) && values[cursor] != cursor) {
            chain.Add(cursor);
            cursor = values[cursor];
        }
        if (values.ContainsKey(cursor)) chain.Add(values[cursor]);
        if (chain.Count > 0) chain.Reverse();
        var works = true;
        if (chain.Count == 0 || chain[0].x != start.x || chain[0].y != start.y) works = false;
        foreach (var entry in chain) {
            if (entry.x < 0 || entry.y < 0 || entry.x >= 120 || entry.y >= 120) {
                works = false;
                break;
            }
        }
        if (works) {
            Corridor newCorridor = null;
            bool first = true;
            foreach (var entry in chain) {
                if (LevelGenGridUtils.IsFloor(entry.x, entry.y, room.floor, building.grid)) continue;
                building.grid[room.floor, entry.x, entry.y] = "x";
                newCorridor = new Corridor();
                if (first) {
                    newCorridor.connectedRooms.Add(room);
                    int entranceDirection;
                    int entranceX = room.ClampEntranceX(end.x);
                    int entranceY = room.ClampEntranceY(end.y);
                    if (end.y < entranceY) entranceDirection = 0;
                    else if (end.x > entranceX) entranceDirection = 1;
                    else if (end.y > entranceY) entranceDirection = 2;
                    else entranceDirection = 3;
                    room.SetEntrance(end.x, end.y, entranceDirection);
                }
                newCorridor.x = entry.x;
                newCorridor.y = entry.y;
                newCorridor.xSize = 1;
                newCorridor.ySize = 1;
                newCorridor.floor = room.floor;
                building.rooms.Add(newCorridor);
                first = false;
            }
            if (newCorridor != null) newCorridor.connectedRooms.Add(room2);
            return true;
        }
        else return false;
    }

    private static bool DrawHorizontalCorridor(int floor, int start, int finish, Room room1, Room room2, Dungeon building) {
        bool reversed = false;
        if (start > finish) {
            var temp = start;
            start = finish;
            finish = temp;
            reversed = true;
        }
        int yMin = Mathf.Max(room1.y, room2.y);
        int yMax = Mathf.Min(room1.y + room1.ySize, room2.y + room2.ySize);
        int y = Random.Range(yMin, yMax);
        if (!LevelGenValidationUtils.ValidHorizontalCorridor(floor, start, finish, y, building)) return false;
        int count = 0;
        while (!LevelGenValidationUtils.CanTraceHorizontalLine(y, room1, room2, building.grid) && count < 10000) {
            y = Random.Range(yMin, yMax);
            count++;
        }
        if (count >= 10000) return false;
        var corridor = new Corridor {
            x = start,
            xSize = finish - start,
            y = y,
            ySize = 1,
            floor = floor
        };
        corridor.connectedRooms.Add(room1);
        corridor.connectedRooms.Add(room2);
        int entranceDirection;
        int entranceX = room1.ClampEntranceX(finish);
        int entranceY = room1.ClampEntranceY(y);
        if (!reversed) entranceDirection = 1;
        else entranceDirection = 3;
        room1.SetEntrance(entranceX, entranceY, entranceDirection);
        if (corridor.xSize > 0) building.rooms.Add(corridor);
        for (int x = start; x < finish; x++) {
            building.grid[floor, x, y] = "x";
        }
        return true;
    }

    private static bool DrawVerticalCorridor(int floor, int start, int finish, Room room1, Room room2, Dungeon building) {
        bool reversed = false;
        if (start > finish) {
            var temp = start;
            start = finish;
            finish = temp;
            reversed = true;
        }
        int xMin = Mathf.Max(room1.x, room2.x);
        int xMax = Mathf.Min(room1.x + room1.xSize, room2.x + room2.xSize);
        int x = Random.Range(xMin, xMax);
        if (!LevelGenValidationUtils.ValidVerticalCorridor(floor, start, finish, x, building.grid)) return false;
        int count = 0;
        while (!LevelGenValidationUtils.CanTraceVerticalLine(x, room1, room2, building.grid) && count < 10000) {
            x = Random.Range(xMin, xMax);
            count++;
        }
        if (count >= 10000) return false;
        var corridor = new Corridor {
            x = x,
            xSize = 1,
            y = start,
            ySize = finish - start,
            floor = floor
        };
        corridor.connectedRooms.Add(room1);
        corridor.connectedRooms.Add(room2);
        int entranceDirection;
        int entranceX = room1.ClampEntranceX(x);
        int entranceY = room1.ClampEntranceY(finish);
        if (!reversed) entranceDirection = 2;
        else entranceDirection = 0;
        room1.SetEntrance(entranceX, entranceY, entranceDirection);
        if (corridor.xSize > 0) building.rooms.Add(corridor);
        building.rooms.Add(corridor);
        for (int y = start; y < finish; y++) building.grid[floor, x, y] = "x";
        return true;
    }

    public static void ConnectSingleBlock(int x, int y, int floor, Dungeon building) {
        var corridors = LevelGenRoomUtils.FetchCorridorsForFloor(floor, building.rooms);
        int count = 0;
        while (corridors.Count > 0 && count < 1000) {
            var corridor = LevelGenRoomUtils.FindClosestCorridorToBlock(x, y, floor, building, corridors);
            foreach (var start in GetSurrounding(corridor)) {
                var _start = new Coordinates(start.x, start.y);
                var _end = new Coordinates(x, y);
                var result = AddCrookedConnectorToBlock(corridor, _start, _end, building);
                if (result) return;
            }
            count++;
            corridors.Remove(corridor);
        }
    }

    public static bool AddCrookedConnectorToBlock(Room room, Coordinates start, Coordinates end, Dungeon building) {
        Graph g = new Graph(room.floor, building.maxDimensions, building.maxDimensions, building.grid);
        Astar astar = new Astar();
        var values = astar.SearchUnfilled(g, start, end);
        var chain = new List<Coordinates>();
        var cursor = end;
        while (values.ContainsKey(cursor) && values[cursor] != cursor) {
            chain.Add(cursor);
            cursor = values[cursor];
        }
        if (values.ContainsKey(cursor)) chain.Add(values[cursor]);
        if (chain.Count > 0) chain.Reverse();
        var works = true;
        if (chain.Count == 0 || chain[0].x != start.x || chain[0].y != start.y) works = false;
        foreach (var entry in chain) {
            if (entry.x < 0 || entry.y < 0 || entry.x >= 120 || entry.y >= 120) {
                works = false;
                break;
            }
        }
        if (works) {
            foreach (var entry in chain) {
                if (LevelGenGridUtils.IsFloor(entry.x, entry.y, room.floor, building.grid)) continue;
                building.grid[room.floor, entry.x, entry.y] = "x";
            }
            return true;
        }
        else return false;
    }
}
