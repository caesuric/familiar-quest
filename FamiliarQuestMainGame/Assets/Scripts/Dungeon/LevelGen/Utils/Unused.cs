using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UnusedLevelGenUtils {
    //private static Coordinates FindNearestHallway(int floor, Coordinates start, DesignedBuilding building) {
    //    var floodFillList = FloodFill(floor, start, new List<Coordinates>(), building);
    //    int distance = int.MaxValue;
    //    Coordinates selected = null;
    //    foreach (var item in floodFillList) {
    //        if (!AdjacentToHallway(floor, item, building)) continue;
    //        var currentDistance = Mathf.Abs(item.x - start.x) + Mathf.Abs(item.y - start.y);
    //        if (currentDistance < distance) {
    //            distance = currentDistance;
    //            selected = item;
    //        }
    //    }
    //    return selected;
    //}

    //private static void GetRoomSideLocation(Room room1, Room room2, out int x, out int y) {
    //    var x1 = room1.x;
    //    var y1 = room1.y;
    //    var x1b = room1.x + room1.xSize - 1;
    //    var y1b = room1.y + room1.ySize - 1;
    //    var x2 = room2.x;
    //    var y2 = room2.y;
    //    var x2b = room2.x + room2.xSize - 1;
    //    var y2b = room2.y + room2.ySize - 1;
    //    var left = x2b < x1;
    //    var right = x1b < x2;
    //    var bottom = y2b < y1;
    //    var top = y1b < y2;
    //    if (top && left) room1.TopLeftCorner(out x, out y);
    //    else if (left && bottom) room1.BottomLeftCorner(out x, out y);
    //    else if (bottom && right) room1.BottomRightCorner(out x, out y);
    //    else if (right && top) room1.TopRightCorner(out x, out y);
    //    else if (left) room1.LeftSide(room2, out x, out y);
    //    else if (right) room1.RightSide(room2, out x, out y);
    //    else if (bottom) room1.BottomSide(room2, out x, out y);
    //    else if (top) room1.TopSide(room2, out x, out y);
    //    else room1.TopSide(room2, out x, out y);
    //}

    //private static int GetCrookedCorridorSidePossibilities(Room room1, Room room2) {
    //    var x1 = room1.x;
    //    var y1 = room1.y;
    //    var x1b = room1.x + room1.xSize - 1;
    //    var y1b = room1.y + room1.ySize - 1;
    //    var x2 = room2.x;
    //    var y2 = room2.y;
    //    var x2b = room2.x + room2.xSize - 1;
    //    var y2b = room2.y + room2.ySize - 1;
    //    var left = x2b < x1;
    //    var right = x1b < x2;
    //    var bottom = y2b < y1;
    //    var top = y1b < y2;
    //    if (top && left) return 2;
    //    else if (left && bottom) return 2;
    //    else if (bottom && right) return 2;
    //    else if (right && top) return 2;
    //    else if (left) return room1.ySize;
    //    else if (right) return room1.ySize;
    //    else if (bottom) return room1.xSize;
    //    else if (top) return room1.xSize;
    //    else return room1.xSize;
    //}

    //private static bool CanTraceVerticalLineWithoutTargets(int x, int y1, int y2, int floor, string[,,] grid) {
    //    if (y1 < y2) return TraceDown(floor, x, y1, y2, grid);
    //    return TraceUp(floor, x, y1, y2, grid);
    //}

    //private static bool CanTraceHorizontalLineWithoutTargets(int y, int x1, int x2, int floor, string[,,] grid) {
    //    if (x1 < x2) return TraceRight(floor, y, x1, x2, grid);
    //    return TraceLeft(floor, y, x1, x2, grid);
    //}

    //private static bool IsAdjacent(int x, int y, Room room) {
    //    var left = room.x;
    //    var right = room.x + room.xSize - 1;
    //    var top = room.y;
    //    var bottom = room.y + room.ySize - 1;
    //    if (Mathf.Abs(top - y) <= 1 && x >= left && x <= right) return true;
    //    if (Mathf.Abs(y - bottom) <= 1 && x >= left && x <= right) return true;
    //    if (Mathf.Abs(left - x) <= 1 && y >= top && y <= bottom) return true;
    //    if (Mathf.Abs(x - right) <= 1 && y >= top && y <= bottom) return true;
    //    return false;
    //}

    //private static bool AdjacentToHallway(int floor, Coordinates location, DesignedBuilding building) {
    //    if (location.x - 1 >= 0 && building.grid[floor, location.x - 1, location.y] == "x") return true;
    //    if (location.x + 1 < building.maxDimensions && building.grid[floor, location.x + 1, location.y] == "x") return true;
    //    if (location.y - 1 <= 0 && building.grid[floor, location.x, location.y - 1] == "x") return true;
    //    if (location.y + 1 < building.maxDimensions && building.grid[floor, location.x, location.y + 1] == "x") return true;
    //    return false;
    //}


    //private static bool TraceUp(int floor, int x, int y1, int y2, string[,,] grid) {
    //    for (int y = y1; y > y2; y--) if (grid[floor, x, y] == "X" || grid[floor, x, y] == "x") return false;
    //    return true;
    //}

    //private static bool TraceDown(int floor, int x, int y1, int y2, string[,,] grid) {
    //    for (int y = y1; y < y2; y++) if (grid[floor, x, y] == "X" || grid[floor, x, y] == "x") return false;
    //    return true;
    //}

    //private static bool TraceLeft(int floor, int y, int x1, int x2, string[,,] grid) {
    //    for (int x = x1; x > x2; x--) if (grid[floor, x, y] == "X" || grid[floor, x, y] == "x") return false;
    //    return true;
    //}

    //private static bool TraceRight(int floor, int y, int x1, int x2, string[,,] grid) {
    //    for (int x = x1; x < x2; x++) if (grid[floor, x, y] == "X" || grid[floor, x, y] == "x") return false;
    //    return true;
    //}

    //private static List<Coordinates> FloodFill(int floor, Coordinates start, List<Coordinates> existing, DesignedBuilding building, int depth = 0) {
    //    if (depth >= 1000) return existing;
    //    if (start.x < 0 || start.x >= building.maxDimensions || start.y < 0 || start.y >= building.maxDimensions) return existing;
    //    if (building.grid[floor, start.x, start.y] == "X" || building.grid[floor, start.x, start.y] == "x") return existing;
    //    if (!existing.Contains(start)) {
    //        existing.Add(start);
    //        existing = FloodFill(floor, new Coordinates(start.x + 1, start.y), existing, building, depth + 1);
    //        existing = FloodFill(floor, new Coordinates(start.x - 1, start.y), existing, building, depth + 1);
    //        existing = FloodFill(floor, new Coordinates(start.x, start.y + 1), existing, building, depth + 1);
    //        existing = FloodFill(floor, new Coordinates(start.x, start.y - 1), existing, building, depth + 1);
    //    }
    //    return existing;
    //}

    //private static int RectDistance(int x1, int y1, int x2, int y2) {
    //    return Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);
    //}
}

