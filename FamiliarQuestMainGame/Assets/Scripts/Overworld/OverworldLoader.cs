using UnityEngine;
using System.Collections;

public static class OverworldLoader {
    public static void Load() {
        OverworldGenerator.instance.elevation = OverworldGenerator.loadedElevation;
        OverworldLandmarkGenerator.landmarks.Clear();
        LoadBase();
        LoadDungeons();
        DetermineHighestElevation();
    }

    private static void LoadBase() {
        OverworldLandmarkGenerator.landmarks.Add(new OverworldLandmark() {
            type = "base",
            position = OverworldGenerator.loadedBaseCoords
        });
        OverworldLandmarkGenerator.landmarks.Add(new OverworldLandmark() {
            type = "startingPosition",
            position = OverworldGenerator.loadedBaseCoords
        });
    }

    private static void LoadDungeons() {
        foreach (var dungeonPosition in OverworldGenerator.loadedDungeonCoords) {
            OverworldLandmarkGenerator.landmarks.Add(new OverworldLandmark() {
                type = "dungeon",
                position = dungeonPosition
            });
        }
    }

    private static void DetermineHighestElevation() {
        OverworldGenerator.instance.highest = 0f;
        for (int x = 0; x < OverworldGenerator.instance.mapSize; x++) {
            for (int y = 0; y < OverworldGenerator.instance.mapSize; y++) {
                if (OverworldGenerator.instance.elevation[x, y] > OverworldGenerator.instance.highest) OverworldGenerator.instance.highest = OverworldGenerator.instance.elevation[x, y];
            }
        }
    }
}
