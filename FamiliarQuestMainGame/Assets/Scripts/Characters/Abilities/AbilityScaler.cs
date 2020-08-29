using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityScaler {
    public static AttackAbility ScaleAttackAbility(float points, Element element, BaseStat baseStat, float damageRatio, float dotDamageRatio, float dotTime, bool isRanged, float cooldown, float mp, float baseMp, float radius, int icon, int hitEffect, int projectile, int aoe, List<AbilityAttribute> abilityAttributes) {
        var startingPoints = points;
        List<AbilityAttribute> paralysis = new List<AbilityAttribute>();
        foreach (var attribute in abilityAttributes) if (attribute.type == "paralyze") paralysis.Add(attribute);
        if (paralysis.Count > 0 && cooldown == 0) foreach (var attribute in paralysis) abilityAttributes.Remove(attribute);
        var newAbility = new AttackAbility {
            element = element,
            baseStat = baseStat,
            dotTime = dotTime,
            isRanged = isRanged,
            cooldown = cooldown,
            mpUsage = mp,
            baseMpUsage = baseMp,
            radius = radius,
            points = points,
            icon = icon,
            hitEffect = hitEffect,
            rangedProjectile = projectile,
            aoe = aoe,
            level = AbilityCalculator.GetLevelFromPoints(startingPoints)
        };
        foreach (var attribute in abilityAttributes) {
            if (attribute.priority >= 50) {
                var pointCost = AbilityAttributeAppraiser.Appraise(newAbility, attribute);
                points -= pointCost;
                if (points >= 0) newAbility.attributes.Add(attribute);
                else points += pointCost;
            }
        }
        var totalDamage = AttackAbilityGenerator.CalculateDamage(points);
        var regularDamage = totalDamage * damageRatio / (damageRatio + dotDamageRatio);
        var dotDamage = totalDamage * dotDamageRatio / (damageRatio + dotDamageRatio);
        newAbility.damage = regularDamage;
        newAbility.dotDamage = dotDamage;
        newAbility.name = AbilityNamer.Name(newAbility);
        newAbility.description = AbilityDescriber.Describe(newAbility);
        newAbility.xp = GetXpFromLevel(newAbility.level);
        return newAbility;
    }

    public static UtilityAbility ScaleUtilityAbility(float points, float cooldown, float mp, float baseMp, string targetType, List<AbilityAttribute> abilityAttributes) {
        var startingPoints = points;
        var newAbility = new UtilityAbility {
            points = points,
            cooldown = cooldown,
            mpUsage = mp,
            baseMpUsage = baseMp,
            targetType = targetType,
            level = AbilityCalculator.GetLevelFromPoints(startingPoints)
        };
        foreach (var attribute in abilityAttributes) {
            points -= AbilityAttributeAppraiser.Appraise(newAbility, attribute);
            if (points >= 0) newAbility.attributes.Add(attribute);
        }
        newAbility.name = AbilityNamer.Name(newAbility);
        newAbility.description = AbilityDescriber.Describe(newAbility);
        newAbility.xp = GetXpFromLevel(newAbility.level);
        return newAbility;
    }

    private static long GetXpFromLevel(int level) {
        if (level == 1) return 0;
        return ExperienceGainer.xpTable[level - 2];
    }
}
