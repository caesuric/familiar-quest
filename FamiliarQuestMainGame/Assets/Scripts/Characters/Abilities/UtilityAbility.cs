using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UtilityAbility: ActiveAbility {
    public string targetType = "";

    public override Ability Copy() {
        var newAbility = new UtilityAbility {
            name = name,
            description = description,
            baseStat = baseStat,
            icon = icon,
            cooldown = cooldown,
            mpUsage = mpUsage,
            baseMpUsage = baseMpUsage,
            radius = radius,
            points = points
        };
        foreach (var attribute in attributes) newAbility.attributes.Add(attribute.Copy());
        return newAbility;
    }

    protected override void LevelUp(int originalLevel, int targetLevel) {
        float targetPoints = 70f;
        for (int i = 1; i < targetLevel; i++) targetPoints *= 1.05f;
        var newAbility = AbilityScaler.ScaleUtilityAbility(targetPoints, cooldown, mpUsage, baseMpUsage, targetType, attributes);
        points = (int)targetPoints;
        mpUsage = newAbility.mpUsage;
        baseMpUsage = newAbility.baseMpUsage;
        attributes = newAbility.attributes;
        description = AbilityDescriber.Describe(this);
    }
}
