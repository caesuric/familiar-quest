﻿using System.Collections.Generic;
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

    protected override void LevelUp(int originalLevel, int targetLevel) {
        LevelUp();
        float originalPoints = 70f;
        float targetPoints = 70f;
        for (int i = 1; i < originalLevel; i++) originalPoints *= 1.05f;
        for (int i = 1; i < targetLevel; i++) targetPoints *= 1.05f;
        float ratio = targetPoints / originalPoints;
        foreach (var attribute in attributes) {
            foreach (var parameter in attribute.parameters) {
                if (parameter.value is float) parameter.value = (float)parameter.value * ratio;
                else if (parameter.value is int) parameter.value = (int)((float)parameter.value * ratio);
            }
        }
        level = targetLevel;
        description = AbilityDescriber.Describe(this);
    }
}
