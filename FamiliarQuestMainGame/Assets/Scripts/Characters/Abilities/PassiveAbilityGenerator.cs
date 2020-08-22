using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class PassiveAbilityGenerator {
    public static PassiveAbility Generate(int level = 1) {
        for (int i = 0; i < 10000; i++) {
            var startingPoints = AbilityCalculator.GetPointsFromLevel(level);
            int numAttributes = RNG.Int(1, 5);
            var ability = new PassiveAbility {
                points = startingPoints,
                level = level
            };
            for (int j = 0; j < numAttributes; j++) {
                for (int k = 0; k < 10000; k++) {
                    var attribute = AbilityAttributeGenerator.Generate(ability);
                    if (attribute != null && attribute.points <= ability.points) {
                        ability.attributes.Add(attribute);
                        ability.points -= attribute.points;
                        break;
                    }
                }
            }
            ability.points = startingPoints;
            ability.SortAttributes();
            ability.icon = AbilityIconSelector.Select(ability);
            ability.name = AbilityNamer.Name(ability);
            ability.description = AbilityDescriber.Describe(ability);
            if (ability.IsValid()) return ability;
        }
        return null;
    }
}
