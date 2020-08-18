//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Spirit {
//    public List<ActiveAbility> activeAbilities = new List<ActiveAbility>();
//    public List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
//    public List<ElementalAffinity> elements = new List<ElementalAffinity>();
//    public List<Element> types = new List<Element>();
//    public string name;
//    public string description = "";
//    public static List<string> nameList = new List<string>();
//    public static Dictionary<string, string> passiveAbilityDescriptions = new Dictionary<string, string>();
//    public static bool initialized = false;

//    public static void Initialize() {
//        initialized = true;
//        var data = TextReader.ReadItems("ClassList");
//        data = TextReader.ReadItems("SpiritNames");
//        foreach (var item in data) nameList.Add(item);
//        var data2 = TextReader.ReadSets("PassiveAbilityDescriptions");
//        foreach (var item in data2) passiveAbilityDescriptions.Add(item[0], item[1]);
//    }
//    public Spirit(List<ActiveAbility> abilities, bool thief = false) {
//        if (!initialized) Initialize();
//        activeAbilities = abilities;
//        //if (thief) passiveAbilities.Add(new PassiveAbility("findSecretDoors"));
//        name = "Starter Spirit";
//        description = "Your friendly neighborhood starter spirit.";
//    }

//    public Spirit(int level) {
//        if (!initialized) Initialize();
//        if (SceneInitializer.instance != null && SceneInitializer.instance.inside && LevelGen.dungeonData != null && LevelGen.dungeonData.dungeonData.elementalAffinity != Element.none) types = new List<Element> { LevelGen.dungeonData.dungeonData.elementalAffinity };
//        else SetTypes();
//        CreateRandomAffinities();
//        RemoveRedundantElements();
//        CreateRandomAbilities(level);
//        ConsolidateStatUsage();
//        SetNameAndDescription();
//    }

//    private void ConsolidateStatUsage() {
//        //if (activeAbilities[0] is AttackAbility || activeAbilities[1] is AttackAbility || activeAbilities[2] is AttackAbility) {
//        //    BaseStat type = BaseStat.luck;
//        //    foreach (var ability in activeAbilities) {
//        //        if (ability is AttackAbility) {
//        //            if (type == BaseStat.luck) type = ((AttackAbility)ability).baseStat;
//        //            else ((AttackAbility)ability).baseStat = type;
//        //        }
//        //    }
//        //}
//    }

//    private void SetNameAndDescription() {
//        int nameRoll = Random.Range(0, nameList.Count);
//        name = nameList[nameRoll];
//        SetTypesDescription();
//        SetAffinitiesDescription();
//        SetPassivesDescription();
//    }

//    private void SetTypesDescription() {
//        description = "<b>Types</b>: ";
//        for (int i = 0; i < types.Count; i++) {
//            var affinity = types[i];
//            description += affinity.ToString();
//            if (i != types.Count - 1) description += ", ";
//        }
//    }

//    private void SetAffinitiesDescription() {
//        description += "\n<b>Affinities</b>: ";
//        for (int i = 0; i < elements.Count; i++) {
//            var affinity = elements[i];
//            description += affinity.type.ToString() + " " + affinity.amount.ToString();
//            if (i != elements.Count - 1) description += ", ";
//        }
//    }

//    private void SetPassivesDescription() {
//        if (passiveAbilities.Count > 0) description += "\n<b>Passive Abilities</b>:\n";

//        //foreach (var passive in passiveAbilities) description += passiveAbilityDescriptions[passive.type] + "\n";
//    }

//    private void SetTypes() {
//        int roll = Random.Range(0, 100);
//        if (roll < 93) types.Add(RandomElement());
//        else if (roll < 98) {
//            types.Add(RandomElement());
//            types.Add(RandomElement());
//        }
//        else types.Add(Element.none);
//    }

//    private void CreateRandomAffinities() {
//        foreach (var type in types) {
//            CreatePositiveAffinities(type);
//            CreateNegativeAffinities(type);
//        }
//    }

//    private void CreatePositiveAffinities(Element type) {
//        int roll = Random.Range(0, 100);
//        if (roll < 5) {
//            // no positive affinities
//        }
//        else if (roll < 90) elements.Add(new ElementalAffinity(type, positive: true));
//        else {
//            elements.Add(new ElementalAffinity(type, positive: true));
//            elements.Add(new ElementalAffinity(type, positive: true));
//        }
//    }

//    private void CreateNegativeAffinities(Element type) {
//        int roll = Random.Range(0, 100);
//        if (roll < 5) {
//            // no negative affinities
//        }
//        else if (roll < 90) elements.Add(new ElementalAffinity(type, positive: false));
//        else {
//            elements.Add(new ElementalAffinity(type, positive: false));
//            elements.Add(new ElementalAffinity(type, positive: false));
//        }
//    }

//    private void RemoveRedundantElements() {
//        var pruneList = new List<ElementalAffinity>();
//        for (int i = 0; i < elements.Count - 1; i++) {
//            for (int j = i + 1; j < elements.Count; j++) {
//                if (elements[i].type == elements[j].type) pruneList.Add(elements[j]);
//                else if (elements[i].type == Element.none) pruneList.Add(elements[i]);
//            }
//        }
//        foreach (var item in pruneList) elements.Remove(item);
//    }

//    private void CreateRandomAbilities(int level=1) {
//        //int passiveRoll = Random.Range(0, 100);
//        //int numPassives;
//        //if (passiveRoll < 50) numPassives = 0;
//        //else if (passiveRoll < 80) numPassives = 1;
//        //else if (passiveRoll < 95) numPassives = 2;
//        //else numPassives = 3;
//        //int numPassives = 0;
//        //for (int i = 0; i < 3; i++) activeAbilities.Add(ActiveAbility.Generate(types));
//        //for (int i = 0; i < 1; i++) activeAbilities.Add(ActiveAbility.Generate(types, level: level));
//        //for (int i = 0; i < numPassives; i++) passiveAbilities.Add(PassiveAbility.Generate(types));
//        int abilityTypeRoll = Random.Range(0, 3);
//        if (abilityTypeRoll < 2) activeAbilities.Add(ActiveAbility.Generate(types, level: level));
//        else {
//            float points = 70;
//            for (int i=1; i<level; i++) {
//                points *= 1.05f;
//            }
//            passiveAbilities.Add(PassiveAbility.Generate((int)points));
//        }
//    }

//    public static Element RandomElement() {
//        int isPhys = Random.Range(0, 2);
//        if (isPhys == 1) return RandomPhysicalElement();
//        else return RandomMagicElement();
//    }

//    public static Element RandomPhysicalElement() {
//        return ElementalAffinity.StringToElement(TextReader.RandomItem("PhysicalElements"));
//    }

//    public static Element RandomMagicElement() {
//        return ElementalAffinity.StringToElement(TextReader.RandomItem("MagicElements"));
//    }
//}
