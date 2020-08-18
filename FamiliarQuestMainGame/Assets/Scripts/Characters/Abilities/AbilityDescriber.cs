using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityDescriber {

    public static string Describe(UtilityAbility ability) {
        return UtilityAbilityDescriber.Describe(ability);
    }

    public static string Describe(AttackAbility ability) {
        return AttackAbilityDescriber.Describe(ability);
    }

    public static string Describe(PassiveAbility ability) {
        return PassiveAbilityDescriber.Describe(ability);
    }
}
