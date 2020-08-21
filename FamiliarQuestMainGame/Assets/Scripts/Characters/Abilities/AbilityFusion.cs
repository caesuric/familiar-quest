using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AbilityFusion {
    public static ActiveAbility Fuse(ActiveAbility ability1, ActiveAbility ability2) {
        var points = GetPoints(ability1, ability2);
        var isAttack = IsAttack(ability1, ability2);
        if (isAttack) return FuseAttack(points, ability1, ability2);
        else return FuseUtility(points, ability1, ability2);
    }

    private static float GetPoints(ActiveAbility ability1, ActiveAbility ability2) {
        var points1 = AbilityCalculator.GetPointsFromLevel(ability1.level);
        var points2 = AbilityCalculator.GetPointsFromLevel(ability2.level);
        var calcPoints = (points1 + points2) / 2;
        calcPoints *= Mathf.Pow(1.04f, 3f);
        var maxPoints = AbilityCalculator.GetPointsFromLevel(PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level);
        return Mathf.Min(calcPoints, maxPoints);
    }

    private static bool IsAttack(ActiveAbility ability1, ActiveAbility ability2) {
        return (ability1 is AttackAbility);
    }

    private static AttackAbility FuseAttack(float points, ActiveAbility ability1, ActiveAbility ability2) {
        var element = ((AttackAbility)ability1).element;
        var icon = ability1.icon;
        var hitEffect = ((AttackAbility)ability1).hitEffect;
        var projectile = ((AttackAbility)ability1).rangedProjectile;
        var aoe = ((AttackAbility)ability1).aoe;
        var baseStat = ability1.baseStat;
        var damageRatio = ((AttackAbility)ability1).damage;
        var dotDamageRatio = ((AttackAbility)ability1).dotDamage;
        var dotTime = ((AttackAbility)ability1).dotTime;
        var isRanged = ((AttackAbility)ability1).isRanged;
        var cooldown = ability1.cooldown;
        var mp = ability1.mpUsage;
        var baseMp = ability2.baseMpUsage;
        var radius = ((AttackAbility)ability1).radius;
        var attributes = GetCombinedAttributes(ability1, ability2);
        return AbilityScaler.ScaleAttackAbility(points, element, baseStat, damageRatio, dotDamageRatio, dotTime, isRanged, cooldown, mp, baseMp, radius, icon, hitEffect, projectile, aoe, attributes);
    }

    public static UtilityAbility FuseUtility(float points, ActiveAbility ability1, ActiveAbility ability2) {
        var cooldown = ability1.cooldown;
        var mp = ability1.mpUsage;
        var baseMp = ability1.baseMpUsage;
        var targetType = ((UtilityAbility)ability1).targetType;
        var attributes = GetCombinedAttributes(ability1, ability2);
        return AbilityScaler.ScaleUtilityAbility(points, cooldown, mp, baseMp, targetType, attributes);
    }

    private static List<AbilityAttribute> GetCombinedAttributes(Ability ability1, Ability ability2) {
        var output = new List<AbilityAttribute>();
        foreach (var attr in ability1.attributes) {
            var existingAttribute = GetExistingAttribute(output, attr);
            if (existingAttribute == null) output.Add(attr.Copy());
            else output[output.IndexOf(existingAttribute)] = SquashAttributes(existingAttribute, attr);
        }
        foreach (var attr in ability2.attributes) {
            var existingAttribute = GetExistingAttribute(output, attr);
            if (existingAttribute == null) output.Add(attr.Copy());
            else output[output.IndexOf(existingAttribute)] = SquashAttributes(existingAttribute, attr);
        }
        output.Sort((AbilityAttribute attr1, AbilityAttribute attr2) => { return attr1.priority.CompareTo(attr2.priority); });
        output.Reverse();
        return output;
    }

    private static AbilityAttribute GetExistingAttribute(List<AbilityAttribute> list, AbilityAttribute checkedAttribute) {
        foreach (var attr in list) if (attr.type == checkedAttribute.type) return attr;
        return null;
    }

    private static AbilityAttribute SquashAttributes(AbilityAttribute attr1, AbilityAttribute attr2) {
        var attr1Copy = attr1.Copy();
        var attr2Copy = attr2.Copy();
        attr1Copy.points = ((attr1Copy.points + attr2Copy.points) / 2) * Mathf.Pow(1.05f, 3);
        attr1Copy.priority = attr1Copy.points + attr2Copy.points;
        foreach (var param in attr1Copy.parameters) {
            var param2 = attr2Copy.FindParameter(param.name);
            if (param.value is float) param.value = ((float)param.value + (float)param2.value) / 2f;
            else if (param.value is int) param.value = ((int)param.value + (int)param2.value) / 2;
        }
        return attr1Copy;
    }
}
