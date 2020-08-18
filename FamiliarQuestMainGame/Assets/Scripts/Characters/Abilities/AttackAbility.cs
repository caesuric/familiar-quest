using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackAbility: ActiveAbility {

}

//public class AttackAbility : ActiveAbility {
//    public float damage = 0;
//    public float dotDamage = 0;
//    public float dotTime = 1;
//    public Element element;
//    public bool isRanged = false;
//    public int rangedProjectile = 0;
//    public int hitEffect = 0;
//    public int aoe = 0;

//    public AttackAbility(string name, string description, float damage, Element element, BaseStat baseStat, int icon = 0, float dotDamage = 0, float dotTime = 1, bool isRanged = false, int rangedProjectile = 0, float cooldown = 0f, int mpUsage = 0, int baseMpUsage=0, float radius = 0f, int hitEffect = 0, int aoe = 0, int points = 70, params AbilityAttribute[] attributes) {
//        this.name = name;
//        this.description = description;
//        this.damage = damage;
//        this.element = element;
//        this.baseStat = baseStat;
//        this.dotDamage = dotDamage;
//        this.dotTime = dotTime;
//        this.isRanged = isRanged;
//        this.rangedProjectile = rangedProjectile;
//        this.icon = icon;
//        this.cooldown = cooldown;
//        this.mpUsage = mpUsage;
//        this.baseMpUsage = baseMpUsage;
//        this.radius = radius;
//        this.hitEffect = hitEffect;
//        this.aoe = aoe;
//        foreach (var item in attributes) this.attributes.Add(item);
//    }

//    public static float CalculateDamage(float points) {
//        return 1f / 70f * points;
//    }

//    public float CalculateDotDamage(Character character) {
//        float attackPower;
//        if (character.GetComponent<PlayerCharacter>() != null) {
//            attackPower = character.GetComponent<PlayerCharacter>().weapon.attackPower;
//        }
//        else {
//            attackPower = character.GetComponent<Monster>().attackFactor;
//        }
//        if (baseStat == BaseStat.strength) {
//            attackPower *= CharacterAttribute.attributes["strength"].instances[character].TotalValue;
//            //attackPower *= character.strength;
//        }
//        else if (baseStat == BaseStat.dexterity) {
//            attackPower *= CharacterAttribute.attributes["dexterity"].instances[character].TotalValue;
//            //attackPower *= character.dexterity;
//        }
//        else {
//            attackPower *= CharacterAttribute.attributes["intelligence"].instances[character].TotalValue;
//            //attackPower *= character.intelligence;
//        }
//        return attackPower * dotDamage / dotTime;
//    }

//    protected override void LevelUp(int originalLevel, int targetLevel) {
//        float targetPoints = 70f;
//        for (int i = 1; i < targetLevel; i++) targetPoints *= 1.05f;
//        var newAbility = AbilityFusion.CreateNewAttackAbilityForFusion((int)targetPoints, element, baseStat, damage, dotDamage, dotTime, isRanged, cooldown, mpUsage, baseMpUsage, radius, icon, hitEffect, rangedProjectile, aoe, attributes);
//        points = (int)targetPoints;
//        damage = newAbility.damage;
//        dotDamage = newAbility.dotDamage;
//        mpUsage = newAbility.mpUsage;
//        baseMpUsage = newAbility.baseMpUsage;
//        attributes = newAbility.attributes;
//        description = AbilityDescriber.Describe(this);
//    }
//}
