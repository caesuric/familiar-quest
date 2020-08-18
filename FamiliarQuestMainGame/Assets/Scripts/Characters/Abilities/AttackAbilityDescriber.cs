using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AttackAbilityDescriber {
    private delegate string DescriberDelegate(AttackAbility ability, AbilityAttribute attribute);
    private static readonly Dictionary<string, DescriberDelegate> descriptionTable;
    private static readonly Dictionary<string, string> simpleDescriptionTable;

    static AttackAbilityDescriber() {
        simpleDescriptionTable = new Dictionary<string, string> {
            ["createDamageZone"] = "Creates damage zone\n",
            ["projectileSpread"] = "Fires projectile spread\n",
            ["jumpBack"] = "Jump back after use.\n",
            ["chargeTowards"] = "Charge towards enemy.\n",
            ["pullTowards"] = "Pull enemy towards you.\n",
            ["knockback"] = "Knockback.\n",
            ["offGCD"] = "Doesn't trigger global cooldown.\n",
            ["paralyze"] = "Paralyzes.\n",
            ["usableWhileParalyzed"] = "Usable while paralyzed.\n",
            ["backstab"] = "Backstabs for 4x damage when in stealth mode.\n",
            ["stealthy"] = "Doesn't break stealth.\n",
            ["mpOverTime"] = "Restores MP over time.\n",
            ["blunting"] = "Nullifies enemy ability to deal damage briefly.\n",
            ["inflictVulnerability"] = "Boosts damage taken by enemy afterwards.\n",
            ["delay"] = "Deals damage after a brief delay.\n",
            ["damageShield"] = "Damage shield.\n",
            ["restoreMP"] = "Restores MP.\n",
            ["removeDebuff"] = "Removes a debuff.\n",
            ["addedDot"] = "Added damage over time.\n"
        };
        descriptionTable = new Dictionary<string, DescriberDelegate> {
            ["increasedCritChance"] = IncreasedCritChanceDescription,
            ["increasedCritDamage"] = IncreasedCritDamageDescription,
            ["speed-"] = SpeedMinusDescription,
            ["immobilizeSelf"] = ImmobilizeSelfDescription,
            ["lifeleech"] = LifeleechDescription,
            ["elementalDamageBuff"] = ElementalDamageBuffDescription
        };
    }

    public static string Describe(AttackAbility ability) {
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

    private static string GetTopDescription(AttackAbility ability) {
        string description = "L" + ability.level.ToString() + " ";
        if (!ability.isRanged) description += "<b>Melee";
        else description += "<b>Ranged";
        description += " " + ability.element.ToString() + " attack, uses {{baseStat}}</b>\n";
        if (ability.mpUsage > 0) description += "<b>MP</b>: " + ability.mpUsage.ToString() + "\n";
        if (ability.radius > 0) description += "<b>Radius</b>: " + ability.radius.ToString() + "\n";
        if (ability.cooldown >= 60) description += "<b>Cooldown</b>: " + (ability.cooldown / 60).ToString() + "m\n";
        else if (ability.cooldown > 0) description += "<b>Cooldown</b>: " + ability.cooldown.ToString() + "s\n";
        if (ability.dotDamage > 0) description += "Deals {{dotDamage}} damage over " + ability.dotTime.ToString() + " seconds.\n";
        else description += "Deals {{damage}} damage.\n";
        return description;
    }

    private static string ElementalDamageBuffDescription(AttackAbility ability, AbilityAttribute attribute) {
        var type = attribute.FindParameter("element").value as string;
        return "Boosts " + type + " damage by " + attribute.FindParameter("degree").value.ToString() + "% for " + attribute.FindParameter("duration").value.ToString() + " seconds after use.\n";
    }

    private static string LifeleechDescription(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (int)(((float)attribute.FindParameter("degree").value) * 100f);
        return "Heals you for " + degree.ToString() + "% of damage dealt.\n";
    }

    private static string ImmobilizeSelfDescription(AttackAbility ability, AbilityAttribute attribute) {
        var duration = (int)attribute.FindParameter("duration").value;
        return "Immobilizes self for " + duration.ToString() + " seconds.\n";
    }

    private static string SpeedMinusDescription(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (int)(((float)attribute.FindParameter("degree").value) * 100f);
        var duration = (int)attribute.FindParameter("duration").value;
        return "Slow target's movement by " + degree.ToString() + "% for " + duration.ToString() + " seconds.\n";
    }

    private static string IncreasedCritDamageDescription(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (int)(((float)attribute.FindParameter("degree").value) * 100f);
        return "Additional " + degree.ToString() + "% damage on critical hit.\n";

    }

    private static string IncreasedCritChanceDescription(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (int)(((float)attribute.FindParameter("degree").value) * 100f);
        return "Additional " + degree.ToString() + "% chance to critically hit.\n";
    }
}
