using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class TreasureVault : Vault {

    private static List<string> possibleEncounterThemes = new List<string> {
        "slowEnemies",
        "largePacks",
        "differingEnemySpeeds",
        "projectiles",
        "knockback",
        "pullAndChargeEffects",
        "elementalResistances",
        "enemiesWithDangerousMeleeAttacks",
        "healingOrRegeneration",
        "suddenSpawns"
    };
    private static List<string> allMonsterTypes = new List<string> {
        "Animated Statue",
        "Ankheg",
        "Bomber",
        "Bound Archon",
        "Cave Cheetah",
        "Corrupted Sorcerer",
        "Cyclops",
        "Dark Bishop",
        "Dark Knight",
        "Dragon",
        "Elder Dragon",
        "Energy Wisplet",
        "Fire Elemental",
        "Gelatinous Mass",
        "Ghost",
        "Ghoul",
        "GOBLIN",
        "Goblin Archer",
        "Goblin Rogue",
        "Golem",
        "Gremlin",
        "Ice Elemental",
        "Imp",
        "Jittery Wisplet",
        "Kobold",
        "Land Squid",
        "Mimic",
        "Minotaur",
        "Mirror Mage",
        "Ogre",
        "Phantom Fungus",
        "Slime",
        "SPIDER",
        "Troll",
        "Warlock",
        "Wolf",
        "Young Dragon"
    };
    private static List<float> nishikadoBaseDifficultyLookup = new List<float>();
    private static List<float> nishikadoEncounterDifficultyLookup = new List<float>();
    private static List<GameObject> monsterPrefabs = new List<GameObject>();
    List<VaultPath> paths = new List<VaultPath>();
    List<string> encounterThemes = new List<string>();

    static TreasureVault() {
        float baseDifficulty = 3f;
        for (int i=1; i<=50; i++) {
            nishikadoBaseDifficultyLookup.Add(baseDifficulty);
            baseDifficulty *= 1.072f;
        }
        float baseMultiplier = 1f;
        for (int i=0; i<7; i++) {
            nishikadoEncounterDifficultyLookup.Add(baseMultiplier);
            baseMultiplier *= 1.31f;
        }
        var monsterList = Resources.LoadAll("Prefabs/Monsters");
        foreach (var monster in monsterList) monsterPrefabs.Add((GameObject)monster);
    }

    public TreasureVault() {
        numFloors = 1;
        maxDimensions = 119;
    }

    public void Initialize() {
        var completed = false;
        while (!completed) {
            rooms = CreateRooms();
            layout = new TreasureVaultLayout(this);
            completed = ((TreasureVaultLayout)layout).TryLayoutRooms();
        }
    }

    public override void AddMonsters(int targetLevel) {
        GenerateEncounterThemes();
        foreach (var path in paths) {
            CreateEncounterPath(path, targetLevel);
        }
    }

    private void GenerateEncounterThemes() {
        int roll = RNG.Int(0, 5);
        if (roll > 1) roll = 1;
        roll++;
        for (int i=0; i<roll; i++) {
            int roll2 = RNG.Int(0, possibleEncounterThemes.Count);
            if (!encounterThemes.Contains(possibleEncounterThemes[roll2])) encounterThemes.Add(possibleEncounterThemes[roll2]);
        }
    }

    private void CreateEncounterPath(VaultPath path, int targetLevel) {
        var difficulties = new List<float>();
        var conceptsUsed = new List<List<bool>>();
        var conceptsIntroduced = new List<bool>();
        var numConcepts = encounterThemes.Count;
        var numPathEncounters = GetPathEncounterNumber(path);
        float scaleFactor = 1;

        // determine difficulty of each encounter
        for (int i=1; i<targetLevel; i++) scaleFactor *= 1.44f;
        for (int i = 0; i < numPathEncounters; i++) {
            var currentDifficulty = nishikadoBaseDifficultyLookup[targetLevel-1] * nishikadoEncounterDifficultyLookup[i] * scaleFactor;
            var fudgeFactor = RNG.Float(0.75f, 1.25f);
            difficulties.Add(currentDifficulty * fudgeFactor);
        }

        // determine which concepts to use in each encounter
        for (int i = 0; i < numConcepts; i++) conceptsIntroduced.Add(false);
        conceptsIntroduced[0] = true;
        var conceptList = new List<bool> { true };
        for (int i = 1; i < numConcepts; i++) {
            conceptList.Add(false);
        }
        conceptsUsed.Add(conceptList);
        for (int i = 1; i < numPathEncounters; i++) {
            conceptList = new List<bool>();
            float chanceOfNew = i / numPathEncounters;
            float roll = RNG.Float(0, 1);
            if (roll <= chanceOfNew && HasUnintroducedConcepts(conceptsIntroduced)) {
                //introduce new concept
                var conceptIndex = conceptsIntroduced.IndexOf(false);
                conceptsIntroduced[conceptIndex] = true;
                for (int j = 0; j < numConcepts; j++) {
                    if (j == conceptIndex) conceptList.Add(true);
                    else conceptList.Add(false);
                }
                conceptsUsed.Add(conceptList);
            }
            else {
                //use existing concepts
                conceptList = new List<bool>();
                foreach (var concept in conceptsIntroduced) {
                    if (!concept) conceptList.Add(false);
                    else {
                        var roll2 = RNG.Int(0, 3);
                        if (roll2 == 0) conceptList.Add(false);
                        else conceptList.Add(true);
                    }
                }
                ValidateConceptSet(conceptList);
                conceptsUsed.Add(conceptList);
            }
        }

        // create monsters for each encounter
        int encounterNumber = 0;
        foreach (var room in path.rooms) {
            if (room is VaultRoom) {
                var vaultRoom = (VaultRoom)room;
                if (vaultRoom.hasEncounter) {
                    AddMonstersForRoom(room, difficulties[encounterNumber], conceptsUsed[encounterNumber]);
                    encounterNumber++;
                }
            }
            else if (room is BossRoom) {
                AddBoss(room, difficulties[encounterNumber], conceptsUsed[encounterNumber]);
                encounterNumber++;
            }
        }
    }

    private void ValidateConceptSet(List<bool> concepts) {
        foreach (var concept in concepts) if (concept) return;
        int roll = RNG.Int(0, concepts.Count);
        concepts[roll] = true;
    }

    private void AddMonstersForRoom(Room room, float difficulty, List<bool> conceptsUsed) {
        var conceptWeights = new List<float>();
        int numConceptsUsed = 0;
        foreach (var concept in conceptsUsed) {
            if (!concept) conceptWeights.Add(0);
            else {
                float baseValue = 1f / encounterThemes.Count;
                float fudgeFactor = RNG.Float(0.5f, 1.5f);
                conceptWeights.Add(baseValue * fudgeFactor);
                numConceptsUsed++;
            }
        }
        NormalizeConceptWeights(conceptWeights);
        float multipliedWeight = 1f;
        foreach (var weight in conceptWeights) if (weight > 0) multipliedWeight *= weight;
        var difficultyFactor = Mathf.Pow(difficulty / multipliedWeight, 1f / numConceptsUsed);
        var conceptDifficulties = new List<float>();
        foreach (var weight in conceptWeights) conceptDifficulties.Add(weight * difficultyFactor);
        //Debug.Log("-----");
        //Debug.Log("intended difficulty is " + difficulty.ToString());
        //Debug.Log("difficulty multiplier for weights is " + difficultyFactor.ToString());
        for (int i = 0; i < conceptDifficulties.Count; i++) if (conceptDifficulties[i] > 0) AddMonstersForConcept(room, conceptDifficulties[i], encounterThemes[i]);
    }

    private void AddMonstersForConcept(Room room, float difficulty, string concept) {
        //Debug.Log("Adding difficulty " + difficulty.ToString() + " monsters for concept " + concept);
        float accumulatedDifficulty = 0;
        int count = 0;
        while (accumulatedDifficulty < difficulty && count < 30) {
            count++;
            accumulatedDifficulty += AddMonsterUpToDifficulty(room, difficulty - accumulatedDifficulty, concept);
        }
    }

    private float AddMonsterUpToDifficulty(Room room, float difficultyLimit, string concept) {
        difficultyLimit = RNG.Float(difficultyLimit / 30f, difficultyLimit * 1.2f);
        var validMonsterTypes = new List<string>();
        var secondaryMonsterTypes = new List<string>();
        switch (concept) {
            case "slowEnemies":
            default:
                validMonsterTypes = new List<string> {
                    "Gelatinous Mass",
                    "Ghost"
                };
                secondaryMonsterTypes = new List<string> {
                    "Animated Statue",
                    "Ankheg",
                    "Bound Archon",
                    "Corrupted Sorcerer",
                    "Cyclops",
                    "Dark Bishop",
                    "Dark Knight",
                    "Dragon",
                    "Elder Dragon",
                    "Energy Wisplet",
                    "Fire Elemental",
                    "Ghoul",
                    "GOBLIN",
                    "Goblin Archer",
                    "Goblin Rogue",
                    "Golem",
                    "Ice Elemental",
                    "Imp",
                    "Jittery Wisplet",
                    "Kobold",
                    "Land Squid",
                    "Mimic",
                    "Minotaur",
                    "Mirror Mage",
                    "Ogre",
                    "Phantom Fungus",
                    "Slime",
                    "SPIDER",
                    "Troll",
                    "Warlock",
                    "Young Dragon"
                };
                break;
            case "largePacks":
                difficultyLimit /= 4;
                break;
            case "differingEnemySpeeds":
                validMonsterTypes = new List<string> {
                    "Bomber",
                    "Gelatinous Mass",
                    "Ghost",
                    "Gremlin",
                    "Cave Cheetah",
                    "Wolf"
                };
                break;
            case "projectiles":
                validMonsterTypes = new List<string> {
                    "Corrupted Sorcerer",
                    "Dark Bishop",
                    "Gelatinous Mass",
                    "Goblin Archer",
                    "Imp",
                    "Jittery Wisplet",
                    "Mirror Mage",
                    "Warlock"
                };
                secondaryMonsterTypes = new List<string> {
                    "Ankheg",
                    "Dark Knight",
                    "Dragon",
                    "Elder Dragon",
                    "Fire Elemental",
                    "Ice Elemental",
                    "Land Squid",
                    "Young Dragon"
                };
                break;
            case "knockback":
                validMonsterTypes = new List<string> {
                    "Cyclops"
                };
                secondaryMonsterTypes = new List<string> {
                    "Minotaur",
                    "Dark Knight"
                };
                break;
            case "pullAndChargeEffects":
                validMonsterTypes = new List<string> {
                    "Minotaur",
                    "Dark Knight"
                };
                secondaryMonsterTypes = new List<string> {
                    "Cyclops"
                };
                break;
            case "elementalResistances":
                validMonsterTypes = new List<string> {
                    "Animated Statue",
                    "Fire Elemental",
                    "Ice Elemental",
                    "Imp",
                    "Gremlin",
                    "Land Squid",
                    "Jittery Wisplet",
                    "Young Dragon",
                    "Dragon",
                    "Elder Dragon",
                    "Dark Knight",
                    "Golem",
                    "Ghoul",
                    "Phantom Fungus",
                    "Mimic",
                    "Ghost",
                    "Corrupted Sorcerer",
                    "Slime",
                    "Gelatinous Mass",
                    "Bound Archon"
                };
                break;
            case "enemiesWithDangerousMeleeAttacks":
                validMonsterTypes = new List<string> {
                    "Bomber",
                    "Cyclops",
                    "Dark Knight",
                    "Ghoul",
                    "Minotaur",
                    "Troll"
                };
                secondaryMonsterTypes = new List<string> {
                    "Ogre",
                    "SPIDER"
                };
                break;
            case "healingOrRegeneration":
                validMonsterTypes = new List<string> {
                    "Bound Archon",
                    "Dark Bishop",
                    "Troll"
                };
                break;
            case "suddenSpawns":
                validMonsterTypes = new List<string> {
                    "Ghost",
                    "Goblin Rogue",
                    "Imp",
                    "Mirror Mage",
                    "Phantom Fungus",
                    "Slime"
                };
                break;
        }
        var monsterType = RetrieveMonsterType(validMonsterTypes, secondaryMonsterTypes, allMonsterTypes, difficultyLimit);
        int qualityRoll = RNG.Int(0, 100);
        int quality = 0;
        if (qualityRoll < 50) quality = 0;
        else if (qualityRoll < 80) quality = 1;
        else if (qualityRoll < 95) quality = 2;
        else quality = 3;
        var monster = new MonsterData(monsterType, monsterType, ConvertDifficultyToLevel(difficultyLimit), quality, null);
        monster.associatedRooms.Add(room);
        monsters.Add(monster);
        //Debug.Log("adding " + monsterType);
        return GetMonsterDifficulty(monster);
    }

    private float GetMonsterDifficulty(MonsterData monster) {
        var baseDifficulty = ConvertLevelToDifficulty(monster.level);
        var qualityMod = ConvertQualityToDifficultyMod(monster.level, monster.quality);
        return baseDifficulty * qualityMod;
    }

    private float ConvertLevelToDifficulty(int level) {
        float output = 1;
        for (int i = 0; i < level; i++) output *= 1.44f;
        return output;
    }

    private float ConvertQualityToDifficultyMod(int level, int quality) {
        float baseScaleFactor = 0.5f + (0.01f * level);
        float scaleFactor = 0.5f + (0.01f * level);
        if (quality == 1) scaleFactor = 1 - ((1 - baseScaleFactor) / 2);
        else if (quality == 2) scaleFactor = 1;
        else if (quality == 3) scaleFactor *= 1.59f;
        return scaleFactor / baseScaleFactor;
    }

    private string RetrieveMonsterType(List<string> validMonsterTypes, List<string> secondaryMonsterTypes, List<string> allMonsterTypes, float difficultyLimit) {
        if (ContainsValidMonster(validMonsterTypes, difficultyLimit)) return GrabValidMonster(validMonsterTypes, difficultyLimit);
        else if (ContainsValidMonster(secondaryMonsterTypes, difficultyLimit)) return GrabValidMonster(secondaryMonsterTypes, difficultyLimit);
        else return GrabValidMonster(allMonsterTypes, difficultyLimit);
    }

    private bool ContainsValidMonster(List<string> monsters, float difficultyLimit) {
        foreach (var monster in monsters) {
            var monsterObj = RetrieveMonster(monster);
            if (MonsterFitsCriteria(monsterObj, difficultyLimit)) return true;
        }
        return false;
    }

    private bool MonsterFitsCriteria(Monster monster, float difficultyLimit) {
        var levelLimit = ConvertDifficultyToLevel(difficultyLimit);
        if (levelLimit >= monster.minLevel) return true;
        else return false;
    }

    private int ConvertDifficultyToLevel(float difficultyLimit) {
        int level = 1;
        while (difficultyLimit >= 1.44f) {
            difficultyLimit /= 1.44f;
            level++;
        }
        return level;
    }

    private Monster RetrieveMonster(string name) {
        foreach (var monster in monsterPrefabs) if (monster.name == name) return monster.GetComponent<Monster>();
        return null;
    }

    private string GrabValidMonster(List<string> monsters, float difficultyLimit) {
        var valid = new List<string>();
        foreach (var monster in monsters) {
            var monsterObj = RetrieveMonster(monster);
            if (MonsterFitsCriteria(monsterObj, difficultyLimit)) valid.Add(monster);
        }
        if (valid.Count == 0) return null;
        int roll = RNG.Int(0, valid.Count);
        return valid[roll];
    }

    private void AddBoss(Room room, float difficulty, List<bool> conceptsUsed) {
        difficulty = RNG.Float(difficulty * 0.8f, difficulty * 1.2f);
        difficulty /= 22;
        var monsterType = RetrieveMonsterType(allMonsterTypes, allMonsterTypes, allMonsterTypes, difficulty);
        var monster = new MonsterData(monsterType, monsterType, ConvertDifficultyToLevel(difficulty), 4, null);
        monster.associatedRooms.Add(room);
        monsters.Add(monster);
    }

    private void NormalizeConceptWeights(List<float> conceptWeights) {
        float total = 0;
        foreach (var weight in conceptWeights) total += weight;
        for (int i = 0; i < conceptWeights.Count; i++) conceptWeights[i] /= total;
    }

    private bool HasUnintroducedConcepts(List<bool> concepts) {
        foreach (var concept in concepts) if (!concept) return true;
        return false;
    }

    private int GetPathEncounterNumber(VaultPath path) {
        int count = 0;
        foreach (var room in path.rooms) {
            if (room is VaultRoom) {
                var vaultRoom = (VaultRoom)room;
                if (vaultRoom.hasEncounter) count++;
            }
            else if (room is BossRoom) count++;
        }
        return count;
    }

    public List<Room> CreateRooms() {
        paths = GeneratePaths();
        var startingRoom = new VaultRoom();
        var roomList = new List<Room>();
        roomList.Add(startingRoom);
        while (!PathsAcceptable(paths)) paths = GeneratePaths();
        var corridors = LinkPaths(paths, startingRoom);
        var roomList2 = GetRoomsFromPaths(paths, corridors);
        foreach (var room in roomList2) roomList.Add(room);
        return roomList;
    }

    private List<VaultPath> GeneratePaths() {
        int numPaths = RNG.Int(1, 5);
        var paths = new List<VaultPath>();
        for (int i = 0; i < numPaths; i++) paths.Add(new VaultPath());
        return paths;
    }

    private bool PathsAcceptable(List<VaultPath> paths) {
        int encounterCount = 0;
        foreach (var path in paths) {
            foreach (var room in path.rooms) {
                if (room is VaultRoom) {
                    var vr = (VaultRoom)room;
                    if (vr.hasEncounter) {
                        encounterCount++;
                    }
                }
            }
        }
        if (encounterCount > 15) return false;
        return true;
    }

    private List<Corridor> LinkPaths(List<VaultPath> paths, VaultRoom startingRoom) {
        var corridors = new List<Corridor>();
        bool first = true;
        foreach (var path in paths) {
            for (int i = 0; i < path.rooms.Count - 1; i++) {
                if (first && i == path.rooms.Count - 2) {
                    first = false;
                    path.rooms[path.rooms.Count - 1] = new BossRoom();
                }
                var room = path.rooms[i];
                var nextRoom = path.rooms[i + 1];
                int corridorRoll = RNG.Int(0, 2);
                var corridor = new Corridor() {
                    connectedRooms = { room, nextRoom }
                };
                corridors.Add(corridor);
                if (corridorRoll==0) corridor.size = 4;
                else corridor.size = RNG.Int(6, 22);
            }
        }
        int corridorRoll2 = RNG.Int(0, 2);
        var corridor2 = new Corridor() {
            connectedRooms = { startingRoom, paths[0].rooms[0] }
        };
        if (corridorRoll2 == 0) corridor2.size = 4;
        else corridor2.size = RNG.Int(6, 22);
        corridors.Add(corridor2);
        foreach (var path in paths) {
            if (path == paths[0]) continue;
            AttachRandomly(path, paths, startingRoom, corridors);
        }
        return corridors;
    }

    private void AttachRandomly(VaultPath path, List<VaultPath> paths, VaultRoom startingRoom, List<Corridor> corridors) {
        int x = RNG.Int(-1, paths.Count);
        if (paths.Count == 1) return;
        while (x == paths.IndexOf(path)) x = RNG.Int(-1, paths.Count);
        if (x == -1) AttachToRoom(path, startingRoom, corridors);
        else {
            var attachPath = paths[x];
            int y = RNG.Int(0, attachPath.rooms.Count);
            var attachRoom = attachPath.rooms[y];
            AttachToRoom(path, attachRoom, corridors);
        }
    }

    private void AttachToRoom(VaultPath path, Room attachRoom, List<Corridor> corridors) {
        var corridor = new Corridor() {
            connectedRooms = { path.rooms[0], attachRoom }
        };
        int corridorRoll = RNG.Int(0, 2);
        if (corridorRoll == 0) corridor.size = 4;
        else corridor.size = RNG.Int(6, 22);
        corridors.Add(corridor);
    }

    private List<Room> GetRoomsFromPaths(List<VaultPath> paths, List<Corridor> corridors) {
        var roomList = new List<Room>();
        foreach (var corridor in corridors) roomList.Add(corridor);
        foreach (var path in paths) foreach (var room in path.rooms) roomList.Add(room);
        return roomList;
    }
}
