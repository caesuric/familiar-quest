using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UtilityAbilityIconSelector {
    private delegate int IconDelegate(AbilityAttribute attribute);
    private static readonly Dictionary<string, int> simpleIconTable;
    private static readonly Dictionary<string, IconDelegate> iconTable;
    
    static UtilityAbilityIconSelector() {
        simpleIconTable = new Dictionary<string, int> {
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
            ["paralyze"] = 20
            
        };
        iconTable = new Dictionary<string, IconDelegate> {
            ["elementalDamageBuff"] = ElementalDamageBuffIcon
        };
    }

    public static int Select(UtilityAbility ability) {
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) break;
            if (simpleIconTable.ContainsKey(attribute.type)) return simpleIconTable[attribute.type];
            else if (iconTable.ContainsKey(attribute.type)) return iconTable[attribute.type](attribute);
            count++;
        }
        return 0;
    }

    private static int ElementalDamageBuffIcon(AbilityAttribute attribute) {
        var element = attribute.FindParameter("element").value as string;
        var elementTable = new Dictionary<string, int> {
            ["slashing"] = 33,
            ["piercing"] = 3,
            ["bashing"] = 32,
            ["fire"] = 9,
            ["ice"] = 34,
            ["acid"] = 12,
            ["light"] = 35,
            ["dark"] = 36
        };
        if (elementTable.ContainsKey(element)) return elementTable[element];
        else return 0;
    }
}
