using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackAbility: ActiveAbility {
    public float damage = 0;
    public float dotDamage = 0;
    public float dotTime = 1;
    public Element element;
    public bool isRanged = false;
    public int rangedProjectile = 0;
    public int hitEffect = 0;
    public int aoe = 0;

    public new bool IsValid() {
        if (!base.IsValid()) return false;
        if (FindAttribute("paralyze") != null && cooldown == 0) return false;
        int knockbackCount = 0;
        int pullTowardsCount = 0;
        foreach (var attribute in attributes) {
            if (attribute.type == "knockback") knockbackCount++;
            else if (attribute.type == "pullTowards") pullTowardsCount++;
            if (knockbackCount > 1 || pullTowardsCount > 1) return false;
        }
        return true;
    }

    public float CalculateDotDamage(Character character) {
        float attackPower;
        if (character.GetComponent<PlayerCharacter>() != null) attackPower = character.GetComponent<PlayerCharacter>().weapon.attackPower;
        else attackPower = character.GetComponent<Monster>().attackFactor;
        if (baseStat == BaseStat.strength) attackPower *= CharacterAttribute.attributes["strength"].instances[character].TotalValue;
        else if (baseStat == BaseStat.dexterity) attackPower *= CharacterAttribute.attributes["dexterity"].instances[character].TotalValue;
        else attackPower *= CharacterAttribute.attributes["intelligence"].instances[character].TotalValue;
        return attackPower * dotDamage / dotTime;
    }

    public override Ability Copy() {
        var newAbility = new AttackAbility {
            name = name,
            description = description,
            damage = damage,
            element = element,
            baseStat = baseStat,
            icon = icon,
            dotDamage = dotDamage,
            dotTime = dotTime,
            isRanged = isRanged,
            rangedProjectile = rangedProjectile,
            cooldown = cooldown,
            currentCooldown = currentCooldown,
            mpUsage = mpUsage,
            baseMpUsage = baseMpUsage,
            radius = radius,
            hitEffect = hitEffect,
            aoe = aoe,
            points = points
        };
        foreach (var attribute in attributes) newAbility.attributes.Add(attribute.Copy());
        return newAbility;
    }

    protected override void LevelUp(int originalLevel, int targetLevel) {
        LevelUp(targetLevel - originalLevel);
        float targetPoints = AbilityCalculator.GetPointsFromLevel(targetLevel);
        AbilityScaler.RemoveSkillTreeEnhancements(this);
        var newAbility = AbilityScaler.ScaleAttackAbility((int)targetPoints, element, baseStat, damage, dotDamage, dotTime, isRanged, cooldown, mpUsage, baseMpUsage, radius, icon, hitEffect, rangedProjectile, aoe, attributes, skillTree);
        level = targetLevel;
        points = targetPoints;
        damage = newAbility.damage;
        dotDamage = newAbility.dotDamage;
        mpUsage = newAbility.mpUsage;
        baseMpUsage = newAbility.baseMpUsage;
        attributes = newAbility.attributes;
        AbilityScaler.AddSkillTreeEnhancements(this);
        description = AbilityDescriber.Describe(this);
    }
}
