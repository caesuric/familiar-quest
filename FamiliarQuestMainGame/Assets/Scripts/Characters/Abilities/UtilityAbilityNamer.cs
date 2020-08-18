using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UtilityAbilityNamer {
    private delegate string NameDelegate(AbilityAttribute attribute);
    private static readonly Dictionary<string, string> simplePrefixesTable;
    private static readonly Dictionary<string, NameDelegate> prefixesTable;
    private static readonly Dictionary<string, string> simpleNamesTable;
    private static readonly Dictionary<string, NameDelegate> namesTable;

    static UtilityAbilityNamer() {
        simplePrefixesTable = new Dictionary<string, string> {
            ["stealthy"] = "Subtle",
            ["usableWhileParalyzed"] = "Cognitive",
            ["offGCD"] = "Swift",
            ["immobilizeSelf"] = "Turret"

        };
        prefixesTable = new Dictionary<string, NameDelegate> {

        };
        simpleNamesTable = new Dictionary<string, string> {
            ["restoreMP"] = "Concentration",
            ["shield"] = "Shield",
            ["heal"] = "Healing",
            ["hot"] = "Regeneration",
            ["mpOverTime"] = "Meditation",
            ["disableDevice"] = "Disable Device",
            ["stealth"] = "Stealth",
            ["grapple"] = "Grappling Hook",
            ["speed-"] = "Slow",
            ["paralyze"] = "Paralyze",
            ["removeDebuff"] = "Cleanse",
            ["removeAllDebuffs"] = "Greater Cleanse",
            ["eatDebuff"] = "Consume Ailment",
            ["speed+"] = "Swiftness"
        };
        namesTable = new Dictionary<string, NameDelegate> {
            ["elementalDamageBuff"] = ElementalDamageBuffName
        };
    }

    public static string Name(UtilityAbility ability) {
        var prefix = "";
        var baseName = "";
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) break;
            if (simplePrefixesTable.ContainsKey(attribute.type)) prefix = simplePrefixesTable[attribute.type];
            else if (prefixesTable.ContainsKey(attribute.type)) prefix = prefixesTable[attribute.type](attribute);
            else if (simpleNamesTable.ContainsKey(attribute.type)) baseName = simpleNamesTable[attribute.type];
            else if (namesTable.ContainsKey(attribute.type)) baseName = namesTable[attribute.type](attribute);
            else baseName = "NAME NOT FOUND";
            count++;
        }
        if (prefix == "") return baseName;
        else return prefix + " " + baseName;
    }

    private static string ElementalDamageBuffName(AbilityAttribute attribute) {
        var element = attribute.FindParameter("element").value as string;
        return element[0].ToString().ToUpper() + element.Substring(1) + " " + "Boost";
    }
}
