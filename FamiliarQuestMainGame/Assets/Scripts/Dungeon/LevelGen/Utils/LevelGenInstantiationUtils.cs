using System.Collections.Generic;
using UnityEngine;

class LevelGenInstantiationUtils : MonoBehaviour {
    public static GameObject InstantiateBlockObject(Dictionary<string, GameObject> parts, Dictionary<string, float> probabilities, float x, float y, bool placeOnNavMesh=false) {
        float roll = Random.Range(0f, 1f);
        var keys = new List<string>();
        foreach (var pair in parts) keys.Add(pair.Key);
        var prefab = DeterminePrefab(parts, probabilities, keys, roll);
        var obj = Instantiate(prefab, new Vector3(5 * x, 0, 5 * y), prefab.transform.rotation);
        if (!placeOnNavMesh) obj.transform.parent = LevelGen.instance.dungeonInstance.transform;
        else obj.transform.parent = LevelGen.instance.navMeshSurface.transform;
        LevelGen.instance.instantiatedObjects.Add(obj);
        return obj;
    }

    public static GameObject InstantiateBlockObjectFixed(float roll, Dictionary<string, GameObject> parts, Dictionary<string, float> probabilities, float x, float y) {
        var keys = new List<string>();
        foreach (var pair in parts) keys.Add(pair.Key);
        var prefab = DeterminePrefab(parts, probabilities, keys, roll);
        var obj = Instantiate(prefab, new Vector3(5 * x, 0, 5 * y), prefab.transform.rotation);
        obj.transform.parent = LevelGen.instance.dungeonInstance.transform;
        LevelGen.instance.instantiatedObjects.Add(obj);
        return obj;
    }
    private static GameObject DeterminePrefab(Dictionary<string, GameObject> prefabs, Dictionary<string, float> prefabProbabilities, List<string> keyList, float roll) {
        float total = 0;
        foreach (var key in keyList) {
            total += prefabProbabilities[key];
            if (total > roll) return prefabs[key];
        }
        return prefabs[keyList[keyList.Count - 1]];
    }

