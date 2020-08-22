using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class UtilityAbilityGenerator {
    public static UtilityAbility Generate(int level = 1) {
        for (int i = 0; i < 10000; i++) {
            var startingPoints = AbilityCalculator.GetPointsFromLevel(level);
            var usesMpRoll = RNG.Int(0, 3);
            bool usesMp = false;
            if (usesMpRoll < 2) usesMp = true;
            int mp, baseMp;
            if (!usesMp) baseMp = 0;
            else {
                var mpResults = AbilityCalculator.GetBaseMpCostAndPointsMod();
                baseMp = mpResults.Item1;
                startingPoints *= mpResults.Item2;
            }
            mp = AbilityCalculator.ScaleMp(baseMp, level);
            var baseStat = RNG.EnumValue<BaseStat>();
            int numAttributes = RNG.Int(1, 5);
            var cooldownResults = AbilityCalculator.GetCooldownAndPointsMod();
            var cooldown = cooldownResults.Item1;
            startingPoints *= cooldownResults.Item2;
            var targetType = RNG.List(new List<string> {
                "player",
                "none",
                "point"
            });
            var ability = new UtilityAbility {
                baseStat = baseStat,
                cooldown = cooldown,
                mpUsage = mp,
                baseMpUsage = baseMp,
                points = startingPoints,
                level = level,
                targetType = targetType
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
