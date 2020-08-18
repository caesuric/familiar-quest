using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityNamer {
    public static string Name(UtilityAbility ability) {
        return UtilityAbilityNamer.Name(ability);
    }

    public static string Name(AttackAbility ability) {
        return AttackAbilityNamer.Name(ability);
    }

    public static string Name(PassiveAbility ability) {
        return PassiveAbilityNamer.Name(ability);
    }
}
