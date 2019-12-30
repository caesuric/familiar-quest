using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : Ability {
    public PassiveAbility(string name, string description, int icon=0, params AbilityAttribute[] attributes) {
        this.name = name;
        this.description = description;
        this.icon = icon;
        foreach (var item in attributes) this.attributes.Add(item);
    }
    public static PassiveAbility Generate(float points) {
        var initialPoints = points;
        int attributesRoll = Random.Range(0, 100);
        int numAttributes = 1;
        if (attributesRoll >= 80) numAttributes = 2;
        var attributes = new List<AbilityAttribute>();
        for (int i=0; i<numAttributes; i++) {
            var attribute = AbilityAttribute.GetPassiveAttribute(points, numAttributes);
            if (attribute != null) {
                points -= attribute.points;
                attributes.Add(attribute);
            }
        }
        if (AbilityValid(attributes)) {
            var ability = new PassiveAbility("", "", 0, attributes.ToArray());
            ability.icon = GetAbilityIcon(ability);
            ability.name = NameAbility(ability);
            ability.points = (int)initialPoints;
            ability.description = DescribeAbility(ability);
            return ability;
        }
        else return Generate(initialPoints);
    }

    public static string NameAbility(PassiveAbility ability) {
        string baseName = "";
        string nameMod = "";
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "damageEnemiesOnScreen":
                default:
                    baseName = "Burning Aura";
                    break;
                case "experienceBoost":
                    baseName = "Fast Learner";
                    break;
                case "knockback":
                    baseName = "Knockback";
                    break;
                case "charge":
                    baseName = "Charge";
                    break;
                case "pullEnemies":
                    baseName = "Grapple Gun";
                    break;
                case "goldBoost":
                    baseName = "Miser";
                    break;
                case "boostDamage":
                    baseName = "Damage Up";
                    break;
                case "reduceDamage":
                    baseName = "Protection";
                    break;
                case "reduceElementalDamage":
                    baseName = attribute.FindParameter("element").stringVal[0].ToString().ToUpper() + attribute.FindParameter("element").stringVal.Substring(1) + " Ward";
                    break;
                case "boostElementalDamage":
                    baseName = attribute.FindParameter("element").stringVal[0].ToString().ToUpper() + attribute.FindParameter("element").stringVal.Substring(1) + " Boost";
                    break;
                case "boostStat":
                    switch (attribute.FindParameter("stat").stringVal) {
                        case "strength":
                            baseName = "Strength Up";
                            break;
                        case "dexterity":
                            baseName = "Dexterity Up";
                            break;
                        case "constitution":
                            baseName = "Constitution Up";
                            break;
                        case "intelligence":
                            baseName = "Intelligence Up";
                            break;
                        case "wisdom":
                            baseName = "Wisdom Up";
                            break;
                        case "luck":
                            baseName = "Luck Up";
                            break;
                    }
                    break;
            }
        }
        if (nameMod == "") return baseName;
        return nameMod + " " + baseName;
    }

    public static string DescribeAbility(PassiveAbility ability) {
        string description = "L" + ability.GetLevel().ToString() + " <b>Passive</b>\n";
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "damageEnemiesOnScreen":
                default:
                    description += "Continously deals " + attribute.FindParameter("degree").floatVal.ToString() + " damage per second to all enemies on screen.\n";
                    break;
                case "experienceBoost":
                    description += "Increases experience gained by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "%.\n";
                    break;
                case "knockback":
                    description += "Adds knockback to all attacks.\n";
                    break;
                case "charge":
                    description += "All melee attacks cause you to charge forward until you hit an enemy.\n";
                    break;
                case "pullEnemies":
                    description += "All ranged attacks pull enemies to you on hit.\n";
                    break;
                case "goldBoost":
                    description += "Increases gold found by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "%.\n";
                    break;
                case "boostDamage":
                    description += "Increases all damage dealt by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "%.\n";
                    break;
                case "reduceDamage":
                    description += "Decreases all damage taken by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "%.\n";
                    break;
                case "reduceElementalDamage":
                    description += "Reduce " + attribute.FindParameter("element").stringVal + " damage taken by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "%.\n";
                    break;
                case "boostElementalDamage":
                    description += "Increase " + attribute.FindParameter("element").stringVal + " damage dealt by " + ((int)(attribute.FindParameter("degree").floatVal * 100f)).ToString() + "%.\n";
                    break;
                case "boostStat":
                    switch (attribute.FindParameter("stat").stringVal) {
                        case "strength":
                            description += "Increases Strength by " + attribute.FindParameter("degree").intVal.ToString() + ".\n";
                            break;
                        case "dexterity":
                            description += "Increases Dexterity by " + attribute.FindParameter("degree").intVal.ToString() + ".\n";
                            break;
                        case "constitution":
                            description += "Increases Constitution by " + attribute.FindParameter("degree").intVal.ToString() + ".\n";
                            break;
                        case "intelligence":
                            description += "Increases Intelligence by " + attribute.FindParameter("degree").intVal.ToString() + ".\n";
                            break;
                        case "wisdom":
                            description += "Increases Wisdom by " + attribute.FindParameter("degree").intVal.ToString() + ".\n";
                            break;
                        case "luck":
                            description += "Increases Luck by " + attribute.FindParameter("degree").intVal.ToString() + ".\n";
                            break;
                    }
                    break;
            }
        }
        return description;
    }

    public static int GetAbilityIcon(PassiveAbility ability) {
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "damageEnemiesOnScreen":
                    return 68;
                case "experienceBoost":
                    return 69;
                case "knockback":
                    return 71;
                case "charge":
                    return 5;
                case "pullEnemies":
                    return 72;
                case "goldBoost":
                    return 73;
                case "boostDamage":
                    return 77;
                case "boostElementalDamage":
                    switch (attribute.FindParameter("element").stringVal) {
                        default:
                        case "piercing":
                            return 26;
                        case "slashing":
                            return 25;
                        case "bashing":
                            return 24;
                        case "fire":
                            return 28;
                        case "ice":
                            return 29;
                        case "acid":
                            return 27;
                        case "light":
                            return 30;
                        case "dark":
                            return 31;
                    }
                case "reduceDamage":
                    return 19;
                case "reduceElementalDamage":
                    return 19;
                case "boostStat":
                    switch (attribute.FindParameter("stat").stringVal) {
                        case "strength":
                            return 74;
                        case "dexterity":
                            return 5;
                        case "constitution":
                            return 48;
                        case "intelligence":
                            return 55;
                        case "wisdom":
                            return 75;
                        case "luck":
                            return 76;
                    }
                    break;
                default:
                    return 0;
            }
        }
        return 0;
    }

    public static bool AbilityValid(List<AbilityAttribute> attributes) {
        return true;
        //var validAttributeList = new List<string>() { "damageEnemiesOnScreen", "experienceBoost" };
        //foreach (var attribute in attributes) if (validAttributeList.Contains(attribute.type)) return true;
        //return false;
    }

    public int GetLevel() {
        var level = 1;
        float currentPoints = points;
        while (currentPoints > 70) {
            currentPoints /= 1.05f;
            level += 1;
        }
        return level;
    }

    public override Ability Copy() {
        var newAbility = new PassiveAbility(name, description, icon, new AbilityAttribute[] { });
        foreach (var attribute in attributes) newAbility.attributes.Add(attribute.Copy());
        return newAbility;
    }
}
