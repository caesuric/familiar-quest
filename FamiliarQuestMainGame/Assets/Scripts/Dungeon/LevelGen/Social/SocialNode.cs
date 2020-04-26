using System.Collections.Generic;
using UnityEngine;

public class SocialNode {
    private static List<string> bossTypes = new List<string>() { "kobold", "goblin", "ogre", "troll", "dragon", "minotaur", "cyclops", "human", "golem", "ghoul", "archon" };
    private static int counter = 0;

    public List<SocialNode> vassals = new List<SocialNode>();
    public SocialNode lord = null;
    public Dictionary<string, float> typeMix = new Dictionary<string, float>();
    public int actualPopulation = 0;
    public int id;
    public string uuid = System.Guid.NewGuid().ToString();

    public SocialNode(List<string> builders, bool differentType, SocialNode lord) {
        if (lord == null) GenerateBoss(builders, differentType);
        else GenerateNormal(builders, differentType, lord);
        id = counter;
        counter++;
    }

    private void GenerateBoss(List<string> builders, bool differentType) {
        int typeRoll;
        if (!differentType) {
            typeRoll = Random.Range(0, builders.Count);
            typeMix.Add(builders[typeRoll], 1);
        }
        else {
            typeRoll = Random.Range(0, bossTypes.Count);
            typeMix.Add(bossTypes[typeRoll], 1);
        }
    }

    private void GenerateNormal(List<string> builders, bool differentType, SocialNode lord) {
        this.lord = lord;
        this.lord.vassals.Add(this);
        List<string> types = new List<string>();
        int numTypesRoll, typeRoll;
        if (!differentType) {
            numTypesRoll = Random.Range(1, builders.Count);
            for (int i = 0; i < numTypesRoll; i++) {
                typeRoll = Random.Range(0, builders.Count);
                types.Add(builders[typeRoll]);
            }
        }
        else {
            numTypesRoll = Random.Range(1, 3);
            for (int i = 0; i < numTypesRoll; i++) {
                typeRoll = Random.Range(0, SocialStructure.builderSpecies.Count);
                types.Add(SocialStructure.builderSpecies[typeRoll]);
            }
        }
        typeMix = DistributeTypes(types);
    }

    private Dictionary<string, float> DistributeTypes(List<string> types) {
        var output = new Dictionary<string, float>();
        float total = 0;
        foreach (var type in types) {
            float populationRoll = Random.Range(0, 1 - total);
            if (output.ContainsKey(type)) output[type] += populationRoll;
            else output.Add(type, populationRoll);
            total += populationRoll;
        }
        output[types[types.Count - 1]] += 1 - total;
        return output;
    }

    public int GetDepth() {
        var cursor = this;
        int count = 0;
        while (cursor.lord != null) {
            cursor = cursor.lord;
            count++;
        }
        return count;
    }
}
