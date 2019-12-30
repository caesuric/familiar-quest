using System.Collections.Generic;
using UnityEngine;

public class SocialStructure {

    public static List<string> builderSpecies = new List<string> { "kobold", "goblin", "ogre", "troll", "minotaur", "cyclops", "human" };
    private static List<string> petSpecies = new List<string> { "ankheg", "cheetah", "imp", "landSquid", "slime", "spider", "wolf", "dragon" };
    private static List<string> servantSpecies = new List<string> { "kobold", "goblin", "imp", "ogre", "troll", "minotaur", "cyclops", "human", "golem" };
    public static List<string> dungeonSpecies = new List<string> { "ankheg", "bomber", "archon", "cheetah", "human", "cyclops", "dragon", "elemental", "slime", "undead", "goblin", "golem", "gremlin", "imp", "wisplet", "kobold", "landSquid", "mimic", "minotaur", "ogre", "fungus", "spider", "troll", "wolf" };
    private static Dictionary<string, List<string>> specificTypes = new Dictionary<string, List<string>> {
        { "bomber", new List<string> { "Bomber"} },
        { "kobold", new List<string> { "Kobold" } },
        { "goblin", new List<string> { "GOBLIN", "Goblin Archer", "Goblin Rogue"} },
        { "ogre", new List<string> { "Ogre" } },
        { "troll", new List<string> { "Troll" } },
        { "minotaur", new List<string> { "Minotaur" } },
        { "cyclops", new List<string> { "Cyclops" } },
        { "human", new List<string> { "Dark Knight", "Warlock", "Corrupted Sorcerer", "Dark Bishop", "Mirror Mage" } },
        { "ankheg", new List<string> { "Ankheg" } },
        { "cheetah", new List<string> { "Cheetah" } },
        { "imp", new List<string> { "Imp" } },
        { "landSquid", new List<string> { "Land Squid" } },
        { "slime", new List<string> { "Slime", "Gelatinous Mass" } },
        { "spider", new List<string> { "Spider" } },
        { "wolf", new List<string> { "Wolf" } },
        { "dragon", new List<string> { "Young Dragon", "Dragon", "Elder Dragon" } },
        { "golem", new List<string> { "Golem" } },
        { "ghoul", new List<string> { "Ghoul" } },
        { "archon", new List<string> { "Bound Archon" } },
        { "elemental", new List<string> { "Fire Elemental", "Ice Elemental"} },
        { "undead", new List<string> { "Ghost", "Ghoul" } },
        { "gremlin", new List<string> { "Gremlin" } },
        { "wisplet", new List<string> { "Jittery Wisplet" } },
        { "mimic", new List<string> { "Animated Statue", "Mimic" } },
        { "fungus", new List<string> { "Phantom Fungus" } }
    };
    private List<string> builders = new List<string>();
    private List<string> pets = new List<string>();
    private List<string> servants = new List<string>();
    public SocialHierarchy hierarchy;
    public List<MonsterData> population = new List<MonsterData>();

    public SocialStructure(int monsterQuantity, bool dungeon = false) {
        if (dungeon) builders = GetDungeonDwellers();
        else builders = GetBuilders();
        pets = GetPets();
        if (dungeon) servants = GetDungeonDwellers();
        else servants = GetServants();
        hierarchy = new SocialHierarchy(builders, pets, servants);
        population = GetPopulation(hierarchy, monsterQuantity);
    }

    public List<string> GetDungeonDwellers() {
        var output = new List<string>();
        int roll = Random.Range(0, dungeonSpecies.Count);
        output.Add(dungeonSpecies[roll]);
        roll = Random.Range(0, 10);
        if (roll <= 4) {
            roll = Random.Range(0, dungeonSpecies.Count);
            output.Add(dungeonSpecies[roll]);
        }
        if (roll <= 1) {
            roll = Random.Range(0, dungeonSpecies.Count);
            output.Add(dungeonSpecies[roll]);
        }
        return output;
    }

    public List<string> GetBuilders() {
        var output = new List<string>();
        int roll = Random.Range(0, builderSpecies.Count);
        output.Add(builderSpecies[roll]);
        roll = Random.Range(0, 10);
        if (roll <= 1) {
            roll = Random.Range(0, builderSpecies.Count);
            output.Add(builderSpecies[roll]);
        }
        return output;
    }

