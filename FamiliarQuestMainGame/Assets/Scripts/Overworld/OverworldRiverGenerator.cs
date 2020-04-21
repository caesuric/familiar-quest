using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public static class OverworldRiverGenerator {
    private static readonly int numRivers = 80;
    public static readonly int riverRadius = 2;
    private static readonly Dictionary<Vector2, float> originalRiverHeights = new Dictionary<Vector2, float>();
    private static List<Vector2> riverPoints = new List<Vector2>();

    public static IEnumerator AddRivers() {
        OverworldGenerator.instance.highest = 0;
        for (int x = 0; x < OverworldGenerator.instance.mapSize; x++) {
            for (int y = 0; y < OverworldGenerator.instance.mapSize; y++) {
                if (OverworldGenerator.instance.elevation[x, y] > OverworldGenerator.instance.highest) OverworldGenerator.instance.highest = OverworldGenerator.instance.elevation[x, y];
            }
        }
        for (int i = 0; i < numRivers; i++) {
            Vector2[] points = new Vector2[20];
            for (int j = 0; j < 20; j++) {
                points[j].x = Random.Range(0, OverworldGenerator.instance.mapSize);
                points[j].y = Random.Range(0, OverworldGenerator.instance.mapSize);
            }
            var highest = FindHighestPoint(points);
            AddRiver((int)highest.x, (int)highest.y);
            if (i % 8 == 0) {
                OverworldGenerator.instance.UpdateProgress(1, (float)i / numRivers);
                yield return null;
            }
        }
    }

    private static Vector2 FindHighestPoint(Vector2[] points) {
        float highestValue = 0;
        Vector2 highestPoint = new Vector2(0, 0);
        foreach (var point in points) {
            if (OverworldGenerator.instance.elevation[(int)point.x, (int)point.y] > highestValue) {
                highestValue = OverworldGenerator.instance.elevation[(int)point.x, (int)point.y];
                highestPoint = point;
            }
        }
        return highestPoint;
    }
    private static void AddRiver(int startingX, int startingY) {
        var roll = Random.Range(0f, 18000f);
        var coords = new Vector2(startingX, startingY);
        var length = 4000;
        var points = new List<float>();
        for (int j = 0; j < length; j++) points.Add(Mathf.PerlinNoise(roll + j, 0));
        var angle = Random.Range(0f, 360f);
        var i = 0;
        while (InBounds((int)coords.x, (int)coords.y) && (OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y] == 0 || OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y] > OverworldTerrainGenerator.perlinWaterProportion / OverworldGenerator.instance.highest)) {
            i++;
            if (i >= length) break;
            var output = RiverWalk(angle, coords, points[i]);
            angle = output.Item1;
            coords = output.Item2;
        }
    }

    private static System.Tuple<float, Vector2> RiverWalk(float angle, Vector2 coords, float noisePoint) {
        RegisterRiverPoint(coords);
        if ((int)coords.x == 0 || (int)coords.y == 0 || (int)coords.x == OverworldGenerator.instance.mapSize - 1 || (int)coords.y == OverworldGenerator.instance.mapSize - 1) return new System.Tuple<float, Vector2>(angle, coords);
        coords = RiverWalkBasedOnElevation(coords);
        RegisterRiverPoint(coords);
        var output = RiverWalkBasedOnPerlinNoise(angle, coords, noisePoint);
        angle = output.Item1;
        coords = output.Item2;
        return new System.Tuple<float, Vector2>(angle, coords);
    }

    private static Vector2 RiverWalkBasedOnElevation(Vector2 coords) {
        var northElev = OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y - 1];
        var southElev = OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y + 1];
        var westElev = OverworldGenerator.instance.elevation[(int)coords.x - 1, (int)coords.y];
        var eastElev = OverworldGenerator.instance.elevation[(int)coords.x + 1, (int)coords.y];
        float minimum = 100;
        if (northElev < minimum && northElev != 0) minimum = northElev;
        if (southElev < minimum && southElev != 0) minimum = southElev;
        if (eastElev < minimum && eastElev != 0) minimum = eastElev;
        if (westElev < minimum && westElev != 0) minimum = westElev;
        if (northElev == minimum) coords = RiverWalkNorth(coords);
        else if (southElev == minimum) coords = RiverWalkSouth(coords);
        else if (westElev == minimum) coords = RiverWalkWest(coords);
        else if (eastElev == minimum) coords = RiverWalkEast(coords);
        return coords;
    }

    private static System.Tuple<float, Vector2> RiverWalkBasedOnPerlinNoise(float angle, Vector2 coords, float noisePoint) {
        if (noisePoint < 0.5f) angle -= (90 * (noisePoint * 2));
        else angle += (90 * (noisePoint - 0.5f) * 2);
        if (angle < 0) angle += 360;
        else if (angle >= 360) angle -= 360;
        if (angle >= 0 && angle < 90 && InBounds((int)coords.x, (int)coords.y - 1) && OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y - 1] != 0) coords = new Vector2(coords.x, coords.y - 1);
        else if (angle >= 90 && angle < 180 && InBounds((int)coords.x + 1, (int)coords.y) && OverworldGenerator.instance.elevation[(int)coords.x + 1, (int)coords.y] != 0) coords = new Vector2(coords.x + 1, coords.y);
        else if (angle >= 180 && angle < 270 && InBounds((int)coords.x, (int)coords.y + 1) && OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y + 1] != 0) coords = new Vector2(coords.x, coords.y + 1);
        else if (angle >= 270 && angle < 360 && InBounds((int)coords.x - 1, (int)coords.y) && OverworldGenerator.instance.elevation[(int)coords.x - 1, (int)coords.y] != 0) coords = new Vector2(coords.x - 1, coords.y);
        return new System.Tuple<float, Vector2>(angle, coords);
    }

    private static Vector2 RiverWalkNorth(Vector2 coords) {
        coords.y--;
        return coords;
    }

    private static Vector2 RiverWalkSouth(Vector2 coords) {
        coords.y++;
        return coords;
    }

    private static Vector2 RiverWalkEast(Vector2 coords) {
        coords.x++;
        return coords;
    }

    private static Vector2 RiverWalkWest(Vector2 coords) {
        coords.x--;
        return coords;
    }

    private static void RegisterRiverPoint(Vector2 coords) {
        if (!InBounds((int)coords.x, (int)coords.y)) return;
        originalRiverHeights[new Vector2((int)coords.x, (int)coords.y)] = OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y];
        OverworldGenerator.instance.elevation[(int)coords.x, (int)coords.y] = 0;
        riverPoints.Add(coords);
    }

    public static IEnumerator WidenRivers() {
        int i = 0;
        foreach (var point in riverPoints) {
            for (int x = (int)point.x - riverRadius; x <= (int)point.x + riverRadius; x++) {
                for (int y = (int)point.y - riverRadius; y <= (int)point.y + riverRadius; y++) {
                    if (InBounds(x, y)) {
                        originalRiverHeights[new Vector2(x, y)] = OverworldGenerator.instance.elevation[x, y];
                        if (OverworldGenerator.instance.elevation[x, y] != 0) OverworldGenerator.instance.elevation[x, y] = 0;
                    }
                }
            }
            if (i % riverPoints.Count / 10 == 0) {
                OverworldGenerator.instance.UpdateProgress(2, (float)i / riverPoints.Count);
                yield return null;
            }
            i++;
        }
    }

    private static bool InBounds(int x, int y) {
        if (x < 0) return false;
        if (y < 0) return false;
        if (x >= OverworldGenerator.instance.mapSize) return false;
        if (y >= OverworldGenerator.instance.mapSize) return false;
        return true;
    }
}