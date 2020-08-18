using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility: Ability {
    public float cooldown = 0f;
    public float currentCooldown = 0f;
    public float mpUsage = 0;
    public float baseMpUsage = 0;
    public float radius = 0;
    public BaseStat baseStat;
}

//public abstract class ActiveAbility : Ability {
//    public float cooldown = 0f;
//    public float currentCooldown = 0f;
//    public int mpUsage = 0;
//    public int baseMpUsage = 0;
//    public float radius = 0;
//    public BaseStat baseStat;

//    public static ActiveAbility Generate(List<Element> baseTypes, int level = 1) {
//        int roll = Random.Range(1, 100);
//        //float points = 70f;
//        //if (level > 1) for (int i = 1; i < level; i++) points *= 1.05f;
//        if (roll < 87) return AttackAbilityTable.Retrieve(level);
//        else return UtilityAbilityTable.Retrieve(level);
//    }

//    public static AbilityAttribute FindAttribute(List<AbilityAttribute> list, string attribute) {
//        foreach (var item in list) if (item.type == attribute) return item;
//        return null;
//    }

//    public override Ability Copy() {
//        if (this is UtilityAbility) return this.CopyAsUtility();
//        else if (this is AttackAbility) return this.CopyAsAttack();
//        return null;
//    }

//    public ActiveAbility CopyAsUtility() {
//        var asUtil = (UtilityAbility)this;
//        var newAbility = new UtilityAbility(asUtil.name, asUtil.description, asUtil.baseStat, asUtil.icon, asUtil.cooldown, asUtil.mpUsage, asUtil.baseMpUsage, asUtil.radius, asUtil.points, new AbilityAttribute[] { });
//        foreach (var attribute in asUtil.attributes) newAbility.attributes.Add(attribute.Copy());
//        return newAbility;
//    }

//    public ActiveAbility CopyAsAttack() {
//        var asAttack = (AttackAbility)this;
//        var newAbility = new AttackAbility(asAttack.name, asAttack.description, asAttack.damage, asAttack.element, asAttack.baseStat, asAttack.icon, asAttack.dotDamage, asAttack.dotTime, asAttack.isRanged, asAttack.rangedProjectile, asAttack.cooldown, asAttack.mpUsage, asAttack.baseMpUsage, asAttack.radius, asAttack.hitEffect, asAttack.aoe, asAttack.points, new AbilityAttribute[] { });
//        foreach (var attribute in asAttack.attributes) newAbility.attributes.Add(attribute.Copy());
//        return newAbility;
//    }

//    public static void AddPointsToAbility(ActiveAbility ability, float points) {
//        ability.points += (int)points;
//    }

//    public int GetLevel() {
//        return GetLevelFromPoints(points);
//    }

//    public static int GetLevelFromPoints(float points) {
//        var level = 1;
//        float currentPoints = points;
//        while (currentPoints > 70) {
//            currentPoints /= 1.05f;
//            level += 1;
//        }
//        return level;
//    }

//    public static int CalculateMpUsage(int baseMp, float points) {
//        var level = GetLevelFromPoints(points);
//        float tempMp = baseMp;
//        for (int i = 0; i < level; i++) tempMp *= 1.05f;
//        return (int)tempMp;
//    }
//}
