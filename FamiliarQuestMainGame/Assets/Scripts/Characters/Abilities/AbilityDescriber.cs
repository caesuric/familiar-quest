using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityDescriber {
    public static string Describe(UtilityAbility ability) {
        string description = "L" + ability.GetLevel().ToString() + " <b>Utility, uses ";
        description += "{{baseStat}}</b>\n";
        int duration;
        if (ability.mpUsage > 0) description += "<b>MP</b>: " + ability.mpUsage.ToString() + "\n";
        if (ability.cooldown >= 60) description += "<b>Cooldown</b>: " + (ability.cooldown / 60).ToString() + "m\n";
        else if (ability.cooldown > 0) description += "<b>Cooldown</b>: " + ability.cooldown.ToString() + "s\n";
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) description += "LATENT - ";
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
            count++;
        }
        return description;
    }

    public static string Describe(AttackAbility ability) {
        string description;
        int degree, duration;
        var level = ability.GetLevel();
        description = "L" + level.ToString() + " ";
        if (!ability.isRanged) description += "<b>Melee";
        else description += "<b>Ranged";
        description += " " + ability.element.ToString() + " attack, uses {{baseStat}}</b>\n";
        if (ability.mpUsage > 0) description += "<b>MP</b>: " + ability.mpUsage.ToString() + "\n";
        if (ability.radius > 0) description += "<b>Radius</b>: " + ability.radius.ToString() + "\n";
        if (ability.cooldown >= 60) description += "<b>Cooldown</b>: " + (ability.cooldown / 60).ToString() + "m\n";
        else if (ability.cooldown > 0) description += "<b>Cooldown</b>: " + ability.cooldown.ToString() + "s\n";
        var attributes = new List<AbilityAttribute>();
        string afterDescription = "";
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) description += "LATENT - ";
            switch (attribute.type) {
                case "createDamageZone":
                    afterDescription += "Creates damage zone\n";
                    break;
                case "projectileSpread":
                    afterDescription += "Fires projectile spread\n";
                    break;
                case "jumpBack":
                    afterDescription += "Jump back after use.\n";
                    break;
                case "chargeTowards":
                    afterDescription += "Charge towards enemy.\n";
                    break;
                case "pulltowards":
                    afterDescription += "Pull enemy towards you.\n";
                    break;
                case "knockback":
                    afterDescription += "Knockback.\n";
                    break;
                case "offGCD":
                    afterDescription += "Doesn't trigger global cooldown.\n";
                    break;
                case "paralyze":
                    afterDescription += "Paralyzes.\n";
                    break;
                case "usableWhileParalyzed":
                    afterDescription += "Usable while paralyzed.\n";
                    break;
                case "backstab":
                    afterDescription += "Backstabs for 4x damage when in stealth mode.\n";
                    break;
                case "stealthy":
                    afterDescription += "Doesn't break stealth.\n";
                    break;
                case "lifeleech":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    afterDescription += "Heals you for " + degree.ToString() + "% of damage dealt.\n";
                    break;
                case "mpOverTime":
                    afterDescription += "Restores MP over time.\n";
                    break;
                case "elementalDamageBuff":
                    var type = attribute.FindParameter("element").stringVal;
                    afterDescription += "Boosts " + type + " damage by " + attribute.FindParameter("degree").floatVal.ToString() + "% for " + attribute.FindParameter("duration").floatVal.ToString() + " seconds after use.\n";
                    break;
                case "blunting":
                    afterDescription += "Nullifies enemy ability to deal damage briefly.\n";
                    break;
                case "inflictVulnerability":
                    afterDescription += "Boosts damage taken by enemy afterwards.\n";
                    break;
                case "delay":
                    afterDescription += "Deals damage after a brief delay.\n";
                    break;
                case "damageShield":
                    afterDescription += "Damage shield.\n";
                    break;
                case "restoreMP":
                    afterDescription += "Restores MP.\n";
                    break;
                case "removeDebuff":
                    afterDescription += "Removes a debuff.\n";
                    break;
                case "addedDot":
                    afterDescription += "Added damage over time.\n";
                    break;
                case "increasedCritChance":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    afterDescription += "Additional " + degree.ToString() + "% chance to critically hit.\n";
                    break;
                case "increasedCritDamage":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    afterDescription += "Additional " + degree.ToString() + "% damage on critical hit.\n";
                    break;
                case "speed-":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    duration = (int)(attribute.FindParameter("duration").floatVal);
                    afterDescription += "Slow target's movement by " + degree.ToString() + "% for " + duration.ToString() + " seconds.\n";
                    break;
                case "immobilizeSelf":
                    duration = (int)(attribute.FindParameter("duration").floatVal);
                    afterDescription += "Immobilizes self for " + duration.ToString() + " seconds.\n";
                    break;
                default:
                    break;
            }
            count++;
        }
        if (ability.dotDamage > 0) {
            description += "Deals {{dotDamage}} damage over " + ability.dotTime.ToString() + " seconds.\n";
        }
        else {
            description += "Deals {{damage}} damage.\n";
        }
        description += afterDescription;
        return description;
    }

    public static string Describe(PassiveAbility ability) {
        string description = "L" + ability.GetLevel().ToString() + " <b>Passive</b>\n";
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) description += "LATENT - ";
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
            count++;
        }
        return description;
    }



}
