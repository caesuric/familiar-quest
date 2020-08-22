using UnityEngine;
using System.Collections;
using System;

public class AbilityAttributeParameter {
    public string name;
    public object value;

    public AbilityAttributeParameter Copy() {
        return new AbilityAttributeParameter {
            name = name,
            value = value
        };
    }
}
