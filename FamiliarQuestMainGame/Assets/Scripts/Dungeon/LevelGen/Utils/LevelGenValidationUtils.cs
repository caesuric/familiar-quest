using System.Collections.Generic;
using UnityEngine;

public static class LevelGenValidationUtils {

    public static bool ValidHorizontalCorridor(int floor, int start, int finish, int yBase, Dungeon building) {
        for (int x = start; x < finish; x++) {
            for (int y = yBase - 1; y <= yBase + 1; y++) {
                if (x <= 0 || x >= 120 || y <= 0 || y >= 120) continue;
                if (LevelGenGridUtils.IsFloor(x, y, floor, building.grid)) return false;
            }
        }
        return true;
    }

    public static bool ValidVerticalCorridor(int floor, int start, int finish, int xBase, string[,,] grid) {
        for (int y = start; y < finish; y++) {
            for (int x = xBase - 1; x <= xBase + 1; x++) {
                if (x <= 0 || x >= 120 || y <= 0 || y >= 120) continue;
                if (LevelGenGridUtils.IsFloor(x, y, floor, grid)) return false;
            }
        }
        return true;
    }

    public static bool IsStraightConnectingLine(Room room1, Room room2, string[,,] grid) {
        if (IsStraightConnectingLineTop(room1, room2, grid)) return true;
        if (IsStraightConnectingLineRight(room1, room2, grid)) return true;
        if (IsStraightConnectingLineBottom(room1, room2, grid)) return true;
        if (IsStraightConnectingLineLeft(room1, room2, grid)) return true;
        return false;
    }

    public static bool IsStraightConnectingLineTop(Room room1, Room room2, string[,,] grid) {
        for (int x = room1.x; x < room1.x + room1.xSize; x++) {
            if (TraceUp(x, room1, room2, grid)) return true;
        }
        return false;
    }

    public static bool IsStraightConnectingLineBottom(Room room1, Room room2, string[,,] grid) {
        for (int x = room1.x; x < room1.x + room1.xSize; x++) {
            if (TraceDown(x, room1, room2, grid)) return true;
        }
        return false;
    }

    public static bool IsStraightConnectingLineRight(Room room1, Room room2, string[,,] grid) {
        for (int y = room1.y; y < room1.y + room1.ySize; y++) {
            if (TraceRight(y, room1, room2, grid)) return true;
        }
        return false;
    }

    public static bool IsStraightConnectingLineLeft(Room room1, Room room2, string[,,] grid) {
        for (int y = room1.y; y < room1.y + room1.ySize; y++) {
            if (TraceLeft(y, room1, room2, grid)) return true;
        }
        return false;
    }

    public static bool InBoundsForRoom(int x, int y, Room room) {
        var ix = room.x;
        var iy = room.y;
        var ax = room.x + room.xSize - 1;
        var ay = room.y + room.ySize - 1;
        if (ix <= x && x <= ax && iy <= y && y <= ay) return true;
        return false;
    }

    public static bool CanTraceVerticalLine(int x, Room room1, Room room2, string[,,] grid) {
        if (room1.y < room2.y) return TraceDown(x, room1, room2, grid);
        return TraceUp(x, room1, room2, grid);
    }

    public static bool CanTraceHorizontalLine(int y, Room room1, Room room2, string[,,] grid) {
        if (room1.x < room2.x) return TraceRight(y, room1, room2, grid);
        return TraceLeft(y, room1, room2, grid);
    }

    public static bool RoomFits(Room room, int floor, int xStart, int yStart, string[,,] grid) {
        for (int x = xStart - DesignedBuilding.roomSpacing; x < xStart + room.xSize + DesignedBuilding.roomSpacing; x++) {
            for (int y = yStart - DesignedBuilding.roomSpacing; y < yStart + room.ySize + DesignedBuilding.roomSpacing; y++) {
                if (x < 0 || y < 0 || x >= 120 || y >= 120) continue;
                if (grid[floor, x, y] == "X") return false;
            }
        }
        var hallwayBlocks = new List<string> { "x", ">", "<", "E" };
        for (int x = xStart - 1; x < xStart + room.xSize + 1; x++) {
            for (int y = yStart - 1; y < yStart + room.ySize + 1; y++) {
                if (x < 0 || y < 0 || x >= 120 || y >= 120) continue;
                if (hallwayBlocks.Contains(grid[floor, x, y])) return false;
            }
        }
        return true;
    }

    public static bool RoomFitsNoLeeway(Dungeon building, Room room, int floor, int xStart, int yStart, string[,,] grid) {
        for (int x = xStart; x <= xStart + room.xSize; x++) {
            for (int y = yStart; y <= yStart + room.ySize; y++) {
                if (x < 0 || y < 0 || x >= 120 || y >= 120 || x > building.maxDimensions || y > building.maxDimensions) return false;
                if (grid[floor, x, y] == "X") return false;
            }
        }
        var hallwayBlocks = new List<string> { "x", ">", "<", "E" };
        for (int x = xStart; x <= xStart + room.xSize; x++) {
            for (int y = yStart; y <= yStart + room.ySize; y++) {
                if (x < 0 || y < 0 || x >= 120 || y >= 120 || x > building.maxDimensions || y > building.maxDimensions) return false;
                if (hallwayBlocks.Contains(grid[floor, x, y])) return false;
            }
        }
        return true;
    }

    private static bool TraceUp(int x, Room room1, Room room2, string[,,] grid) {
        for (int y = room1.y - 1; y > 0; y--) {
            if (InBoundsForRoom(x, y, room2)) return true;
            if (grid[room1.floor, x, y] == "X" || grid[room1.floor, x, y] == "x") return false;
        }
        return false;
    }

    private static bool TraceDown(int x, Room room1, Room room2, string[,,] grid) {
        for (int y = room1.y + room1.ySize; y < 120; y++) {
            if (InBoundsForRoom(x, y, room2)) return true;
            if (grid[room1.floor, x, y] == "X" || grid[room1.floor, x, y] == "x") return false;
        }
        return false;
    }

    private static bool TraceLeft(int y, Room room1, Room room2, string[,,] grid) {
        for (int x = room1.x - 1; x > 0; x--) {
            if (InBoundsForRoom(x, y, room2)) return true;
            if (grid[room1.floor, x, y] == "X" || grid[room1.floor, x, y] == "x") return false;
        }
        return false;
    }

    private static bool TraceRight(int y, Room room1, Room room2, string[,,] grid) {
        for (int x = room1.x + room1.xSize; x < 120; x++) {
            if (InBoundsForRoom(x, y, room2)) return true;
            if (grid[room1.floor, x, y] == "X" || grid[room1.floor, x, y] == "x") return false;
        }
        return false;
    }
}
