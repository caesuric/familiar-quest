using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class UtilityAttributeTable {

    public static AbilityAttribute Retrieve(float points, int mp, float cooldown, int numAttributes, string targetType) {
        string roll = TableRoller.Roll("UtilityAttributes_" + targetType);
        var table = new Dictionary<string, AbilityAttributeSource>() {
            { "offGCD", () => GetOffGCD(points, mp, cooldown, numAttributes, targetType) },
            { "usableWhileParalyzed", () => GetUsableWhileParalyzed(points, mp, cooldown, numAttributes, targetType) },
            { "mpOverTime", () => GetMpOverTime(points, mp, cooldown, numAttributes, targetType) },
            { "elementalDamageBuff", () => GetElementalDamageBuff(points, mp, cooldown, numAttributes, targetType) },
            { "heal", () => GetHeal(points, mp, cooldown, numAttributes, targetType) },
            { "hot", () => GetHot(points, mp, cooldown, numAttributes, targetType) },
            { "shield", () => GetShield(points, mp, cooldown, numAttributes, targetType) },
            { "restoreMP", () => GetRestoreMP(points, mp, cooldown, numAttributes, targetType) },
            { "disableDevice", () => GetDisableDevice(points, mp, cooldown, numAttributes, targetType) },
            { "stealth", () => GetStealth(points, mp, cooldown, numAttributes, targetType) },
            { "stealthy", () => GetStealthy(points, mp, cooldown, numAttributes, targetType) },
            { "speed-", () => GetSpeedMinus(points, mp, cooldown, numAttributes, targetType) },
            { "paralyze", () => GetParalyze(points, mp, cooldown, numAttributes, targetType) },
            { "immobilizeSelf", () => GetImmobilizeSelf(points, mp, cooldown, numAttributes, targetType) },
            { "removeDebuff", () => GetRemoveDebuff(points, mp, cooldown, numAttributes, targetType) },
            { "removeAllDebuffs", () => GetRemoveAllDebuffs(points, mp, cooldown, numAttributes, targetType) },
            { "eatDebuff", () => GetEatDebuff(points, mp, cooldown, numAttributes, targetType) },
            { "speed+", () => GetSpeedPlus(points, mp, cooldown, numAttributes, targetType) },
            { "grapplingHook", () => GetGrapplingHook(points, mp, cooldown, numAttributes, targetType) }
        };
        return table[roll]();
    }

    private static AbilityAttribute GetOffGCD(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (numAttributes > 1 && cooldown > 0) return new AbilityAttribute("offGCD", points / 2);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetUsableWhileParalyzed(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (numAttributes > 1) return new AbilityAttribute("usableWhileParalyzed", points * 0.15f);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetMpOverTime(float points, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("mpOverTime", points, new AbilityParameter("degree", DataType.intType, intVal: (int)(80 * points / 70)), new AbilityParameter("duration", DataType.floatType, floatVal: 8));
    }

    private static AbilityAttribute GetElementalDamageBuff(float points, int mp, float cooldown, int numAttributes, string targetType) {
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
        if (points >= pointCost) return new AbilityAttribute("elementalDamageBuff", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: damageBoost), new AbilityParameter("duration", DataType.floatType, floatVal: 12), new AbilityParameter("element", DataType.stringType, stringVal: element.ToString()));
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetHeal(float points, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("heal", points, new AbilityParameter("degree", DataType.floatType, floatVal: 4f * points / 80f));
    }

    private static AbilityAttribute GetHot(float points, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("hot", points, new AbilityParameter("degree", DataType.floatType, floatVal: 8f * points / 80f), new AbilityParameter("duration", DataType.floatType, floatVal: 8f));
    }

    private static AbilityAttribute GetShield(float points, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("shield", points, new AbilityParameter("degree", DataType.floatType, floatVal: points / 320f), new AbilityParameter("stat", DataType.stringType, stringVal: "strength"));
    }

    private static AbilityAttribute GetRestoreMP(float points, int mp, float cooldown, int numAttributes, string targetType) {
        return new AbilityAttribute("restoreMP", points, new AbilityParameter("degree", DataType.floatType, floatVal: 40f * points / 140f));
    }

    private static AbilityAttribute GetDisableDevice(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 86) return new AbilityAttribute("disableDevice", 86, new AbilityParameter("radius", DataType.floatType, floatVal: 2f));
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetStealth(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 86) return new AbilityAttribute("stealth", 86);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetStealthy(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (numAttributes > 1) return new AbilityAttribute("stealthy", points * 0.1f);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetSpeedMinus(float points, int mp, float cooldown, int numAttributes, string targetType) {
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
        if (points >= pointCost && rollForRadius) return new AbilityAttribute("speed-", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degreeRoll / 100f), new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll), new AbilityParameter("radius", DataType.floatType, floatVal: radius));
        else if (points >= pointCost) return new AbilityAttribute("speed-", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degreeRoll / 100f), new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll));
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetParalyze(float points, int mp, float cooldown, int numAttributes, string targetType) {
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
        if (points >= pointCost && rollForRadius) return new AbilityAttribute("paralyze", pointCost, new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll), new AbilityParameter("radius", DataType.floatType, floatVal: radius));
        else if (points >= pointCost) return new AbilityAttribute("paralyze", pointCost, new AbilityParameter("duration", DataType.floatType, floatVal: durationRoll));
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetImmobilizeSelf(float points, int mp, float cooldown, int numAttributes, string targetType) {
        int duration = Random.Range(2, 9);
        if (numAttributes > 1) return new AbilityAttribute("immobilizeSelf", 0 - (points * duration / 5f), new AbilityParameter("duration", DataType.floatType, floatVal: duration));
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetRemoveDebuff(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 82) return new AbilityAttribute("removeDebuff", 82);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetRemoveAllDebuffs(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 738) return new AbilityAttribute("removeAllDebuffs", 738);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetEatDebuff(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 164) return new AbilityAttribute("eatDebuff", 164);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetSpeedPlus(float points, int mp, float cooldown, int numAttributes, string targetType) {
        int duration = Random.Range(2, 11);
        int degreeRoll = Random.Range(30, 120);
        if (degreeRoll > 100) degreeRoll = 100;
        float pointCost = 86f * (float)duration / 5f * (float)degreeRoll / 100f;
        if (points >= pointCost) return new AbilityAttribute("speed+", pointCost, new AbilityParameter("duration", DataType.floatType, floatVal: duration), new AbilityParameter("degree", DataType.floatType, floatVal: degreeRoll / 100f));
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    private static AbilityAttribute GetGrapplingHook(float points, int mp, float cooldown, int numAttributes, string targetType) {
        if (points >= 78) return new AbilityAttribute("grapplingHook", 78);
        else return Retrieve(points, mp, cooldown, numAttributes, targetType);
    }
}
