using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityTables {
    public static readonly Dictionary<Element, Dictionary<bool, string>> baseAttackNames;
    public static readonly Dictionary<Element, int> baseHitEffects;
    public static readonly Dictionary<Element, int> baseProjectiles;
    public static readonly Dictionary<Element, int> baseAoes;
    public static readonly Dictionary<Element, int> baseDamageZones;

    static AbilityTables() {
        baseAttackNames = new Dictionary<Element, Dictionary<bool, string>> {
            [Element.acid] = new Dictionary<bool, string> {
                [true] = "Acid Bolt",
                [false] = "Acid Strike"
            },
            [Element.fire] = new Dictionary<bool, string> {
                [true] = "Firebolt",
                [false] = "Cinderstrike"
            },
            [Element.ice] = new Dictionary<bool, string> {
                [true] = "Frostbolt",
                [false] = "Froststrike"
            },
            [Element.dark] = new Dictionary<bool, string> {
                [true] = "Voidbolt",
                [false] = "Voidstrike"
            },
            [Element.light] = new Dictionary<bool, string> {
                [true] = "Sunbolt",
                [false] = "Sunstrike"
            },
            [Element.piercing] = new Dictionary<bool, string> {
                [true] = "Arrow",
                [false] = "Stab"
            },
            [Element.bashing] = new Dictionary<bool, string> {
                [true] = "Stone",
                [false] = "Bash"
            },
            [Element.slashing] = new Dictionary<bool, string> {
                [true] = "Shuriken",
                [false] = "Slash"
            },
            [Element.none] = new Dictionary<bool, string> {
                [true] = "Nullbolt",
                [false] = "Nullstrike"
            }
        };
        baseHitEffects = new Dictionary<Element, int> {
            [Element.bashing] = 1,
            [Element.slashing] = 0,
            [Element.piercing] = 2,
            [Element.fire] = 3,
            [Element.ice] = 4,
            [Element.acid] = 5,
            [Element.light] = 6,
            [Element.dark] = 7,
            [Element.none] = 8
        };
        baseProjectiles = new Dictionary<Element, int> {
            [Element.bashing] = 3,
            [Element.slashing] = 4,
            [Element.piercing] = 0,
            [Element.fire] = 1,
            [Element.ice] = 5,
            [Element.acid] = 2,
            [Element.light] = 6,
            [Element.dark] = 7,
            [Element.none] = 8
        };
        baseAoes = new Dictionary<Element, int> {
            [Element.bashing] = 2,
            [Element.slashing] = 0,
            [Element.piercing] = 1,
            [Element.fire] = 3,
            [Element.ice] = 4,
            [Element.acid] = 5,
            [Element.light] = 6,
            [Element.dark] = 7,
            [Element.none] = 8
        };
        baseDamageZones = new Dictionary<Element, int> {
            [Element.bashing] = 0,
            [Element.slashing] = 1,
            [Element.piercing] = 2,
            [Element.fire] = 3,
            [Element.ice] = 4,
            [Element.acid] = 5,
            [Element.light] = 6,
            [Element.dark] = 7,
            [Element.none] = 8
        };
    }
}
