using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityTables {
    public static readonly Dictionary<Element, Dictionary<bool, string>> baseAttackNames;

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
    }
}
