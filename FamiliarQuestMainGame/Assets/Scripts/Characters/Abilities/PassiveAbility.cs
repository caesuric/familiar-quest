using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility : Ability {
    public PassiveAbility(string name, string description, int icon=0, params AbilityAttribute[] attributes) {
        this.name = name;
        this.description = description;
        this.icon = icon;
        foreach (var item in attributes) this.attributes.Add(item);
    }
    public static PassiveAbility Generate(float points) {
        var initialPoints = points;
        var priority = Random.Range(12.5f, 100f);
        int attributesRoll = Random.Range(0, 100);
        int numAttributes = Random.Range(0, 5);
        //if (attributesRoll >= 80) numAttributes = 2;
        var attributes = new List<AbilityAttribute>();
        for (int i=0; i<numAttributes; i++) {
            var attribute = AbilityAttribute.GetPassiveAttribute(points, priority, numAttributes);
            if (attribute != null) {
                if (attribute.priority >= 50 && i < 4) points -= attribute.points;
                attributes.Add(attribute);
            }
        }
        if (AbilityValid(attributes)) {
            var ability = new PassiveAbility("", "", 0, attributes.ToArray());
            ability.SortAttributes();
            ability.icon = AbilityIconGenerator.Retrieve(ability);
            ability.name = AbilityNamer.Name(ability);
            ability.points = (int)initialPoints;
            ability.description = AbilityDescriber.Describe(ability);
            return ability;
        }
        else return Generate(initialPoints);
    }

    public static bool AbilityValid(List<AbilityAttribute> attributes) {
        return true;
        //var validAttributeList = new List<string>() { "damageEnemiesOnScreen", "experienceBoost" };
        //foreach (var attribute in attributes) if (validAttributeList.Contains(attribute.type)) return true;
        //return false;
    }

    public int GetLevel() {
        var level = 1;
        float currentPoints = points;
        while (currentPoints > 70) {
            currentPoints /= 1.05f;
            level += 1;
        }
        return level;
    }

    public override Ability Copy() {
        var newAbility = new PassiveAbility(name, description, icon, new AbilityAttribute[] { });
        foreach (var attribute in attributes) newAbility.attributes.Add(attribute.Copy());
        return newAbility;
    }
}
