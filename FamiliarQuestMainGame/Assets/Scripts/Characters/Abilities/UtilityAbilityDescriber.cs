using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class UtilityAbilityDescriber {
    private delegate string DescriberDelegate(Ability ability, AbilityAttribute attribute);
    private static readonly Dictionary<string, DescriberDelegate> descriptionTable;
    private static readonly Dictionary<string, string> simpleDescriptionTable;

    static UtilityAbilityDescriber() {
        simpleDescriptionTable = new Dictionary<string, string> {
            ["restoreMP"] = "Restores {{restoreMP}} MP.\n",
            ["shield"] = "Shields you from {{shield}} damage.\n",
            ["heal"] = "Heals {{healing}} damage.\n",
            ["hot"] = "Heals {{hot}}.\n",
            ["mpOverTime"] = "Restores {{restoreMpOverTime}}.\n",
            ["stealthy"] = "Doesn't break stealth.\n",
            ["usableWhileParalyzed"] = "Usable while paralyzed.\n",
            ["offGCD"] = "Doesn't triger global cooldown.\n",
            ["disableDevice"] = "Unlocks and disarms nearby doors, chests, and traps.\n",
            ["stealth"] = "Enter stealth mode.\n",
            ["grapple"] = "Fires a grappling hook.\n",
            ["removeDebuff"] = "Removes a debuff.\n",
            ["removeAllDebuffs"] = "Removes all debuffs.\n",
            ["eatDebuff"] = "Removes a debuff to gain 50 MP.\n",
            ["grapplingHook"] = "Pulls user directly forward to the next creature or object ahead of them.\n"
        };
        descriptionTable = new Dictionary<string, DescriberDelegate> {
            ["elementalDamageBuff"] = ElementalDamageBuffDescription,
            ["speed-"] = SpeedMinusDescription,
            ["paralyze"] = ParalyzeDescription,
            ["immobilizeSelf"] = ImmobilizeSelfDescription,
            ["speed+"] = SpeedPlusDescription
        };
    }

    public static string Describe(UtilityAbility ability) {
        string description = GetTopDescription(ability);
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) description += "LATENT - ";
            if (simpleDescriptionTable.ContainsKey(attribute.type)) description += simpleDescriptionTable[attribute.type];
            else if (descriptionTable.ContainsKey(attribute.type)) description += descriptionTable[attribute.type](ability, attribute);
            else description += attribute.type + " - DESCRIPTION NOT FOUND.\n";
            count++;
        }
        return description;
    }

    private static string GetTopDescription(UtilityAbility ability) {
        string description = "L" + ability.level.ToString() + " <b>Utility, uses ";
        description += "{{baseStat}}</b>\n";
        if (ability.mpUsage > 0) description += "<b>MP</b>: " + ability.mpUsage.ToString() + "\n";
        if (ability.cooldown >= 60) description += "<b>Cooldown</b>: " + (ability.cooldown / 60).ToString() + "m\n";
        else if (ability.cooldown > 0) description += "<b>Cooldown</b>: " + ability.cooldown.ToString() + "s\n";
        return description;
    }

    private static string SpeedPlusDescription(Ability ability, AbilityAttribute attribute) {
        var duration = Mathf.FloorToInt((float)attribute.FindParameter("duration").value);
        return "Increases target player's speed by " + Mathf.FloorToInt(((float)attribute.FindParameter("degree").value) * 100f).ToString() + "% for " + duration.ToString() + " seconds.\n";
    }

    private static string ImmobilizeSelfDescription(Ability ability, AbilityAttribute attribute) {
        var duration = Mathf.FloorToInt((float)attribute.FindParameter("duration").value);
        return "Immobilizes self for " + duration.ToString() + " seconds.\n";
    }

    private static string ParalyzeDescription(Ability ability, AbilityAttribute attribute) {
        var utilityAbility = ability as UtilityAbility;
        if (utilityAbility.targetType == "none") return "Paralyze all enemies within radius " + Mathf.FloorToInt((float)attribute.FindParameter("radius").value).ToString() + " for " + Mathf.FloorToInt((float)attribute.FindParameter("duration").value).ToString() + " seconds.\n";
        else if (utilityAbility.targetType == "point" && attribute.FindParameter("radius") != null) return "Paralyze all enemies within radius " + Mathf.FloorToInt((float)attribute.FindParameter("radius").value).ToString() + " of targeted point for " + Mathf.FloorToInt((float)attribute.FindParameter("duration").value).ToString() + " seconds.\n";
        else return "Paralyze targeted enemy for " + Mathf.FloorToInt((float)attribute.FindParameter("duration").value).ToString() + " seconds.\n";
    }

    private static string SpeedMinusDescription(Ability ability, AbilityAttribute attribute) {
        var utilityAbility = ability as UtilityAbility;
        if (utilityAbility.targetType == "none") return "Slows all enemies within radius " + Mathf.FloorToInt((float)(attribute.FindParameter("radius").value)).ToString() + " by " + Mathf.FloorToInt((float)attribute.FindParameter("degree").value * 100f).ToString() + "% for " + Mathf.FloorToInt((float)attribute.FindParameter("duration").value).ToString() + " seconds.\n";
        else if (utilityAbility.targetType == "point" && attribute.FindParameter("radius") != null) return "Slows all enemies within radius " + Mathf.FloorToInt((float)attribute.FindParameter("radius").value).ToString() + " of targeted point by " + Mathf.FloorToInt((float)attribute.FindParameter("degree").value * 100f).ToString() + "% for " + Mathf.FloorToInt((float)attribute.FindParameter("duration").value).ToString() + " seconds.\n";
        else return "Slows targeted enemy by " + Mathf.FloorToInt((float)attribute.FindParameter("degree").value * 100f).ToString() + "% for " + Mathf.FloorToInt((float)attribute.FindParameter("duration").value).ToString() + " seconds.\n";
    }

    private static string ElementalDamageBuffDescription(Ability ability, AbilityAttribute attribute) {
        var element = attribute.FindParameter("element").value.ToString();
        return "Boosts " + element + " damage by " + attribute.FindParameter("degree").value.ToString() + "% for " + attribute.FindParameter("duration").value.ToString() + " seconds after use.\n";
    }
}
