using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class PassiveAttributeTable {

    public static AbilityAttribute Retrieve(float points, int numAttributes) {
        try {
            if (points == 0) return null;
            string roll = TableRoller.Roll("PassiveAttributes");
            var table = new Dictionary<string, AbilityAttributeSource>() {
                { "damageEnemiesOnScreen", () => GetDamageEnemiesOnScreen(points, numAttributes) },
                { "experienceBoost", () => GetExperienceBoost(points, numAttributes) },
                { "knockback", () => GetKnockback(points, numAttributes) },
                { "charge", () => GetCharge(points, numAttributes) },
                { "pullEnemies", () => GetPullEnemies(points, numAttributes) },
                { "goldBoost", () => GetGoldBoost(points, numAttributes) },
                { "boostStat", () => GetBoostStat(points, numAttributes) },
                { "boostDamage", () => GetBoostDamage(points, numAttributes) },
                { "reduceDamage", () => GetReduceDamage(points, numAttributes) },
                { "reduceElementalDamage", () => GetReduceElementalDamage(points, numAttributes) },
                { "boostElementalDamage", () => GetBoostElementalDamage(points, numAttributes) }
            };
            return table[roll]();
        }
        catch {
            return null;
        }
    }

    private static AbilityAttribute GetDamageEnemiesOnScreen(float points, int numAttributes) {
        int degree = Random.Range(1, 23);
        float pointCost = degree * 70f;
        if (points >= pointCost) return new AbilityAttribute("damageEnemiesOnScreen", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree));
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetExperienceBoost(float points, int numAttributes) {
        int degree = Random.Range(5, 31);
        float pointCost = degree * 14f;
        if (points >= pointCost) return new AbilityAttribute("experienceBoost", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetKnockback(float points, int numAttributes) {
        if (points >= 139) return new AbilityAttribute("knockback", 139);
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetCharge(float points, int numAttributes) {
        if (points >= 70) return new AbilityAttribute("charge", 70);
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetPullEnemies(float points, int numAttributes) {
        if (points >= 70) return new AbilityAttribute("pullEnemies", 70);
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetGoldBoost(float points, int numAttributes) {
        int degree = Random.Range(5, 31);
        float pointCost = degree * 14f;
        if (points >= pointCost) return new AbilityAttribute("goldBoost", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetBoostStat(float points, int numAttributes) {
        int degree = Random.Range(3, 101);
        float pointCost = degree * 70f / 3f;
        var stats = new List<string> { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        int statRoll = Random.Range(0, 6);
        var randomStat = stats[statRoll];
        if (points >= pointCost) return new AbilityAttribute("boostStat", pointCost, new AbilityParameter("degree", DataType.intType, intVal: degree), new AbilityParameter("stat", DataType.stringType, stringVal: randomStat));
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetBoostDamage(float points, int numAttributes) {
        int degree = Random.Range(5, 31);
        float pointCost = degree * 14f;
        if (points >= pointCost) return new AbilityAttribute("boostDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetReduceDamage(float points, int numAttributes) {
        int degree = Random.Range(15, 31);
        float pointCost = degree * 14f / 3f;
        if (points >= pointCost) return new AbilityAttribute("reduceDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f));
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetReduceElementalDamage(float points, int numAttributes) {
        int degree = Random.Range(60, 151);
        float pointCost = degree * 7f / 6f;
        int elementRoll = Random.Range(0, 8);
        var elements = new List<string> { "piercing", "slashing", "bashing", "fire", "ice", "acid", "light", "dark" };
        string randomElement = elements[elementRoll];
        if (points >= pointCost) return new AbilityAttribute("reduceElementalDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f), new AbilityParameter("element", DataType.stringType, stringVal: randomElement));
        else return Retrieve(points, numAttributes);
    }

    private static AbilityAttribute GetBoostElementalDamage(float points, int numAttributes) {
        int degree = Random.Range(20, 151);
        float pointCost = degree * 7f / 2f;
        int elementRoll = Random.Range(0, 8);
        var elements = new List<string> { "piercing", "slashing", "bashing", "fire", "ice", "acid", "light", "dark" };
        var randomElement = elements[elementRoll];
        if (points >= pointCost) return new AbilityAttribute("boostElementalDamage", pointCost, new AbilityParameter("degree", DataType.floatType, floatVal: degree / 100f), new AbilityParameter("element", DataType.stringType, stringVal: randomElement));
        else return Retrieve(points, numAttributes);
    }
}
