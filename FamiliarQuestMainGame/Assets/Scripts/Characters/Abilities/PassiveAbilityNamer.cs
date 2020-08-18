using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PassiveAbilityNamer {
    private delegate string NameDelegate(AbilityAttribute attribute);
    private static readonly Dictionary<string, string> simplePrefixesTable;
    private static readonly Dictionary<string, NameDelegate> prefixesTable;
    private static readonly Dictionary<string, string> simpleNamesTable;
    private static readonly Dictionary<string, NameDelegate> namesTable;

    static PassiveAbilityNamer() {
        simplePrefixesTable = new Dictionary<string, string> {

        };
        prefixesTable = new Dictionary<string, NameDelegate> {

        };
        simpleNamesTable = new Dictionary<string, string> {
            ["damageEnemiesOnScreen"] = "Burning Aura",
            ["experienceBoost"] = "Fast Learner",
            ["knockback"] = "Knockback",
            ["charge"] = "Charge",
            ["pullEnemies"] = "Grapple Gun",
            ["goldBoost"] = "Miser",
            ["boostDamage"] = "Damage Up",
            ["reduceDamage"] = "Protection",

        };
        namesTable = new Dictionary<string, NameDelegate> {
            ["reduceElementalDamage"] = ReduceElementalDamageName,
            ["boostElementalDamage"] = BoostElementalDamageName,
            ["boostStat"] = BoostStatName
        };
    }

    public static string Name(PassiveAbility ability) {
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

    private static string ReduceElementalDamageName(AbilityAttribute attribute) {
        return ((string)attribute.FindParameter("element").value)[0].ToString().ToUpper() + ((string)attribute.FindParameter("element").value).Substring(1) + " Ward";
    }

    private static string BoostElementalDamageName(AbilityAttribute attribute) {
        return ((string)attribute.FindParameter("element").value)[0].ToString().ToUpper() + ((string)attribute.FindParameter("element").value).Substring(1) + " Boost";
    }

    private static string BoostStatName(AbilityAttribute attribute) {
        var stat = attribute.FindParameter("stat").value as string;
        return stat[0].ToString().ToUpper() + stat.Substring(1) + " Up";
    }
}
