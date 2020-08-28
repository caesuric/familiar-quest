using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityDescriber {

    public static string Describe(Ability ability) {
        if (ability is AttackAbility attackAbility) return Describe(attackAbility);
        else if (ability is UtilityAbility utilityAbility) return Describe(utilityAbility);
        else if (ability is PassiveAbility passiveAbility) return Describe(passiveAbility);
        else return "";
    }

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
