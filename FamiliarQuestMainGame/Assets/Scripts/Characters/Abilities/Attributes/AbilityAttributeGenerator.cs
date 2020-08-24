using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class AbilityAttributeGenerator {
    public static AbilityAttribute Generate(Ability ability) {
        if (ability is AttackAbility attackAbility) return Generate(attackAbility);
        else if (ability is UtilityAbility utilityAbility) return Generate(utilityAbility);
        else if (ability is PassiveAbility passiveAbility) return Generate(passiveAbility);
        else return null;
    }

    public static AbilityAttribute Generate(AttackAbility ability) {
        return AttackAbilityAttributeGenerator.Generate(ability);
    }

    public static AbilityAttribute Generate(UtilityAbility ability) {
        return UtilityAbilityAttributeGenerator.Generate(ability);
    }

    public static AbilityAttribute Generate(PassiveAbility ability) {
        return PassiveAbilityAttributeGenerator.Generate(ability);
    }
}
