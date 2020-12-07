using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class AbilityCalculator {
    public static Dictionary<BaseStat, float> pointsMultiplierByBaseStat = new Dictionary<BaseStat, float> {
        [BaseStat.intelligence] = 1f,
        [BaseStat.dexterity] = 1f,
        [BaseStat.strength] = 3f
    };
    public static Dictionary<float, float> pointsMultiplierByRadius = new Dictionary<float, float> {
        [0f] = 1f,
        [2f] = 0.5f,
        [4f] = 1f / 8f,
        [6f] = 1f / 18f,
        [8f] = 1f / 32f
    };
    public static Dictionary<float, float> pointsMultiplierByDotTime = new Dictionary<float, float> {
        [0f] = 1f,
        [4f] = 1.25f,
        [8f] = 2f,
        [12f] = 2.5f
    };
    public static Dictionary<int, float> pointsMultiplierByMpUsage = new Dictionary<int, float> {
        [0] = 1f,
        [20] = 1.5f,
        [40] = 2f,
        [60] = 3f,
        [80] = 4f
    };
    public static Dictionary<float, float> pointsMultiplierByCooldown = new Dictionary<float, float> {
        [0f] = 1f,
        [1.5f] = 1.3f,
        [3f] = 1.4f,
        [8f] = 1.5f,
        [15f] = 1.67f,
        [30f] = 2f,
        [90f] = 2.22f,
        [150f] = 2.83f
    };

    public static float GetPointsFromLevel(int level) {
        float points = 70f;
        for (int i = 1; i < level; i++) points *= 1.05f;
        return points;
    }

    public static int GetLevelFromPoints(float points) {
        int level = 1;
        while (points > 70f) {
            points /= 1.05f;
            level++;
        }
        return level;
    }

    public static int ScaleMp(int baseMp, int level) {
        float tempMp = baseMp;
        for (int i = 1; i < level; i++) tempMp *= 1.1f;
        return (int)tempMp;
    }

    public static Tuple<int, float> GetBaseMpCostAndPointsMod() {
        var mpRoll = RNG.Int(0, 100);
        int mpUsage;
        if (mpRoll < 80) mpUsage = 40;
        else if (mpRoll < 90) mpUsage = 20;
        else if (mpRoll < 95) mpUsage = 60;
        else mpUsage = 80;
        return new Tuple<int, float>(mpUsage, pointsMultiplierByMpUsage[mpUsage]);
    }

    public static Tuple<float, float> GetCooldownAndPointsMod() {
        int hasCooldownRoll = RNG.Int(0, 100);
        if (hasCooldownRoll >= 35) return new Tuple<float, float>(0, 1f);
        var cooldownRoll = RNG.Int(0, 7);
        var cooldownList = new List<float> { 1.5f, 3f, 8f, 15f, 30f, 90f, 150f };
        var cooldown = cooldownList[cooldownRoll];
        return new Tuple<float, float>(cooldown, pointsMultiplierByCooldown[cooldown]);
    }

    public static Element StringToElement(string text) {
        return (Element)Enum.Parse(typeof(Element), text, true);
    }
}
