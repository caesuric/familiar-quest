using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UtilityAttributeTable {

    public static AbilityAttribute Retrieve(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        string roll = TableRoller.Roll("UtilityAttributes_" + targetType);
        var table = new Dictionary<string, AbilityAttributeSource>() {
            { "offGCD", () => GetOffGCD(points, priority, mp, cooldown, numAttributes, targetType) },
            { "usableWhileParalyzed", () => GetUsableWhileParalyzed(points, priority, mp, cooldown, numAttributes, targetType) },
            { "mpOverTime", () => GetMpOverTime(points, priority, mp, cooldown, numAttributes, targetType) },
            { "elementalDamageBuff", () => GetElementalDamageBuff(points, priority, mp, cooldown, numAttributes, targetType) },
            { "heal", () => GetHeal(points, priority, mp, cooldown, numAttributes, targetType) },
            { "hot", () => GetHot(points, priority, mp, cooldown, numAttributes, targetType) },
            { "shield", () => GetShield(points, priority, mp, cooldown, numAttributes, targetType) },
            { "restoreMP", () => GetRestoreMP(points, priority, mp, cooldown, numAttributes, targetType) },
            { "disableDevice", () => GetDisableDevice(points, priority, mp, cooldown, numAttributes, targetType) },
            { "stealth", () => GetStealth(points, priority, mp, cooldown, numAttributes, targetType) },
            { "stealthy", () => GetStealthy(points, priority, mp, cooldown, numAttributes, targetType) },
            { "speed-", () => GetSpeedMinus(points, priority, mp, cooldown, numAttributes, targetType) },
            { "paralyze", () => GetParalyze(points, priority, mp, cooldown, numAttributes, targetType) },
            { "immobilizeSelf", () => GetImmobilizeSelf(points, priority, mp, cooldown, numAttributes, targetType) },
            { "removeDebuff", () => GetRemoveDebuff(points, priority, mp, cooldown, numAttributes, targetType) },
            { "removeAllDebuffs", () => GetRemoveAllDebuffs(points, priority, mp, cooldown, numAttributes, targetType) },
            { "eatDebuff", () => GetEatDebuff(points, priority, mp, cooldown, numAttributes, targetType) },
            { "speed+", () => GetSpeedPlus(points, priority, mp, cooldown, numAttributes, targetType) },
            { "grapplingHook", () => GetGrapplingHook(points, priority, mp, cooldown, numAttributes, targetType) }
        };
        try {
            return table[roll]();
        }
        catch {
            return null;
        }
    }

    private static AbilityAttribute GetOffGCD(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (numAttributes > 1 && cooldown > 0) return new AbilityAttribute("offGCD", points / 2, priority);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetUsableWhileParalyzed(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (numAttributes > 1) return new AbilityAttribute("usableWhileParalyzed", points * 0.15f, priority);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetMpOverTime(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("mpOverTime", points, priority, new AbilityParameter("degree", DataType.intType, intVal: (int)(80 * points / 70)), new AbilityParameter("duration", DataType.floatType, floatVal: 8));
    }

    private static AbilityAttribute GetElementalDamageBuff(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        int degreeRoll = Random.Range(0, 3);
        float pointCost;
        float damageBoost;
        switch (degreeRoll) {
            case 0:
                damageBoost = 100;
                pointCost = 140;
                break;
            case 1:
                damageBoost = 50;
                pointCost = 70;
                break;
            case 2:
            default:
                damageBoost = 25;
                pointCost = 35;
                break;
        }
        var element = Spirit.RandomElement();
        if (points >= pointCost) return new AbilityAttribute("elementalDamageBuff", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: damageBoost), new AbilityParameter("duration", DataType.floatType, floatVal: 12), new AbilityParameter("element", DataType.stringType, stringVal: element.ToString()));
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetHeal(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("heal", points, priority, new AbilityParameter("degree", DataType.floatType, floatVal: 4f * points / 80f));
    }

    private static AbilityAttribute GetHot(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("hot", points, priority, new AbilityParameter("degree", DataType.floatType, floatVal: 8f * points / 80f), new AbilityParameter("duration", DataType.floatType, floatVal: 8f));
    }

    private static AbilityAttribute GetShield(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("shield", points, priority, new AbilityParameter("degree", DataType.floatType, floatVal: points / 320f), new AbilityParameter("stat", DataType.stringType, stringVal: "strength"));
    }

    private static AbilityAttribute GetRestoreMP(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("restoreMP", points, priority, new AbilityParameter("degree", DataType.floatType, floatVal: 40f * points / 140f));
    }

    private static AbilityAttribute GetDisableDevice(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 86) return new AbilityAttribute("disableDevice", 86, priority, new AbilityParameter("radius", DataType.floatType, floatVal: 2f));
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetStealth(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 86) return new AbilityAttribute("stealth", priority, 86);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetStealthy(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (numAttributes > 1) return new AbilityAttribute("stealthy", points * 0.1f, priority);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetSpeedMinus(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        int degreeRoll = Random.Range(20, 70);
        int durationRoll = Random.Range(2, 9);
        int radiusRoll;
        float radius = 2;
        float pointCost = 140f * (degreeRoll / 100f) * durationRoll / 10f;
        bool rollForRadius = false;
        if (targetType == "none") {
            rollForRadius = true;
            pointCost /= 2f;
        }
        else {
            int rollForRoll = Random.Range(0, 2);
            if (rollForRoll == 0) rollForRadius = true;
        }
        if (rollForRadius) {
            radiusRoll = Random.Range(0, 30);
            if (radiusRoll < 21) {
                radius = 2;
                pointCost *= 4f;
            }
            else if (radiusRoll < 27) {
                radius = 4;
                pointCost *= 16f;
            }
            else if (radiusRoll < 29) {
                radius = 6;
                pointCost *= 36f;
            }
            else {
                radius = 8;
                pointCost *= 64f;
            }
        }
        if (points >= pointCost && rollForRadius) return new AbilityAttribute("speed-", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degreeRoll / 100f), new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll), new AbilityParameter("radius", DataType.floatType, floatVal: radius));
        else if (points >= pointCost) return new AbilityAttribute("speed-", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degreeRoll / 100f), new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll));
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetParalyze(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        int durationRoll = Random.Range(2, 9);
        float pointCost = 280f * durationRoll / 10f;
        bool rollForRadius = false;
        int radiusRoll;
        float radius = 2;
        if (targetType == "none") {
            rollForRadius = true;
            pointCost /= 2f;
        }
        else {
            int rollForRoll = Random.Range(0, 2);
            if (rollForRoll == 0) rollForRadius = true;
        }
        if (rollForRadius) {
            radiusRoll = Random.Range(0, 30);
            if (radiusRoll < 21) {
                radius = 2;
                pointCost *= 4f;
            }
            else if (radiusRoll < 27) {
                radius = 4;
                pointCost *= 16f;
            }
            else if (radiusRoll < 29) {
                radius = 6;
                pointCost *= 36f;
            }
            else {
                radius = 8;
                pointCost *= 64f;
            }
        }
        if (points >= pointCost && rollForRadius) return new AbilityAttribute("paralyze", pointCost, priority, new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll), new AbilityParameter("radius", DataType.floatType, floatVal: radius));
        else if (points >= pointCost) return new AbilityAttribute("paralyze", pointCost, priority, new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll));
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetImmobilizeSelf(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        int duration = Random.Range(2, 9);
        if (numAttributes > 1) return new AbilityAttribute("immobilizeSelf", 0 - (points * duration / 5f), priority, new AbilityParameter("duration", DataType.floatType, floatVal: duration));
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetRemoveDebuff(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 82) return new AbilityAttribute("removeDebuff", 82, priority);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetRemoveAllDebuffs(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 738) return new AbilityAttribute("removeAllDebuffs", 738, priority);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetEatDebuff(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 164) return new AbilityAttribute("eatDebuff", 164, priority);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetSpeedPlus(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        int duration = Random.Range(2, 11);
        int degreeRoll = Random.Range(30, 120);
        if (degreeRoll > 100) degreeRoll = 100;
        float pointCost = 86f * (float)duration / 5f * (float)degreeRoll / 100f;
        if (points >= pointCost) return new AbilityAttribute("speed+", pointCost, priority, new AbilityParameter("duration", DataType.floatType, floatVal: duration), new AbilityParameter("degree", DataType.floatType, floatVal: degreeRoll / 100f));
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetGrapplingHook(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 78) return new AbilityAttribute("grapplingHook", 78, priority);
        else return Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
    }
}
