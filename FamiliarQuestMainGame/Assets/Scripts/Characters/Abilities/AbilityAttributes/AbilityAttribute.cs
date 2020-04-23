using UnityEngine;
using System.Collections;
using System.Collections.Generic;

delegate AbilityAttribute AbilityAttributeSource();

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
        return AttackAttributeTable.Retrieve(points, mp, isDot, radius, isRanged, cooldown, baseElement);
    }

    public static AbilityAttribute GetUtilityAttribute(float points, int mp, float cooldown, int numAttributes, string targetType) {
        return UtilityAttributeTable.Retrieve(points, mp, cooldown, numAttributes, targetType);
    }

    public static AbilityAttribute GetPassiveAttribute(float points, int numAttributes) {
        return PassiveAttributeTable.Retrieve(points, numAttributes);
    }
}
