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
            if (element == Element.none) element = RNG.EnumValue<Element>();
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
            int numAttributes = RNG.Int(0, 6);
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
            int count = 0;
            for (int j = 0; j < numAttributes; j++) {
                for (int k = 0; k < 10000; k++) {
                    count = 0;
                    var attribute = AbilityAttributeGenerator.Generate(ability);
                    if (attribute != null && attribute.points <= ability.points) {
                        ability.attributes.Add(attribute);
                        if (attribute.priority >= 50 && count < 4) {
                            ability.points -= attribute.points;
                            count++;
                        }
                        if (attribute.priority >= 50 && count < 4 && attribute.type == "createDamageZone") ability.aoe = AbilityTables.baseDamageZones[element];
                        break;
                    }
                }
            }

            // sort the abilities added and make sure they are processed in the order they will always be processed on future level ups, so that attributes dependent on order for cost will resolve correctly
            ability.SortAttributes();
            ability.points = startingPoints;
            count = 0;
            foreach (var attribute in ability.attributes) {
                if (attribute.priority >= 50) {
                    var pointCost = AbilityAttributeAppraiser.Appraise(ability, attribute);
                    if (count < 4) ability.points -= pointCost;
                    if (ability.points <= 0) {
                        ability.attributes.Remove(attribute);
                        ability.points += pointCost;
                    }
                    count++;
                }
            }

            var damage = CalculateDamage(ability.points);
            if (isDot) ability.dotDamage = damage;
            else ability.damage = damage;
            ability.points = AbilityCalculator.GetPointsFromLevel(level);
            ability.SortAttributes();
            ability.icon = AbilityIconSelector.Select(ability);
            ability.name = AbilityNamer.Name(ability);
            ability.description = AbilityDescriber.Describe(ability);
            if (ability.IsValid()) {
                ability.skillTree = new AbilitySkillTree(ability);
                return ability;
            }
        }
        return null;
    }

    public static float CalculateDamage(float points) {
        return Mathf.Max(points / 70f, 0f);
    }

    private static Tuple<BaseStat, float> GetBaseStatAndPointsMod(bool isRanged, bool usesMp) {
        BaseStat baseStat;
        if (isRanged && usesMp) baseStat = BaseStat.intelligence;
        else if (isRanged) baseStat = BaseStat.dexterity;
        else baseStat = BaseStat.strength;
        return new Tuple<BaseStat, float>(baseStat, AbilityCalculator.pointsMultiplierByBaseStat[baseStat]);
    }

    private static Tuple<float, float> GetRadiusAndPointsMod() {
        var radiusRoll = RNG.Int(0, 300);
        float radius;
        if (radiusRoll < 270) radius = 0f;
        else if (radiusRoll < 291) radius = 2f;
        else if (radiusRoll < 297) radius = 4f;
        else if (radiusRoll < 299) radius = 6f;
        else radius = 8f;
        return new Tuple<float, float>(radius, AbilityCalculator.pointsMultiplierByRadius[radius]);
    }

    private static Tuple<bool, float, float> GetDotStatusAndTimeAndPointsMod() {
        var dotRoll = RNG.Int(0, 10000);
        bool isDot = !(dotRoll < 8500);
        float dotDuration;
        if (dotRoll < 8500) dotDuration = 0f;
        else if (dotRoll < 8875) dotDuration = 4f;
        else if (dotRoll < 9625) dotDuration = 8f;
        else dotDuration = 12f;
        return new Tuple<bool, float, float>(isDot, dotDuration, AbilityCalculator.pointsMultiplierByDotTime[dotDuration]);
    }
}
