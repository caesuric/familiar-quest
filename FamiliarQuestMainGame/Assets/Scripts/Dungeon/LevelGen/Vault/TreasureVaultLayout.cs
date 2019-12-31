using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TreasureVaultLayout : BuildingLayout {
    public TreasureVaultLayout(Vault treasureVault) {
        building = treasureVault;
    }

    public override void LayoutRooms() {
        building.numFloors = 1;
        var worked = SetupVault();
        while (!worked) worked = SetupVault();
    }

    public bool TryLayoutRooms() {
        building.numFloors = 1;
        var worked = SetupVault();
        return worked;
    }

    public bool SetupVault() {
        LevelGenGridUtils.FillGrid(building.grid, building.numFloors, 120, 120);
        var success = true;
        var rooms = new List<Room>();
        foreach (var room in building.rooms) rooms.Add(room);
        success = LevelGenRoomUtils.PackPreConnectedRooms(building, rooms);
        return success;
    }
}
