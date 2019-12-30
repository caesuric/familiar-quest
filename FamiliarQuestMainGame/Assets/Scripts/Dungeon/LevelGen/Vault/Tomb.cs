using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tomb : Vault {
    private static List<string> monsterTypes = new List<string> {
        "Dark Knight",
        "Warlock",
        "Corrupted Sorcerer",
        "Dark Bishop",
        "Slime",
        "Gelatinous Mass",
        "Ghoul",
        "Ghost",
        "Jittery Wisplet"
    };

    public Tomb() {
        rooms = CreateRooms();
        layout = new TreasureVaultLayout(this);
        layout.LayoutRooms();
    }

    public override void AddMonsters(int targetLevel) {
        var localMonsterTypes = new List<string>();
        int numMonsterTypes = Random.Range(3, 8);
        for (int i = 0; i < numMonsterTypes; i++) localMonsterTypes.Add(monsterTypes[Random.Range(0, monsterTypes.Count)]);
        foreach (var room in rooms) {
            if (room is Corridor) continue;
            int quantity = Random.Range(3, 8);
            for (int i = 0; i < quantity; i++) monsters.Add(GenerateMonster(targetLevel, room, localMonsterTypes[Random.Range(0, localMonsterTypes.Count)]));
        }
    }

    public MonsterData GenerateMonster(int targetLevel, Room room, string type) {
        int level, quality;
        int difficultyRoll = Random.Range(0, 100);
        int qualityRoll = Random.Range(0, 100);
        if (difficultyRoll < 12) level = Random.Range(1, LevelGen.targetLevel); //easy
        else if (difficultyRoll < 75) level = LevelGen.targetLevel; //normal
        else if (difficultyRoll < 97) level = Mathf.Min(LevelGen.targetLevel + (UnityEngine.Random.Range(1, 5)), LevelGen.targetLevel * 2); //hard
        else level = Mathf.Min(LevelGen.targetLevel + (UnityEngine.Random.Range(5, 7)), LevelGen.targetLevel * 3); //extreme
        if (qualityRoll < 75) quality = 0;
        else if (qualityRoll < 90) quality = 1;
        else if (qualityRoll < 97) quality = 2;
        else quality = 3;
        var monster = new MonsterData(type, type, level, quality, null);
        monster.associatedRooms.Add(room);
        return monster;
    }

    private List<Room> CreateRooms() {
        var output = new List<Room>();
        for (int roomNumber = 0; roomNumber < 36; roomNumber++) {
            output.Add(new VaultRoom() {
                size = Random.Range(4, 26)
            });
        }
        var bossRoom = new BossRoom() { size = 100 };
        output.Add(bossRoom);
        return output;
    }

}
