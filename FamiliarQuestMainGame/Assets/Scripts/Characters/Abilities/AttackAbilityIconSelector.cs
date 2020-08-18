using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AttackAbilityIconSelector {
    private delegate int IconDelegate(AbilityAttribute attribute);
    private static readonly Dictionary<string, int> simpleIconTable;
    private static readonly Dictionary<Element, int> basicMeleeIcons;
    private static readonly Dictionary<Element, int> basicRangedIcons;
    private static readonly Dictionary<Element, int> basicAoeIcons;

    static AttackAbilityIconSelector() {
        simpleIconTable = new Dictionary<string, int> {
            ["lifeleech"] = 47,
            ["blunting"] = 19,
            ["damageShield"] = 19,
            ["inflictVulnerability"] = 48,
            ["paralyze"] = 49
        };
        basicMeleeIcons = new Dictionary<Element, int>() {
            [Element.bashing] = 24,
            [Element.slashing] = 25,
            [Element.piercing] = 26,
            [Element.acid] = 27,
            [Element.fire] = 28,
            [Element.ice] = 29,
            [Element.light] = 30,
            [Element.dark] = 31,
            [Element.none] = 37
        };
        basicRangedIcons = new Dictionary<Element, int>() {
            [Element.bashing] = 32,
            [Element.slashing] = 33,
            [Element.piercing] = 3,
            [Element.acid] = 12,
            [Element.fire] = 9,
            [Element.ice] = 34,
            [Element.light] = 35,
            [Element.dark] = 36,
            [Element.none] = 38
        };
        basicAoeIcons = new Dictionary<Element, int>() {
            [Element.bashing] = 40,
            [Element.slashing] = 41,
            [Element.piercing] = 42,
            [Element.acid] = 43,
            [Element.fire] = 10,
            [Element.ice] = 44,
            [Element.light] = 45,
            [Element.dark] = 46,
            [Element.none] = 39
        };
    }

    public static int Select(AttackAbility ability) {
        int count = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.priority < 50 || count > 3) break;
            if (simpleIconTable.ContainsKey(attribute.type)) return simpleIconTable[attribute.type];
            count++;
        }
        if (ability.radius > 0) return basicAoeIcons[ability.element];
        else if (ability.isRanged) return basicRangedIcons[ability.element];
        else return basicMeleeIcons[ability.element];
    }
}
