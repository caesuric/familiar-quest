using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AttackAttributeTable {

    public static AbilityAttribute Retrieve(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        string roll = TableRoller.Roll("AttackAttributes");
        var table = new Dictionary<string, AbilityAttributeSource>() {
            { "createDamageZone", () => GetCreateDamageZone(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "projectileSpread", () => GetProjectileSpread(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "jumpBack", () => GetJumpBack(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "chargeTowards", () => GetChargeTowards(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "pullTowards", () => GetPullTowards(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "knockback", () => GetKnockback(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "offGCD", () => GetOffGCD(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "paralyze", () => GetParalyze(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "usableWhileParalyzed", () => GetUsableWhileParalyzed(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "lifeleech", () => GetLifeleech(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "mpOverTime", () => GetMpOverTime(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "elementalDamageBuff", () => GetElementalDamageBuff(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "blunting", () => GetBlunting(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "inflictVulnerability", () => GetInflictVulnerability(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "delay", () => GetDelay(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "damageShield", () => GetDamageShield(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "restoreMP", () => GetRestoreMP(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "removeDebuff", () => GetRemoveDebuff(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "addedDot", () => GetAddedDot(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "backstab", () => GetBackstab(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "increasedCritChance", () => GetIncreasedCritChance(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "increasedCritDamage", () => GetIncreasedCritDamage(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "speed-", () => GetSpeedMinus(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "immobilizeSelf", () => GetImmobilizeSelf(points, mp, isDot, radius, isRanged, cooldown, baseElement) },
            { "stealthy", () => GetStealthy(points, mp, isDot, radius, isRanged, cooldown, baseElement) }
        };
        return table[roll]();
    }

    private static AbilityAttribute GetCreateDamageZone(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        if (radius > 0 && isDot) return new AbilityAttribute("createDamageZone", 0);
        else return Retrieve(points, mp, isDot, radius, isRanged, cooldown, baseElement);
    }

    private static AbilityAttribute GetProjectileSpread(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        if (isRanged) return new AbilityAttribute("projectileSpread", points * 0.7f);
        else return Retrieve(points, mp, isDot, radius, isRanged, cooldown, baseElement);
    }

    private static AbilityAttribute GetJumpBack(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("jumpBack", points * 0.25f, new AbilityParameter("degree", DataType.floatType, floatVal: 5f));
    }

    private static AbilityAttribute GetChargeTowards(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        if (!isRanged) return new AbilityAttribute("chargeTowards", points * 0.25f);
        else return Retrieve(points, mp, isDot, radius, isRanged, cooldown, baseElement);
    }

    private static AbilityAttribute GetPullTowards(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        if (isRanged) return new AbilityAttribute("pullTowards", points * 0.25f);
        else return Retrieve(points, mp, isDot, radius, isRanged, cooldown, baseElement);
    }

    private static AbilityAttribute GetKnockback(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("knockback", points * 0.25f, new AbilityParameter("degree", DataType.floatType, floatVal: 5f));
    }

    private static AbilityAttribute GetOffGCD(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        if (cooldown > 0) return new AbilityAttribute("offGCD", points * 0.5f);
        else return Retrieve(points, mp, isDot, radius, isRanged, cooldown, baseElement);
    }

    private static AbilityAttribute GetParalyze(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("paralyze", points * 0.5f, new AbilityParameter("duration", DataType.floatType, floatVal: 3));
    }

    private static AbilityAttribute GetUsableWhileParalyzed(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("usableWhileParalyzed", points * 0.15f);
    }

    private static AbilityAttribute GetLifeleech(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        int leechAmountInt = Random.Range(5, 120);
        leechAmountInt = Mathf.Min(leechAmountInt, 100);
        float leechAmount = leechAmountInt / 100f;
        return new AbilityAttribute("lifeleech", points * leechAmount / 2f, new AbilityParameter("degree", DataType.floatType, floatVal: leechAmount));
    }

    private static AbilityAttribute GetMpOverTime(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("mpOverTime", points * 0.2f, new AbilityParameter("degree", DataType.intType, intVal: (int)(points * 80f / 70f)), new AbilityParameter("duration", DataType.floatType, floatVal: 8));
    }

    private static AbilityAttribute GetElementalDamageBuff(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        var degreeRoll = Random.Range(0, 3);
        float damageBoost;
        float pointCost;
        switch (degreeRoll) {
            case 0:
                damageBoost = 100;
                pointCost = points * 0.06f;
                break;
            case 1:
                damageBoost = 50;
                pointCost = points * 0.03f;
                break;
            case 2:
            default:
                damageBoost = 25;
                pointCost = points * 0.01f;
                break;
        }
        int elementRoll = Random.Range(0, 100);
        Element element;
        if (elementRoll < 50) element = baseElement;
        else if (elementRoll < 95) {
            element = baseElement;
            int affinityRoll = Random.Range(0, ElementalAffinity.alliances[element].Count);
            element = ElementalAffinity.alliances[element][affinityRoll];
        }
        else element = Spirit.RandomElement();
        return new AbilityAttribute("elementalDamageBuff", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: damageBoost), new AbilityParameter("duration", DataType.floatType, floatVal: 12), new AbilityParameter("element", DataType.stringType, stringVal: element.ToString()));
    }

    private static AbilityAttribute GetBlunting(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("blunting", points * 0.5f, new AbilityParameter("degree", DataType.floatType, floatVal: AttackAbility.CalculateDamage(points * 0.5f)));
    }

    private static AbilityAttribute GetInflictVulnerability(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        var degreeRoll = Random.Range(0, 3);
        float damageBoost;
        float pointCost;
        switch (degreeRoll) {
            case 0:
                damageBoost = 100;
                pointCost = points * 0.75f;
                break;
            case 1:
                damageBoost = 50;
                pointCost = points * 0.5f;
                break;
            case 2:
            default:
                damageBoost = 25;
                pointCost = points * 0.25f;
                break;
        }
        return new AbilityAttribute("inflictVulnerability", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: damageBoost), new AbilityParameter("duration", DataType.floatType, floatVal: 12));
    }

    private static AbilityAttribute GetDelay(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("delay", points * -0.5f, new AbilityParameter("time", DataType.floatType, floatVal: 4));
    }

    private static AbilityAttribute GetDamageShield(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("damageShield", points * 0.5f, new AbilityParameter("degree", DataType.floatType, floatVal: points * 0.5f / 320));
    }

    private static AbilityAttribute GetRestoreMP(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("restoreMP", points * 0.2f, new AbilityParameter("degree", DataType.intType, intVal: (int)(points / 70f * 40f)));
    }

    private static AbilityAttribute GetRemoveDebuff(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("removeDebuff", points * 0.15f);
    }

    private static AbilityAttribute GetAddedDot(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        int dotDurationRoll = Random.Range(0, 100);
        float dotDuration;
        float dotMultiplier;
        if (dotDurationRoll < 25) {
            dotDuration = 4;
            dotMultiplier = 1.5f;
        }
        else if (dotDurationRoll < 75) {
            dotDuration = 8;
            dotMultiplier = 3f;
        }
        else {
            dotDuration = 12;
            dotMultiplier = 4f;
        }
        return new AbilityAttribute("addedDot", points * 0.5f, new AbilityParameter("degree", DataType.floatType, floatVal: AttackAbility.CalculateDamage(points * 0.5f * dotMultiplier)), new AbilityParameter("duration", DataType.floatType, floatVal: dotDuration));
    }

    private static AbilityAttribute GetBackstab(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("backstab", points * 0.25f, new AbilityParameter("degree", DataType.floatType, floatVal: 4));
    }

    private static AbilityAttribute GetIncreasedCritChance(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        int increasedChanceRollInt = Random.Range(10, 50);
        float increasedChanceRoll = increasedChanceRollInt / 100f;
        return new AbilityAttribute("increasedCritChance", points * increasedChanceRoll, new AbilityParameter("degree", DataType.floatType, floatVal: increasedChanceRoll));
    }

    private static AbilityAttribute GetIncreasedCritDamage(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        int increasedDamageRollInt = Random.Range(100, 200);
        float increasedDamageRoll = increasedDamageRollInt / 100f;
        return new AbilityAttribute("increasedCritDamage", points * increasedDamageRoll / 4f, new AbilityParameter("degree", DataType.floatType, floatVal: increasedDamageRoll));
    }

    private static AbilityAttribute GetSpeedMinus(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        int speedMinusDegreeInt = Random.Range(20, 70);
        float speedMinusDegree = speedMinusDegreeInt / 100f;
        int speedMinusDurationInt = Random.Range(2, 10);
        float speedMinusDuration = speedMinusDurationInt;
        return new AbilityAttribute("speed-", points * speedMinusDegree / 10f * speedMinusDuration, new AbilityParameter("degree", DataType.floatType, floatVal: speedMinusDegree), new AbilityParameter("duration", DataType.floatType, floatVal: speedMinusDuration));
    }

    private static AbilityAttribute GetImmobilizeSelf(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        var duration = Random.Range(2, 9);
        return new AbilityAttribute("immobilizeSelf", 0 - (points * duration / 5f), new AbilityParameter("duration", DataType.floatType, floatVal: duration));
    }

    private static AbilityAttribute GetStealthy(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        return new AbilityAttribute("stealthy", points * 0.1f);
    }
}
