using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class AbilityScaler {
    public static AttackAbility ScaleAttackAbility(float points, Element element, BaseStat baseStat, float damageRatio, float dotDamageRatio, float dotTime, bool isRanged, float cooldown, float mp, float baseMp, float radius, int icon, int hitEffect, int projectile, int aoe, List<AbilityAttribute> abilityAttributes, AbilitySkillTree skillTree) {
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
            level = AbilityCalculator.GetLevelFromPoints(startingPoints),
            skillTree = skillTree
        };
        ModifyAttackAbilityPointsForQualities(newAbility);
        points = newAbility.points;
        int count = 0;
        foreach (var attribute in abilityAttributes) {
            if (attribute.priority >= 50) {
                var pointCost = AbilityAttributeAppraiser.Appraise(newAbility, attribute);
                if (count < 4) points -= pointCost;
                if (count < 4 && points >= 0) {
                    newAbility.attributes.Add(attribute);
                    newAbility.points -= pointCost;
                }
                else if (count < 4 && points < 0) points += pointCost;
                count++;
            }
            else newAbility.attributes.Add(attribute);
        }
        var totalDamage = AttackAbilityGenerator.CalculateDamage(points);
        var regularDamage = totalDamage * damageRatio / (damageRatio + dotDamageRatio);
        var dotDamage = totalDamage * dotDamageRatio / (damageRatio + dotDamageRatio);
        newAbility.damage = regularDamage;
        newAbility.dotDamage = dotDamage;
        newAbility.name = AbilityNamer.Name(newAbility);
        newAbility.description = AbilityDescriber.Describe(newAbility);
        newAbility.xp = GetXpFromLevel(newAbility.level);
        SetMpUsage(newAbility);
        return newAbility;
    }

    private static void ModifyAttackAbilityPointsForQualities(AttackAbility ability) {
        ability.points *= AbilityCalculator.pointsMultiplierByBaseStat[ability.baseStat];
        ability.points *= AbilityCalculator.pointsMultiplierByRadius[ability.radius];
        ability.points *= AbilityCalculator.pointsMultiplierByDotTime[ability.dotTime];
        ability.points *= AbilityCalculator.pointsMultiplierByCooldown[ability.cooldown];
        ability.points *= AbilityCalculator.pointsMultiplierByMpUsage[(int)ability.baseMpUsage];
    }

    private static void ModifyUtilityAbilityPointsForQualities(UtilityAbility ability) {
        ability.points *= AbilityCalculator.pointsMultiplierByCooldown[ability.cooldown];
        ability.points *= AbilityCalculator.pointsMultiplierByMpUsage[(int)ability.baseMpUsage];
    }

    public static UtilityAbility ScaleUtilityAbility(float points, float cooldown, float mp, float baseMp, string targetType, List<AbilityAttribute> abilityAttributes, AbilitySkillTree skillTree) {
        var startingPoints = points;
        var newAbility = new UtilityAbility {
            points = points,
            cooldown = cooldown,
            mpUsage = mp,
            baseMpUsage = baseMp,
            targetType = targetType,
            level = AbilityCalculator.GetLevelFromPoints(startingPoints),
            skillTree = skillTree
        };
        ModifyUtilityAbilityPointsForQualities(newAbility);
        foreach (var attribute in abilityAttributes) {
            var pointCost = AbilityAttributeAppraiser.Appraise(newAbility, attribute);
            points -= pointCost;
            if (points >= 0) {
                newAbility.attributes.Add(attribute);
                newAbility.points -= pointCost;
            }
            else points += pointCost;
        }
        newAbility.name = AbilityNamer.Name(newAbility);
        newAbility.description = AbilityDescriber.Describe(newAbility);
        newAbility.xp = GetXpFromLevel(newAbility.level);
        SetMpUsage(newAbility);
        return newAbility;
    }

    private static long GetXpFromLevel(int level) {
        if (level == 1) return 0;
        return ExperienceGainer.xpTable[level - 2];
    }

    private static void SetMpUsage(ActiveAbility ability) {
        var mpUsage = ability.baseMpUsage;
        for (int i = 1; i < ability.level; i++) mpUsage *= 1.05f;
        ability.mpUsage = mpUsage;
    }

    public static void RemoveSkillTreeEnhancements(ActiveAbility ability) {
        foreach (var layer in ability.skillTree.nodesByLayer) {
            foreach (var node in layer) {
                if (node.active) node.Remove();
            }
        }
        
    }

    public static void AddSkillTreeEnhancements(ActiveAbility ability) {
        foreach (var layer in ability.skillTree.nodesByLayer) {
            foreach (var node in layer) {
                if (node.active) node.Activate();
            }
        }
    }
}
