using System.Collections.Generic;

public static class LevelGenGridUtils {

    public static void FillGrid(string[,,] grid, int floors, int xMax, int yMax) {
        for (int i = 0; i < floors; i++) {
            for (int x = 0; x < xMax; x++) {
                for (int y = 0; y < yMax; y++) {
                    grid[i, x, y] = " ";
                }
            }
        }
    }

    public static void FillRoom(Room room, int floor, int xStart, int yStart, string[,,] grid) {
        for (int x = xStart; x < xStart + room.xSize; x++) {
            for (int y = yStart; y < yStart + room.ySize; y++) {
                grid[floor, x, y] = "X";
            }
        }
    }

    public static void FillCorridor(Room room, int floor, int xStart, int yStart, string[,,] grid) {
        for (int x = xStart; x < xStart + room.xSize; x++) {
            for (int y = yStart; y < yStart + room.ySize; y++) {
                grid[floor, x, y] = "x";
            }
        }
    }

    public static bool IsCorner(int x, int y, int floor, string[,,] grid, bool northCovered = false, bool eastCovered = false, bool southCovered = false, bool westCovered = false) {
        if (!IsFloor(x, y, floor, grid)) return false;
        var left = (x == 0 || !IsFloor(x, y, floor, grid));
        var right = (x == grid.GetLength(1) - 1 || !IsFloor(x, y, floor, grid));
        var top = (y == 0 || !IsFloor(x, y, floor, grid));
        var bottom = (y == grid.GetLength(2) - 1 || !IsFloor(x, y, floor, grid));
        if (left && top && !right && !bottom && !westCovered && !northCovered) return true;
        if (left && bottom && !right && !top && !westCovered && !southCovered) return true;
        if (right && top && !left && !bottom && !eastCovered && !northCovered) return true;
        if (right && bottom && !left && !top && !eastCovered && !southCovered) return true;
        return false;
    }

    public static int GetCornerRotation(int x, int y, int floor, string[,,] grid) {
        var left = (x == 0 || !IsFloor(x, y, floor, grid));
        var right = (x == grid.GetLength(1) - 1 || !IsFloor(x, y, floor, grid));
        var top = (y == 0 || !IsFloor(x, y, floor, grid));
        var bottom = (y == grid.GetLength(2) - 1 || !IsFloor(x, y, floor, grid));
        if (left && top && !right && !bottom) return 180;
        if (left && bottom && !right && !top) return 270;
        if (right && top && !left && !bottom) return 90;
        if (right && bottom && !left && !top) return 0;
        return 0;
    }

    public static bool IsDoubleWallTarget(int x, int y, int floor, string[,,] grid) {
        if (IsDoubleWallTargetNorth(x, y, floor, grid)) return true;
        if (IsDoubleWallTargetEast(x, y, floor, grid)) return true;
        if (IsDoubleWallTargetSouth(x, y, floor, grid)) return true;
        if (IsDoubleWallTargetWest(x, y, floor, grid)) return true;
        return false;
    }

