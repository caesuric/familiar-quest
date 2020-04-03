using System.Collections.Generic;
using UnityEngine;

public class SecondaryStatUtility {
    public static readonly List<int> averages = new List<int>() {
        10,
        13,
        16,
        18,
        22,
        27,
        32,
        38,
        47,
        55,
        64,
        75,
        90,
        96,
        117,
        134,
        163,
        184,
        205,
        246,
        280,
        306,
        361,
        412,
        466,
        529,
        591,
        676,
        759,
        865,
        967,
        1097,
        1243,
        1380,
        1550,
        1762,
        1955,
        2209,
        2451,
        2796,
        3050,
        3400,
        3811,
        4268,
        4694,
        5303,
        5870,
        6473,
        7120,
        7832
    };

    public static int CalcHp(int stat, int level) {
        float hp = stat * 29f;
        for (int i = 1; i < level; i++) hp *= 1.1f;
        return (int)hp;
    }

    public static float CalcReceivedHealing(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0.25f + (0.75f * percent * 2);
        else return 1f + ((percent - 0.5f) * 2);
    }

    public static float CalcHpRegen(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0f;
        else return (percent - 0.5f) * CalcHp(stat, level) / 100f;
    }

    public static float CalcArmorMultiplier(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0.25f + (0.75f * percent * 2);
        else return 1f + ((percent - 0.5f) * 2);
    }

    public static float CalcPhysicalResist(int stat, int level) {
        var percent = GetPercent(stat, level);
        return percent;
    }

    public static float CalcMentalResist(int stat, int level) {
        var percent = GetPercent(stat, level);
        return percent;
    }

    public static int CalcMp(int stat, int level) {
        float mp = stat * 29f;
        for (int i = 1; i < level; i++) mp *= 1.1f;
        return (int)mp;
    }

    public static float CalcMpRegenRate(int stat, int level) {
        float baseMpRegenPercent = 33 / (float)CalcMp(10, 1);
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0.25f + (0.75f * percent * 2) * baseMpRegenPercent * CalcMp(stat, level);
        else return (1f + ((percent - 0.5f) * 0.15f)) * baseMpRegenPercent * CalcMp(stat, level);
    }

    public static float CalcHealingMultiplier(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0.25f + (0.75f * percent * 2);
        else return 1f + ((percent - 0.5f) * 2);
    }

    public static float CalcMoveSpeed(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0.75f + (0.25f * percent * 2);
        else return 1f + ((percent - 0.5f) * 1);
    }

    public static float CalcCooldownReduction(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0;
        else return (percent - 0.5f) / 2f;
    }

    public static float CalcCriticalHitRate(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0.2f * percent * 2;
        else return 0.2f + (0.3f * (percent - 0.5f) * 2);
    }

    public static float CalcCriticalDamage(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 1f + (percent * 2);
        else return 2f + ((percent - 0.5f) * 2);
    }

    public static float CalcStatusEffectDurationBonus(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return -0.8f + (0.8f * percent * 2);
        else return (0.8f * (percent - 0.5f) * 2);
    }

    public static float CalcItemFindRate(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return (0.2f * percent * 2);
        else return 0.2f + (0.3f * (percent - 0.5f) * 2);
    }

    public static float CalcElementalResistanceFromLuck(int stat, int level) {
        var percent = GetPercent(stat, level);
        if (percent <= 0.5f) return 0;
        else return (0.5f * (percent - 0.5f) * 2);
    }

    public static int GetLevel(Component component) {
        var experienceGainer = component.GetComponent<ExperienceGainer>();
        var monsterScaler = component.GetComponent<MonsterScaler>();
        if (experienceGainer != null) return experienceGainer.level;
        if (monsterScaler != null) return monsterScaler.level;
        return 0;
    }

    public static int DetermineMaximum(int level) {
        //int primaryStat = 20;
        //int otherStats = 8;
        //for (int i = 1; i < level; i++) {
        //    int bonusPoints = (int)((primaryStat + (otherStats * 5f)) * 0.02f); //1.02f
        //    if (bonusPoints < 1) bonusPoints = 1;
        //    primaryStat = (int)Mathf.Max(primaryStat + 1f, primaryStat * 1.08f);
        //    otherStats = (int)Mathf.Max(otherStats + 1f, otherStats * 1.08f);
        //    primaryStat += bonusPoints;
        //}
        //return primaryStat;
        return averages[level - 1] * 2;
    }

    public static int DetermineMinimum(int level) {
        //int primaryStat = 1;
        //int otherStatTotal = 59;
        //for (int i = 1; i < level; i++) {
        //    int bonusPoints = (int)((primaryStat + (otherStatTotal)) * 0.02f); //1.02f
        //    if (bonusPoints < 1) bonusPoints = 1;
        //    primaryStat = (int)Mathf.Max(primaryStat + 1f, primaryStat * 1.08f);
        //    otherStatTotal = (int)Mathf.Max(otherStatTotal + 5f, otherStatTotal * 1.08f);
        //    otherStatTotal += bonusPoints;
        //}
        //return primaryStat;
        return 1;
    }

    public static int DetermineAverage(int level) {
        //int primaryStat = 10;
        //for (int i = 1; i < level; i++) {
        //    int bonusPoints = (int)((primaryStat * 6) * 0.02f);
        //    if (bonusPoints < 1) bonusPoints = 1;
        //    primaryStat = (int)Mathf.Max(primaryStat + 1f, primaryStat * 1.08f);
        //    primaryStat += (int)Mathf.Round(bonusPoints / 6);
        //}
        //return primaryStat;
        return averages[level - 1];
    }

    public static float GetPercent(int stat, int level) {
        var min = DetermineMinimum(level);
        var max = DetermineMaximum(level);
        var avg = DetermineAverage(level);
        stat = Mathf.Min(max, stat);
        stat = Mathf.Max(min, stat);
        if (stat == avg) return 0.5f;
        else if (stat < avg) {
            float range = avg - min;
            stat -= min;
            return stat / range * 0.5f;
        }
        else {
            float range = max - avg;
            stat -= avg;
            return 0.5f + (stat / range * 0.5f);
        }
    }
}
