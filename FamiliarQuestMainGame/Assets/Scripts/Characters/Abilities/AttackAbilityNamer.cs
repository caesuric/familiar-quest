using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AttackAbilityNamer {
    private delegate string NameDelegate(AbilityAttribute attribute);
    private static readonly Dictionary<string, string> simplePrefixesTable;
    private static readonly Dictionary<string, NameDelegate> prefixesTable;
    private static readonly Dictionary<string, string> simpleNamesTable;
    private static readonly Dictionary<string, NameDelegate> namesTable;
    private static readonly Dictionary<string, string> simpleSuffixesTable;
    private static readonly Dictionary<string, NameDelegate> suffixesTable;

    static AttackAbilityNamer() {
        simplePrefixesTable = new Dictionary<string, string> {
            ["jumpBack"] = "Retreating",
            ["chargeTowards"] = "Charging",
            ["knockback"] = "Pushing",
            ["offGCD"] = "Swift",
            ["paralyze"] = "Paralyzing",
            ["usableWhileParalyzed"] = "Cognitive",
            ["backstab"] = "Treacherous",
            ["stealthy"] = "Subtle",
            ["lifeleech"] = "Vampiric",
            ["mpOverTime"] = "Meditative",
            ["inflictVulnerability"] = "Tactical",
            ["delay"] = "Delayed",
            ["damageShield"] = "Aegis",
            ["removeDebuff"] = "Cleansing",
            ["addedDot"] = "Draining",
            ["increasedCritChance"] = "Sharp"
        };
        prefixesTable = new Dictionary<string, NameDelegate> {
            
        };
        simpleNamesTable = new Dictionary<string, string> {
            
        };
        namesTable = new Dictionary<string, NameDelegate> {

        };
        simpleSuffixesTable = new Dictionary<string, string> {
            ["createDamageZone"] = "of Danger",
            ["projectileSpread"] = "Fusillade",
            ["pullTowards"] = "of Grappling",
            ["blunting"] = "of Blunting",
            ["restoreMP"] = "of Concentration",
            ["increasedCritDamage"] = "of Piercing",
            ["speed-"] = "of Slowing",
            ["immobilizeSelf"] = "of the Turret"
        };
        suffixesTable = new Dictionary<string, NameDelegate> {
            ["elementalDamageBuff"] = ElementalDamageBuffSuffix
        };
    }

    public static string Name(AttackAbility ability) {
        var prefixes = new List<string>();
        var baseName = AbilityTables.baseAttackNames[ability.element][ability.isRanged];
        var suffixes = new List<string>();
        if (ability.mpUsage > 0) prefixes.Add("Arcane");
        if (ability.radius > 0) prefixes.Add("Exploding");
        if (ability.dotDamage > 0) prefixes.Add("Draining");
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) break;
            if (simplePrefixesTable.ContainsKey(attribute.type)) prefixes.Add(simplePrefixesTable[attribute.type]);
            else if (prefixesTable.ContainsKey(attribute.type)) prefixes.Add(prefixesTable[attribute.type](attribute));
            else if (simpleNamesTable.ContainsKey(attribute.type)) baseName = simpleNamesTable[attribute.type];
            else if (namesTable.ContainsKey(attribute.type)) baseName = namesTable[attribute.type](attribute);
            else if (simpleSuffixesTable.ContainsKey(attribute.type)) suffixes.Add(simpleSuffixesTable[attribute.type]);
            else if (suffixesTable.ContainsKey(attribute.type)) suffixes.Add(suffixesTable[attribute.type](attribute));
            else baseName = "NAME NOT FOUND";
            count++;
        }
        for (int i=0; i<2 && i<prefixes.Count; i++) {
            var prefix = prefixes[i];
            if (prefix == "Draining" && baseName.Contains("Draining")) continue;
            baseName = prefix + " " + baseName;
        }
        if (suffixes.Count > 0) baseName += " " + suffixes[0];
        return baseName;
    }

    private static string ElementalDamageBuffSuffix(AbilityAttribute attribute) {
        var type = attribute.FindParameter("element").value as string;
        var suffixes = new Dictionary<string, string> {
            ["bashing"] = "of Achilles",
            ["piercing"] = "of Achilles",
            ["slashing"] = "of Achilles",
            ["fire"] = "of the Inferno",
            ["ice"] = "of Frostbite",
            ["acid"] = "of Solvency",
            ["light"] = "of Sunburn",
            ["dark"] = "of the Voidgaze",
            ["none"] = "of Nullity"
        };
        if (suffixes.ContainsKey(type)) return suffixes[type];
        else return "of ATTRIBUTE NAME NOT FOUND";
    }
}
