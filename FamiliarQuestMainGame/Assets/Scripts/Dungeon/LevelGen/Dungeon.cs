using System.Collections.Generic;
using UnityEngine;

public abstract class Dungeon {
    public List<Room> rooms = new List<Room>();
    public int numFloors = 0;
    public string[,,] grid = new string[5, 120, 120];
    public int maxSocialTier = 0;
    public int maxDimensions = 0;
    public BuildingLayout layout;

    public void ResettleRooms(List<SocialStructure> socialStructures) {
        var unassignedRooms = new List<Room>();
        var totalSize = 0f;
        foreach (var tempRoom in rooms) {
            unassignedRooms.Add(tempRoom);
            totalSize += tempRoom.size;
        }
        var room = unassignedRooms[Random.Range(0, unassignedRooms.Count)];
        var count = 0;
        foreach (var socialStructure in socialStructures) {
            foreach (var monster in socialStructure.population) {
                if (count >= room.size) {
                    unassignedRooms.Remove(room);
                    room = unassignedRooms[Random.Range(0, unassignedRooms.Count)];
                    count = 0;
                }
                monster.associatedRooms.Add(room);
                count++;
            }
        }
    }

    public string PrintGrid() {
        var output = "";
        for (int i = 0; i < numFloors; i++) output += PrintGridFloor(i);
        return output;
    }

    public string PrintGridFloor(int floor) {
        var output = "Floor " + (floor + 1).ToString() + "\n";
        int maxX = maxDimensions; //GetMaxUsedGridX(floor);
        int maxY = maxDimensions; //GetMaxUsedGridY(floor);
        for (int y = 0; y <= maxY; y++) {
            for (int x = 0; x <= maxX; x++) {
                var spot = grid[floor, x, y];
                if (!LevelGenGridUtils.IsFloor(x, y, floor, grid) && spot != "R" && spot != "*") spot = ".";
                output += spot + "  ";
            }
            output += "\n";
        }
        output += "-----------------------------\n";
        return output;
    }
}
