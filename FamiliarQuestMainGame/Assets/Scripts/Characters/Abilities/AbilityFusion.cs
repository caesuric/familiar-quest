using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AbilityFusion {
    public static ActiveAbility Fuse(ActiveAbility ability1, ActiveAbility ability2) {
        float calcPoints = (ability1.points + ability2.points) / 2;
        calcPoints *= Mathf.Pow(1.05f, 3);
        float maxPoints = 70;
        for (int i = 1; i < PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level; i++) maxPoints = maxPoints * 1.05f;
        if (calcPoints > maxPoints) calcPoints = maxPoints;
        int points = (int)calcPoints;
        bool isAttack = false;
        if (AbilityMenu.instance.fusionAbilityTypeChoice == 0 && ability1 is AttackAbility) isAttack = true;
        else if (AbilityMenu.instance.fusionAbilityTypeChoice == 1 && ability2 is AttackAbility) isAttack = true;
        if (isAttack) return FuseAttack(points, ability1, ability2);
        else return FuseUtility(points, ability1, ability2);
    }

    public static AttackAbility FuseAttack(int points, ActiveAbility ability1, ActiveAbility ability2) {
        Element element;
        int icon, hitEffect, projectile, aoe;
        element = ((AttackAbility)ability1).element;
        icon = ((AttackAbility)ability1).icon;
        hitEffect = ((AttackAbility)ability1).hitEffect;
        projectile = ((AttackAbility)ability1).rangedProjectile;
        aoe = GetAoe(element);
        BaseStat baseStat;
        baseStat = ability1.baseStat;
        float damageRatio;
        float dotDamageRatio;
        float dotTime;
        damageRatio = ((AttackAbility)ability1).damage;
        dotDamageRatio = ((AttackAbility)ability1).dotDamage;
        dotTime = ((AttackAbility)ability1).dotTime;
        bool isRanged = ((AttackAbility)ability1).isRanged;
        float cooldown;
        cooldown = ability1.cooldown;
        int mp, baseMp;
        mp = ability1.mpUsage;
        baseMp = ability1.baseMpUsage;
        float radius;
        radius = ((AttackAbility)ability1).radius;
        var attributes = GetCombinedAttributes(ability1, ability2);
        return CreateNewAttackAbilityForFusion(points, element, baseStat, damageRatio, dotDamageRatio, dotTime, isRanged, cooldown, mp, baseMp, radius, icon, hitEffect, projectile, aoe, attributes);
    }

    private static AttackAbility CreateNewAttackAbilityForFusion(int points, Element element, BaseStat baseStat, float damageRatio, float dotDamageRatio, float dotTime, bool isRanged, float cooldown, int mp, int baseMp, float radius, int icon, int hitEffect, int projectile, int aoe, List<AbilityAttribute> abilityAttributes) {
        var startingPoints = points;
        List<AbilityAttribute> paralysis = new List<AbilityAttribute>();
        foreach (var attribute in abilityAttributes) if (attribute.type == "paralyze") paralysis.Add(attribute);
        if (paralysis.Count > 0 && cooldown == 0) foreach (var attribute in paralysis) abilityAttributes.Remove(attribute);
        points = CalculateAttackAbilityPoints(points, damageRatio, dotDamageRatio, dotTime, isRanged, cooldown, mp, baseMp, radius, abilityAttributes);
        var totalDamage = AttackAbility.CalculateDamage(points);
        var regularDamage = totalDamage * damageRatio / (damageRatio + dotDamageRatio);
        var dotDamage = totalDamage * dotDamageRatio / (damageRatio + dotDamageRatio);
        var newAbility = new AttackAbility("", "", regularDamage, element, baseStat, dotDamage: dotDamage, dotTime: dotTime, isRanged: isRanged, cooldown: cooldown, mpUsage: ActiveAbility.CalculateMpUsage(baseMp, points), baseMpUsage: baseMp, radius: radius, icon: icon, hitEffect: hitEffect, rangedProjectile: projectile, aoe: aoe, attributes: abilityAttributes.ToArray()) {
            points = startingPoints
        };
        newAbility.name = AbilityNamer.Name(newAbility);
        newAbility.description = AbilityDescriber.Describe(newAbility);
        return newAbility;
    }

    private static int CalculateAttackAbilityPoints(int points, float damageRatio, float dotDamageRatio, float dotTime, bool isRanged, float cooldown, int mp, int baseMp, float radius, List<AbilityAttribute> abilityAttributes) {
        var normalDamagePercentage = damageRatio / (damageRatio + dotDamageRatio);
        var dotPointMultiplier = 1f;
        if (dotTime == 4) dotPointMultiplier = 1.5f;
        else if (dotTime == 8) dotPointMultiplier = 3;
        else if (dotTime == 12) dotPointMultiplier = 4;
        var nonDotPoints = points * normalDamagePercentage;
        var dotPoints = points - nonDotPoints;
        dotPoints *= dotPointMultiplier;
        points = (int)(nonDotPoints + dotPoints);
        if (!isRanged) points = (int)(points * 1.5f);
        if (cooldown == 1.5f) points = (int)(points * 1.2f);
        else if (cooldown == 3) points = (int)(points * 1.3f);
        else if (cooldown == 8) points = (int)(points * 1.4f);
        else if (cooldown == 15) points = (int)(points * 1.5f);
        else if (cooldown == 30) points = (int)(points * 2f);
        else if (cooldown == 90) points = (int)(points * 6f);
        else if (cooldown == 150) points = (int)(points * 10f);
        if (baseMp == 0) baseMp = mp;
        if (baseMp == 40) points = (int)(points * 1.25f);
        else if (baseMp == 60) points = (int)(points * 1.5f);
        else if (baseMp == 80) points = (int)(points * 1.75f);
        else if (baseMp == 20) points = (int)(points * 1.125f);
        if (radius != 0) points = points / (int)(radius * radius);
        foreach (var attribute in abilityAttributes) {
            if (attribute.priority < 50) continue;
            switch (attribute.type) {
                case "createDamageZone":
                    break;
                case "projectileSpread":
                    points = (int)(points * 0.3f);
                    break;
                case "jumpBack":
                    points = (int)(points * 0.75f);
                    break;
                case "chargeTowards":
                    points = (int)(points * 0.75f);
                    break;
                case "pullTowards":
                    points = (int)(points * 0.75f);
                    break;
                case "knockback":
                    points = (int)(points * 0.75f);
                    break;
                case "offGCD":
                    points = (int)(points * 0.5f);
                    break;
                case "paralyze":
                    points = (int)(points * 0.5f);
                    break;
                case "usableWhileParalyzed":
                    points = (int)(points * 0.85f);
                    break;
                case "lifeleech":
                    points = (int)(points * 0.5f);
                    break;
                case "mpOverTime":
                    points = (int)(points * 0.8f);
                    break;
                case "elementalDamageBuff":
                    switch (((int)(attribute.FindParameter("degree").floatVal)).ToString()) {
                        case "100":
                            points = (int)(points * 0.94f);
                            break;
                        case "50":
                            points = (int)(points * 0.97f);
                            break;
                        case "25":
                            points = (int)(points * 0.99f);
                            break;
                    }
                    break;
                case "blunting":
                    points = (int)(points * 0.5f);
                    break;
                case "inflictVulnerability":
                    switch (((int)(attribute.FindParameter("degree").floatVal)).ToString()) {
                        case "100":
                            points = (int)(points * 0.25f);
                            break;
                        case "50":
                            points = (int)(points * 0.5f);
                            break;
                        case "25":
                            points = (int)(points * 0.75f);
                            break;
                    }
                    break;
                case "delay":
                    points = (int)(points * 1.5f);
                    break;
                case "damageShield":
                    points = (int)(points * 0.5f);
                    break;
                case "restoreMP":
                    points = (int)(points * 0.8f);
                    break;
                case "removeDebuff":
                    points = (int)(points * 0.85f);
                    break;
                case "addedDot":
                    var dotDamage = attribute.FindParameter("degree").floatVal;
                    var dotTimeSub = attribute.FindParameter("duration").floatVal;
                    var pointCost = dotDamage * 70;
                    if (dotTimeSub == 4) pointCost /= 1.5f;
                    if (dotTimeSub == 8) pointCost /= 3;
                    else pointCost /= 4;
                    points -= (int)pointCost;
                    break;
                case "backstab":
                    points = (int)(points * 0.75f);
                    break;
                case "stealthy":
                    points = (int)(points * 0.9f);
                    break;
            }
        }
        return points;
    }

    public static UtilityAbility FuseUtility(int points, ActiveAbility ability1, ActiveAbility ability2) {
        float cooldown;
        cooldown = ability1.cooldown;
        int mp = ability1.mpUsage;
        int baseMp = ability1.baseMpUsage;
        var attributes = GetCombinedAttributes(ability1, ability2);
        return CreateNewUtilityAbilityForFusion(points, cooldown, mp, baseMp, attributes);
    }

    private static UtilityAbility CreateNewUtilityAbilityForFusion(int points, float cooldown, int mp, int baseMp, List<AbilityAttribute> abilityAttributes) {
        var startingPoints = points;
        points = CalculateUtilityAbilityPoints(points, mp, baseMp, cooldown, abilityAttributes);
        var newAbility = new UtilityAbility("", "", cooldown: cooldown, mpUsage: ActiveAbility.CalculateMpUsage(baseMp, points), baseMpUsage: baseMp, attributes: abilityAttributes.ToArray()) {
            points = startingPoints
        };
        newAbility.name = AbilityNamer.Name(newAbility);
        newAbility.description = AbilityDescriber.Describe(newAbility);
        newAbility.icon = AbilityIconGenerator.Retrieve(newAbility);
        return newAbility;
    }

    private static int CalculateUtilityAbilityPoints(int points, int mp, int baseMp, float cooldown, List<AbilityAttribute> abilityAttributes) {
        if (cooldown == 1.5f) points = (int)(points * 1.2f);
        else if (cooldown == 3) points = (int)(points * 1.3f);
        else if (cooldown == 8) points = (int)(points * 1.4f);
        else if (cooldown == 15) points = (int)(points * 1.5f);
        else if (cooldown == 30) points = (int)(points * 2f);
        else if (cooldown == 90) points = (int)(points * 6f);
        else if (cooldown == 150) points = (int)(points * 10f);
        if (baseMp == 0) baseMp = mp;
        if (baseMp == 40) points = (int)(points * 1.25f);
        else if (baseMp == 60) points = (int)(points * 1.5f);
        else if (baseMp == 80) points = (int)(points * 1.75f);
        else if (baseMp == 20) points = (int)(points * 1.125f);
        foreach (var attribute in abilityAttributes) {
            if (attribute.priority < 50) continue;
            switch (attribute.type) {
                case "offGCD":
                    points = (int)(points * 0.5f);
                    break;
                case "usableWhileParalyzed":
                    points = (int)(points * 0.85f);
                    break;
                case "elementalDamageBuff":
                    switch (((int)(attribute.FindParameter("degree").floatVal)).ToString()) {
                        case "100":
                            points -= 140;
                            break;
                        case "50":
                            points -= 70;
                            break;
                        case "25":
                            points -= 35;
                            break;
                    }
                    break;
                case "disableDevice":
                    points -= 73;
                    break;
                case "stealth":
                    points -= 73;
                    break;
                case "stealthy":
                    points = (int)(points * 0.9f);
                    break;
                case "grapplingHook":
                    points -= 119;
                    break;
            }
        }
        foreach (var attribute in abilityAttributes) {
            switch (attribute.type) {
                case "mpOverTime":
                    attribute.FindParameter("degree").intVal = (int)(80f * points / 70f);
                    points = 0;
                    break;
                case "heal":
                    attribute.FindParameter("degree").floatVal = 4f * points / 80f;
                    points = 0;
                    break;
                case "hot":
                    attribute.FindParameter("degree").floatVal = 8f * points / 80f;
                    points = 0;
                    break;
                case "shield":
                    attribute.FindParameter("degree").floatVal = points / 320f;
                    points = 0;
                    break;
                case "restoreMP":
                    attribute.FindParameter("degree").floatVal = 40f * points / 140f;
                    points = 0;
                    break;
            }
        }
        return points;
    }

    public static void RecalculateAbility(ActiveAbility ability) {
        int points;
        if (ability is AttackAbility asAttack) {
            points = CalculateAttackAbilityPoints(ability.points, asAttack.damage, asAttack.dotDamage, asAttack.dotTime, asAttack.isRanged, ability.cooldown, ability.mpUsage, ability.baseMpUsage, asAttack.radius, ability.attributes);
            ability.points = points;
            var damageRatio = asAttack.damage;
            var dotDamageRatio = asAttack.dotDamage;
            var totalDamage = AttackAbility.CalculateDamage(points);
            var regularDamage = totalDamage * damageRatio / (damageRatio + dotDamageRatio);
            var dotDamage = totalDamage * dotDamageRatio / (damageRatio + dotDamageRatio);
            asAttack.damage = regularDamage;
            asAttack.dotDamage = dotDamage;
            ability.name = AbilityNamer.Name(asAttack);
            ability.description = AbilityDescriber.Describe(asAttack);
        }
        else if (ability is UtilityAbility) {
            points = CalculateUtilityAbilityPoints(ability.points, ability.mpUsage, ability.baseMpUsage, ability.cooldown, ability.attributes);
            ability.points = points;
            ability.name = AbilityNamer.Name((UtilityAbility)ability);
            ability.description = AbilityDescriber.Describe((UtilityAbility)ability);
            ability.icon = AbilityIconGenerator.Retrieve((UtilityAbility)ability);
        }
    }

    private static int GetAoe(Element element) {
        switch (element) {
            case Element.slashing:
                return 0;
            case Element.piercing:
                return 1;
            case Element.bashing:
                return 2;
            case Element.fire:
                return 3;
            case Element.ice:
                return 4;
            case Element.acid:
                return 5;
            case Element.light:
                return 6;
            case Element.dark:
                return 7;
            case Element.none:
            default:
                return 8;
        }
    }

    private static List<AbilityAttribute> GetCombinedAttributes(Ability ability1, Ability ability2) {
        var output = new List<AbilityAttribute>();
        foreach (var attr in ability1.attributes) {
            var existingAttribute = GetExistingAttribute(output, attr);
            if (existingAttribute == null) output.Add(attr.Copy());
            else output[output.IndexOf(existingAttribute)] = SquashAttributes(existingAttribute, attr);
        }
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
            if (param.type == DataType.floatType) param.floatVal = (param.floatVal + param2.floatVal) / 2f;
            else if (param.type == DataType.intType) param.intVal = (param.intVal + param2.intVal) / 2;
        }
        return attr1Copy;
    }
}
