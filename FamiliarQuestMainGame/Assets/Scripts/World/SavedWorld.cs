using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedWorld {
    public List<SavedItem> inventory = new List<SavedItem>();
    public string name = "";
    public float[,] elevation = new float[1024, 1024];
    public SavedVector2 baseCoords = new SavedVector2();
    public List<SavedDungeon> dungeons = new List<SavedDungeon>();
    public bool savedOverworld = false;
    public static SavedWorld lastSavedWorld;

    public static SavedWorld BrandNewWorld(string name) {
        var obj = new SavedWorld {
            name = name
        };
        return obj;
    }

    public static SavedWorld ConvertFrom(GameObject go) {
        var obj = new SavedWorld();
        var worldAutoSaver = go.GetComponent<WorldAutoSaver>();
        foreach (var item in PlayerCharacter.localPlayer.inventory.items) obj.inventory.Add(SavedItem.ConvertFrom(item));
        obj.name = worldAutoSaver.worldName;
        if (OverworldGenerator.instance != null) {
            obj.savedOverworld = true;
            obj.elevation = OverworldGenerator.instance.elevation;
            foreach (var landmark in OverworldLandmarkGenerator.landmarks) {
                if (landmark.type == "base") obj.baseCoords = SavedVector2.ConvertFrom(landmark.position);
                else if (landmark.type == "dungeon") obj.dungeons.Add(SavedDungeon.ConvertFrom((OverworldDungeon)landmark));
            }
        }
        else if (lastSavedWorld != null) {
            obj.savedOverworld = lastSavedWorld.savedOverworld;
            obj.elevation = lastSavedWorld.elevation;
            obj.baseCoords = lastSavedWorld.baseCoords;
            obj.dungeons = lastSavedWorld.dungeons;
        }
        lastSavedWorld = obj;
        return obj;
    }

    public void ConvertTo(GameObject go) {
        var worldAutoSaver = go.GetComponent<WorldAutoSaver>();
        foreach (var item in inventory) if (item != null) PlayerCharacter.localPlayer.inventory.items.Add(item.ConvertTo());
        worldAutoSaver.worldName = name;
        if (savedOverworld) {
            OverworldGenerator.loadedPreviouslyMadeWorld = true;
            OverworldGenerator.loadedElevation = elevation;
            OverworldGenerator.loadedBaseCoords = baseCoords.ConvertTo();
            OverworldGenerator.loadedDungeons = new List<OverworldDungeon>();
            foreach (var dungeon in dungeons) OverworldGenerator.loadedDungeons.Add(dungeon.ConvertTo());
        }
    }

    public static void OverwriteDungeonData(OverworldDungeon dungeonData) {
        if (lastSavedWorld == null) return;
        for (int i = 0; i < lastSavedWorld.dungeons.Count; i++) {
            var dungeon = lastSavedWorld.dungeons[i];
            if (dungeon.uuid == dungeonData.uuid) {
                Debug.Log("hit1");
                lastSavedWorld.dungeons[i] = SavedDungeon.ConvertFrom(dungeonData);
                break;
            }
        }
    }
}

[System.Serializable]
public class SavedDungeon {
    public bool entered = false;
    public SavedVector2 location = null;
    public List<SavedRoom> rooms = new List<SavedRoom>();
    public int numFloors = 0;
    public string[,,] grid = new string[5, 120, 120];
    public int maxSocialTier = 0;
    public int maxDimensions = 0;
    public List<SavedMonsterData> monsters = new List<SavedMonsterData>();
    public List<SavedVaultPath> paths = new List<SavedVaultPath>();
    public List<string> encounterThemes = new List<string>();
    public List<int> seeds = new List<int>();
    public string uuid = null;

    public static SavedDungeon ConvertFrom(OverworldDungeon dungeon) {
        if (dungeon.dungeonData == null) return new SavedDungeon() {
            location = SavedVector2.ConvertFrom(dungeon.position),
            uuid = dungeon.uuid
        };
        var output = new SavedDungeon {
            entered = true,
            location = SavedVector2.ConvertFrom(dungeon.position),
            numFloors = dungeon.dungeonData.numFloors,
            grid = dungeon.dungeonData.grid,
            maxSocialTier = dungeon.dungeonData.maxSocialTier,
            maxDimensions = dungeon.dungeonData.maxDimensions,
            encounterThemes = dungeon.dungeonData.encounterThemes,
            seeds = dungeon.seeds,
            uuid = dungeon.uuid
        };
        foreach (var room in dungeon.dungeonData.rooms) output.rooms.Add(SavedRoom.ConvertFrom(room));
        foreach (var monster in dungeon.dungeonData.monsters) output.monsters.Add(SavedMonsterData.ConvertFrom(monster));
        foreach (var path in dungeon.dungeonData.paths) output.paths.Add(SavedVaultPath.ConvertFrom(path));
        return output;
    }

    public OverworldDungeon ConvertTo() {
        var output = new OverworldDungeon {
            dungeonData = new TreasureVault() {
                numFloors = numFloors,
                grid = grid,
                maxSocialTier = maxSocialTier,
                maxDimensions = maxDimensions,
                encounterThemes = encounterThemes
            },
            position = location.ConvertTo(),
            type = "dungeon",
            entered = entered,
            seeds = seeds,
            uuid = uuid
        };
        foreach (var room in rooms) output.dungeonData.rooms.Add(room.ConvertTo());
        foreach (var monster in monsters) output.dungeonData.monsters.Add(monster.ConvertTo(output.dungeonData.rooms, output.dungeonData.monsters));
        foreach (var path in paths) output.dungeonData.paths.Add(path.ConvertTo(output.dungeonData.rooms));
        return output;
    }
}

