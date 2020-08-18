using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityAttribute {
    public string type;
    public float points;
    public float priority = 0;
    public List<AbilityAttributeParameter> parameters = new List<AbilityAttributeParameter>();

    public AbilityAttributeParameter FindParameter(string name) {
        foreach (var item in parameters) if (item.name == name) return item;
        return null;
    }

    public AbilityAttribute Copy() {
        var newAttribute = new AbilityAttribute {
            type = type,
            points = points,
            priority = priority,
            parameters = new List<AbilityAttributeParameter>()
        };
        foreach (var parameter in parameters) newAttribute.parameters.Add(parameter.Copy());
        return newAttribute;
    }
}

//delegate AbilityAttribute AbilityAttributeSource();

//public class AbilityAttribute {
//    public string type;
//    public float points;
//    public float priority = 0;
//    public List<AbilityParameter> parameters = new List<AbilityParameter>();

//    public AbilityAttribute(string type, float points, float priority, params AbilityParameter[] parameters) {
//        this.type = type;
//        this.points = points;
//        this.priority = priority;
//        foreach (var item in parameters) this.parameters.Add(item);
//    }
//    public AbilityAttribute(string type, params AbilityParameter[] parameters) {
//        this.type = type;
//        foreach (var item in parameters) this.parameters.Add(item);
//    }

//    public AbilityParameter FindParameter(string name) {
//        foreach (var item in parameters) if (item.name == name) return item;
//        return null;
//    }

//    public AbilityAttribute Copy() {
//        var newAttribute = new AbilityAttribute(type, points, priority, new AbilityParameter[] { });
//        foreach (var parameter in parameters) newAttribute.parameters.Add(parameter.Copy());
//        return newAttribute;
//    }

//    public static AbilityAttribute GetAttackAttribute(float points, float priority, int mp, bool isDot, float radius, bool isRanged, float cooldown, Element baseElement) {
//        return AttackAttributeTable.Retrieve(points, priority, mp, isDot, radius, isRanged, cooldown, baseElement);
//    }

//    public static AbilityAttribute GetUtilityAttribute(float points, float priority, int mp, float cooldown, int numAttributes, string targetType) {
//        return UtilityAttributeTable.Retrieve(points, priority, mp, cooldown, numAttributes, targetType);
//    }

//    public static AbilityAttribute GetPassiveAttribute(float points, float priority, int numAttributes) {
//        return PassiveAttributeTable.Retrieve(points, priority, numAttributes);
//    }
//}
