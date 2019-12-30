 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAbility : Ability {
    public float cooldown = 0f;
    public float currentCooldown = 0f;
    public int mpUsage = 0;
    public int baseMpUsage = 0;
    public float radius = 0;
    public BaseStat baseStat;
    public static Dictionary<string, List<ActiveAbility>> classDefaults = new Dictionary<string, List<ActiveAbility>>();
    static ActiveAbility() {
        classDefaults.Add("archer", new List<ActiveAbility>() {
            new AttackAbility("Repel", "<b>Ranged piercing attack, uses {{baseStat}}</b>\nDeals {{damage}} damage.\n\nKnockback", 1f, Element.piercing, BaseStat.dexterity, isRanged: true, icon: 3, attributes: new AbilityAttribute("knockback", new AbilityParameter("degree", DataType.floatType, floatVal: 5.0f))),
            new UtilityAbility("Killer Instinct", "<b>Utility</b>\n<b>Cooldown</b>: 1m\n\nDamage target deals is increased by 50% for 15 seconds.\n", icon: 4, cooldown: 60f, attributes: new AbilityAttribute("damage+", new AbilityParameter("degree", DataType.floatType, floatVal: 0.5f), new AbilityParameter("duration", DataType.floatType, floatVal: 15f))),
            new UtilityAbility("Swiftness", "<b>Utility</b>\n<b>Cooldown</b>: 1m\n\nTarget's speed is doubled for 15 seconds.\n", icon: 5, cooldown: 60f, attributes: new AbilityAttribute("speed+", new AbilityParameter("degree", DataType.floatType, floatVal: 1f), new AbilityParameter("duration", DataType.floatType, floatVal: 15f)))
        });
        classDefaults.Add("infernoMage", new List<ActiveAbility>() {
            new AttackAbility("Firebolt", "<b>Ranged fire attack, uses {{baseStat}}</b>\n<b>MP</b>: 40\nDeals {{damage}} damage.\n", 1.25f, Element.fire, BaseStat.intelligence, isRanged: true, icon: 9, rangedProjectile: 1, mpUsage: 40),
            new AttackAbility("Fireball", "<b>Ranged fire attack, uses {{baseStat}}</b>\n<b>MP</b>: 40\n<b>Radius</b>: 2\n<b>Cooldown</b>: 8s\nDeals {{damage}} damage.\n", 1.76f, Element.fire, BaseStat.intelligence, isRanged: true, icon: 10, rangedProjectile: 1, cooldown: 8,  mpUsage: 40, radius: 2, aoe: 3),
            new UtilityAbility("Quelling Words", "<b>Utility</b>\n<b>Radius</b>: 10\n<b>Cooldown</b>: 1m30s\n\nCauses enemy action to cease for 3 seconds.", icon: 11, cooldown: 90f, radius: 10f, attributes: new AbilityAttribute("timestop", new AbilityParameter("duration", DataType.floatType, floatVal: 3f)))
        });
        classDefaults.Add("warlock", new List<ActiveAbility>() {
            new AttackAbility("Curse", "<b>Ranged dark attack, uses {{baseStat}}</b>\n<b>MP</b>: 40\nDeals {{dotDamage}} damage over 4 seconds.\n", 0f, Element.dark, BaseStat.intelligence, isRanged: true, icon: 12, rangedProjectile: 7, dotDamage: 1.875f, dotTime: 4f, mpUsage: 40),
            //new UtilityAbility("Consume Ailment", "Removes a negative status ailment and restores 50 MP.\nCooldown: 1m", icon: 13, cooldown: 60f, attributes: new AbilityAttribute("eatDebuff")),
            new AttackAbility("Cloudkill", "<b>Ranged acid attack, uses {{baseStat}}</b>\n<b>MP</b>: 80\n<b>Radius</b>: 3\n<b>Cooldown</b>: 30s\nDeals {{dotDamage}} damage over 6 seconds.\n\nCreates Damage Zone", 0f, Element.acid, BaseStat.intelligence, isRanged: true, icon: 50, rangedProjectile: 2, radius: 3, dotDamage: 2.47f, dotTime: 6f, mpUsage: 80, cooldown: 8, aoe: 0, attributes: new AbilityAttribute("createDamageZone")),
            new AttackAbility("Absorbtion", "<b>Melee dark attack, uses {{baseStat}}</b>\nDeals {{damage}} damage.\n\nHeals you for damage dealt\n", 0.75f, Element.dark, BaseStat.intelligence, icon: 14, attributes: new AbilityAttribute("lifeleech", new AbilityParameter("degree", DataType.floatType, floatVal: 1f)))
        });
        classDefaults.Add("paladin", new List<ActiveAbility>() {
            new AttackAbility("Brave Strike", "<b>Melee piercing attack, uses {{baseStat}}</b>\nDeals {{damage}} damage.\n\nGenerates x4 aggro\n", 1.5f, Element.piercing, BaseStat.strength, icon: 15, attributes: new AbilityAttribute("aggroMultiplier", new AbilityParameter("degree", DataType.floatType, floatVal: 4f))),
            new AttackAbility("Smite", "<b>Melee light attack, uses {{baseStat}}</b>\n<b>MP</b>: 40\nDeals {{damage}} damage.", 1.875f, Element.light, BaseStat.strength, hitEffect: 6, icon: 16, mpUsage: 40),
            new UtilityAbility("Lay on Hands", "<b>Utility, uses wisdom</b>\n<b>Cooldown</b>: 30s\n\nHeal target for {{healing}}.", icon: 17, cooldown: 30f, attributes: new AbilityAttribute("heal", new AbilityParameter("degree", DataType.floatType, floatVal: 61.53f)))
        });
        classDefaults.Add("fighter", new List<ActiveAbility>() {
            //new AttackAbility("Whirlwind Attack", "<b>Melee piercing attack, uses {{baseStat}}</b>\n<b>Radius</b>: 4\nDeals {{damage}} damage.\n\nGenerates x4 aggro\n", 0.375f, Element.piercing, BaseStat.strength, icon: 18, radius: 2f, attributes: new AbilityAttribute("aggroMultiplier", new AbilityParameter("degree", DataType.floatType, floatVal: 4f))),
            new AttackAbility("Attack", "<b>Melee slashing attack, uses {{baseStat}}</b>\nDeals {{damage}} damage.\n\nGenerates x4 aggro\n", 1.5f, Element.slashing, BaseStat.strength, icon: 18, attributes: new AbilityAttribute("aggroMultiplier", new AbilityParameter("degree", DataType.floatType, floatVal: 4f))),
            new UtilityAbility("Perfect Defense", "<b>Utility, uses strength</b>\n<b>Cooldown</b>: 1m\n\nShield self for {{shield}}.\n", icon: 19, cooldown: 60f, attributes: new AbilityAttribute("shield", new AbilityParameter("degree", DataType.floatType, floatVal: 15.3825f), new AbilityParameter("stat", DataType.stringType, stringVal: "strength"))),
            new AttackAbility("Stun", "<b>Melee bashing attack, uses {{baseStat}}</b>\n<b>Cooldown</b>: 2s\nDeals {{damage}} damage.\n\nKnockback\nInflicts paralysis for 3 seconds", 1.31f, Element.bashing, BaseStat.strength, icon: 20, cooldown: 3f, hitEffect: 1, attributes: new AbilityAttribute[] {new AbilityAttribute("paralyze", new AbilityParameter("duration", DataType.floatType, floatVal: 3f)), new AbilityAttribute("knockback", new AbilityParameter("degree", DataType.floatType, floatVal: 5f))})

        });
        classDefaults.Add("cleric", new List<ActiveAbility>() {
            new AttackAbility("Bash", "<b>Melee bashing attack, uses {{baseStat}}</b>\nDeals {{damage}} damage.", 1.5f, Element.bashing, BaseStat.strength, icon: 15),
            new UtilityAbility("Cure", "<b>Utility, uses wisdom</b>\n<b>MP</b>: 60\n\nHeals target for {{healing}}.", icon: 21, mpUsage: 60, attributes: new AbilityAttribute("heal", new AbilityParameter("degree", DataType.floatType, floatVal: 6f))),
            // new UtilityAbility("Freedom", "<b>Utility</b>\n<b>MP</b>: 40\n<b>Cooldown</b>: 30s   \n\nRemove all debuffs from target.\nOnly triggers cooldown if debuffs are removed.\nUsable while paralyzed.\n", icon: 22, cooldown: 30f, mpUsage: 40, attributes: new AbilityAttribute[] {new AbilityAttribute("removeAllDebuffs"), new AbilityAttribute("usableWhileParalyzed")}),
            new AttackAbility("Flamestrike", "<b>Ranged fire attack, uses {{baseStat}}</b>\n<b>MP</b>: 60\n<b>Radius</b>: 4\n<b>Cooldown</b>: 8s\nDeals {{damage}} damage.\n", 0.57f, Element.fire, BaseStat.intelligence, cooldown: 8f, isRanged: true, icon: 23, rangedProjectile: 1, mpUsage: 60, radius: 4, aoe: 3)
        });
        classDefaults.Add("thief", new List<ActiveAbility>() {
            new AttackAbility("Backstab", "<b>Melee piercing attack, uses {{baseStat}}</b>\nDeals {{damage}} damage.\n\nDamage x4 if in stealth.", 1.375f, Element.piercing, BaseStat.strength, icon: 63, attributes: new AbilityAttribute("backstab", new AbilityParameter("degree", DataType.floatType, floatVal: 4))),
            new UtilityAbility("Disable Device", "<b>Utility</b>\n<b>Range</b>: 2\n\nUnlocks doors and chests and disarms traps\nDoesn't break stealth", icon: 62, attributes: new AbilityAttribute[] {new AbilityAttribute("disableDevice", new AbilityParameter("radius", DataType.floatType, floatVal: 2f)), new AbilityAttribute("stealthy")}),
            new UtilityAbility("Stealth", "<b>Utility</b>\n<b>Cooldown</b>: 3s\n\nEnters or exits stealth mode. In stealth mode, you move at half speed, enemies can only see you in a limited range straight ahead of them, and footsteps are silenced. Animals can still smell you. Can't be used in combat.", icon: 64, cooldown: 3f, attributes: new AbilityAttribute("stealth"))
        });
    }

    public static ActiveAbility Generate(List<Element> baseTypes, int level = 1) {
        int roll = Random.Range(1, 100);
        float points = 70f;
        if (level > 1) for (int i = 1; i < level; i++) points *= 1.05f;
        if (roll < 87) return AttackAbility.Generate(baseTypes, points);
        else return UtilityAbility.Generate(baseTypes, points);
    }

    public static AbilityAttribute FindAttribute(List<AbilityAttribute> list, string attribute) {
        foreach (var item in list) if (item.type == attribute) return item;
        return null;
    }

    public static ActiveAbility Fuse(ActiveAbility ability1, ActiveAbility ability2) {
        float calcPoints = (ability1.points + ability2.points) / 2;
        calcPoints *= 1.05f;
        float maxPoints = 70;
        for (int i = 1; i < PlayerCharacter.localPlayer.GetComponent<ExperienceGainer>().level; i++) maxPoints = maxPoints * 1.05f;
        if (calcPoints > maxPoints) calcPoints = maxPoints;
        int points = (int)calcPoints;
        bool isAttack = false;
        if (AbilityMenu.instance.fusionAbilityTypeChoice == 0 && ability1 is AttackAbility) isAttack = true;
        else if (AbilityMenu.instance.fusionAbilityTypeChoice == 1 && ability2 is AttackAbility) isAttack = true;
        if (isAttack) return FuseAttack(points, ability1, ability2);
        else return FuseUtility(points, ability1, ability2);
    }

    public static AttackAbility FuseAttack(int points, ActiveAbility ability1, ActiveAbility ability2) {
        var am = AbilityMenu.instance;
        Element element;
        int icon, hitEffect, projectile, aoe;
        if (am.fusionElementChoice == 0) {
            element = ((AttackAbility)ability1).element;
            icon = ((AttackAbility)ability1).icon;
            hitEffect = ((AttackAbility)ability1).hitEffect;
            projectile = ((AttackAbility)ability1).rangedProjectile;
        }
        else {
            element = ((AttackAbility)ability2).element;
            icon = ((AttackAbility)ability2).icon;
            hitEffect = ((AttackAbility)ability2).hitEffect;
            projectile = ((AttackAbility)ability2).rangedProjectile;
        }
        aoe = GetAoe(element);
        BaseStat baseStat;
        if (am.fusionBaseStatChoice == 0) baseStat = ability1.baseStat;
        else baseStat = ability2.baseStat;
        float damageRatio;
        float dotDamageRatio;
        float dotTime;
        if (am.fusionDotChoice == 0) {
            damageRatio = ((AttackAbility)ability1).damage;
            dotDamageRatio = ((AttackAbility)ability1).dotDamage;
            dotTime = ((AttackAbility)ability1).dotTime;
        }
        else {
            damageRatio = ((AttackAbility)ability2).damage;
            dotDamageRatio = ((AttackAbility)ability2).dotDamage;
            dotTime = ((AttackAbility)ability2).dotTime;
        }
        bool isRanged = am.fusionIsRangedChoice;
        float cooldown;
        if (am.fusionCooldownChoice == 0) cooldown = ability1.cooldown;
        else cooldown = ability2.cooldown;
        int mp, baseMp;
        if (am.fusionMpUsageChoice == 0) {
            mp = ability1.mpUsage;
            baseMp = ability1.baseMpUsage;
        }
        else {
            mp = ability2.mpUsage;
            baseMp = ability2.baseMpUsage;
        }
        float radius;
        if (am.fusionRadiusChoice == 0) radius = ((AttackAbility)ability1).radius;
        else radius = ((AttackAbility)ability2).radius;
        return CreateNewAttackAbilityForFusion(points, element, baseStat, damageRatio, dotDamageRatio, dotTime, isRanged, cooldown, mp, baseMp, radius, icon, hitEffect, projectile, aoe, am.fusionAbilityAttributesSelected);
    }

    private static AttackAbility CreateNewAttackAbilityForFusion(int points, Element element, BaseStat baseStat, float damageRatio, float dotDamageRatio, float dotTime, bool isRanged, float cooldown, int mp, int baseMp, float radius, int icon, int hitEffect, int projectile, int aoe, List<AbilityAttribute> abilityAttributes) {
        var startingPoints = points;
        List<AbilityAttribute> paralysis = new List<AbilityAttribute>();
        foreach (var attribute in abilityAttributes) if (attribute.type == "paralyze") paralysis.Add(attribute);
        if (paralysis.Count>0 && cooldown==0) foreach (var attribute in paralysis) abilityAttributes.Remove(attribute);
        points = CalculateAttackAbilityPoints(points, damageRatio, dotDamageRatio, dotTime, isRanged, cooldown, mp, baseMp, radius, abilityAttributes);
        var totalDamage = AttackAbility.CalculateDamage(points);
        var regularDamage = totalDamage * damageRatio / (damageRatio + dotDamageRatio);
        var dotDamage = totalDamage * dotDamageRatio / (damageRatio + dotDamageRatio);
        var newAbility = new AttackAbility("", "", regularDamage, element, baseStat, dotDamage: dotDamage, dotTime: dotTime, isRanged: isRanged, cooldown: cooldown, mpUsage: CalculateMpUsage(baseMp, points), baseMpUsage: baseMp, radius: radius, icon: icon, hitEffect: hitEffect, rangedProjectile: projectile, aoe: aoe, attributes: abilityAttributes.ToArray());
        newAbility.points = startingPoints;
        newAbility.name = AttackAbility.NameAbility(newAbility);
        newAbility.description = AttackAbility.DescribeAbility(newAbility);
        return newAbility;
    }

    private static int CalculateAttackAbilityPoints(int points, float damageRatio, float dotDamageRatio, float dotTime, bool isRanged, float cooldown, int mp, int baseMp, float radius, List<AbilityAttribute> abilityAttributes) {
        var normalDamagePercentage = damageRatio / (damageRatio + dotDamageRatio);
        var dotPointMultiplier = 1f;
        if (dotTime == 4) dotPointMultiplier = 1.5f;
        else if (dotTime == 8) dotPointMultiplier = 3;
        else if (dotTime == 12) dotPointMultiplier = 4;
        var nonDotPoints = points * normalDamagePercentage;
        var dotPoints = points - nonDotPoints;
        dotPoints *= dotPointMultiplier;
        points = (int)(nonDotPoints + dotPoints);
        if (!isRanged) points = (int)(points * 1.5f);
        if (cooldown == 1.5f) points = (int)(points * 1.2f);
        else if (cooldown == 3) points = (int)(points * 1.3f);
        else if (cooldown == 8) points = (int)(points * 1.4f);
        else if (cooldown == 15) points = (int)(points * 1.5f);
        else if (cooldown == 30) points = (int)(points * 2f);
        else if (cooldown == 90) points = (int)(points * 6f);
        else if (cooldown == 150) points = (int)(points * 10f);
        if (baseMp == 0) baseMp = mp;
        if (baseMp == 40) points = (int)(points * 1.25f);
        else if (baseMp == 60) points = (int)(points * 1.5f);
        else if (baseMp == 80) points = (int)(points * 1.75f);
        else if (baseMp == 20) points = (int)(points * 1.125f);
        if (radius != 0) points = points / (int)(radius * radius);
        foreach (var attribute in abilityAttributes) {
            switch (attribute.type) {
                case "createDamageZone":
                    break;
                case "projectileSpread":
                    points = (int)(points * 0.3f);
                    break;
                case "jumpBack":
                    points = (int)(points * 0.75f);
                    break;
                case "chargeTowards":
                    points = (int)(points * 0.75f);
                    break;
                case "pullTowards":
                    points = (int)(points * 0.75f);
                    break;
                case "knockback":
                    points = (int)(points * 0.75f);
                    break;
                case "offGCD":
                    points = (int)(points * 0.5f);
                    break;
                case "paralyze":
                    points = (int)(points * 0.5f);
                    break;
                case "usableWhileParalyzed":
                    points = (int)(points * 0.85f);
                    break;
                case "lifeleech":
                    points = (int)(points * 0.5f);
                    break;
                case "mpOverTime":
                    points = (int)(points * 0.8f);
                    break;
                case "elementalDamageBuff":
                    switch (((int)(attribute.FindParameter("degree").floatVal)).ToString()) {
                        case "100":
                            points = (int)(points * 0.94f);
                            break;
                        case "50":
                            points = (int)(points * 0.97f);
                            break;
                        case "25":
                            points = (int)(points * 0.99f);
                            break;
                    }
                    break;
                case "blunting":
                    points = (int)(points * 0.5f);
                    break;
                case "inflictVulnerability":
                    switch (((int)(attribute.FindParameter("degree").floatVal)).ToString()) {
                        case "100":
                            points = (int)(points * 0.25f);
                            break;
                        case "50":
                            points = (int)(points * 0.5f);
                            break;
                        case "25":
                            points = (int)(points * 0.75f);
                            break;
                    }
                    break;
                case "delay":
                    points = (int)(points * 1.5f);
                    break;
                case "damageShield":
                    points = (int)(points * 0.5f);
                    break;
                case "restoreMP":
                    points = (int)(points * 0.8f);
                    break;
                case "removeDebuff":
                    points = (int)(points * 0.85f);
                    break;
                case "addedDot":
                    var dotDamage = attribute.FindParameter("degree").floatVal;
                    var dotTimeSub = attribute.FindParameter("duration").floatVal;
                    var pointCost = dotDamage * 70;
                    if (dotTimeSub == 4) pointCost /= 1.5f;
                    if (dotTimeSub == 8) pointCost /= 3;
                    else pointCost /= 4;
                    points -= (int)pointCost;
                    break;
                case "backstab":
                    points = (int)(points * 0.75f);
                    break;
                case "stealthy":
                    points = (int)(points * 0.9f);
                    break;
            }
        }
        return points;
    }

    public static UtilityAbility FuseUtility(int points, ActiveAbility ability1, ActiveAbility ability2) {
        var am = AbilityMenu.instance;
        float cooldown;
        if (am.fusionCooldownChoice == 0) cooldown = ability1.cooldown;
        else cooldown = ability2.cooldown;
        int mp, baseMp;
        if (am.fusionMpUsageChoice == 0) {
            mp = ability1.mpUsage;
            baseMp = ability1.baseMpUsage;
        }
        else {
            mp = ability2.mpUsage;
            baseMp = ability2.baseMpUsage;
        }
        return CreateNewUtilityAbilityForFusion(points, cooldown, mp, baseMp, am.fusionAbilityAttributesSelected);
    }

    private static UtilityAbility CreateNewUtilityAbilityForFusion(int points, float cooldown, int mp, int baseMp, List<AbilityAttribute> abilityAttributes) {
        var startingPoints = points;
        points = CalculateUtilityAbilityPoints(points, mp, baseMp, cooldown, abilityAttributes);
        var newAbility = new UtilityAbility("", "", cooldown: cooldown, mpUsage: CalculateMpUsage(baseMp, points), baseMpUsage: baseMp, attributes: abilityAttributes.ToArray());
        newAbility.points = startingPoints;
        newAbility.name = UtilityAbility.NameAbility(newAbility);
        newAbility.description = UtilityAbility.DescribeAbility(newAbility);
        newAbility.icon = UtilityAbility.GetAbilityIcon(newAbility);
        return newAbility;
    }

    private static int CalculateUtilityAbilityPoints(int points, int mp, int baseMp, float cooldown, List<AbilityAttribute> abilityAttributes) {
        if (cooldown == 1.5f) points = (int)(points * 1.2f);
        else if (cooldown == 3) points = (int)(points * 1.3f);
        else if (cooldown == 8) points = (int)(points * 1.4f);
        else if (cooldown == 15) points = (int)(points * 1.5f);
        else if (cooldown == 30) points = (int)(points * 2f);
        else if (cooldown == 90) points = (int)(points * 6f);
        else if (cooldown == 150) points = (int)(points * 10f);
        if (baseMp == 0) baseMp = mp;
        if (baseMp == 40) points = (int)(points * 1.25f);
        else if (baseMp == 60) points = (int)(points * 1.5f);
        else if (baseMp == 80) points = (int)(points * 1.75f);
        else if (baseMp == 20) points = (int)(points * 1.125f);
        foreach (var attribute in abilityAttributes) {
            switch (attribute.type) {
                case "offGCD":
                    points = (int)(points * 0.5f);
                    break;
                case "usableWhileParalyzed":
                    points = (int)(points * 0.85f);
                    break;
                case "elementalDamageBuff":
                    switch (((int)(attribute.FindParameter("degree").floatVal)).ToString()) {
                        case "100":
                            points -= 140;
                            break;
                        case "50":
                            points -= 70;
                            break;
                        case "25":
                            points -= 35;
                            break;
                    }
                    break;
                case "disableDevice":
                    points -= 73;
                    break;
                case "stealth":
                    points -= 73;
                    break;
                case "stealthy":
                    points = (int)(points * 0.9f);
                    break;
                case "grapplingHook":
                    points -= 119;
                    break;
            }
        }
        foreach (var attribute in abilityAttributes) {
            switch (attribute.type) {
                case "mpOverTime":
                    attribute.FindParameter("degree").intVal = (int)(80f * points / 70f);
                    points = 0;
                    break;
                case "heal":
                    attribute.FindParameter("degree").floatVal = 4f * points / 80f;
                    points = 0;
                    break;
                case "hot":
                    attribute.FindParameter("degree").floatVal = 8f * points / 80f;
                    points = 0;
                    break;
                case "shield":
                    attribute.FindParameter("degree").floatVal = points / 320f;
                    points = 0;
                    break;
                case "restoreMP":
                    attribute.FindParameter("degree").floatVal = 40f * points / 140f;
                    points = 0;
                    break;
            }
        }
        return points;
    }

    public override Ability Copy() {
        if (this is UtilityAbility) return this.CopyAsUtility();
        else if (this is AttackAbility) return this.CopyAsAttack();
        return null;
    }

    public ActiveAbility CopyAsUtility() {
        var asUtil = (UtilityAbility)this;
        var newAbility = new UtilityAbility(asUtil.name, asUtil.description, asUtil.baseStat, asUtil.icon, asUtil.cooldown, asUtil.mpUsage, asUtil.baseMpUsage, asUtil.radius, asUtil.points, new AbilityAttribute[] { });
        foreach (var attribute in asUtil.attributes) newAbility.attributes.Add(attribute.Copy());
        return newAbility;
    }

    public ActiveAbility CopyAsAttack() {
        var asAttack = (AttackAbility)this;
        var newAbility = new AttackAbility(asAttack.name, asAttack.description, asAttack.damage, asAttack.element, asAttack.baseStat, asAttack.icon, asAttack.dotDamage, asAttack.dotTime, asAttack.isRanged, asAttack.rangedProjectile, asAttack.cooldown, asAttack.mpUsage, asAttack.baseMpUsage, asAttack.radius, asAttack.hitEffect, asAttack.aoe, asAttack.points, new AbilityAttribute[] { });
        foreach (var attribute in asAttack.attributes) newAbility.attributes.Add(attribute.Copy());
        return newAbility;
    }

    public static void Enhance(ActiveAbility ability, List<Dust> dust) {
        foreach (var item in dust) {
            switch (item.type) {
                case "power":
                    var points = item.quantity / 3;
                    AddPointsToAbility(ability, points);
                    break;
                case "createDamageZone":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("createDamageZone"));
                    break;
                case "projectileSpread":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("projectileSpread"));
                    break;
                case "jumpBack":
                    ability.attributes.Add(new AbilityAttribute("jumpBack", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3)));
                    break;
                case "chargeTowards":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("chargeTowards"));
                    break;
                case "pullTowards":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("pullTowards"));
                    break;
                case "knockback":
                    ability.attributes.Add(new AbilityAttribute("knockback", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3)));
                    break;
                case "offGCD":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("offGCD"));
                    break;
                case "paralyze":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("paralyze", new AbilityParameter("duration", DataType.floatType, floatVal: 3)));
                    break;
                case "usableWhileParalyzed":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("usableWhileParalyzed"));
                    break;
                case "lifeleech":
                    ability.attributes.Add(new AbilityAttribute("lifeleech", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3)));
                    break;
                case "mpOverTime":
                    ability.attributes.Add(new AbilityAttribute("mpOverTime", new AbilityParameter("degree", DataType.intType, intVal: (int)(item.quantity / 3)), new AbilityParameter("duration", DataType.floatType, floatVal: 8)));
                    break;
                case "elementalDamageBuff":
                    var element = Spirit.RandomElement();
                    ability.attributes.Add(new AbilityAttribute("elementalDamageBuff", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3), new AbilityParameter("duration", DataType.floatType, floatVal: 12), new AbilityParameter("element", DataType.stringType, stringVal: element.ToString())));
                    break;
                case "blunting":
                    ability.attributes.Add(new AbilityAttribute("blunting", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3)));
                    break;
                case "inflictVulnerability":
                    ability.attributes.Add(new AbilityAttribute("inflictVulnerability", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3), new AbilityParameter("duration", DataType.floatType, floatVal: 12)));
                    break;
                case "delay":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("delay", new AbilityParameter("time", DataType.floatType, floatVal: 4)));
                    break;
                case "damageShield":
                    ability.attributes.Add(new AbilityAttribute("damageShield", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3)));
                    break;
                case "restoreMP":
                    ability.attributes.Add(new AbilityAttribute("restoreMP", new AbilityParameter("degree", DataType.intType, intVal: (int)(item.quantity / 3))));
                    break;
                case "removeDebuff":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("removeDebuff"));
                    break;
                case "addedDot":
                    ability.attributes.Add(new AbilityAttribute("addedDot", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3), new AbilityParameter("duration", DataType.floatType, floatVal: 12)));
                    break;
                case "backstab":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("backstab"));
                    break;
                case "stealthy":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("stealthy"));
                    break;
                case "heal":
                    ability.attributes.Add(new AbilityAttribute("heal", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3)));
                    break;
                case "hot":
                    ability.attributes.Add(new AbilityAttribute("hot", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3), new AbilityParameter("duration", DataType.floatType, floatVal: 8f)));
                    break;
                case "shield":
                    ability.attributes.Add(new AbilityAttribute("shield", new AbilityParameter("degree", DataType.floatType, floatVal: item.quantity / 3), new AbilityParameter("stat", DataType.stringType, stringVal: "strength")));
                    break;
                case "disableDevice":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("disableDevice"));
                    break;
                case "stealth":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("stealth"));
                    break;
                case "grapplingHook":
                    if (item.quantity >= 3) ability.attributes.Add(new AbilityAttribute("grapplingHook"));
                    break;
            }
        }
        RecalculateAbility(ability);
    }

    public static void ConsumeDust(ActiveAbility ability, List<Dust> dust, List<Dust> dustUsed) {
        var cullList = new List<Dust>();
        foreach (var item in dustUsed) {
            switch (item.type) {
                case "power":
                    item.quantity = 0;
                    break;
                case "createDamageZone":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "projectileSpread":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "jumpBack":
                    item.quantity = 0;
                    break;
                case "chargeTowards":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "pullTowards":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "knockback":
                    item.quantity = 0;
                    break;
                case "offGCD":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "paralyze":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "usableWhileParalyzed":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "lifeleech":
                    item.quantity = 0;
                    break;
                case "mpOverTime":
                    item.quantity = 0;
                    break;
                case "elementalDamageBuff":
                    item.quantity = 0;
                    break;
                case "blunting":
                    item.quantity = 0;
                    break;
                case "inflictVulnerability":
                    item.quantity = 0;
                    break;
                case "delay":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "damageShield":
                    item.quantity = 0;
                    break;
                case "restoreMP":
                    item.quantity = 0;
                    break;
                case "removeDebuff":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "addedDot":
                    item.quantity = 0;
                    break;
                case "backstab":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "stealthy":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "heal":
                    item.quantity = 0;
                    break;
                case "hot":
                    item.quantity = 0;
                    break;
                case "shield":
                    item.quantity = 0;
                    break;
                case "disableDevice":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "stealth":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
                case "grapplingHook":
                    if (item.quantity >= 3) item.quantity -= 3;
                    break;
            }
            if (item.quantity <= 0) cullList.Add(item);
        }
        foreach (var item in cullList) dust.Remove(item);
    }

    public static void AddPointsToAbility(ActiveAbility ability, float points) {
        ability.points += (int)points;
    }

    public static void RecalculateAbility(ActiveAbility ability) {
        int points;
        if (ability is AttackAbility) {
            var asAttack = (AttackAbility)ability;
            points = CalculateAttackAbilityPoints(ability.points, asAttack.damage, asAttack.dotDamage, asAttack.dotTime, asAttack.isRanged, ability.cooldown, ability.mpUsage, ability.baseMpUsage, asAttack.radius, ability.attributes);
            ability.points = points;
            var damageRatio = asAttack.damage;
            var dotDamageRatio = asAttack.dotDamage;
            var totalDamage = AttackAbility.CalculateDamage(points);
            var regularDamage = totalDamage * damageRatio / (damageRatio + dotDamageRatio);
            var dotDamage = totalDamage * dotDamageRatio / (damageRatio + dotDamageRatio);
            asAttack.damage = regularDamage;
            asAttack.dotDamage = dotDamage;
            ability.name = AttackAbility.NameAbility(asAttack);
            ability.description = AttackAbility.DescribeAbility(asAttack);
        }
        else if (ability is UtilityAbility) {
            points = CalculateUtilityAbilityPoints(ability.points, ability.mpUsage, ability.baseMpUsage, ability.cooldown, ability.attributes);
            ability.points = points;
            ability.name = UtilityAbility.NameAbility((UtilityAbility)ability);
            ability.description = UtilityAbility.DescribeAbility((UtilityAbility)ability);
            ability.icon = UtilityAbility.GetAbilityIcon((UtilityAbility)ability);
        }
    }

    public int GetLevel() {
        return GetLevelFromPoints(points);
    }

    public static int GetLevelFromPoints(float points) {
        var level = 1;
        float currentPoints = points;
        while (currentPoints > 70) {
            currentPoints /= 1.05f;
            level += 1;
        }
        return level;
    }

    public static int CalculateMpUsage(int baseMp, float points) {
        var level = GetLevelFromPoints(points);
        float tempMp = baseMp;
        for (int i = 0; i < level; i++) tempMp *= 1.05f;
        return (int)tempMp;
    }

    private static int GetAoe(Element element) {
        switch (element) {
            case Element.slashing:
                return 0;
            case Element.piercing:
                return 1;
            case Element.bashing:
                return 2;
            case Element.fire:
                return 3;
            case Element.ice:
                return 4;
            case Element.acid:
                return 5;
            case Element.light:
                return 6;
            case Element.dark:
                return 7;
            case Element.none:
            default:
                return 8;
        }
    }
}
