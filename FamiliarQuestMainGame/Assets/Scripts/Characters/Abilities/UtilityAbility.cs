using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilityAbility : ActiveAbility {

    private static List<float> cooldownTable = new List<float>() { 1.5f, 3, 8, 15, 30, 90, 150 };
    private static List<float> cooldownPointsTable = new List<float>() { 1.2f, 1.3f, 1.4f, 1.5f, 2f, 6f, 10f };
    private static List<string> targetTypeTable = new List<string>() { "player", "none", "point" };

    public UtilityAbility() {
        // placeholder
    }

    public string targetType = "";

    public UtilityAbility(string name, string description, BaseStat baseStat = BaseStat.strength, int icon = 0, float cooldown = 0f, int mpUsage = 0, int baseMpUsage = 0, float radius = 0, int points = 70, params AbilityAttribute[] attributes) {
        this.name = name;
        this.description = description;
        this.icon = icon;
        this.cooldown = cooldown;
        this.mpUsage = mpUsage;
        this.baseMpUsage = baseMpUsage;
        this.radius = radius;
        this.baseStat = baseStat;
        foreach (var item in attributes) this.attributes.Add(item);
    }
    public static UtilityAbility Generate(List<Element> baseTypes, float points) {
        var initialPoints = points;
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
        int numAttributes;
        if (attributesRoll < 80) numAttributes = 1;
        else numAttributes = 2;
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
            var attribute = AbilityAttribute.GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            if (attribute != null) {
                points -= attribute.points;
                attributes.Add(attribute);
            }
        }
        if (AbilityValid(attributes)) {
            var ability = new UtilityAbility("", "", baseStat, cooldown: cooldown, mpUsage: mp, icon: icon, attributes: attributes.ToArray(), points: (int)initialPoints);
            ability.targetType = targetType;
            ability.icon = GetAbilityIcon(ability);
            ability.name = NameAbility(ability);
            ability.points = (int)initialPoints;
            ability.description = DescribeAbility(ability);
            return ability;
        }
        else return Generate(baseTypes, initialPoints);
    }

    public static string NameAbility(UtilityAbility ability) {
        string baseName = "";
        string nameMod = "";
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "restoreMP":
                    baseName = "Concentration";
                    break;
                case "shield":
                    baseName = "Shield";
                    break;
                case "elementalDamageBuff":
                    var element = attribute.FindParameter("element").stringVal;
                    baseName = element[0].ToString().ToUpper() + element.Substring(1) + " " + "Boost";
                    break;
                case "heal":
                    baseName = "Healing";
                    break;
                case "hot":
                    baseName = "Regeneration";
                    break;
                case "mpOverTime":
                    baseName = "Meditation";
                    break;
                case "stealthy":
                    nameMod = "Subtle";
                    break;
                case "usableWhileParalyzed":
                    nameMod = "Cognitive";
                    break;
                case "offGCD":
                    nameMod = "Swift";
                    break;
                case "disableDevice":
                    baseName = "Disable Device";
                    break;
                case "stealth":
                    baseName = "Stealth";
                    break;
                case "grapple":
                    baseName = "Grappling Hook";
                    break;
                case "speed-":
                    baseName = "Slow";
                    break;
                case "paralyze":
                    baseName = "Paralyze";
                    break;
                case "immobilizeSelf":
                    nameMod = "Turret";
                    break;
                case "removeDebuff":
                    baseName = "Cleanse";
                    break;
                case "removeAllDebuffs":
                    baseName = "Greater Cleanse";
                    break;
                case "eatDebuff":
                    baseName = "Consume Ailment";
                    break;
                case "speed+":
                    baseName = "Swiftness";
                    break;
                default:
                    break;
            }
        }
        if (nameMod == "") return baseName;
        return nameMod + " " + baseName;
    }

    public static string DescribeAbility(UtilityAbility ability) {
        string description = "L" + ability.GetLevel().ToString() + " <b>Utility, uses ";
        description += "{{baseStat}}</b>\n";
        int duration;
        if (ability.mpUsage > 0) description += "<b>MP</b>: " + ability.mpUsage.ToString() + "\n";
        if (ability.cooldown >= 60) description += "<b>Cooldown</b>: " + (ability.cooldown / 60).ToString() + "m\n";
        else if (ability.cooldown > 0) description += "<b>Cooldown</b>: " + ability.cooldown.ToString() + "s\n";
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "restoreMP":
                    description += "Restores {{restoreMP}} MP.\n";
                    break;
                case "shield":
                    description += "Shields you from {{shield}} damage.\n";
                    break;
                case "elementalDamageBuff":
                    var element = attribute.FindParameter("element").stringVal;
                    description += "Boosts " + element + " damage by " + attribute.FindParameter("degree").floatVal.ToString() + "% for " + attribute.FindParameter("duration").floatVal.ToString() + " seconds after use.\n";
                    break;
                case "heal":
                    description += "Heals {{healing}} damage.\n";
                    break;
                case "hot":
                    description += "Heals {{hot}}.\n";
                    break;
                case "mpOverTime":
                    description += "Restores {{restoreMpOverTime}}.\n";
                    break;
                case "stealthy":
                    description += "Doesn't break stealth.\n";
                    break;
                case "usableWhileParalyzed":
                    description += "Usable while paralyzed.\n";
                    break;
                case "offGCD":
                    description += "Doesn't triger global cooldown.\n";
                    break;
                case "disableDevice":
                    description += "Unlocks and disarms adjacent doors and traps.\n";
                    break;
                case "stealth":
                    description += "Enter stealth mode.\n";
                    break;
                case "grapple":
                    description += "Fires a grappling hook.\n";
                    break;
                case "speed-":
                    if (ability.targetType == "none") description += "Slows all enemies within radius " + ((int)(attribute.FindParameter("radius").floatVal)).ToString() + " by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "% for " + ((int)(attribute.FindParameter("duration").floatVal)).ToString() + " seconds.\n";
                    else if (ability.targetType == "point" && attribute.FindParameter("radius") != null) description += "Slows all enemies within radius " + ((int)(attribute.FindParameter("radius").floatVal)).ToString() + " of targeted point by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "% for " + ((int)(attribute.FindParameter("duration").floatVal)).ToString() + " seconds.\n";
                    else description += "Slows targeted enemy by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "% for " + ((int)(attribute.FindParameter("duration").floatVal)).ToString() + " seconds.\n";
                    break;
                case "paralyze":
                    if (ability.targetType == "none") description += "Paralyze all enemies within radius " + ((int)(attribute.FindParameter("radius").floatVal)).ToString() + " for " + ((int)(attribute.FindParameter("duration").floatVal)).ToString() + " seconds.\n";
                    else if (ability.targetType == "point" && attribute.FindParameter("radius") != null) description += "Paralyze all enemies within radius " + ((int)(attribute.FindParameter("radius").floatVal)).ToString() + " of targeted point for " + ((int)(attribute.FindParameter("duration").floatVal)).ToString() + " seconds.\n";
                    else description += "Paralyze targeted enemy for " + ((int)(attribute.FindParameter("duration").floatVal)).ToString() + " seconds.\n";
                    break;
                case "immobilizeSelf":
                    duration = (int)(attribute.FindParameter("duration").floatVal);
                    description += "Immobilizes self for " + duration.ToString() + " seconds.\n";
                    break;
                case "removeDebuff":
                    description += "Removes a debuff.\n";
                    break;
                case "removeAllDebuffs":
                    description += "Removes all debuffs.\n";
                    break;
                case "eatDebuff":
                    description += "Removes a debuff to gain 50 MP.\n";
                    break;
                case "speed+":
                    duration = (int)(attribute.FindParameter("duration").floatVal);
                    description += "Increases target player's speed by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "% for " + duration.ToString() + " seconds.\n";
                    break;
                default:
                    break;
            }
        }
        return description;
    }

    public static int GetAbilityIcon(UtilityAbility ability) {
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "restoreMP":
                    return 60;
                case "shield":
                    return 19;
                case "elementalDamageBuff":
                    var element = attribute.FindParameter("element").stringVal;
                    switch (element) {
                        case "slashing":
                            return 33;
                        case "piercing":
                            return 3;
                        case "bashing":
                            return 32;
                        case "fire":
                            return 9;
                        case "ice":
                            return 34;
                        case "acid":
                            return 12;
                        case "light":
                            return 35;
                        case "dark":
                            return 36;
                    }
                    break;
                case "heal":
                    return 21;
                case "hot":
                    return 61;
                case "mpOverTime":
                    return 60;
                case "stealthy":
                    break;
                case "usableWhileParalyzed":
                    break;
                case "offGCD":
                    break;
                case "disableDevice":
                    return 62;
                case "stealth":
                    return 64;
                case "grapple":
                    return 66;
                case "speed-":
                    return 70;
                case "speed+":
                    return 5;
                case "paralyze":
                    return 20;
                default:
                    break;
            }
        }
        return 0;
    }

    private static bool AbilityValid(List<AbilityAttribute> attributes) {
        if (DuplicateStealthy(attributes)) return false;
        var validAttributeList = new List<string>() { "restoreMP", "shield", "elementalDamageBuff", "heal", "hot", "mpOverTime", "disableDevice", "stealth", "grapple", "speed-", "paralyze", "removeDebuff", "removeAllDebuffs", "eatDebuff", "speed+" };
        foreach (var attribute in attributes) if (validAttributeList.Contains(attribute.type)) return true;
        return false;
    }

    public static BaseStat DetermineBaseStat(int usesMPRoll) {
        if (usesMPRoll == 2) return BaseStat.wisdom;
        else if (usesMPRoll == 1) return BaseStat.dexterity;
        else return BaseStat.strength;
    }

    public static int MpCost(int mpRoll) {
        if (mpRoll < 80) return 40;
        else if (mpRoll < 90) return 20;
        else if (mpRoll < 95) return 60;
        else return 80;
    }

    public static float MpPointMod(int mpRoll) {
        if (mpRoll < 80) return 1.25f;
        else if (mpRoll < 90) return 1.125f;
        else if (mpRoll < 95) return 1.5f;
        else return 1.75f;
    }

    public static bool DuplicateStealthy(List<AbilityAttribute> attributes) {
        foreach (var attribute in attributes) foreach (var attribute2 in attributes) if (attribute.type == "stealthy" && attribute2.type == "stealthy") return true;
        return false;
    }
}