using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AbilityAttributeAppraiser {
    private delegate float AttackDelegate(AttackAbility ability, AbilityAttribute attribute);
    private delegate float UtilityDelegate(UtilityAbility ability, AbilityAttribute attribute);
    private delegate float PassiveDelegate(PassiveAbility ability, AbilityAttribute attribute);

    private static readonly Dictionary<string, float> simpleAttackValues;
    private static readonly Dictionary<string, float> simpleAttackMultipliers;
    private static readonly Dictionary<string, AttackDelegate> complexAttackMethods;
    private static readonly Dictionary<string, float> simpleUtilityValues;
    private static readonly Dictionary<string, float> simpleUtilityMultipliers;
    private static readonly Dictionary<string, UtilityDelegate> complexUtilityMethods;
    private static readonly Dictionary<string, float> simplePassiveValues;
    private static readonly Dictionary<string, float> simplePassiveMultipliers;
    private static readonly Dictionary<string, PassiveDelegate> complexPassiveMethods;

    static AbilityAttributeAppraiser() {
        simpleAttackValues = new Dictionary<string, float> {
            ["createDamageZone"] = 0
        };
        simpleAttackMultipliers = new Dictionary<string, float> {
            ["projectileSpread"] = 0.7f,
            ["jumpBack"] = 0.25f,
            ["chargeTowards"] = 0.25f,
            ["pullTowards"] = 0.25f,
            ["knockback"] = 0.25f,
            ["offGCD"] = 0.5f,
            ["paralyze"] = 0.5f,
            ["usableWhileParalyzed"] = 0.15f,
            ["mpOverTime"] = 0.2f,
            ["blunting"] = 0.5f,
            ["delay"] = -0.5f,
            ["damageShield"] = 0.5f,
            ["restoreMP"] = 0.2f,
            ["removeDebuff"] = 0.15f,
            ["addedDot"] = 0.5f,
            ["backstab"] = 0.25f,
            ["stealthy"] = 0.1f
        };
        complexAttackMethods = new Dictionary<string, AttackDelegate> {
            ["lifeleech"] = GetLifeleech,
            ["elementalDamageBuff"] = GetElementalDamageBuff,
            ["inflictVulnerability"] = GetInflictVulnerability,
            ["increasedCritChance"] = GetIncreasedCritChance,
            ["increasedCritDamage"] = GetIncreasedCritDamage,
            ["speed-"] = GetSpeedMinus,
            ["immobilizeSelf"] = GetImmobilizeSelf
        };
        simpleUtilityValues = new Dictionary<string, float> {
            ["disableDevice"] = 86,
            ["stealth"] = 86,
            ["removeDebuff"] = 82,
            ["removeAllDebuffs"] = 738,
            ["eatDebuff"] = 164,
            ["grapplingHook"] = 78
        };
        simpleUtilityMultipliers = new Dictionary<string, float> {
            ["offGCD"] = 0.5f,
            ["usableWhileParalyzed"] = 0.15f,
            ["mpOverTime"] = 1f,
            ["heal"] = 1f,
            ["hot"] = 1f,
            ["shield"] = 1f,
            ["restoreMP"] = 1f,
            ["stealthy"] = 0.1f
        };
        complexUtilityMethods = new Dictionary<string, UtilityDelegate> {
            ["elementalDamageBuff"] = GetElementalDamageBuff,
            ["speed-"] = GetSpeedMinus,
            ["paralyze"] = GetParalyze,
            ["immobilizeSelf"] = GetImmobilizeSelf,
            ["speed+"] = GetSpeedPlus
        };
        simplePassiveValues = new Dictionary<string, float> {
            ["knockback"] = 139,
            ["charge"] = 70,
            ["pullEnemies"] = 70
        };
        simplePassiveMultipliers = new Dictionary<string, float> {

        };
        complexPassiveMethods = new Dictionary<string, PassiveDelegate> {
            ["damageEnemiesOnScreen"] = GetDamageEnemiesOnScreen,
            ["experienceBoost"] = GetExperienceBoost,
            ["goldBoost"] = GetGoldBoost,
            ["boostStat"] = GetBoostStat,
            ["boostDamage"] = GetBoostDamage,
            ["reduceDamage"] = GetReduceDamage,
            ["reduceElementalDamage"] = GetReduceElementalDamage,
            ["boostElementalDamage"] = GetBoostElementalDamage
        };
    }

    public static float Appraise(AttackAbility ability, AbilityAttribute attribute) {
        if (simpleAttackValues.ContainsKey(attribute.type)) return simpleAttackValues[attribute.type];
        else if (simpleAttackMultipliers.ContainsKey(attribute.type)) return simpleAttackMultipliers[attribute.type] * ability.points;
        else if (complexAttackMethods.ContainsKey(attribute.type)) return complexAttackMethods[attribute.type](ability, attribute);
        return 1000000000;
    }

    public static float Appraise(UtilityAbility ability, AbilityAttribute attribute) {
        if (simpleUtilityValues.ContainsKey(attribute.type)) return simpleUtilityValues[attribute.type];
        else if (simpleUtilityMultipliers.ContainsKey(attribute.type)) return simpleUtilityMultipliers[attribute.type] * ability.points;
        else if (complexUtilityMethods.ContainsKey(attribute.type)) return complexUtilityMethods[attribute.type](ability, attribute);
        return 1000000000;
    }

    public static float Appraise(PassiveAbility ability, AbilityAttribute attribute) {
        if (simplePassiveValues.ContainsKey(attribute.type)) return simplePassiveValues[attribute.type];
        else if (simplePassiveMultipliers.ContainsKey(attribute.type)) return simplePassiveMultipliers[attribute.type] * ability.points;
        else if (complexPassiveMethods.ContainsKey(attribute.type)) return complexPassiveMethods[attribute.type](ability, attribute);
        return 1000000000;
    }

    private static float GetLifeleech(AttackAbility ability, AbilityAttribute attribute) {
        return ability.points * (float)attribute.FindParameter("degree").value / 2f;
    }

    private static float GetElementalDamageBuff(AttackAbility ability, AbilityAttribute attribute) {
        var damageBoost = (float)attribute.FindParameter("degree").value;
        if (damageBoost == 100) return ability.points * 0.06f;
        else if (damageBoost == 50) return ability.points * 0.03f;
        else return ability.points * 0.01f;
    }

    private static float GetInflictVulnerability(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        if (degree == 100) return ability.points * 0.75f;
        else if (degree == 50) return ability.points * 0.5f;
        else return ability.points * 0.25f;
    }

    private static float GetIncreasedCritChance(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return ability.points * degree;
    }

    private static float GetIncreasedCritDamage(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return ability.points * degree / 4f;
    }

    private static float GetSpeedMinus(AttackAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        var duration = (float)attribute.FindParameter("duration").value;
        return ability.points * degree / 10f * duration;
    }

    private static float GetImmobilizeSelf(AttackAbility ability, AbilityAttribute attribute) {
        var duration = (float)attribute.FindParameter("duration").value;
        return 0 - (ability.points * duration / 5f);
    }

    private static float GetElementalDamageBuff(UtilityAbility ability, AbilityAttribute attribute) {
        var damageBoost = (float)attribute.FindParameter("degree").value;
        if (damageBoost == 100) return 140f;
        else if (damageBoost == 50) return 70f;
        else return 35;
    }

    private static float GetSpeedMinus(UtilityAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        var duration = (float)attribute.FindParameter("duration").value;
        float radius = 0f;
        if (attribute.ContainsAttribute("radius")) radius = (float)attribute.FindParameter("radius").value;
        var pointCost = 140f * degree * duration / 10f;
        if (radius == 2) pointCost *= 2f;
        else if (radius == 4) pointCost *= 8f;
        else if (radius == 6) pointCost *= 18f;
        else pointCost *= 32f;
        return pointCost;
    }

    private static float GetParalyze(UtilityAbility ability, AbilityAttribute attribute) {
        var duration = (float)attribute.FindParameter("duration").value;
        float radius = 0f;
        if (attribute.ContainsAttribute("radius")) radius = (float)attribute.FindParameter("radius").value;
        var pointCost = 280f * duration / 10f;
        if (radius == 2) pointCost *= 2f;
        else if (radius == 4) pointCost *= 8f;
        else if (radius == 6) pointCost *= 18f;
        else pointCost *= 32f;
        return pointCost;
    }

    private static float GetImmobilizeSelf(UtilityAbility ability, AbilityAttribute attribute) {
        var duration = (float)attribute.FindParameter("duration").value;
        return 0 - (ability.points * duration / 5f);
    }

    private static float GetSpeedPlus(UtilityAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        var duration = (float)attribute.FindParameter("duration").value;
        return 86f * duration / 5f * degree;
    }

    private static float GetDamageEnemiesOnScreen(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 70f;
    }

    private static float GetExperienceBoost(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 1400f;
    }

    private static float GetGoldBoost(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 1400f;
    }

    private static float GetBoostStat(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 70f / 3f;
    }

    private static float GetBoostDamage(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 1400f;
    }

    private static float GetReduceDamage(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 1400f / 3f;
    }
    private static float GetReduceElementalDamage(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 700f / 6f;
    }

    private static float GetBoostElementalDamage(PassiveAbility ability, AbilityAttribute attribute) {
        var degree = (float)attribute.FindParameter("degree").value;
        return degree * 700f / 2f;
    }
}