    public static bool IsDoubleWallTargetNorth(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x + 1, y, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsFloor(x + 1, y, floor, grid) && !IsFloor(x, y - 1, floor, grid) && !IsFloor(x + 1, y - 1, floor, grid) && (x / 2) * 2 == x) return true;
        return false;
    }

    public static bool IsDoubleWallTargetSouth(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x - 1, y, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsFloor(x - 1, y, floor, grid) && !IsFloor(x, y + 1, floor, grid) && !IsFloor(x - 1, y + 1, floor, grid) && (x / 2) * 2 == x) return true;
        return false;
    }

    public static bool IsDoubleWallTargetWest(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x, y - 1, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsFloor(x, y - 1, floor, grid) && !IsFloor(x - 1, y, floor, grid) && !IsFloor(x - 1, y - 1, floor, grid) && (y / 2) * 2 == y) return true;
        return false;
    }

    public static bool IsDoubleWallTargetEast(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x, y + 1, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsFloor(x, y + 1, floor, grid) && !IsFloor(x + 1, y, floor, grid) && !IsFloor(x + 1, y + 1, floor, grid) && (y / 2) * 2 == y) return true;
        return false;
    }

    public static bool IsDoubleWallSecondaryTarget(int x, int y, int floor, string[,,] grid) {
        if (IsDoubleWallSecondaryTargetNorth(x, y, floor, grid)) return true;
        if (IsDoubleWallSecondaryTargetSouth(x, y, floor, grid)) return true;
        if (IsDoubleWallSecondaryTargetEast(x, y, floor, grid)) return true;
        if (IsDoubleWallSecondaryTargetWest(x, y, floor, grid)) return true;
        return false;
    }

    public static bool IsDoubleWallSecondaryTargetNorth(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x - 1, y, floor, grid)) return false;
        if (IsNorthCoveredByReverseCorner(x - 1, y, floor, grid)) return false;
        if (IsFullTileTarget(x - 1, y - 1, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsNonStairsFloor(x - 1, y, floor, grid) && !IsFloor(x, y - 1, floor, grid) && !IsFloor(x - 1, y - 1, floor, grid) && (x / 2) * 2 != x) return true;
        return false;
    }

    public static bool IsDoubleWallSecondaryTargetSouth(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x + 1, y, floor, grid)) return false;
        if (IsSouthCoveredByReverseCorner(x + 1, y, floor, grid)) return false;
        if (IsFullTileTarget(x + 1, y + 1, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsNonStairsFloor(x + 1, y, floor, grid) && !IsFloor(x, y + 1, floor, grid) && !IsFloor(x + 1, y + 1, floor, grid) && (x / 2) * 2 != x) return true;
        return false;
    }

    public static bool IsDoubleWallSecondaryTargetWest(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x, y + 1, floor, grid)) return false;
        if (IsWestCoveredByReverseCorner(x, y + 1, floor, grid)) return false;
        if (IsFullTileTarget(x - 1, y + 1, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsNonStairsFloor(x, y + 1, floor, grid) && !IsFloor(x - 1, y, floor, grid) && !IsFloor(x - 1, y + 1, floor, grid) && (y / 2) * 2 != y) return true;
        return false;
    }

    public static bool IsDoubleWallSecondaryTargetEast(int x, int y, int floor, string[,,] grid) {
        if (IsCorner(x, y, floor, grid) || IsCorner(x, y - 1, floor, grid)) return false;
        if (IsEastCoveredByReverseCorner(x, y - 1, floor, grid)) return false;
        if (IsFullTileTarget(x + 1, y - 1, floor, grid)) return false;
        if (IsFloor(x, y, floor, grid) && IsNonStairsFloor(x, y - 1, floor, grid) && !IsFloor(x + 1, y, floor, grid) && !IsFloor(x + 1, y - 1, floor, grid) && (y / 2) * 2 != y) return true;
        return false;
    }

    public static bool IsWallNorth(int x, int y, int floor, string[,,] grid) {
        return (IsFloor(x, y, floor, grid) && !IsFloor(x, y - 1, floor, grid));
    }

    public static bool IsWallSouth(int x, int y, int floor, string[,,] grid) {
        return (IsFloor(x, y, floor, grid) && !IsFloor(x, y + 1, floor, grid));
    }

    public static bool IsWallWest(int x, int y, int floor, string[,,] grid) {
        return (IsFloor(x, y, floor, grid) && !IsFloor(x - 1, y, floor, grid));
    }

    public static bool IsWallEast(int x, int y, int floor, string[,,] grid) {
        return (IsFloor(x, y, floor, grid) && !IsFloor(x + 1, y, floor, grid));
    }

    public static bool IsFloor(int x, int y, int floor, string[,,] grid) {
        if (x < 0 || y < 0 || x >= grid.GetLength(1) || y >= grid.GetLength(2)) return false;
        var valid = new List<string> { "X", "x", "E", "<", ">" };
        return (valid.Contains(grid[floor, x, y]));
    }

    public static bool IsStairs(int x, int y, int floor, string[,,] grid) {
        if (x < 0 || y < 0 || x >= grid.GetLength(1) || y >= grid.GetLength(2)) return false;
        var valid = new List<string> { "E", "<", ">" };
        return (valid.Contains(grid[floor, x, y]));
    }

    public static bool IsCorridor(int x, int y, int floor, string[,,] grid) {
        if (x < 0 || y < 0 || x >= grid.GetLength(1) || y >= grid.GetLength(2)) return false;
        var valid = new List<string> { "x", "E", "<", ">" };
        return (valid.Contains(grid[floor, x, y]));
    }

    public static bool IsNonStairsFloor(int x, int y, int floor, string[,,] grid) {
        var invalid = new List<string> { ">", "<", "E" };
        if (x < 0 || y < 0 || x >= grid.GetLength(1) || y >= grid.GetLength(2)) return false;
        if (invalid.Contains(grid[floor, x, y])) return false;
        return IsFloor(x, y, floor, grid);
    }

    public static float GetWallRotation(int x, int y, int floor, string[,,] grid) {
        if (IsWallWest(x, y, floor, grid)) return 90;
        if (IsWallEast(x, y, floor, grid)) return 270;
        if (IsWallSouth(x, y, floor, grid)) return 180;
        if (IsWallNorth(x, y, floor, grid)) return 0;
        return 0;
    }

    public static float GetXSideDressingAdjustment(int x, int y, int floor, string[,,] grid) {
        if (IsWallWest(x, y, floor, grid)) return -0.3f;
        if (IsWallEast(x, y, floor, grid)) return 0.3f;
        return 0;
    }

    public static float GetYSideDressingAdjustment(int x, int y, int floor, string[,,] grid) {
        if (IsWallWest(x, y, floor, grid)) return 0;
        if (IsWallEast(x, y, floor, grid)) return 0;
        if (IsWallNorth(x, y, floor, grid)) return -0.3f;
        if (IsWallSouth(x, y, floor, grid)) return 0.3f;
        return 0;
    }

    public static bool IsReverseCornerTopLeft(int x, int y, int floor, string[,,] grid) {
        if (IsFullTileTarget(x - 1, y - 1, floor, grid)) return false;
        if (!IsFloor(x, y, floor, grid)) return false;
        if (IsFloor(x - 1, y, floor, grid) && IsFloor(x, y - 1, floor, grid) && !IsFloor(x - 1, y - 1, floor, grid)) return true;
        return false;
    }

    public static bool IsReverseCornerTopRight(int x, int y, int floor, string[,,] grid) {
        if (IsFullTileTarget(x + 1, y - 1, floor, grid)) return false;
        if (!IsFloor(x, y, floor, grid)) return false;
        if (IsFloor(x + 1, y, floor, grid) && IsFloor(x, y - 1, floor, grid) && !IsFloor(x + 1, y - 1, floor, grid)) return true;
        return false;
    }

    public static bool IsReverseCornerBottomLeft(int x, int y, int floor, string[,,] grid) {
        if (IsFullTileTarget(x - 1, y + 1, floor, grid)) return false;
        if (!IsFloor(x, y, floor, grid)) return false;
        if (IsFloor(x - 1, y, floor, grid) && IsFloor(x, y + 1, floor, grid) && !IsFloor(x - 1, y + 1, floor, grid)) return true;
        return false;
    }

    public static bool IsReverseCornerBottomRight(int x, int y, int floor, string[,,] grid) {
        if (IsFullTileTarget(x + 1, y + 1, floor, grid)) return false;
        if (!IsFloor(x, y, floor, grid)) return false;
        if (IsFloor(x + 1, y, floor, grid) && IsFloor(x, y + 1, floor, grid) && !IsFloor(x + 1, y + 1, floor, grid)) return true;
        return false;
    }

    public static bool IsFullTileTarget(int x, int y, int floor, string[,,] grid) {
        if (IsFloor(x, y, floor, grid)) return false;
        if (IsFloor(x - 1, y, floor, grid) && IsFloor(x + 1, y, floor, grid)) return true;
        if (IsFloor(x, y - 1, floor, grid) && IsFloor(x, y + 1, floor, grid)) return true;
        return false;
    }

    public static bool IsCoveredByReverseCorner(int x, int y, int floor, string[,,] grid) {
        if (IsWestCoveredByReverseCorner(x, y, floor, grid)) return true;
        if (IsEastCoveredByReverseCorner(x, y, floor, grid)) return true;
        if (IsNorthCoveredByReverseCorner(x, y, floor, grid)) return true;
        if (IsSouthCoveredByReverseCorner(x, y, floor, grid)) return true;
        return false;
    }

    public static bool IsWestCoveredByReverseCorner(int x, int y, int floor, string[,,] grid) {
        return (IsReverseCornerTopLeft(x, y + 1, floor, grid) || IsReverseCornerBottomLeft(x, y - 1, floor, grid));
    }

    public static bool IsEastCoveredByReverseCorner(int x, int y, int floor, string[,,] grid) {
        return (IsReverseCornerTopRight(x, y + 1, floor, grid) || IsReverseCornerBottomRight(x, y - 1, floor, grid));
    }

    public static bool IsNorthCoveredByReverseCorner(int x, int y, int floor, string[,,] grid) {
        return (IsReverseCornerTopLeft(x + 1, y, floor, grid) || IsReverseCornerTopRight(x - 1, y, floor, grid));
    }

    public static bool IsSouthCoveredByReverseCorner(int x, int y, int floor, string[,,] grid) {
        return (IsReverseCornerBottomLeft(x + 1, y, floor, grid) || IsReverseCornerBottomRight(x - 1, y, floor, grid));
    }
}
