//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ElementalAffinity {
//    public Element type;
//    public int amount;
//    public static Dictionary<Element, List<Element>> alliances = new Dictionary<Element, List<Element>>();
//    public static Dictionary<Element, Element> enemies = new Dictionary<Element, Element>();

//    static ElementalAffinity() {
//        var data = TextReader.ReadSets("ElementalAlliances");
//        foreach (var item in data)
//        {
//            var list = new List<Element>();
//            for (int i = 1; i < item.Length; i++) list.Add(StringToElement(item[i]));
//            alliances.Add(StringToElement(item[0]), list);
//        }
//        data = TextReader.ReadSets("ElementalEnemies");
//        foreach (var item in data) enemies.Add(StringToElement(item[0]), StringToElement(item[1]));
//    }

//    public float CalculateDamage() {
//        return 1 + ((float)amount) / 100;
//    }

//    public ElementalAffinity(Element type, int amount) {
//        this.type = type;
//        this.amount = amount;
//    }

//    public ElementalAffinity(Element baseType, bool positive=true) {
//        amount = int.Parse(TableRoller.Roll("ElementalAffinityDegree"));
//        if (!positive) amount *= -1;
//        if (positive) type = GetPositiveAffinity(baseType);
//        else type = GetNegativeAffinity(baseType);
//    }

//    private Element GetPositiveAffinity(Element baseType)
//    {
//        int elementRoll = UnityEngine.Random.Range(0, 100);
//        if (elementRoll < 50) return baseType;
//        else if (elementRoll < 95)
//        {
//            int allianceRoll = UnityEngine.Random.Range(0, alliances[baseType].Count);
//            return alliances[baseType][allianceRoll];
//        }
//        return Spirit.RandomElement();
//    }

//    private Element GetNegativeAffinity(Element baseType)
//    {
//        int elementRoll = UnityEngine.Random.Range(0, 100);
//        if (elementRoll < 50) return enemies[baseType];
//        else if (elementRoll < 95)
//        {
//            int allianceRoll = UnityEngine.Random.Range(0, alliances[enemies[baseType]].Count);
//            return alliances[enemies[baseType]][allianceRoll];
//        }
//        else return Spirit.RandomElement();
//    }

//    public static Element StringToElement(string text)
//    {
//        return (Element)System.Enum.Parse(typeof(Element), text, true);
//    }
//}
