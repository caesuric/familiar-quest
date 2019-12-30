using System.Collections.Generic;
using UnityEngine;

public class DesignedBuilding : Dungeon {

    public static Dictionary<string, float> monsterRoomSizes = new Dictionary<string, float> {
        { "kobold", 1 },
        { "goblin", 1 },
        { "ogre", 4 },
        { "troll", 4 },
        { "minotaur", 4 },
        { "cyclops", 4 },
        { "human", 2 },
        { "ankheg", 1 },
        { "cheetah", 4 },
        { "imp", 1 },
        { "landSquid", 2 },
        { "slime", 1 },
        { "spider", 1 },
        { "wolf", 2 },
        { "dragon", 2 },
        { "golem", 4 },
        { "ghoul", 2 },
        { "archon", 2 }
    };
    public static int roomSpacing = 3;
    public static int floorPadding = 24;
    public static float globalSpaceMultiplier = 3.5f;

    public DesignedBuilding(SocialStructure socialStructure) {
        maxSocialTier = GetMaxSocialTier(socialStructure);
        var livingQuarters = GetLivingQuarters(socialStructure);
        var commonSpaces = GetCommonSpaces(socialStructure);
        DetermineSize(livingQuarters);
        DetermineSize(commonSpaces);
        foreach (var room in livingQuarters) rooms.Add(room);
        foreach (var room in commonSpaces) rooms.Add(room);
        var bossRoom = new BossRoom() { size = 100};
        rooms.Add(bossRoom);
        socialStructure.population[0].quality = 4;
        socialStructure.population[0].associatedRooms = new List<Room> { bossRoom };
        layout = new DesignedBuildingLayout(this);
        layout.LayoutRooms();
        //Debug.Log(Print(livingQuarters, commonSpaces));
    }

    private int GetMaxSocialTier(SocialStructure socialStructure) {
        int maximum = 0;
        foreach (var node in socialStructure.hierarchy.allNodes) maximum = Mathf.Max(node.GetDepth(), maximum);
        return maximum;
    }

    private List<LivingQuarters> GetLivingQuarters(SocialStructure socialStructure) {
        var output = new List<LivingQuarters>();
        foreach (var node in socialStructure.hierarchy.allNodes) {
            var temp = GetSpecificLivingQuarters(node, socialStructure);
            foreach (var node2 in temp) output.Add(node2);
        }
        return output;
    }

    private List<LivingQuarters> GetSpecificLivingQuarters(SocialNode node, SocialStructure socialStructure) {
        return GetSpecificRooms<LivingQuarters>(node, socialStructure, 20);
    }

    private List<CommonSpace> GetSpecificCommonSpace(SocialNode node, SocialStructure socialStructure) {
        return GetSpecificRooms<CommonSpace>(node, socialStructure, 80);
    }

    private List<T> GetSpecificRooms<T>(SocialNode node, SocialStructure socialStructure, int roomPopulationScale) where T : AssignedRoom, new() {
        var output = new List<T>();
        var numTiers = maxSocialTier + 1;
        float grandiosity = ((float)numTiers - (float)node.GetDepth()) / (float)numTiers;
        float numberPerRoomRoll = Random.Range(grandiosity / 2, grandiosity * 1.5f);
        int numberPerRoom = (int)(roomPopulationScale - (numberPerRoomRoll * roomPopulationScale));
        numberPerRoom = Mathf.Max(numberPerRoom, 1);
        int numRooms = (int)(Mathf.Ceil((float)FetchPopulationCount(socialStructure, node) / (float)numberPerRoom));
        for (int i = 0; i < numRooms; i++) {
            grandiosity = Random.Range(grandiosity / 2, grandiosity * 1.5f);
            T room = new T {
                grandiosity = grandiosity,
                socialTier = node.GetDepth()
            };
            output.Add(room);
        }
        AssignInhabitants(socialStructure, node, output, numberPerRoom);
        return output;
    }

    private int FetchPopulationCount(SocialStructure socialStructure, SocialNode node) {
        int count = 0;
        foreach (var mob in socialStructure.population) if (mob.node == node) count++;
        return count;
    }

    private void AssignInhabitants<T>(SocialStructure socialStructure, SocialNode node, List<T> rooms, int numberPerRoom) where T : AssignedRoom {
        var mobList = GetMonstersForNode(node, socialStructure);
        int currentMonster = 0;
        foreach (var room in rooms) {
            while (room.inhabitants.Count < numberPerRoom && currentMonster < mobList.Count) {
                var monster = mobList[currentMonster];
                room.inhabitants.Add(monster);
                monster.associatedRooms.Add(room);
                currentMonster++;
            }
        }
    }

    private List<MonsterData> GetMonstersForNode(SocialNode node, SocialStructure socialStructure) {
        var output = new List<MonsterData>();
        foreach (var monster in socialStructure.population) if (monster.node == node) output.Add(monster);
        return output;
    }

    private List<CommonSpace> GetCommonSpaces(SocialStructure socialStructure) {
        var output = new List<CommonSpace>();
        foreach (var node in socialStructure.hierarchy.allNodes) {
            var temp = GetSpecificCommonSpace(node, socialStructure);
            foreach (var node2 in temp) output.Add(node2);
        }
        return output;
    }

    private void DetermineSize<T>(List<T> rooms) where T : AssignedRoom {
        foreach (var room in rooms) SetRoomSize(room);
    }

    private void SetRoomSize(AssignedRoom room) {
        if (room.inhabitants.Count == 0) {
            room.size = 1;
            return;
        }
        float baseSize = DetermineSizeForMonsterType(room.inhabitants[0].generalType, room.inhabitants.Count);
        baseSize *= room.grandiosity * globalSpaceMultiplier;
        if (room is CommonSpace) baseSize /= 2;
        if (baseSize < 1) baseSize = 1;
        room.size = Mathf.Floor(baseSize);
    }

    private float DetermineSizeForMonsterType(string type, int count) {
        return monsterRoomSizes[type] * count;
    }

    public string Print(List<LivingQuarters> livingQuarters, List<CommonSpace> commonSpaces) {
        var output = "Room summary:\n";
        output += "Living quarters:\n";
        int index = 0;
        foreach (var room in livingQuarters) {
            output += "Room #" + index.ToString() + ":\n";
            output += "Grandiosity: " + room.grandiosity.ToString() + "\n";
            output += "Size: " + room.size.ToString() + "\n";
            output += "Social Tier: " + room.socialTier.ToString() + "\n";
            output += "Inhabitants: " + room.inhabitants.Count.ToString() + " " + room.inhabitants[0].generalType + "\n";
            output += "\n";
            index++;
        }
        output += "Common Rooms:\n";
        foreach (var room in commonSpaces) {
            output += "Room #" + index.ToString() + ":\n";
            output += "Grandiosity: " + room.grandiosity.ToString() + "\n";
            output += "Size: " + room.size.ToString() + "\n";
            output += "Social Tier: " + room.socialTier.ToString() + "\n";
            output += "Users: " + room.inhabitants.Count.ToString() + "\n";
            output += "\n";
            index++;
        }
        return output;
    }
}