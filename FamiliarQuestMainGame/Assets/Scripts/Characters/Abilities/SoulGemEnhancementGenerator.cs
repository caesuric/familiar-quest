using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class SoulGemEnhancementGenerator {
    private delegate SoulGemEnhancement SoulGemEnhancementDelegate(Ability ability);
    private static readonly Dictionary<string, SoulGemEnhancementDelegate> attributes;
    private static readonly Dictionary<string, int> icons;
    private static readonly Dictionary<string, string> friendlyAttributeNames;

    static SoulGemEnhancementGenerator() {
        attributes = new Dictionary<string, SoulGemEnhancementDelegate> {
            ["reduceDotTime"] = GetReduceDotTime,
            ["reduceCooldown"] = GetReduceCooldown,
            ["reduceMpUsage"] = GetReduceMpUsage,
            ["increaseRadius"] = GetIncreasedAttackRadius,
            ["increaseDamage"] = GetIncreaseDamage,
            ["increaseDotDamage"] = GetIncreaseDotDamage,
            ["increasedDegree"] = GetIncreasedDegree,
            ["increasedDuration"] = GetIncreasedDuration,
            ["increasedRadius"] = GetIncreasedRadius,
            ["decreaseDelay"] = GetDecreaseDelay,
            ["addAttribute"] = GetAddAttribute,
            ["activeLatentAttribute"] = GetActivateAttribute,
            ["removeDrawback"] = GetRemoveDrawback
        };

        icons = new Dictionary<string, int> {
            ["lifeleech"] = 47,
            ["blunting"] = 19,
            ["damageShield"] = 19,
            ["inflictVulnerability"] = 48,
            ["paralyze"] = 49,
            ["restoreMP"] = 60,
            ["shield"] = 19,
            ["heal"] = 21,
            ["hot"] = 61,
            ["mpOverTime"] = 60,
            ["disableDevice"] = 62,
            ["stealth"] = 64,
            ["grapple"] = 66,
            ["speed-"] = 70,
            ["speed+"] = 5,
            ["paralyze"] = 20,
            ["damageEnemiesOnScreen"] = 68,
            ["experienceBoost"] = 69,
            ["knockback"] = 71,
            ["charge"] = 5,
            ["pullEnemies"] = 72,
            ["goldBoost"] = 73,
            ["boostDamage"] = 77,
            ["reduceDamage"] = 19,
            ["reduceElementalDamage"] = 19,
            ["chargeTowards"] = 5
        };
        friendlyAttributeNames = new Dictionary<string, string> {
            ["lifeleech"] = "lifeleech",
            ["blunting"] = "blunting",
            ["damageShield"] = "damage shield",
            ["inflictVulnerability"] = "inflict vulnerability",
            ["paralyze"] = "paralyze",
            ["restoreMP"] = "restore MP",
            ["shield"] = "shield",
            ["heal"] = "heal",
            ["hot"] = "heal over time",
            ["mpOverTime"] = "MP over time",
            ["disableDevice"] = "disable device",
            ["stealth"] = "stealth",
            ["grapple"] = "grapple",
            ["speed-"] = "speed down",
            ["speed+"] = "speed up",
            ["paralyze"] = "paralyze",
            ["damageEnemiesOnScreen"] = "burning aura",
            ["experienceBoost"] = "experience boost",
            ["knockback"] = "knockback",
            ["charge"] = "charge",
            ["pullEnemies"] = "pull enemies",
            ["goldBoost"] = "gold boost",
            ["boostDamage"] = "damage boost",
            ["reduceDamage"] = "reduce damage",
            ["reduceElementalDamage"] = "reduce elemental damage",
            ["boostStat"] = "boost stat",
            ["pullTowards"] = "pull towards",
            ["addedDot"] = "added damage over time",
            ["increasedCritChance"] = "increased critical hit chance",
            ["increasedCritDamage"] = "increased critical hit damage",
            ["offGCD"] = "off GCD",
            ["grapplingHook"] = "grappling hook",
            ["boostElementalDamage"] = "boost elemental damage",
            ["chargeTowards"] = "charge"
        };
    }

    public static List<SoulGemEnhancement> Generate(Ability ability) {
        for (int i = 0; i < 10000; i++) {
            string roll = RNG.DictionaryKey(attributes);
            var output = attributes[roll](ability);
            if (output != null) return new List<SoulGemEnhancement> { output };
        }
        return null;
    }

    private static SoulGemEnhancement GetReduceDotTime(Ability ability) {
        if (ability is AttackAbility attackAbility && attackAbility.dotTime > 0) {
            int newDotTime = (int)(attackAbility.dotTime * 0.8f);
            return new SoulGemEnhancement {
                name = "Reduce Damage Time",
                description = "Reduces damage over time effect duration by 20% while maintaining full damage",
                generalType = "base",
                type = "reduceDotTime",
                target = "dotTime",
                subTarget = "",
                effect = newDotTime - attackAbility.dotTime,
                icon = "reduceDotTime"
            };
        }
        else return null;
    }

    private static SoulGemEnhancement GetReduceCooldown(Ability ability) {
        if (ability is ActiveAbility activeAbility && activeAbility.cooldown > 0) {
            int newCooldown = (int)(activeAbility.cooldown * 0.8f);
            return new SoulGemEnhancement {
                name = "Reduce Cooldown",
                description = "Reduces cooldown duration by 20%",
                generalType = "base",
                type = "reduceCooldown",
                target = "cooldown",
                subTarget = "",
                effect = newCooldown - activeAbility.cooldown,
                icon = "reduceCooldown"
            };
        }
        else return null;
    }

    private static SoulGemEnhancement GetReduceMpUsage(Ability ability) {
        if (ability is ActiveAbility activeAbility && activeAbility.mpUsage > 0) {
            int newMpUsage = (int)(activeAbility.mpUsage * 0.8f);
            return new SoulGemEnhancement {
                name = "Reduce Mana Cost",
                description = "Reduces MP usage by 20%",
                generalType = "base",
                type = "reduceMpUsage",
                target = "mpUsage",
                subTarget = "",
                effect = newMpUsage - activeAbility.mpUsage,
                icon = "reduceMpUsage"
            };
        }
        else return null;
    }

    private static SoulGemEnhancement GetIncreasedAttackRadius(Ability ability) {
        if (ability is ActiveAbility activeAbility && activeAbility.radius > 0) {
            float newRadius = activeAbility.radius * 1.2f;
            return new SoulGemEnhancement {
                name = "Increase Radius",
                description = "Increases radius by 20%",
                generalType = "base",
                type = "increaseRadius",
                target = "radius",
                subTarget = "",
                effect = newRadius = activeAbility.radius,
                icon = "increaseRadius"
            };
        }
        else return null;
    }

    private static SoulGemEnhancement GetIncreaseDamage(Ability ability) {
        if (ability is AttackAbility attackAbility && attackAbility.damage > 0) {
            float newDamage = attackAbility.damage * 1.05f;
            return new SoulGemEnhancement {
                name = "Damage Up",
                description = "Increases damage by 5%",
                generalType = "base",
                type = "increaseDamage",
                target = "damage",
                subTarget = "",
                effect = newDamage - attackAbility.damage,
                icon = "increaseDamage"
            };
        }
        else return null;
    }

    private static SoulGemEnhancement GetIncreaseDotDamage(Ability ability) {
        if (ability is AttackAbility attackAbility && attackAbility.dotDamage > 0) {
            float newDamage = attackAbility.dotDamage * 1.05f;
            return new SoulGemEnhancement {
                name = "Damage Over Time Up",
                description = "Increases damage over time by 5%",
                generalType = "base",
                type = "increaseDotDamage",
                target = "dotDamage",
                subTarget = "",
                effect = newDamage - attackAbility.dotDamage,
                icon = "increaseDamage"
            };
        }
        else return null;
    }

    private static SoulGemEnhancement GetIncreasedDegree(Ability ability) {
        var validAttributeTypes = new Dictionary<string, string> {
            ["jumpBack"] = "Jump Back",
            ["knockback"] = "Knockback",
            ["lifeleech"] = "Lifeleech",
            ["mpOverTime"] = "MP Over Time",
            ["elementalDamageBuff"] = "Elemental Damage Buff",
            ["blunting"] = "Blunting",
            ["inflictVulnerability"] = "Inflict Vulnerability",
            ["damageShield"] = "Damage Shield",
            ["restoreMP"] = "MP Restoration",
            ["addedDot"] = "Added Damage Over Time",
            ["backstab"] = "Backstab",
            ["increasedCritChance"] = "Increased Crit Chance",
            ["increasedCritDamage"] = "Increased Crit Damage",
            ["speed-"] = "Speed Down",
            ["heal"] = "Healing",
            ["hot"] = "Healing Over Time",
            ["speed+"] = "Speed Up",
            ["damageEnemiesOnScreen"] = "Burning Aura",
            ["experienceBoost"] = "ExperienceBoost",
            ["goldBoost"] = "Gold Boost",
            ["boostStat"] = "Stat Boost",
            ["reduceElementalDamage"] = "Elemental Damage Reduction",
            ["boostElementalDamage"] = "Elemental Damage Boost"
        };
        var validAttributeInstances = new List<AbilityAttribute>();
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (validAttributeTypes.ContainsKey(attribute.type) && count < 4 && attribute.priority >= 50) validAttributeInstances.Add(attribute);
            count++;
        }
        if (validAttributeInstances.Count == 0) return null;
        var chosenAttribute = RNG.List(validAttributeInstances);
        var newDegree = ((float)chosenAttribute.FindParameter("degree").value) * 1.3f;
        return new SoulGemEnhancement {
            name = validAttributeTypes[chosenAttribute.type] + " Degree Up",
            description = "Increases " + validAttributeTypes[chosenAttribute.type] + " degree by 30%",
            generalType = "modifyAttribute",
            type = "increasedDegree",
            target = chosenAttribute.type,
            subTarget = "degree",
            effect = newDegree - (float)chosenAttribute.FindParameter("degree").value,
            icon = chosenAttribute.type + "Degree"
        };
    }

    private static SoulGemEnhancement GetIncreasedDuration(Ability ability) {
        var validAttributeTypes = new Dictionary<string, string> {
            ["paralyze"] = "Paralyze",
            ["elementalDamageBuff"] = "Elemental Damage Buff",
            ["inflictVulnerability"] = "Inflict Vulnerability",
            ["speed-"] = "Speed Down",
            ["speed+"] = "Speed Up"
        };
        var validAttributeInstances = new List<AbilityAttribute>();
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (validAttributeTypes.ContainsKey(attribute.type) && count < 4 && attribute.priority >= 50) validAttributeInstances.Add(attribute);
            count++;
        }
        if (validAttributeInstances.Count == 0) return null;
        var chosenAttribute = RNG.List(validAttributeInstances);
        var newDuration = ((float)chosenAttribute.FindParameter("duration").value) * 1.3f;
        return new SoulGemEnhancement {
            name = validAttributeTypes[chosenAttribute.type] + " Duration Up",
            description = "Increases " + validAttributeTypes[chosenAttribute.type] + " duration by 30%",
            generalType = "modifyAttribute",
            type = "increasedDuration",
            target = chosenAttribute.type,
            subTarget = "duration",
            effect = newDuration - (float)chosenAttribute.FindParameter("duration").value,
            icon = chosenAttribute.type + "Duration"
        };
    }

    private static SoulGemEnhancement GetIncreasedRadius(Ability ability) {
        var validAttributeTypes = new Dictionary<string, string> {
            ["paralyze"] = "Paralyze",
            ["disableDevice"] = "Disable Device",
            ["speed-"] = "Speed Down"
        };
        var validAttributeInstances = new List<AbilityAttribute>();
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (validAttributeTypes.ContainsKey(attribute.type) && attribute.FindParameter("radius") != null && count < 4 && attribute.priority >= 50) validAttributeInstances.Add(attribute);
            count++;
        }
        if (validAttributeInstances.Count == 0) return null;
        var chosenAttribute = RNG.List(validAttributeInstances);
        var newRadius = ((float)chosenAttribute.FindParameter("radius").value) * 1.3f;
        return new SoulGemEnhancement {
            name = validAttributeTypes[chosenAttribute.type] + " Radius Up",
            description = "Increases " + validAttributeTypes[chosenAttribute.type] + " radius by 30%",
            generalType = "modifyAttribute",
            type = "increasedRadius",
            target = chosenAttribute.type,
            subTarget = "radius",
            effect = newRadius - (float)chosenAttribute.FindParameter("radius").value,
            icon = "effectRadius"
        };
    }

    private static SoulGemEnhancement GetDecreaseDelay(Ability ability) {
        var delay = ability.FindAttribute("delay");
        if (delay == null || delay.priority < 50 || ability.attributes.IndexOf(delay) > 3) return null;
        var newDelay = ((float)delay.FindParameter("time").value) / 2f;
        return new SoulGemEnhancement {
            name = "Reduce Delay",
            description = "Decreases delay by 50%",
            generalType = "modifyAttribute",
            type = "decreasedTime",
            target = "delay",
            subTarget = "time",
            effect = newDelay - (float)delay.FindParameter("time").value,
            icon = "decreaseDelay"
        };
    }

    private static SoulGemEnhancement GetAddAttribute(Ability ability) {
        if (!ability.CanAddNewAttribute()) return null;
        var attribute = AbilityAttributeGenerator.Generate(ability);
        if (attribute == null || attribute.type == "delay" || attribute.type == "immobilizeSelf") return null;
        var icon = "addAttribute";
        if (icons.ContainsKey(attribute.type)) icon = icons[attribute.type].ToString();
        var friendlyName = attribute.type;
        if (friendlyAttributeNames.ContainsKey(friendlyName)) friendlyName = friendlyAttributeNames[friendlyName];
        return new SoulGemEnhancement {
            name = "Add " + Capitalize(friendlyName),
            description = "Adds " + friendlyName + " to ability",
            generalType = "addAttribute",
            type = "addAttribute",
            target = attribute.type,
            icon = icon
        };
    }

    private static SoulGemEnhancement GetActivateAttribute(Ability ability) {
        var validAttributeInstances = new List<AbilityAttribute>();
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if ((count > 3 || attribute.priority < 50) && attribute.type != "delay" && attribute.type != "immobilizeSelf") validAttributeInstances.Add(attribute);
            count++;
        }
        if (validAttributeInstances.Count == 0) return null;
        var chosenAttribute = RNG.List(validAttributeInstances);
        var icon = "addAttribute";
        if (icons.ContainsKey(chosenAttribute.type)) icon = icons[chosenAttribute.type].ToString();
        var friendlyName = chosenAttribute.type;
        if (friendlyAttributeNames.ContainsKey(friendlyName)) friendlyName = friendlyAttributeNames[friendlyName];
        return new SoulGemEnhancement {
            name = "Activate " + Capitalize(friendlyName),
            description = "Activates ability's " + friendlyName + " attribute",
            generalType = "activateAttribute",
            type = "activateAttribute",
            target = chosenAttribute.type,
            icon = icon,
            effect = RNG.Float(50, 100)
        };
    }

    private static SoulGemEnhancement GetRemoveDrawback(Ability ability) {
        var validAttributeTypes = new Dictionary<string, string> {
            ["delay"] = "Delay",
            ["immobilizeSelf"] = "Immobilize Self"
        };
        var validAttributeInstances = new List<AbilityAttribute>();
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (validAttributeTypes.ContainsKey(attribute.type) && count < 4 && attribute.priority >= 50) validAttributeInstances.Add(attribute);
            count++;
        }
        if (validAttributeInstances.Count == 0) return null;
        var chosenAttribute = RNG.List(validAttributeInstances);
        return new SoulGemEnhancement {
            name = "Remove " + validAttributeTypes[chosenAttribute.type],
            description = "Removes " + validAttributeTypes[chosenAttribute.type] + " from the ability.",
            generalType = "removeDrawback",
            type = "removeDrawback",
            target = chosenAttribute.type,
            icon = "removeDrawback"
        };
    }

    private static string Capitalize(string input) {
        TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
        return textInfo.ToTitleCase(input);
    }
}
