using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class AbilityGenerator {
    public static Ability Generate(int level = 1) {
        var roll = RNG.Int(0, 300);
        if (roll < 100) return PassiveAbilityGenerator.Generate(level);
        else if (roll < 273) return AttackAbilityGenerator.Generate(level);
        else return UtilityAbilityGenerator.Generate(level);
    }
}