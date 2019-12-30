using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ActiveCastleLivingQuartersGen : MonoBehaviour {
    private static Dictionary<string, Dictionary<string, Dictionary<string, GameObject>>> prefabs = LevelGenPrefabs.prefabs;
    private static Dictionary<string, Dictionary<string, Dictionary<string, float>>> prefabProbability = LevelGenPrefabs.prefabProbability;

    public static void AddDressing(LivingQuarters room, string[,,] grid) {
        AddBeds(room);
        AddLivingQuarterSideFurnishing(room, grid);
    }

    private static void AddBeds(LivingQuarters room) {
        var population = room.inhabitants.Count;
        var size = Mathf.Sqrt(DesignedBuilding.monsterRoomSizes[room.inhabitants[0].generalType]);
        int bedType = GetBedType(room);
        if (bedType == 1) population /= 2;
        var beds = prefabs["castle"]["beds" + bedType];
        var bedProbs = prefabProbability["castle"]["beds" + bedType];
        int bedsX = (int)Mathf.Sqrt(population);
        if (bedsX == 0) bedsX = 1;
        int bedsY = population / bedsX;
        int bedRotation = Random.Range(0, 4);
        for (int x = 0; x < bedsX; x++) {
            for (int y = 0; y < bedsY; y++) {
                var xFudge = Random.Range(-0.05f, 0.05f);
                var yFudge = Random.Range(-0.05f, 0.05f);
                var obj = LevelGenInstantiationUtils.InstantiateBlockObject(beds, bedProbs, xFudge + room.x - 0.5f + ((float)(room.xSize) * (x + 1) / (bedsX + 1)), yFudge + room.y - 0.5f + ((float)(room.ySize) * (y + 1) / (bedsY + 1)));
                obj.transform.localScale = new Vector3(size, size, size);
                var rotationFudge = Random.Range(-2.5f, 2.5f);
                obj.transform.Rotate(0, (90 * bedRotation) + rotationFudge, 0);
                int nightstandRoll = Random.Range(0, 2);
                if (nightstandRoll == 0) AddNightstand(xFudge + room.x - 0.5f + ((float)(room.xSize) * (x + 1) / (bedsX + 1)), yFudge + room.y - 0.5f + ((float)(room.ySize) * (y + 1) / (bedsY + 1)), 90 * bedRotation, size);
            }
        }
    }

    private static void AddNightstand(float x, float y, float rotation, float size) {
        x *= 5;
        y *= 5;
        if (rotation == 90) {
            x -= 0.5f * size;
            y -= 1.5f * size;
        }
        else if (rotation == 270) {
            x += 0.5f * size;
            y += 1.5f * size;
        }
        else if (rotation == 180) {
            x += 1.5f * size;
            y += 0.5f * size;
        }
        else if (rotation == 0) {
            x -= 1.5f * size;
            y -= 0.5f * size;
        }
        float xFudge = Random.Range(-0.25f, 0.25f);
        float yFudge = Random.Range(-0.25f, 0.25f);
        var obj = Instantiate(LevelGen.instance.nightstand, new Vector3(x + xFudge, 0, y + yFudge), LevelGen.instance.nightstand.transform.rotation);
        obj.transform.parent = LevelGen.instance.dungeonInstance.transform;
        LevelGen.instance.instantiatedObjects.Add(obj);
        obj.transform.localScale = new Vector3(size, size, size);
        obj.transform.Rotate(0, Random.Range(0, 360), 0);
        int clutterRoll = Random.Range(0, 4);
        if (clutterRoll > 0) {
            var clutterX = (x + xFudge) / 5;
            var clutterY = (y + yFudge) / 5;
            clutterX += Random.Range(-0.025f, 0.025f) * size;
            clutterY += Random.Range(-0.025f, 0.025f) * size;
            var clutter = prefabs["castle"]["nightstandClutter"];
            var clutterProbs = prefabProbability["castle"]["nightstandClutter"];
            var obj2 = LevelGenInstantiationUtils.InstantiateBlockObject(clutter, clutterProbs, clutterX, clutterY);
            obj2.transform.position = new Vector3(obj2.transform.position.x, 0.61f * size, obj2.transform.position.z);
            obj2.transform.Rotate(0, Random.Range(0, 360), 0);
        }
    }

    public static int GetBedType(AssignedRoom room) {
        int bedType = (int)(room.grandiosity / 0.125);
        if (bedType < 3) bedType = 0;
        else bedType -= 2;
        if (bedType > 8) bedType = 8;
        if (room.inhabitants.Count > 1 && bedType >= 7) bedType = 6;
        bedType++;
        return bedType;
    }

    private static void AddLivingQuarterSideFurnishing(LivingQuarters room, string[,,] grid) {
        var population = room.inhabitants.Count;
        var size = Mathf.Sqrt(DesignedBuilding.monsterRoomSizes[room.inhabitants[0].generalType]);
        int bedType = GetBedType(room);
        if (bedType == 1) population /= 2;
        bool fancy = (bedType >= 6);
        Dictionary<string, GameObject> furniture;
        Dictionary<string, float> furnitureProbs;
        if (fancy) {
            furniture = prefabs["castle"]["livingQuarterSideFurnitureFancy"];
            furnitureProbs = prefabProbability["castle"]["livingQuarterSideFurnitureFancy"];
        }
        else {
            furniture = prefabs["castle"]["livingQuarterSideFurniture"];
            furnitureProbs = prefabProbability["castle"]["livingQuarterSideFurniture"];
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
