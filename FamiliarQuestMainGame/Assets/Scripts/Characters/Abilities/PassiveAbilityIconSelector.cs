using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PassiveAbilityIconSelector {
    private delegate int IconDelegate(AbilityAttribute attribute);
    private static readonly Dictionary<string, int> simpleIconTable;
    private static readonly Dictionary<string, IconDelegate> iconTable;
    
    static PassiveAbilityIconSelector() {
        simpleIconTable = new Dictionary<string, int> {
            ["damageEnemiesOnScreen"] = 68,
            ["experienceBoost"] = 69,
            ["knockback"] = 71,
            ["charge"] = 5,
            ["pullEnemies"] = 72,
            ["goldBoost"] = 73,
            ["boostDamage"] = 77,
            ["reduceDamage"] = 19,
            ["reduceElementalDamage"] = 19,
        };
        iconTable = new Dictionary<string, IconDelegate> {
            ["boostElementalDamage"] = BoostElementalDamageIcon,
            ["boostStat"] = BoostStatIcon
        };
    }

    public static int Select(PassiveAbility ability) {
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) break;
            if (simpleIconTable.ContainsKey(attribute.type)) return simpleIconTable[attribute.type];
            else if (iconTable.ContainsKey(attribute.type)) return iconTable[attribute.type](attribute);
            count++;
        }
        return 0;
    }

    private static int BoostElementalDamageIcon(AbilityAttribute attribute) {
        var element = attribute.FindParameter("element").value as string;
        var elementTable = new Dictionary<string, int> {
            ["piercing"] = 26,
            ["slashing"] = 25,
            ["bashing"] = 24,
            ["fire"] = 28,
            ["ice"] = 29,
            ["acid"] = 27,
            ["light"] = 30,
            ["dark"] = 31
        };
        if (elementTable.ContainsKey(element)) return elementTable[element];
        else return 0;
    }

    private static int BoostStatIcon(AbilityAttribute attribute) {
        var stat = attribute.FindParameter("stat").value as string;
        var statTable = new Dictionary<string, int> {
            ["strength"] = 74,
            ["dexterity"] = 5,
            ["constitution"] = 48,
            ["intelligence"] = 55,
            ["wisdom"] = 75,
            ["luck"] = 76
        };
        if (statTable.ContainsKey(stat)) return statTable[stat];
        else return 0;
    }
}
