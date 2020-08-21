using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class AttackAbilityGenerator {
    public static AttackAbility Generate(int level = 1, Element element = Element.none) {
        for (int i = 0; i < 10000; i++) {
            var startingPoints = AbilityCalculator.GetPointsFromLevel(level);
            var isRanged = RNG.Bool();
            var usesMp = RNG.Bool();
            var statResults = GetBaseStatAndPointsMod(isRanged, usesMp);
            var baseStat = statResults.Item1;
            startingPoints *= statResults.Item2;
            int mp, baseMp;
            if (!usesMp) baseMp = 0;
            else {
                var mpResult = AbilityCalculator.GetBaseMpCostAndPointsMod();
                baseMp = mpResult.Item1;
                startingPoints *= mpResult.Item2;
            }
            mp = AbilityCalculator.ScaleMp(baseMp, level);
            if (element == Element.none) RNG.EnumValue<Element>();
            int hitEffect = 0, projectile = 0;
            if (isRanged) projectile = AbilityTables.baseProjectiles[element];
            else hitEffect = AbilityTables.baseHitEffects[element];
            var radiusResults = GetRadiusAndPointsMod();
            float radius = radiusResults.Item1;
            startingPoints *= radiusResults.Item2;
            int aoe = 0;
            if (radius > 0) aoe = AbilityTables.baseAoes[element];
            var dotResults = GetDotStatusAndTimeAndPointsMod();
            var isDot = dotResults.Item1;
            var dotTime = dotResults.Item2;
            startingPoints *= dotResults.Item3;
            int numAttributes = RNG.Int(0, 8);
            var cooldownResults = AbilityCalculator.GetCooldownAndPointsMod();
            var cooldown = cooldownResults.Item1;
            startingPoints *= cooldownResults.Item2;
            var ability = new AttackAbility {
                element = element,
                baseStat = baseStat,
                dotTime = dotTime,
                isRanged = isRanged,
                cooldown = cooldown,
                mpUsage = mp,
                baseMpUsage = baseMp,
                radius = radius,
                hitEffect = hitEffect,
                rangedProjectile = projectile,
                aoe = aoe,
                points = startingPoints,
                level = level,
                damage = 0,
                dotDamage = 0
            };
            for (int j = 0; j < numAttributes; j++) {
                for (int k = 0; k < 10000; k++) {
                    var attribute = AbilityAttributeGenerator.Generate(ability);
                    if (attribute != null && attribute.points <= ability.points) {
                        ability.attributes.Add(attribute);
                        ability.points -= attribute.points;
                        if (attribute.type == "createDamageZone") ability.aoe = AbilityTables.baseDamageZones[element];
                        break;
                    }
                }
            }
            var damage = CalculateDamage(ability.points);
            if (isDot) ability.dotDamage = damage;
            else ability.damage = damage;
            ability.points = startingPoints;
            ability.icon = AbilityIconSelector.Select(ability);
            ability.name = AbilityNamer.Name(ability);
            ability.description = AbilityDescriber.Describe(ability);
            if (ability.IsValid()) return ability;
        }
        return null;
    }

    public static float CalculateDamage(float points) {
        return 1f / 70f * points;
    }

    private static Tuple<BaseStat, float> GetBaseStatAndPointsMod(bool isRanged, bool usesMp) {
        if (isRanged && usesMp) return new Tuple<BaseStat, float>(BaseStat.intelligence, 1f);
        else if (isRanged) return new Tuple<BaseStat, float>(BaseStat.dexterity, 1f);
        else return new Tuple<BaseStat, float>(BaseStat.strength, 3f);
    }

    private static Tuple<float, float> GetRadiusAndPointsMod() {
        var radiusRoll = RNG.Int(0, 300);
        if (radiusRoll < 270) return new Tuple<float, float>(0f, 1f);
        else if (radiusRoll < 291) return new Tuple<float, float>(2f, 0.5f);
        else if (radiusRoll < 297) return new Tuple<float, float>(4f, 1f / 8f);
        else if (radiusRoll < 299) return new Tuple<float, float>(6f, 1f / 18f);
        else return new Tuple<float, float>(8f, 1f / 32f);
    }

    private static Tuple<bool, float, float> GetDotStatusAndTimeAndPointsMod() {
        var dotRoll = RNG.Int(0, 10000);
        if (dotRoll < 8500) return new Tuple<bool, float, float>(false, 0f, 1f);
        else if (dotRoll < 8875) return new Tuple<bool, float, float>(true, 4f, 1.25f);
        else if (dotRoll < 9625) return new Tuple<bool, float, float>(true, 8f, 2f);
        else return new Tuple<bool, float, float>(true, 12f, 2.5f);
    }
}