[System.Serializable]
public class SavedVector2 {
    public float x;
    public float y;

    public static SavedVector2 ConvertFrom(Vector2 vector2) {
        return new SavedVector2() {
            x = vector2.x,
            y = vector2.y
        };
    }

    public Vector2 ConvertTo() {
        return new Vector2(x, y);
    }
}

[System.Serializable]
public class SavedVector3 {
    public float x;
    public float y;
    public float z;

    public static SavedVector3 ConvertFrom(Vector3 vector3) {
        return new SavedVector3() {
            x = vector3.x,
            y = vector3.y,
            z = vector3.z
        };
    }

    public Vector3 ConvertTo() {
        return new Vector3(x, y, z);
    }
}

[System.Serializable]
public class SavedRoom {
    public string uuid;
    public float size;
    public float grandiosity;
    public int x;
    public int y;
    public int floor;
    public int xSize;
    public int ySize;
    public List<SavedVector3> entrances = new List<SavedVector3>();
    public List<SavedVector2> dressingLocations = new List<SavedVector2>();

    public static SavedRoom ConvertFrom(Room room) {
        var output = new SavedRoom {
            uuid = room.uuid,
            size = room.size,
            grandiosity = room.grandiosity,
            x = room.x,
            y = room.y,
            floor = room.floor,
            xSize = room.xSize,
            ySize = room.ySize
        };
        foreach (var entrance in room.entrances) output.entrances.Add(SavedVector3.ConvertFrom(entrance));
        foreach (var dressingLocation in room.dressingLocations) output.dressingLocations.Add(SavedVector2.ConvertFrom(dressingLocation));
        return output;
    }

    public Room ConvertTo() {
        var output = new VaultRoom {
            uuid = uuid,
            size = size,
            grandiosity = grandiosity,
            x = x,
            y = y,
            floor = floor,
            xSize = xSize,
            ySize = ySize
        };
        foreach (var entrance in entrances) output.entrances.Add(entrance.ConvertTo());
        foreach (var dressingLocation in dressingLocations) output.dressingLocations.Add(dressingLocation.ConvertTo());
        return output;
    }
}

[System.Serializable]
public class SavedMonsterData {
    public string generalType;
    public string specificType;
    public int level;
    public int quality;
    public SavedSocialNode node;
    public List<string> associatedRoomIds = new List<string>();

    public static SavedMonsterData ConvertFrom(MonsterData monsterData) {
        var output = new SavedMonsterData() {
            generalType = monsterData.generalType,
            specificType = monsterData.specificType,
            level = monsterData.level,
            quality = monsterData.quality,
            node = SavedSocialNode.ConvertFrom(monsterData.node),
        };
        foreach (var room in monsterData.associatedRooms) output.associatedRoomIds.Add(room.uuid);
        return output;
    }

    public MonsterData ConvertTo(List<Room> rooms, List<MonsterData> monsterData) {
        MonsterData output;
        if (node != null) output = new MonsterData(generalType, specificType, level, quality, node.ConvertTo(monsterData));
        else output = new MonsterData(generalType, specificType, level, quality, null);
        foreach (var id in associatedRoomIds) {
            foreach (var room in rooms) {
                if (room.uuid==id) {
                    output.associatedRooms.Add(room);
                    break;
                }
            }
        }
        return output;
    }
}

[System.Serializable]
public class SavedVaultPath {
    public List<string> roomIds = new List<string>();

    public static SavedVaultPath ConvertFrom(VaultPath path) {
        var output = new SavedVaultPath();
        foreach (var room in path.rooms) output.roomIds.Add(room.uuid);
        return output;
    }

    public VaultPath ConvertTo(List<Room> rooms) {
        var output = new VaultPath();
        foreach (var id in roomIds) {
            foreach (var room in rooms) {
                if (room.uuid==id) {
                    output.rooms.Add(room);
                    break;
                }
            }
        }
        return output;
    }
}

[System.Serializable]
public class SavedSocialNode {
    public string uuid;
    public List<string> vassalIds = new List<string>();
    public string lordId = null;
    public List<string> monsterTypes = new List<string>();
    public List<float> monsterTypePercentages = new List<float>();
    public int actualPopulation = 0;
    public int id;

    public static SavedSocialNode ConvertFrom(SocialNode node) {
        if (node == null) return null;
        var output = new SavedSocialNode {
            uuid = node.uuid,
            actualPopulation = node.actualPopulation,
            id = node.id,
            lordId = node.lord.uuid
        };
        foreach (var vassal in node.vassals) output.vassalIds.Add(vassal.uuid);
        foreach (var kvp in node.typeMix) {
            output.monsterTypes.Add(kvp.Key);
            output.monsterTypePercentages.Add(kvp.Value);
        }
        return output;
    }

    public SocialNode ConvertTo(List<MonsterData> monsterData) {
        var output = new SocialNode(new List<string> { "goblin" }, false, null) {
            uuid = uuid,
            actualPopulation = actualPopulation,
            id = id
        };
        for (int i=0; i<monsterTypes.Count; i++) output.typeMix.Add(monsterTypes[i], monsterTypePercentages[i]);
        foreach (var monster in monsterData) {
            if (monster.node.uuid==lordId) {
                output.lord = monster.node;
                monster.node.vassals.Add(output);
                break;
            }
        }
        return output;
    }
}