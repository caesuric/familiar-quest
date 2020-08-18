using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility: Ability {
    public override Ability Copy() {
        var newAbility = new PassiveAbility {
            name = name,
            description = description,
            icon = icon
        };
        foreach (var attribute in attributes) newAbility.attributes.Add(attribute.Copy());
        return newAbility;
    }
}

//public class PassiveAbility : Ability {
//    public PassiveAbility(string name, string description, int icon=0, params AbilityAttribute[] attributes) {
//        this.name = name;
//        this.description = description;
//        this.icon = icon;
//        foreach (var item in attributes) this.attributes.Add(item);
//    }
//    public static PassiveAbility Generate(float points) {
//        var initialPoints = points;
//        var priority = Random.Range(12.5f, 100f);
//        int attributesRoll = Random.Range(0, 100);
//        int numAttributes = Random.Range(1, 5);
//        //if (attributesRoll >= 80) numAttributes = 2;
//        var attributes = new List<AbilityAttribute>();
//        for (int i=0; i<numAttributes; i++) {
//            var attribute = AbilityAttribute.GetPassiveAttribute(points, priority, numAttributes);
//            if (attribute != null) {
//                if (attribute.priority >= 50 && i < 4) points -= attribute.points;
//                attributes.Add(attribute);
//            }
//        }
//        if (AbilityValid(attributes)) {
//            var ability = new PassiveAbility("", "", 0, attributes.ToArray());
//            ability.SortAttributes();
//            ability.icon = AbilityIconGenerator.Retrieve(ability);
//            ability.name = AbilityNamer.Name(ability);
//            ability.points = (int)initialPoints;
//            ability.SetLevel(GetLevelFromPoints((int)initialPoints));
//            ability.description = AbilityDescriber.Describe(ability);
//            return ability;
//        }
//        else return Generate(initialPoints);
//    }

//    public static bool AbilityValid(List<AbilityAttribute> attributes) {
//        foreach (var attr in attributes) if (attr.priority >= 50) return true;
//        return false;
//    }

//    public int GetLevel() {
//        var level = 1;
//        float currentPoints = points;
//        while (currentPoints > 70) {
//            currentPoints /= 1.05f;
//            level += 1;
//        }
//        return level;
//    }

//    public override Ability Copy() {
//        var newAbility = new PassiveAbility(name, description, icon, new AbilityAttribute[] { });
//        foreach (var attribute in attributes) newAbility.attributes.Add(attribute.Copy());
//        return newAbility;
//    }

//    protected override void LevelUp(int originalLevel, int targetLevel) {
//        float originalPoints = 70f;
//        float targetPoints = 70f;
//        for (int i = 1; i < originalLevel; i++) originalPoints *= 1.05f;
//        for (int i = 1; i < targetLevel; i++) targetPoints *= 1.05f;
//        float ratio = targetPoints / originalPoints;
//        foreach (var attribute in attributes) {
//            foreach (var parameter in attribute.parameters) {
//                if (parameter.type == DataType.floatType) parameter.floatVal *= ratio;
//                else if (parameter.type == DataType.intType) parameter.intVal = (int)(parameter.intVal * ratio);
//            }
//        }
//        description = AbilityDescriber.Describe(this);
//    }
//}
