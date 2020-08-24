using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AbilityAttribute {
    public string type;
    public float points;
    public float priority = 0;
    public List<AbilityAttributeParameter> parameters = new List<AbilityAttributeParameter>();

    public AbilityAttributeParameter FindParameter(string name) {
        foreach (var item in parameters) if (item.name == name) return item;
        return null;
    }

    public bool ContainsAttribute(string name) {
        foreach (var item in parameters) if (item.name == name) return true;
        return false;
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
