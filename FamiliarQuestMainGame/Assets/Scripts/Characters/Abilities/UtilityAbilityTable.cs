using UnityEngine;
using System.Collections.Generic;

public static class UtilityAbilityTable {
    private static readonly List<float> cooldownTable = new List<float>() { 1.5f, 3, 8, 15, 30, 90, 150 };
    private static readonly List<float> cooldownPointsTable = new List<float>() { 1.2f, 1.3f, 1.4f, 1.5f, 2f, 6f, 10f };
    private static readonly List<string> targetTypeTable = new List<string>() { "player", "none", "point" };

    public static UtilityAbility Retrieve(float points) {
        var initialPoints = points;
        var priority = Random.Range(12.5f, 100f);
        int usesMPRoll = Random.Range(0, 3);
        BaseStat baseStat = DetermineBaseStat(usesMPRoll);
        int mp;
        if (usesMPRoll < 2) mp = 0;
        else {
            var mpRoll = Random.Range(0, 100);
            mp = MpCost(mpRoll);
            points *= MpPointMod(mpRoll);
        }
        int attributesRoll = Random.Range(0, 100);
        int numAttributes = Random.Range(1, 5);
        //if (attributesRoll < 80) numAttributes = 1;
        //else numAttributes = 2;
        float cooldown = 0;
        int hasCDRoll = Random.Range(0, 100);
        if (hasCDRoll < 35) {
            int cooldownRoll = Random.Range(0, 7);
            cooldown = cooldownTable[cooldownRoll];
            points *= cooldownPointsTable[cooldownRoll];
        }

        int targetRoll = Random.Range(0, 3);
        var targetType = targetTypeTable[targetRoll];
        int icon = 0;
        var attributes = new List<AbilityAttribute>();
        for (int i = 0; i < numAttributes; i++) {
            var attribute = AbilityAttribute.GetUtilityAttribute(points, priority, mp, cooldown, numAttributes, targetType);
            if (attribute != null) {
                if (attribute.priority >= 50 && i < 4) points -= attribute.points;
                attributes.Add(attribute);
            }
        }
        if (AbilityValid(attributes)) {
            var ability = new UtilityAbility("", "", baseStat, cooldown: cooldown, mpUsage: mp, icon: icon, attributes: attributes.ToArray(), points: (int)initialPoints) {
                targetType = targetType
            };
            ability.icon = AbilityIconGenerator.Retrieve(ability);
            ability.name = AbilityNamer.Name(ability);
            ability.points = (int)initialPoints;
            ability.SetLevel(Ability.GetLevelFromPoints((int)initialPoints));
            ability.description = AbilityDescriber.Describe(ability);
            return ability;
        }
        else return Retrieve(initialPoints);
    }

    private static BaseStat DetermineBaseStat(int usesMPRoll) {
        if (usesMPRoll == 2) return BaseStat.wisdom;
        else if (usesMPRoll == 1) return BaseStat.dexterity;
        else return BaseStat.strength;
    }

    private static int MpCost(int mpRoll) {
        if (mpRoll < 80) return 40;
        else if (mpRoll < 90) return 20;
        else if (mpRoll < 95) return 60;
        else return 80;
    }

    private static float MpPointMod(int mpRoll) {
        if (mpRoll < 80) return 1.25f;
        else if (mpRoll < 90) return 1.125f;
        else if (mpRoll < 95) return 1.5f;
        else return 1.75f;
    }

    private static bool AbilityValid(List<AbilityAttribute> attributes) {
        if (DuplicateStealthy(attributes)) return false;
        var validAttributeList = new List<string>() { "restoreMP", "shield", "elementalDamageBuff", "heal", "hot", "mpOverTime", "disableDevice", "stealth", "grapple", "speed-", "paralyze", "removeDebuff", "removeAllDebuffs", "eatDebuff", "speed+" };
        var found = false;
        foreach (var attribute in attributes) {
            if (validAttributeList.Contains(attribute.type)) {
                found = true;
                break;
            }
        }
        if (!found) return false;
        found = false;
        foreach (var attribute in attributes) {
            if (attribute.priority >= 50) {
                found = true;
                break;
            }
        }
        return found;
    }

    public static bool DuplicateStealthy(List<AbilityAttribute> attributes) {
        foreach (var attribute in attributes) foreach (var attribute2 in attributes) if (attribute.type == "stealthy" && attribute2.type == "stealthy") return true;
        return false;
    }
}
