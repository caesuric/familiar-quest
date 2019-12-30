using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ability {
    public List<AbilityAttribute> attributes = new List<AbilityAttribute>();
    public int icon;
    public int points = 70;
    public string name;
    public string description;
    public abstract Ability Copy();

    public AbilityAttribute FindAttribute(string attribute) {
        foreach (var item in attributes) if (item.type == attribute) return item;
        return null;
    }
}
