using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AbilityCalculator {
    public static float GetPointsFromLevel(int level) {
        float points = 70f;
        for (int i = 1; i < level; i++) points *= 1.05f;
        return points;
    }
}
