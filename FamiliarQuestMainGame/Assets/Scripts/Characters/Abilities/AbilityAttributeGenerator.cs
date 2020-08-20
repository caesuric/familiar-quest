using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class AbilityAttributeGenerator {
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
