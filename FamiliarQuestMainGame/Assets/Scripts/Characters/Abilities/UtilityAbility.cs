using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilityAbility : ActiveAbility {
    public string targetType = "";

    public UtilityAbility(string name, string description, BaseStat baseStat = BaseStat.strength, int icon = 0, float cooldown = 0f, int mpUsage = 0, int baseMpUsage = 0, float radius = 0, int points = 70, params AbilityAttribute[] attributes) {
        this.name = name;
        this.description = description;
        this.icon = icon;
        this.cooldown = cooldown;
        this.mpUsage = mpUsage;
        this.baseMpUsage = baseMpUsage;
        this.radius = radius;
        this.baseStat = baseStat;
        foreach (var item in attributes) this.attributes.Add(item);
    }
}