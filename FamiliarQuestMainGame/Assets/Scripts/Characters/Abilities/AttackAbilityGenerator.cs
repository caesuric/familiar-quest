using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class AttackAbilityGenerator {
    public static AttackAbility Generate(int level = 1) {
        for (int i = 0; i < 10000; i++) {
            var startingPoints = AbilityCalculator.GetPointsFromLevel(level);
            var isRanged = RNG.Bool();
            var usesMp = RNG.Bool();
            var statResults = GetBaseStatAndPointsMod(isRanged, usesMp);
            var baseStat = statResults.Item1;
            startingPoints *= statResults.Item2;
            int mp, baseMp;
            if (usesMp) baseMp = 0;
            else {
                var mpResult = GetBaseMpCostAndPointsMod();
                baseMp = mpResult.Item1;
                startingPoints *= mpResult.Item2;
            }
            mp = ScaleMp(baseMp, level);
            var element = RNG.EnumValue<Element>();
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
            var cooldownResults = GetCooldownAndPointsMod();
            var cooldown = cooldownResults.Item1;
            startingPoints *= cooldownResults.Item2;
            var attributes = new List<AbilityAttribute>();
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

    private static Tuple<int, float> GetBaseMpCostAndPointsMod() {
        var mpRoll = RNG.Int(0, 100);
        if (mpRoll < 80) return new Tuple<int, float>(40, 2f);
        else if (mpRoll < 90) return new Tuple<int, float>(20, 1.5f);
        else if (mpRoll < 95) return new Tuple<int, float>(60, 3f);
        else return new Tuple<int, float>(80, 4f);
    }

    private static int ScaleMp(int baseMp, int level) {
        float tempMp = baseMp;
        for (int i = 1; i < level; i++) tempMp *= 1.1f;
        return (int)tempMp;
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
        var dotRoll = RNG.Int(1, 10000);
        if (dotRoll < 8500) return new Tuple<bool, float, float>(false, 0f, 1f);
        else if (dotRoll < 8875) return new Tuple<bool, float, float>(true, 4f, 1.25f);
        else if (dotRoll < 9625) return new Tuple<bool, float, float>(true, 8f, 2f);
        else return new Tuple<bool, float, float>(true, 12f, 2.5f);
    }

    private static Tuple<float, float> GetCooldownAndPointsMod() {
        int hasCooldownRoll = RNG.Int(0, 100);
        if (hasCooldownRoll >= 35) return new Tuple<float, float>(0, 1f);
        var cooldownRoll = RNG.Int(0, 7);
        if (cooldownRoll == 0) return new Tuple<float, float>(1.5f, 1.3f);
        else if (cooldownRoll == 1) return new Tuple<float, float>(3f, 1.4f);
        else if (cooldownRoll == 2) return new Tuple<float, float>(8f, 1.5f);
        else if (cooldownRoll == 3) return new Tuple<float, float>(15f, 1.67f);
        else if (cooldownRoll == 4) return new Tuple<float, float>(30f, 2f);
        else if (cooldownRoll == 5) return new Tuple<float, float>(90f, 2.22f);
        else return new Tuple<float, float>(150f, 5f);
    }
}
