using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityAttribute {
    public string type;
    public float points;
    public List<AbilityParameter> parameters = new List<AbilityParameter>();

    public AbilityAttribute(string type, float points, params AbilityParameter[] parameters) {
        this.type = type;
        this.points = points;
        foreach (var item in parameters) this.parameters.Add(item);
    }
    public AbilityAttribute(string type, params AbilityParameter[] parameters) {
        this.type = type;
        foreach (var item in parameters) this.parameters.Add(item);
    }
    public AbilityParameter FindParameter(string name) {
        foreach (var item in parameters) if (item.name == name) return item;
        return null;
    }

    public AbilityAttribute Copy() {
        var newAttribute = new AbilityAttribute(type, points, new AbilityParameter[] { });
        foreach (var parameter in parameters) newAttribute.parameters.Add(parameter.Copy());
        return newAttribute;
    }

    public static AbilityAttribute GetAttackAttribute(float points, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
        string roll = TableRoller.Roll("AttackAttributes");
        int degreeRoll;
        int duration;
        float damageBoost;
        float pointCost;
        switch (roll) {
            case "createDamageZone":
                if (radius > 0 && isDot) return new AbilityAttribute("createDamageZone", 0);
                else return GetAttackAttribute(points, mp, isDot, radius, isRanged, cooldown, baseElement);
            case "projectileSpread":
                if (isRanged) return new AbilityAttribute("projectileSpread", points * 0.7f);
                else return GetAttackAttribute(points, mp, isDot, radius, isRanged, cooldown, baseElement);
            case "jumpBack":
                return new AbilityAttribute("jumpBack", points * 0.25f, new AbilityParameter("degree", DataType.floatType, floatVal: 5f));
            case "chargeTowards":
                if (!isRanged) return new AbilityAttribute("chargeTowards", points * 0.25f);
                else return GetAttackAttribute(points, mp, isDot, radius, isRanged, cooldown, baseElement);
            case "pullTowards":
                if (isRanged) return new AbilityAttribute("pullTowards", points * 0.25f);
                else return GetAttackAttribute(points, mp, isDot, radius, isRanged, cooldown, baseElement);
            case "knockback":
                return new AbilityAttribute("knockback", points * 0.25f, new AbilityParameter("degree", DataType.floatType, floatVal: 5f));
            case "offGCD":
                if (cooldown > 0) return new AbilityAttribute("offGCD", points * 0.5f);
                else return GetAttackAttribute(points, mp, isDot, radius, isRanged, cooldown, baseElement);
            case "paralyze":
                return new AbilityAttribute("paralyze", points * 0.5f, new AbilityParameter("duration", DataType.floatType, floatVal: 3));
            case "usableWhileParalyzed":
                return new AbilityAttribute("usableWhileParalyzed", points * 0.15f);
            case "lifeleech":
                int leechAmountInt = Random.Range(5, 120);
                leechAmountInt = Mathf.Min(leechAmountInt, 100);
                float leechAmount = leechAmountInt / 100f;
                return new AbilityAttribute("lifeleech", points * leechAmount / 2f, new AbilityParameter("degree", DataType.floatType, floatVal: leechAmount));
            case "mpOverTime":
                return new AbilityAttribute("mpOverTime", points * 0.2f, new AbilityParameter("degree", DataType.intType, intVal: (int)(points * 80f / 70f)), new AbilityParameter("duration", DataType.floatType, floatVal: 8));
            case "elementalDamageBuff":
                degreeRoll = Random.Range(0, 3);
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
            case "blunting":
                return new AbilityAttribute("blunting", points * 0.5f, new AbilityParameter("degree", DataType.floatType, floatVal: AttackAbility.CalculateDamage(points * 0.5f)));
            case "inflictVulnerability":
                degreeRoll = Random.Range(0, 3);
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
            case "delay":
                return new AbilityAttribute("delay", points * -0.5f, new AbilityParameter("time", DataType.floatType, floatVal: 4));
            case "damageShield":
                return new AbilityAttribute("damageShield", points * 0.5f, new AbilityParameter("degree", DataType.floatType, floatVal: points * 0.5f / 320));
            case "restoreMP":
                return new AbilityAttribute("restoreMP", points * 0.2f, new AbilityParameter("degree", DataType.intType, intVal: (int)(points / 70f * 40f)));
            case "removeDebuff":
                return new AbilityAttribute("removeDebuff", points * 0.15f);
            case "addedDot":
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
            case "backstab":
                return new AbilityAttribute("backstab", points * 0.25f, new AbilityParameter("degree", DataType.floatType, floatVal: 4));
            case "increasedCritChance":
                int increasedChanceRollInt = Random.Range(10, 50);
                float increasedChanceRoll = increasedChanceRollInt / 100f;
                return new AbilityAttribute("increasedCritChance", points * increasedChanceRoll, new AbilityParameter("degree", DataType.floatType, floatVal: increasedChanceRoll));
            case "increasedCritDamage":
                int increasedDamageRollInt = Random.Range(100, 200);
                float increasedDamageRoll = increasedDamageRollInt / 100f;
                return new AbilityAttribute("increasedCritDamage", points * increasedDamageRoll / 4f, new AbilityParameter("degree", DataType.floatType, floatVal: increasedDamageRoll));
            case "speed-":
                int speedMinusDegreeInt = Random.Range(20, 70);
                float speedMinusDegree = speedMinusDegreeInt / 100f;
                int speedMinusDurationInt = Random.Range(2, 10);
                float speedMinusDuration = speedMinusDurationInt;
                return new AbilityAttribute("speed-", points * speedMinusDegree / 10f * speedMinusDuration, new AbilityParameter("degree", DataType.floatType, floatVal: speedMinusDegree), new AbilityParameter("duration", DataType.floatType, floatVal: speedMinusDuration));
            case "immobilizeSelf":
                duration = Random.Range(2, 9);
                return new AbilityAttribute("immobilizeSelf", 0 - (points * duration / 5f), new AbilityParameter("duration", DataType.floatType, floatVal: duration));
            case "stealthy":
            default:
                return new AbilityAttribute("stealthy", points * 0.1f);
        }
    }

    public static AbilityAttribute GetUtilityAttribute(float points, int mp, float cooldown, int numAttributes, string targetType) {
        int radiusRoll, degreeRoll, durationRoll, duration;
        float radius = 0f;
        float pointCost;
        bool rollForRadius;
        if (points == 0) return null;
        string roll = TableRoller.Roll("UtilityAttributes_" + targetType);
        switch (roll) {
            case "offGCD":
                if (numAttributes > 1 && cooldown > 0) return new AbilityAttribute("offGCD", points / 2);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "usableWhileParalyzed":
                if (numAttributes > 1) return new AbilityAttribute("usableWhileParalyzed", points * 0.15f);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "mpOverTime":
                return new AbilityAttribute("mpOverTime", points, new AbilityParameter("degree", DataType.intType, intVal: (int)(80 * points / 70)), new AbilityParameter("duration", DataType.floatType, floatVal: 8));
            case "elementalDamageBuff":
                degreeRoll = Random.Range(0, 3);
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
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "heal":
                return new AbilityAttribute("heal", points, new AbilityParameter("degree", DataType.floatType, floatVal: 4f * points / 80f));
            case "hot":
                return new AbilityAttribute("hot", points, new AbilityParameter("degree", DataType.floatType, floatVal: 8f * points / 80f), new AbilityParameter("duration", DataType.floatType, floatVal: 8f));
            case "shield":
                return new AbilityAttribute("shield", points, new AbilityParameter("degree", DataType.floatType, floatVal: points / 320f), new AbilityParameter("stat", DataType.stringType, stringVal: "strength"));
            case "restoreMP":
                return new AbilityAttribute("restoreMP", points, new AbilityParameter("degree", DataType.floatType, floatVal: 40f * points / 140f));
            case "disableDevice":
                if (points >= 86) return new AbilityAttribute("disableDevice", 86, new AbilityParameter("radius", DataType.floatType, floatVal: 2f));
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "stealth":
                if (points >= 86) return new AbilityAttribute("stealth", 86);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "stealthy":
                if (numAttributes > 1) return new AbilityAttribute("stealthy", points * 0.1f);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "speed-":
                degreeRoll = Random.Range(20, 70);
                durationRoll = Random.Range(2, 9);
                pointCost = 140f * (degreeRoll / 100f) * durationRoll / 10f;
                rollForRadius = false;
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
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "paralyze":
                durationRoll = Random.Range(2, 9);
                pointCost = 280f * durationRoll / 10f;
                rollForRadius = false;
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
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "immobilizeSelf":
                duration = Random.Range(2, 9);
                return new AbilityAttribute("immobilizeSelf", 0 - (points * duration / 5f), new AbilityParameter("duration", DataType.floatType, floatVal: duration));
            case "removeDebuff":
                if (points >= 82) return new AbilityAttribute("removeDebuff", 82);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "removeAllDebuffs":
                if (points >= 738) return new AbilityAttribute("removeAllDebuffs", 738);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "eatDebuff":
                if (points >= 164) return new AbilityAttribute("eatDebuff", 164);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "speed+":
                duration = Random.Range(2, 11);
                degreeRoll = Random.Range(30, 120);
                if (degreeRoll > 100) degreeRoll = 100;
                pointCost = 86f * (float)duration / 5f * (float)degreeRoll / 100f;
                if (points>= pointCost) return new AbilityAttribute("speed+", pointCost, new AbilityParameter("duration", DataType.floatType, floatVal: duration), new AbilityParameter("degree", DataType.floatType, floatVal: degreeRoll / 100f));
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
            case "grapplingHook":
            default:
                if (points >= 78) return new AbilityAttribute("grapplingHook", 78);
                else return GetUtilityAttribute(points, mp, cooldown, numAttributes, targetType);
        }
    }

    public static AbilityAttribute GetPassiveAttribute(float points, int numAttributes) {
        if (points == 0) return null;
        string roll = TableRoller.Roll("PassiveAttributes");
        float pointCost;
        var elements = new List<string> { "piercing", "slashing", "bashing", "fire", "ice", "acid", "light", "dark" };
        int elementRoll;
        string randomElement;
        int degree;
        switch (roll) {
            case "damageEnemiesOnScreen":
            default:
                degree = Random.Range(2, 23);
                pointCost = degree * 35f;
                if (points >= pointCost) return new AbilityAttribute("damageEnemiesOnScreen", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree));
                else return GetPassiveAttribute(points, numAttributes);
            case "experienceBoost":
                degree = Random.Range(5, 31);
                pointCost = degree * 14f;
                if (points >= pointCost) return new AbilityAttribute("experienceBoost", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
                else return GetPassiveAttribute(points, numAttributes);
            case "knockback":
                if (points >= 139) return new AbilityAttribute("knockback", 139);
                else return GetPassiveAttribute(points, numAttributes);
            case "charge":
                if (points >= 70) return new AbilityAttribute("charge", 70);
                else return GetPassiveAttribute(points, numAttributes);
            case "pullEnemies":
                if (points >= 70) return new AbilityAttribute("pullEnemies", 70);
                else return GetPassiveAttribute(points, numAttributes);
            case "goldBoost":
                degree = Random.Range(5, 31);
                pointCost = degree * 14f;
                if (points >= pointCost) return new AbilityAttribute("goldBoost", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
                else return GetPassiveAttribute(points, numAttributes);
            case "boostStat":
                degree = Random.Range(3, 101);
                pointCost = degree * 70f / 3f;
                var stats = new List<string> { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
                int statRoll = Random.Range(0, 6);
                var randomStat = stats[statRoll];
                if (points >= pointCost) return new AbilityAttribute("boostStat", pointCost, new AbilityParameter("degree", DataType.intType, intVal: degree), new AbilityParameter("stat", DataType.stringType, stringVal: randomStat));
                else return GetPassiveAttribute(points, numAttributes);
            case "boostDamage":
                degree = Random.Range(15, 31);
                pointCost = degree * 14f / 3f;
                if (points >= pointCost) return new AbilityAttribute("boostDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
                else return GetPassiveAttribute(points, numAttributes);
            case "reduceDamage":
                degree = Random.Range(15, 31);
                pointCost = degree * 14f / 3f;
                if (points >= pointCost) return new AbilityAttribute("reduceDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
                else return GetPassiveAttribute(points, numAttributes);
            case "reduceElementalDamage":
                degree = Random.Range(60, 151);
                pointCost = degree * 7f / 6f;
                elementRoll = Random.Range(0, 8);
                randomElement = elements[elementRoll];
                if (points >= pointCost) return new AbilityAttribute("reduceElementalDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f), new AbilityParameter("element", DataType.stringType, stringVal: randomElement));
                else return GetPassiveAttribute(points, numAttributes);
            case "boostElementalDamage":
                degree = Random.Range(60, 151);
                pointCost = degree * 7f / 6f;
                elementRoll = Random.Range(0, 8);
                randomElement = elements[elementRoll];
                if (points >= pointCost) return new AbilityAttribute("boostElementalDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f), new AbilityParameter("element", DataType.stringType, stringVal: randomElement));
                else return GetPassiveAttribute(points, numAttributes);
        }
    }
}
