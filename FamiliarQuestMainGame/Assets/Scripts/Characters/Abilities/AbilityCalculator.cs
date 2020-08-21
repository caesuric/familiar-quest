using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public static class AbilityCalculator {
    public static float GetPointsFromLevel(int level) {
        float points = 70f;
        for (int i = 1; i < level; i++) points *= 1.05f;
        return points;
    }

    public static int GetLevelFromPoints(float points) {
        int level = 1;
        while (points>70f) {
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
        if (mpRoll < 80) return new Tuple<int, float>(40, 2f);
        else if (mpRoll < 90) return new Tuple<int, float>(20, 1.5f);
        else if (mpRoll < 95) return new Tuple<int, float>(60, 3f);
        else return new Tuple<int, float>(80, 4f);
    }

    public static Tuple<float, float> GetCooldownAndPointsMod() {
        int hasCooldownRoll = RNG.Int(0, 100);
        if (hasCooldownRoll >= 35) return new Tuple<float, float>(0, 1f);
        var cooldownRoll = RNG.Int(0, 7);
        if (cooldownRoll == 0) return new Tuple<float, float>(1.5f, 1.3f);
        else if (cooldownRoll == 1) return new Tuple<float, float>(3f, 1.4f);
        else if (cooldownRoll == 2) return new Tuple<float, float>(8f, 1.5f);
        else if (cooldownRoll == 3) return new Tuple<float, float>(15f, 1.67f);
        else if (cooldownRoll == 4) return new Tuple<float, float>(30f, 2f);
        else if (cooldownRoll == 5) return new Tuple<float, float>(90f, 2.22f);
        else return new Tuple<float, float>(150f, 5f);
    }
}
