using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VaultPath {
    public List<Room> rooms = new List<Room>();

    public VaultPath() {
        GenerateRooms();
        while (!RoomsValid()) GenerateRooms();
    }

    private void GenerateRooms() {
        rooms.Clear();
        int roomCount = RNG.Int(1, 15);
        for (int i=0; i<roomCount; i++) {
            var vr = new VaultRoom();
            vr.floor = 0;
            int encounterRoll = RNG.Int(0, 4);
            if (encounterRoll>0) {
                vr.hasEncounter = true;
            }
            if (i == roomCount - 1) vr.hasTreasure = true;
            rooms.Add(vr);
        }
    }

    private bool RoomsValid() {
        int encounterCount = 0;
        foreach (var room in rooms) {
            if (room is VaultRoom) {
                var vr = (VaultRoom)room;
                if (vr.hasEncounter) encounterCount++;
            }
        }
        if (encounterCount==0 || encounterCount > 7) return false;
        return true;
    }
}