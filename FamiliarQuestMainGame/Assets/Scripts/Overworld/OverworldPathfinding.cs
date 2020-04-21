using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class OverworldPathfinding {
    private static string[,] floodFillGrid = null;

    public static Vector3 GetValidRandomPosition() {
        var startingPosition = OverworldLandmarkGenerator.landmarks[0].position;
        if (floodFillGrid == null) {
            floodFillGrid = new string[1024, 1024];
            for (int x = 0; x < 1024; x++) {
                for (int y = 0; y < 1024; y++) {
                    var height = OverworldGenerator.instance.terrain.SampleHeight(new Vector3(x, 0, y));
                    if (height / OverworldGenerator.instance.newHighest < 1 - OverworldTerrainGenerator.perlinMountainProportion && height > 0) floodFillGrid[x, y] = "y";
                    else floodFillGrid[x, y] = "n";
                }
            }
            FloodFill(floodFillGrid, startingPosition, "y", "r");
        }
        while (true) {
            var x = Random.Range(0, 1024);
            var y = Random.Range(0, 1024);
            if (floodFillGrid[x, y] == "r") return new Vector3(x, 24, y);
        }
    }

    private static void FloodFill(string[,] floodFillGrid, Vector2 position, string targetLetter, string replacementLetter) {
        List<Vector2> queue = new List<Vector2> {
            position
        };
        floodFillGrid[(int)position.x, (int)position.y] = replacementLetter;
        while (queue.Count > 0) {
            position = queue[0];
            queue.Remove(position);
            var x = (int)position.x;
            var y = (int)position.y;
            if (x < 0 || y < 0 || x >= OverworldGenerator.instance.mapSize || y >= OverworldGenerator.instance.mapSize) continue;
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x - 1, position.y), targetLetter, replacementLetter);
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x + 1, position.y), targetLetter, replacementLetter);
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x, position.y - 1), targetLetter, replacementLetter);
            FloodFillStep(floodFillGrid, queue, new Vector2(position.x, position.y + 1), targetLetter, replacementLetter);
        }
    }

    private static void FloodFillStep(string[,] floodFillGrid, List<Vector2> queue, Vector2 position, string targetLetter, string replacementLetter) {
        var x = (int)position.x;
        var y = (int)position.y;
        if (x < 0 || y < 0 || x >= OverworldGenerator.instance.mapSize || y >= OverworldGenerator.instance.mapSize) return;
        if (floodFillGrid[x, y] == targetLetter && floodFillGrid[x, y] != replacementLetter) {
            floodFillGrid[x, y] = replacementLetter;
            queue.Add(position);
        }
    }
}
