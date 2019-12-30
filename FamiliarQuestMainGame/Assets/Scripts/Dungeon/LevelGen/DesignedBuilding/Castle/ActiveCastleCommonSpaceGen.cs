using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ActiveCastleCommonSpaceGen : MonoBehaviour {
    private static Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> prefabs = LevelGenPrefabs.prefabs;
    private static Dictionary<string, Dictionary<string, Dictionary<string, float>>> prefabProbability = LevelGenPrefabs.prefabProbability;

    public static void AddDressing(CommonSpace room, string[,,] grid) {
        AddCommonSpaceTables(room);
        AddCommonSpaceSideFurnishing(room, grid);
    }

    private static void AddCommonSpaceTables(CommonSpace room) {
        var population = room.inhabitants.Count;
        var size = Mathf.Sqrt(DesignedBuilding.monsterRoomSizes[room.inhabitants[0].generalType]);
        int bedType = ActiveCastleLivingQuartersGen.GetBedType(room);
        if (bedType == 1) population /= 2;
        population /= 2;
        bool fancy = (bedType >= 6);
        Dictionary<string, GameObject> tables;
        Dictionary<string, float> tableProbs;
        if (!fancy) {
            tables = prefabs["castle"]["tables"];
            tableProbs = prefabProbability["castle"]["tables"];
        }
        else {
            tables = prefabs["castle"]["tablesFancy"];
            tableProbs = prefabProbability["castle"]["tablesFancy"];
        }
        int tablesX = (int)Mathf.Sqrt(population);
        if (tablesX == 0) tablesX = 1;
        int tablesY = population / tablesX;
        int tableRotation = Random.Range(0, 4);
        float tableRoll = Random.Range(0f, 1f);
        for (int x = 0; x < tablesX; x++) {
            for (int y = 0; y < tablesY; y++) {
                var xFudge = Random.Range(-0.05f, 0.05f);
                var yFudge = Random.Range(-0.05f, 0.05f);
                var obj = LevelGenInstantiationUtils.InstantiateBlockObjectFixed(tableRoll, tables, tableProbs, xFudge + room.x - 0.5f + ((float)(room.xSize) * (x + 1) / (tablesX + 1)), yFudge + room.y - 0.5f + ((float)(room.ySize) * (y + 1) / (tablesY + 1)));
                obj.transform.localScale = new Vector3(size, size, size);
                var rotationFudge = Random.Range(-5f, 5f);
                obj.transform.Rotate(0, (90 * tableRotation) + rotationFudge, 0);
                AddChairsToTable(xFudge + room.x - 0.5f + ((float)(room.xSize) * (x + 1) / (tablesX + 1)), yFudge + room.y - 0.5f + ((float)(room.ySize) * (y + 1) / (tablesY + 1)), size, fancy, obj, 90 * tableRotation);
                int clutterRoll = Random.Range(0, 4);
                if (clutterRoll > 1) {
                    var clutterX = xFudge + room.x - 0.5f + ((float)(room.xSize) * (x + 1) / (tablesX + 1));
                    var clutterY = yFudge + room.y - 0.5f + ((float)(room.ySize) * (y + 1) / (tablesY + 1));
                    clutterX += Random.Range(-0.05f, 0.05f) * size;
                    clutterY += Random.Range(-0.05f, 0.05f) * size;
                    var clutter = prefabs["castle"]["tableClutter"];
                    var clutterProbs = prefabProbability["castle"]["tableClutter"];
                    var obj2 = LevelGenInstantiationUtils.InstantiateBlockObject(clutter, clutterProbs, clutterX, clutterY);
                    obj2.transform.position = new Vector3(obj2.transform.position.x, obj.GetComponentInChildren<Renderer>().bounds.size.y, obj2.transform.position.z);
                    obj2.transform.Rotate(0, Random.Range(0, 360), 0);
                }
            }
        }
    }

    private static void AddChairsToTable(float x, float y, float size, bool fancy, GameObject tableObj, float tableRotation) {
        Dictionary<string, GameObject> chairs;
        Dictionary<string, float> chairProbs;
        if (!fancy) {
            chairs = prefabs["castle"]["chairs"];
            chairProbs = prefabProbability["castle"]["chairs"];
        }
        else {
            chairs = prefabs["castle"]["chairsFancy"];
            chairProbs = prefabProbability["castle"]["chairsFancy"];
        }
        float chairRoll = Random.Range(0f, 1f);
        var tableSize = tableObj.GetComponentInChildren<Renderer>().bounds.size;
        float spacingMultiplier = 0.15f;
        float spacing = Mathf.Min(tableSize.x, tableSize.z) * spacingMultiplier;
        if ((tableRotation == 0 || tableRotation == 180) && (tableSize.x > tableSize.z * 1.5f || tableSize.z > tableSize.x * 1.5f)) {
            AddIndividualChair(x + tableSize.x / 20f, y - spacing, size, chairs, chairProbs, chairRoll, 0);
            AddIndividualChair(x - tableSize.x / 20f, y + spacing, size, chairs, chairProbs, chairRoll, 180);
            AddIndividualChair(x + tableSize.x / 20f, y + spacing, size, chairs, chairProbs, chairRoll, 180);
            AddIndividualChair(x - tableSize.x / 20f, y - spacing, size, chairs, chairProbs, chairRoll, 0);
        }
        else if (tableSize.x > tableSize.z * 1.5f || tableSize.z > tableSize.x * 1.5f) {
            AddIndividualChair(x - spacing, y + tableSize.z / 20f, size, chairs, chairProbs, chairRoll, 90);
            AddIndividualChair(x + spacing, y - tableSize.z / 20f, size, chairs, chairProbs, chairRoll, 270);
            AddIndividualChair(x + spacing, y + tableSize.z / 20f, size, chairs, chairProbs, chairRoll, 270);
            AddIndividualChair(x - spacing, y - tableSize.z / 20f, size, chairs, chairProbs, chairRoll, 90);
        }
        else {
            spacing = Mathf.Max(tableSize.z, tableSize.x) * spacingMultiplier;
            AddIndividualChair(x - spacing, y, size, chairs, chairProbs, chairRoll, 90);
            AddIndividualChair(x + spacing, y, size, chairs, chairProbs, chairRoll, 270);
            AddIndividualChair(x, y - spacing, size, chairs, chairProbs, chairRoll, 0);
            AddIndividualChair(x, y + spacing, size, chairs, chairProbs, chairRoll, 180);
        }
    }

    private static void AddIndividualChair(float x, float y, float size, Dictionary<string, GameObject> chairs, Dictionary<string, float> chairProbs, float roll, float angle) {
        var xFudge = Random.Range(-0.05f, 0.05f);
        var yFudge = Random.Range(-0.05f, 0.05f);
        int rightChairRoll = Random.Range(0, 100);
        int facingRoll = Random.Range(0, 100);
        var rotationFudge = Random.Range(-10f, 10f);
        if (facingRoll >= 95) rotationFudge = Random.Range(-180f, 180f);
        else if (facingRoll >= 90) rotationFudge += 180;
        GameObject obj;
        if (rightChairRoll > 4) obj = LevelGenInstantiationUtils.InstantiateBlockObjectFixed(roll, chairs, chairProbs, x + xFudge, y + yFudge);
        else obj = LevelGenInstantiationUtils.InstantiateBlockObject(chairs, chairProbs, x + xFudge, y + yFudge);
        obj.transform.localScale = new Vector3(size, size, size);
        obj.transform.Rotate(0, angle + rotationFudge, 0);
    }

    private static void AddCommonSpaceSideFurnishing(CommonSpace room, string[,,] grid) {
        var population = room.inhabitants.Count;
        var size = Mathf.Sqrt(DesignedBuilding.monsterRoomSizes[room.inhabitants[0].generalType]);
        int bedType = ActiveCastleLivingQuartersGen.GetBedType(room);
        if (bedType == 1) population /= 2;
        bool fancy = (bedType >= 6);
        population /= 2;
        Dictionary<string, GameObject> furniture;
        Dictionary<string, float> furnitureProbs;
        if (fancy) {
            furniture = prefabs["castle"]["commonSpaceSideFurnitureFancy"];
            furnitureProbs = prefabProbability["castle"]["commonSpaceSideFurnitureFancy"];
        }
        else {
            furniture = prefabs["castle"]["commonSpaceSideFurniture"];
            furnitureProbs = prefabProbability["castle"]["commonSpaceSideFurniture"];
        }
        var potentialSpots = LevelGenRoomUtils.GetPotentialSideDressingSpots(room, grid);
        foreach (var spot in potentialSpots) {
            var roll = Random.Range(0, 100);
            if (roll < 100 * population / (float)potentialSpots.Count) {
                var xFudge = Random.Range(-0.05f, 0.05f);
                var yFudge = Random.Range(-0.05f, 0.05f);
                var rotationFudge = Random.Range(-2.5f, 2.5f);
                var wallRotation = LevelGenGridUtils.GetWallRotation((int)spot.x, (int)spot.y, room.floor, grid);
                var xAdjust = LevelGenGridUtils.GetXSideDressingAdjustment((int)spot.x, (int)spot.y, room.floor, grid);
                var yAdjust = LevelGenGridUtils.GetYSideDressingAdjustment((int)spot.x, (int)spot.y, room.floor, grid);
                var obj = LevelGenInstantiationUtils.InstantiateBlockObject(furniture, furnitureProbs, xAdjust + xFudge + spot.x, yAdjust + yFudge + spot.y);
                room.dressingLocations.Add(spot);
                obj.transform.localScale = new Vector3(size, size, size);
                obj.transform.Rotate(0, rotationFudge + wallRotation, 0);
            }
        }
    }
}