    public List<string> GetPets() {
        var output = new List<string>();
        int roll = Random.Range(0, 10);
        int roll2;
        if (roll == 0) {
            roll2 = Random.Range(0, petSpecies.Count);
            output.Add(petSpecies[roll2]);
        }
        if (roll <= 4) {
            roll2 = Random.Range(0, petSpecies.Count);
            output.Add(petSpecies[roll2]);
        }
        return output;
    }

    public List<string> GetServants() {
        var output = new List<string>();
        int roll = Random.Range(0, 10);
        int roll2, count;
        if (roll == 0) count = 2;
        else if (roll <= 4) count = 1;
        else count = 0;
        count = Mathf.Max(count - pets.Count, 0);
        if (count >= 1) {
            roll2 = Random.Range(0, petSpecies.Count);
            output.Add(servantSpecies[roll2]);
        }
        if (roll >= 2) {
            roll2 = Random.Range(0, petSpecies.Count);
            output.Add(servantSpecies[roll2]);
        }
        return output;
    }

    protected List<MonsterData> GetPopulation(SocialHierarchy hierarchy, int quantity) {
        var output = new List<MonsterData>();
        var quantities = GetPopulations(hierarchy, quantity);
        foreach (var node in hierarchy.allNodes) {
            node.actualPopulation = quantities[node];
            foreach (var monster in GenerateMonstersForNode(node, quantities[node])) output.Add(monster);
        }
        return output;
    }

    private Dictionary<SocialNode, int> GetPopulations(SocialHierarchy hierarchy, int quantity) {
        var output = new Dictionary<SocialNode, float>();
        foreach (var node in hierarchy.allNodes) output.Add(node, GetPopulation(node));
        output = NormalizePopulations(output, quantity);
        var finalOutput = FloorPopulations(output);
        return finalOutput;
    }

    private float GetPopulation(SocialNode node) {
        float answer = 1;
        for (int i = 0; i < node.GetDepth(); i++) {
            int roll = Random.Range(2, 5);
            answer *= roll;
        }
        return answer;
    }

    private Dictionary<SocialNode, float> NormalizePopulations(Dictionary<SocialNode, float> populations, float quantity) {
        var output = new Dictionary<SocialNode, float>();
        float total = 0;
        foreach (var pair in populations) total += pair.Value;
        foreach (var pair in populations) output.Add(pair.Key, Mathf.Max(pair.Value * (quantity / total), 1));
        return output;
    }

    private Dictionary<SocialNode, int> FloorPopulations(Dictionary<SocialNode, float> populations) {
        var output = new Dictionary<SocialNode, int>();
        foreach (var pair in populations) output.Add(pair.Key, (int)(pair.Value));
        return output;
    }

    public List<MonsterData> GenerateMonstersForNode(SocialNode node, int quantity) {
        var output = new List<MonsterData>();
        foreach (var typePair in node.typeMix) {
            for (int i = 0; i < typePair.Value * quantity; i++) output.Add(GenerateMonster(typePair.Key, node));
        }
        return output;
    }

    public MonsterData GenerateMonster(string type, SocialNode node) {
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
        if (node.lord == null) quality = 3; //4
        return new MonsterData(type, GetSpecificMonsterType(type), level, quality, node);
    }

    public string GetSpecificMonsterType(string type) {
        var types = specificTypes[type];
        int typeRoll = Random.Range(0, types.Count);
        return types[typeRoll];
    }

    public string Print() {
        var output = "";
        output += "NEW SOCIAL STRUCTURE\n--------------------\n";
        output += "Builders: " + PrintStringList(builders) + "\n";
        output += "Servants: " + PrintStringList(servants) + "\n";
        output += "Pets: " + PrintStringList(pets) + "\n";
        output += "---------------------------------------\n";
        output += "Social Class Details:\n";
        foreach (var node in hierarchy.allNodes) output += PrintNode(node);
        return output;
    }

    public string PrintStringList(List<string> list) {
        var output = "";
        if (list.Count == 0) return output;
        foreach (var item in list) output += ", " + item;
        output = output.Substring(2);
        return output;
    }

    public string PrintNode(SocialNode node) {
        var output = "";
        output += "Node ID: " + node.id.ToString() + "\n";
        output += "Population: " + node.actualPopulation.ToString() + "\n";
        if (node.lord == null) output += "Society Leader!\n";
        else output += "Boss: " + node.lord.id.ToString() + "\n";
        output += "Population breakdown: \n";
        foreach (var pair in node.typeMix) output += pair.Key + ": " + Mathf.Floor(pair.Value * 100).ToString() + "%\n";
        output += "\n";
        return output;
    }
}