    public static void InstantiateFloorBlock(string type, int x, int y, string block) {
        var castleParts = LevelGenPrefabs.prefabs[type];
        var castleProbabilities = LevelGenPrefabs.prefabProbability[type];
        var blockTypes = LevelGen.blockLookups[type];
        if (block == null) return;
        if (!blockTypes.ContainsKey(block)) return;
        var blockType = blockTypes[block];
        var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y, true);
        int rotationRoll = Random.Range(0, 4);
        obj.transform.Rotate(0, 90 * rotationRoll, 0);
    }

    public static void InstantiateWall(string type, int x, int y, int floor, string block, string[,,] grid) {
        var castleParts = LevelGenPrefabs.prefabs[type];
        var castleProbabilities = LevelGenPrefabs.prefabProbability[type];
        string blockType;
        var northCovered = false;
        var southCovered = false;
        var eastCovered = false;
        var westCovered = false;
        if (!LevelGenGridUtils.IsFloor(x, y, floor, grid)) return;
        if (block == "E") {
            blockType = "entrance";
            if (LevelGenGridUtils.IsFloor(x + 1, y, floor, grid) && !LevelGenGridUtils.IsFloor(x - 1, y, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x - 1, y);
                obj.transform.Rotate(0, 180, 0);
                westCovered = true;
                LevelGen.instance.entranceLocation = new Vector3((x + 1) * 5, 0, y * 5);
                LevelGen.instance.entranceAngle = 180;
            }
            else if (LevelGenGridUtils.IsFloor(x - 1, y, floor, grid) && !LevelGenGridUtils.IsFloor(x + 1, y, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x + 1, y);
                obj.transform.Rotate(0, 0, 0);
                eastCovered = true;
                LevelGen.instance.entranceLocation = new Vector3((x - 1) * 5, 0, y * 5);
                LevelGen.instance.entranceAngle = 0;
            }
            else if (LevelGenGridUtils.IsFloor(x, y + 1, floor, grid) && !LevelGenGridUtils.IsFloor(x, y - 1, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y - 1);
                obj.transform.Rotate(0, 90, 0);
                northCovered = true;
                LevelGen.instance.entranceLocation = new Vector3(x * 5, 0, (y + 1) * 5);
                LevelGen.instance.entranceAngle = 90;
            }
            else if (LevelGenGridUtils.IsFloor(x, y - 1, floor, grid) && !LevelGenGridUtils.IsFloor(x, y + 1, floor, grid)) {
            //else {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y + 1);
                obj.transform.Rotate(0, 270, 0);
                southCovered = true;
                LevelGen.instance.entranceLocation = new Vector3(x * 5, 0, (y - 1) * 5);
                LevelGen.instance.entranceAngle = 270;
            }
        }
        else if (block == ">") {
            blockType = "stairsToNext";
            if (LevelGenGridUtils.IsFloor(x + 1, y, floor, grid) && !LevelGenGridUtils.IsFloor(x - 1, y, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y);
                obj.transform.Rotate(0, 180, 0);
                westCovered = true;
                LevelGen.instance.exitLocation = new Vector3((x + 1) * 5, 0, y * 5);
                LevelGen.instance.exitAngle = 180;
            }
            else if (LevelGenGridUtils.IsFloor(x - 1, y, floor, grid) && !LevelGenGridUtils.IsFloor(x + 1, y, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y);
                obj.transform.Rotate(0, 0, 0);
                eastCovered = true;
                LevelGen.instance.exitLocation = new Vector3((x - 1) * 5, 0, y * 5);
                LevelGen.instance.exitAngle = 0;
            }
            else if (LevelGenGridUtils.IsFloor(x, y + 1, floor, grid) && !LevelGenGridUtils.IsFloor(x, y - 1, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y);
                obj.transform.Rotate(0, 90, 0);
                northCovered = true;
                LevelGen.instance.exitLocation = new Vector3(x * 5, 0, (y + 1) * 5);
                LevelGen.instance.exitAngle = 90;
            }
            else if (LevelGenGridUtils.IsFloor(x, y - 1, floor, grid) && !LevelGenGridUtils.IsFloor(x, y + 1, floor, grid)) {
            //else {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y);
                obj.transform.Rotate(0, 270, 0);
                LevelGen.instance.exitLocation = new Vector3(x * 5, 0, (y - 1) * 5);
                LevelGen.instance.exitAngle = 270;
                southCovered = true;
            }
        }
        else if (block == "<") {
            blockType = "stairsToLast";
            if (LevelGenGridUtils.IsFloor(x + 1, y, floor, grid) && !LevelGenGridUtils.IsFloor(x - 1, y, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x - 1.5f, y);
                obj.transform.Rotate(0, 0, 0);
                westCovered = true;
                LevelGen.instance.entranceLocation = new Vector3(x * 5, 0, y * 5);
                LevelGen.instance.entranceAngle = 180;
            }
            else if (LevelGenGridUtils.IsFloor(x - 1, y, floor, grid) && !LevelGenGridUtils.IsFloor(x + 1, y, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x + 1.5f, y);
                obj.transform.Rotate(0, 180, 0);
                eastCovered = true;
                LevelGen.instance.entranceLocation = new Vector3(x * 5, 0, y * 5);
                LevelGen.instance.entranceAngle = 0;
            }
            else if (LevelGenGridUtils.IsFloor(x, y + 1, floor, grid) && !LevelGenGridUtils.IsFloor(x, y - 1, floor, grid)) {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y - 1.5f);
                obj.transform.Rotate(0, 270, 0);
                northCovered = true;
                LevelGen.instance.entranceLocation = new Vector3(x * 5, 0, y * 5);
                LevelGen.instance.entranceAngle = 90;
            }
            else if (LevelGenGridUtils.IsFloor(x, y - 1, floor, grid) && !LevelGenGridUtils.IsFloor(x, y + 1, floor, grid)) {
            //else {
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y + 1.5f);
                obj.transform.Rotate(0, 90, 0);
                southCovered = true;
                LevelGen.instance.entranceLocation = new Vector3(x * 5, 0, y * 5);
                LevelGen.instance.entranceAngle = 270;
            }
        }
        if (LevelGenGridUtils.IsWallNorth(x, y, floor, grid) && LevelGenGridUtils.IsFloor(x, y - 2, floor, grid) && !northCovered && grid[floor, x, y - 1] != "w") {
            blockType = "oneTileWall";
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y - 1);
            obj.transform.Rotate(0, 90, 0);
            northCovered = true;
            grid[floor, x, y-1] = "w";
        }
        if (LevelGenGridUtils.IsWallSouth(x, y, floor, grid) && LevelGenGridUtils.IsFloor(x, y + 2, floor, grid) && !southCovered) {
            southCovered = true;
        }
        if (LevelGenGridUtils.IsWallWest(x, y, floor, grid) && LevelGenGridUtils.IsFloor(x - 2, y, floor, grid) && !westCovered && grid[floor, x - 1, y] != "w") {
            blockType = "oneTileWall";
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x - 1, y);
            obj.transform.Rotate(0, 0, 0);
            westCovered = true;
            grid[floor, x-1, y] = "w";
        }
        if (LevelGenGridUtils.IsWallEast(x, y, floor, grid) && LevelGenGridUtils.IsFloor(x + 2, y, floor, grid) && !eastCovered) {
            eastCovered = true;
        }
        if (LevelGenGridUtils.IsFullTileTarget(x + 1, y, floor, grid)) eastCovered = true;
        if (LevelGenGridUtils.IsFullTileTarget(x - 1, y, floor, grid)) westCovered = true;
        if (LevelGenGridUtils.IsFullTileTarget(x, y + 1, floor, grid)) southCovered = true;
        if (LevelGenGridUtils.IsFullTileTarget(x, y - 1, floor, grid)) northCovered = true;
        if (LevelGenGridUtils.IsReverseCornerTopLeft(x, y, floor, grid)) {
            blockType = "reverseCornerWall";
            var rotation = 0;
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x - 1.12f, y - 1.12f);
            obj.transform.Rotate(0, rotation, 0);
        }
        if (LevelGenGridUtils.IsReverseCornerTopRight(x, y, floor, grid)) {
            blockType = "reverseCornerWall";
            var rotation = 270;
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x + 1.12f, y - 1.12f);
            obj.transform.Rotate(0, rotation, 0);
        }
        if (LevelGenGridUtils.IsReverseCornerBottomLeft(x, y, floor, grid)) {
            blockType = "reverseCornerWall";
            var rotation = 90;
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x - 1.12f, y + 1.12f);
            obj.transform.Rotate(0, rotation, 0);
        }
        if (LevelGenGridUtils.IsReverseCornerBottomRight(x, y, floor, grid)) {
            blockType = "reverseCornerWall";
            var rotation = 180;
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x + 1.12f, y + 1.12f);
            obj.transform.Rotate(0, rotation, 0);
        }
        if (LevelGenGridUtils.IsCoveredByReverseCorner(x, y, floor, grid)) {
            if (LevelGenGridUtils.IsWestCoveredByReverseCorner(x, y, floor, grid)) westCovered = true;
            if (LevelGenGridUtils.IsEastCoveredByReverseCorner(x, y, floor, grid)) eastCovered = true;
            if (LevelGenGridUtils.IsNorthCoveredByReverseCorner(x, y, floor, grid)) northCovered = true;
            if (LevelGenGridUtils.IsSouthCoveredByReverseCorner(x, y, floor, grid)) southCovered = true;
        }

        if (LevelGenGridUtils.IsCorner(x, y, floor, grid, northCovered, eastCovered, southCovered, westCovered)) {
            var rotation = LevelGenGridUtils.GetCornerRotation(x, y, floor, grid);
            if (rotation == 180) {
                westCovered = true;
                northCovered = true;
            }
            else if (rotation == 270) {
                westCovered = true;
                southCovered = true;
            }
            else if (rotation == 90) {
                eastCovered = true;
                northCovered = true;
            }
            else if (rotation == 0) {
                eastCovered = true;
                southCovered = true;
            }
            blockType = "cornerWall";
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y);
            obj.transform.Rotate(0, rotation, 0);
        }
        if (LevelGenGridUtils.IsDoubleWallTarget(x, y, floor, grid)) {
            blockType = "doubleWall";
            if (LevelGenGridUtils.IsDoubleWallTargetNorth(x, y, floor, grid) && !northCovered) {
                northCovered = true;
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x + 0.5f, y);
                obj.transform.Rotate(0, 270, 0);
            }
            if (LevelGenGridUtils.IsDoubleWallTargetEast(x, y, floor, grid) && !eastCovered) {
                eastCovered = true;
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y + 0.5f);
                obj.transform.Rotate(0, 180, 0);
            }
            if (LevelGenGridUtils.IsDoubleWallTargetSouth(x, y, floor, grid) && !southCovered) {
                southCovered = true;
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x - 0.5f, y);
                obj.transform.Rotate(0, 90, 0);
            }
            if (LevelGenGridUtils.IsDoubleWallTargetWest(x, y, floor, grid) && !westCovered) {
                westCovered = true;
                var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y - 0.5f);
                obj.transform.Rotate(0, 0, 0);
            }
        }
        if (LevelGenGridUtils.IsDoubleWallSecondaryTarget(x, y, floor, grid)) {
            if (LevelGenGridUtils.IsDoubleWallSecondaryTargetNorth(x, y, floor, grid)) northCovered = true;
            if (LevelGenGridUtils.IsDoubleWallSecondaryTargetSouth(x, y, floor, grid)) southCovered = true;
            if (LevelGenGridUtils.IsDoubleWallSecondaryTargetEast(x, y, floor, grid)) eastCovered = true;
            if (LevelGenGridUtils.IsDoubleWallSecondaryTargetWest(x, y, floor, grid)) westCovered = true;
        }

        blockType = "wall";
        if (LevelGenGridUtils.IsWallNorth(x, y, floor, grid) && !northCovered) {
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y - 0.7f);
            obj.transform.Rotate(0, 90, 0);
        }
        if (LevelGenGridUtils.IsWallEast(x, y, floor, grid) && !eastCovered) {
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x + 0.7f, y);
            obj.transform.Rotate(0, 180, 0);
        }
        if (LevelGenGridUtils.IsWallSouth(x, y, floor, grid) && !southCovered) {
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x, y + 0.7f);
            obj.transform.Rotate(0, 270, 0);
        }
        if (LevelGenGridUtils.IsWallWest(x, y, floor, grid) && !westCovered) {
            var obj = InstantiateBlockObject(castleParts[blockType], castleProbabilities[blockType], x - 0.7f, y);
            obj.transform.Rotate(0, 0, 0);
        }
    }
}