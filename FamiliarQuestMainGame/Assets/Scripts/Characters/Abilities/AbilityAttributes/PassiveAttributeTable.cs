using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PassiveAttributeTable {

    public static AbilityAttribute Retrieve(float points, float priority, int numAttributes) {
        try {
            if (points == 0) return null;
            string roll = TableRoller.Roll("PassiveAttributes");
            var table = new Dictionary<string, AbilityAttributeSource>() {
                { "damageEnemiesOnScreen", () => GetDamageEnemiesOnScreen(points, priority, numAttributes) },
                { "experienceBoost", () => GetExperienceBoost(points, priority, numAttributes) },
                { "knockback", () => GetKnockback(points, priority, numAttributes) },
                { "charge", () => GetCharge(points, priority, numAttributes) },
                { "pullEnemies", () => GetPullEnemies(points, priority, numAttributes) },
                { "goldBoost", () => GetGoldBoost(points, priority, numAttributes) },
                { "boostStat", () => GetBoostStat(points, priority, numAttributes) },
                { "boostDamage", () => GetBoostDamage(points, priority, numAttributes) },
                { "reduceDamage", () => GetReduceDamage(points, priority, numAttributes) },
                { "reduceElementalDamage", () => GetReduceElementalDamage(points, priority, numAttributes) },
                { "boostElementalDamage", () => GetBoostElementalDamage(points, priority, numAttributes) }
            };
            return table[roll]();
        }
        catch {
            return null;
        }
    }

    private static AbilityAttribute GetDamageEnemiesOnScreen(float points, float priority, int numAttributes) {
        int degree = Random.Range(1, 23);
        float pointCost = degree * 70f;
        if (points >= pointCost) return new AbilityAttribute("damageEnemiesOnScreen", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degree));
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetExperienceBoost(float points, float priority, int numAttributes) {
        int degree = Random.Range(5, 31);
        float pointCost = degree * 14f;
        if (points >= pointCost) return new AbilityAttribute("experienceBoost", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetKnockback(float points, float priority, int numAttributes) {
        if (points >= 139) return new AbilityAttribute("knockback", 139, priority);
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetCharge(float points, float priority, int numAttributes) {
        if (points >= 70) return new AbilityAttribute("charge", 70, priority);
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetPullEnemies(float points, float priority, int numAttributes) {
        if (points >= 70) return new AbilityAttribute("pullEnemies", 70, priority);
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetGoldBoost(float points, float priority, int numAttributes) {
        int degree = Random.Range(5, 31);
        float pointCost = degree * 14f;
        if (points >= pointCost) return new AbilityAttribute("goldBoost", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetBoostStat(float points, float priority, int numAttributes) {
        int degree = Random.Range(3, 101);
        float pointCost = degree * 70f / 3f;
        var stats = new List<string> { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        int statRoll = Random.Range(0, 6);
        var randomStat = stats[statRoll];
        if (points >= pointCost) return new AbilityAttribute("boostStat", pointCost, priority, new AbilityParameter("degree", DataType.intType, intVal: degree), new AbilityParameter("stat", DataType.stringType, stringVal: randomStat));
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetBoostDamage(float points, float priority, int numAttributes) {
        int degree = Random.Range(5, 31);
        float pointCost = degree * 14f;
        if (points >= pointCost) return new AbilityAttribute("boostDamage", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetReduceDamage(float points, float priority, int numAttributes) {
        int degree = Random.Range(15, 31);
        float pointCost = degree * 14f / 3f;
        if (points >= pointCost) return new AbilityAttribute("reduceDamage", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetReduceElementalDamage(float points, float priority, int numAttributes) {
        int degree = Random.Range(60, 151);
        float pointCost = degree * 7f / 6f;
        int elementRoll = Random.Range(0, 8);
        var elements = new List<string> { "piercing", "slashing", "bashing", "fire", "ice", "acid", "light", "dark" };
        string randomElement = elements[elementRoll];
        if (points >= pointCost) return new AbilityAttribute("reduceElementalDamage", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f), new AbilityParameter("element", DataType.stringType, stringVal: randomElement));
        else return Retrieve(points, priority, numAttributes);
    }

    private static AbilityAttribute GetBoostElementalDamage(float points, float priority, int numAttributes) {
        int degree = Random.Range(20, 151);
        float pointCost = degree * 7f / 2f;
        int elementRoll = Random.Range(0, 8);
        var elements = new List<string> { "piercing", "slashing", "bashing", "fire", "ice", "acid", "light", "dark" };
        var randomElement = elements[elementRoll];
        if (points >= pointCost) return new AbilityAttribute("boostElementalDamage", pointCost, priority, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f), new AbilityParameter("element", DataType.stringType, stringVal: randomElement));
        else return Retrieve(points, priority, numAttributes);
    }
}
