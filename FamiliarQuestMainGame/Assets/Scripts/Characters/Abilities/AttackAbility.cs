using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackAbility : ActiveAbility {
    public float damage = 0;
    public float dotDamage = 0;
    public float dotTime = 1;
    public Element element;
    public bool isRanged = false;
    public int rangedProjectile = 0;
    public int hitEffect = 0;
    public int aoe = 0;

    public AttackAbility(string name, string description, float damage, Element element, BaseStat baseStat, int icon = 0, float dotDamage = 0, float dotTime = 1, bool isRanged = false, int rangedProjectile = 0, float cooldown = 0f, int mpUsage = 0, int baseMpUsage=0, float radius = 0f, int hitEffect = 0, int aoe = 0, int points = 70, params AbilityAttribute[] attributes) {
        this.name = name;
        this.description = description;
        this.damage = damage;
        this.element = element;
        this.baseStat = baseStat;
        this.dotDamage = dotDamage;
        this.dotTime = dotTime;
        this.isRanged = isRanged;
        this.rangedProjectile = rangedProjectile;
        this.icon = icon;
        this.cooldown = cooldown;
        this.mpUsage = mpUsage;
        this.baseMpUsage = baseMpUsage;
        this.radius = radius;
        this.hitEffect = hitEffect;
        this.aoe = aoe;
        foreach (var item in attributes) this.attributes.Add(item);
    }

    private static Dictionary<Element, Dictionary<bool, string>> baseNames;
    private static Dictionary<Element, int> baseHitEffects;
    private static Dictionary<Element, int> baseProjectiles;
    private static Dictionary<Element, int> baseAoes;
    private static Dictionary<Element, int> baseDamageZones;
    private static Dictionary<Element, int> basicMeleeIcons;
    private static Dictionary<Element, int> basicRangedIcons;
    private static Dictionary<Element, int> basicAoeIcons;
    private static Dictionary<string, int> attributeIcons;

    static AttackAbility() {
        baseHitEffects = new Dictionary<Element, int>() {
            { Element.bashing, 1 },
            { Element.slashing, 0 },
            { Element.piercing, 2 },
            { Element.fire, 3 },
            { Element.ice, 4 },
            { Element.acid, 5 },
            { Element.light, 6 },
            { Element.dark, 7 },
            { Element.none, 8 }
        };
        baseProjectiles = new Dictionary<Element, int>() {
            { Element.bashing, 3 },
            { Element.slashing, 4 },
            { Element.piercing,  0 },
            { Element.fire,  1 },
            { Element.ice, 5 },
            { Element.acid, 2 },
            { Element.light, 6 },
            { Element.dark, 7 },
            { Element.none, 8 }
        };
        baseAoes = new Dictionary<Element, int>() {
            { Element.bashing, 2 },
            { Element.slashing, 0 },
            { Element.piercing,  1 },
            { Element.fire,  3 },
            { Element.ice, 4 },
            { Element.acid, 5 },
            { Element.light, 6 },
            { Element.dark, 7 },
            { Element.none, 8 }
        };
        baseDamageZones = new Dictionary<Element, int>() {
            {Element.acid, 0},
            {Element.bashing, 1},
            {Element.dark, 2},
            {Element.fire, 3},
            {Element.ice, 4},
            {Element.light, 5},
            {Element.none, 6},
            {Element.piercing, 7},
            {Element.slashing, 8}
        };
        baseNames = new Dictionary<Element, Dictionary<bool, string>>() {
            {
                Element.acid, new Dictionary<bool, string>() {
                    { true, "Acid Bolt" },
                    { false, "Acid Strike" }
                }
            },
            {
                Element.fire, new Dictionary<bool, string>() {
                    { true, "Firebolt" },
                    { false, "Cinderstrike" }
                }
            },
            {
                Element.ice, new Dictionary<bool, string>() {
                    { true, "Frostbolt" },
                    { false, "Froststrike" }
                }
             },
            {
                Element.dark, new Dictionary<bool, string>() {
                    { true, "Voidbolt" },
                    { false, "Voidstrike" }
                }
            },
            {
                Element.light, new Dictionary<bool, string>() {
                    { true, "Sunbolt" },
                    { false, "Sunstrike" }
                }
            },
            {
                Element.piercing, new Dictionary<bool, string>() {
                    { true, "Arrow" },
                    { false, "Stab" }
                }
            },
            {
                Element.bashing, new Dictionary<bool, string>() {
                    { true, "Stone" },
                    { false, "Bash" }
                }
            },
            {
                Element.slashing, new Dictionary<bool, string>() {
                    { true, "Shuriken" },
                    { false, "Slash" }
                }
            },
            {
                Element.none, new Dictionary<bool, string>() {
                    { true, "Nullbolt" },
                    { false, "Nullstrike" }
                }
            }
        };
        basicMeleeIcons = new Dictionary<Element, int>() {
            { Element.bashing, 24 },
            { Element.slashing, 25 },
            { Element.piercing, 26 },
            { Element.acid, 27 },
            { Element.fire, 28 },
            { Element.ice, 29 },
            { Element.light, 30 },
            { Element.dark, 31 },
            { Element.none, 37 }
        };
        basicRangedIcons = new Dictionary<Element, int>() {
            { Element.bashing, 32 },
            { Element.slashing, 33 },
            { Element.piercing, 3 },
            { Element.acid, 12 },
            { Element.fire, 9 },
            { Element.ice, 34 },
            { Element.light, 35 },
            { Element.dark, 36 },
            { Element.none, 38 }
        };
        basicAoeIcons = new Dictionary<Element, int>() {
            { Element.bashing, 40 },
            { Element.slashing, 41 },
            { Element.piercing, 42 },
            { Element.acid, 43 },
            { Element.fire, 10 },
            { Element.ice, 44 },
            { Element.light, 45 },
            { Element.dark, 46 },
            { Element.none, 39 }
        };
        attributeIcons = new Dictionary<string, int>()
        {
            { "lifeleech", 47 },
            { "blunting", 19 },
            { "damageShield", 19 },
            { "inflictVulnerability", 48 },
            { "paralyze", 49 }
        };
    }

    public static string NameAbility(AttackAbility ability) {
        var name = baseNames[ability.element][ability.isRanged];
        int prefixes = 0;
        int suffixes = 0;
        if (ability.mpUsage > 0) {
            name = "Arcane " + name;
            prefixes++;
        }
        if (ability.radius > 0) {
            name = "Exploding " + name;
            prefixes++;
        }
        if (ability.dotDamage>0 && prefixes < 2) {
            name = "Draining " + name;
            prefixes++;
        }
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "createDamageZone":
                    if (suffixes < 1) {
                        name = name + " of Danger";
                        suffixes++;
                    }
                    break;
                case "projectileSpread":
                    if (suffixes < 1) name = name + " Fusillade";
                    break;
                case "jumpBack":
                    if (prefixes < 2) {
                        name = "Retreating " + name;
                        prefixes++;
                    }
                    break;
                case "chargeTowards":
                    if (prefixes < 2) {
                        name = "Charging " + name;
                        prefixes++;
                    }
                    break;
                case "pulltowards":
                    if (suffixes < 1) {
                        name = name + " of Grappling";
                        suffixes++;
                    }
                    break;
                case "knockback":
                    if (prefixes < 2) {
                        name = "Pushing " + name;
                        prefixes++;
                    }
                    break;
                case "offGCD":
                    if (prefixes < 2) {
                        name = "Swift " + name;
                        prefixes++;
                    }
                    break;
                case "paralyze":
                    if (prefixes < 2) {
                        name = "Paralyzing " + name;
                        prefixes++;
                    }
                    break;
                case "usableWhileParalyzed":
                    if (prefixes < 2) {
                        name = "Cognitive " + name;
                        prefixes++;
                    }
                    break;
                case "backstab":
                    if (prefixes < 2) {
                        name = "Treacherous " + name;
                        prefixes++;
                    }
                    break;
                case "stealthy":
                    if (prefixes < 2) {
                        name = "Subtle " + name;
                        prefixes++;
                    }
                    break;
                case "lifeleech":
                    if (prefixes < 2) {
                        name = "Vampiric " + name;
                        prefixes++;
                    }
                    break;
                case "mpOverTime":
                    if (prefixes < 2) {
                        name = "Meditative " + name;
                        prefixes++;
                    }
                    break;
                case "elementalDamageBuff":
                    var type = attribute.FindParameter("element").stringVal;
                    if (suffixes < 1) {
                        switch (type) {
                            case "bashing":
                                name = name + " of Achilles";
                                break;
                            case "piercing":
                                name = name + " of Achilles";
                                break;
                            case "slashing":
                                name = name + " of Achilles";
                                break;
                            case "fire":
                                name = name + " of the Inferno";
                                break;
                            case "ice":
                                name = name + " of Frostbite";
                                break;
                            case "acid":
                                name = name + " of Solvency";
                                break;
                            case "light":
                                name = name + " of Sunburn";
                                break;
                            case "dark":
                                name = name + " of the Voidgaze";
                                break;
                            case "none":
                                name = name + " of Nullity";
                                break;
                        }
                        suffixes++;
                    }
                    break;
                case "blunting":
                    if (suffixes < 1) {
                        name = name + " of Blunting";
                        suffixes++;
                    }
                    break;
                case "inflictVulnerability":
                    if (prefixes < 2) {
                        name = "Tactical " + name;
                        prefixes++;
                    }
                    break;
                case "delay":
                    if (prefixes < 2) {
                        name = "Delayed " + name;
                        prefixes++;
                    }
                    break;
                case "damageShield":
                    if (prefixes < 2) {
                        name = "Aegis " + name;
                        prefixes++;
                    }
                    break;
                case "restoreMP":
                    if (suffixes < 1) {
                        name = name + " of Concentration";
                        suffixes++;
                    }
                    break;
                case "removeDebuff":
                    if (prefixes < 2) {
                        name = "Cleansing " + name;
                        prefixes++;
                    }
                    break;
                case "addedDot":
                    if (prefixes < 2) {
                        name = "Draining " + name;
                        prefixes++;
                    }
                    break;
                case "increasedCritChance":
                    if (prefixes < 2) {
                        name = "Sharp " + name;
                        prefixes++;
                    }
                    break;
                case "increasedCritDamage":
                    if (suffixes < 1) {
                        name = name + " of Piercing";
                        suffixes++;
                    }
                    break;
                case "speed-":
                    if (suffixes < 1) {
                        name = name + " of Slowing";
                        suffixes++;
                    }
                    break;
                case "immobilizeSelf":
                    if (suffixes < 1) {
                        name = name + " of the Turret";
                        suffixes++;
                    }
                    break;
                default:
                    break;
            }
        }
        return name;
    }

    public static string DescribeAbility(AttackAbility ability) {
        string description;
        int degree, duration;
        var level = ability.GetLevel();
        description = "L" + level.ToString() + " ";
        if (!ability.isRanged) description += "<b>Melee";
        else description += "<b>Ranged";
        description += " " + ability.element.ToString() + " attack, uses {{baseStat}}</b>\n";
        if (ability.mpUsage > 0) description += "<b>MP</b>: " + ability.mpUsage.ToString() + "\n";
        if (ability.radius > 0) description += "<b>Radius</b>: " + ability.radius.ToString() + "\n";
        if (ability.cooldown >= 60) description += "<b>Cooldown</b>: " + (ability.cooldown / 60).ToString() + "m\n";
        else if (ability.cooldown > 0) description += "<b>Cooldown</b>: " + ability.cooldown.ToString() + "s\n";
        var attributes = new List<AbilityAttribute>();
        string afterDescription = "";
        foreach (var attribute in ability.attributes) {
            switch (attribute.type) {
                case "createDamageZone":
                    afterDescription += "Creates damage zone\n";
                    break;
                case "projectileSpread":
                    afterDescription += "Fires projectile spread\n";
                    break;
                case "jumpBack":
                    afterDescription += "Jump back after use.\n";
                    break;
                case "chargeTowards":
                    afterDescription += "Charge towards enemy.\n";
                    break;
                case "pulltowards":
                    afterDescription += "Pull enemy towards you.\n";
                    break;
                case "knockback":
                    afterDescription += "Knockback.\n";
                    break;
                case "offGCD":
                    afterDescription += "Doesn't trigger global cooldown.\n";
                    break;
                case "paralyze":
                    afterDescription += "Paralyzes.\n";
                    break;
                case "usableWhileParalyzed":
                    afterDescription += "Usable while paralyzed.\n";
                    break;
                case "backstab":
                    afterDescription += "Backstabs for 4x damage when in stealth mode.\n";
                    break;
                case "stealthy":
                    afterDescription += "Doesn't break stealth.\n";
                    break;
                case "lifeleech":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    afterDescription += "Heals you for " + degree.ToString() + "% of damage dealt.\n";
                    break;
                case "mpOverTime":
                    afterDescription += "Restores MP over time.\n";
                    break;
                case "elementalDamageBuff":
                    var type = attribute.FindParameter("element").stringVal;
                    afterDescription += "Boosts " + type + " damage after use.\n";
                    break;
                case "blunting":
                    afterDescription += "Nullifies enemy ability to deal damage briefly.\n";
                    break;
                case "inflictVulnerability":
                    afterDescription += "Boosts damage taken by enemy afterwards.\n";
                    break;
                case "delay":
                    afterDescription += "Deals damage after a brief delay.\n";
                    break;
                case "damageShield":
                    afterDescription += "Damage shield.\n";
                    break;
                case "restoreMP":
                    afterDescription += "Restores MP.\n";
                    break;
                case "removeDebuff":
                    afterDescription += "Removes a debuff.\n";
                    break;
                case "addedDot":
                    afterDescription += "Added damage over time.\n";
                    break;
                case "increasedCritChance":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    afterDescription += "Additional " + degree.ToString() + "% chance to critically hit.\n";
                    break;
                case "increasedCritDamage":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    afterDescription += "Additional " + degree.ToString() + "% damage on critical hit.\n";
                    break;
                case "speed-":
                    degree = (int)(attribute.FindParameter("degree").floatVal * 100f);
                    duration = (int)(attribute.FindParameter("duration").floatVal);
                    afterDescription += "Slow target's movement by " + degree.ToString() + "% for " + duration.ToString() + " seconds.\n";
                    break;
                case "immobilizeSelf":
                    duration = (int)(attribute.FindParameter("duration").floatVal);
                    afterDescription += "Immobilizes self for " + duration.ToString() + " seconds.\n";
                    break;
                default:
                    break;
            }
        }
        if (ability.dotDamage>0) {
            description += "Deals {{dotDamage}} damage over " + ability.dotTime.ToString() + " seconds.\n";
        }
        else {
            description += "Deals {{damage}} damage.\n";
        }
        description += afterDescription;
        return description;
    }

    public static AttackAbility Generate(List<Element> baseTypes, float points = 70f) {
        var startingPoints = points;
        int rangedRoll = Random.Range(0, 2);
        int usesMPRoll = Random.Range(0, 2);
        BaseStat baseStat;
        if (rangedRoll == 1 && usesMPRoll == 1) {
            baseStat = BaseStat.intelligence;
        }
        else if (rangedRoll == 1 && usesMPRoll == 0) {
            baseStat = BaseStat.dexterity;
        }
        else {
            baseStat = BaseStat.strength;
            points = Mathf.Floor(points * 1.5f);
        }
        int mp, baseMp;
        if (usesMPRoll == 0) {
            mp = 0;
        }
        else {
            var mpRoll = Random.Range(0, 100);
            if (mpRoll < 80) {
                points = Mathf.Floor(points * 1.25f);
                mp = 40;
            }
            else if (mpRoll < 90) {
                points = Mathf.Floor(points * 1.125f);
                mp = 20;
            }
            else if (mpRoll < 95) {
                points = Mathf.Floor(points * 1.5f);
                mp = 60;
            }
            else {
                points = Mathf.Floor(points * 1.75f);
                mp = 80;
            }
        }
        baseMp = mp;
        float tempMp = mp;
        for (int i = 1; i < LevelGen.targetLevel; i++) tempMp *= 1.1f;
        mp = (int)tempMp;
        int elementRoll = Random.Range(0, 100);
        Element element;
        if (elementRoll < 75) {
            int typeRoll = Random.Range(0, baseTypes.Count);
            element = baseTypes[typeRoll];
        }
        else if (elementRoll < 98) {
            int typeRoll = Random.Range(0, baseTypes.Count);
            element = baseTypes[typeRoll];
            int affinityRoll = Random.Range(0, ElementalAffinity.alliances[element].Count);
            element = ElementalAffinity.alliances[element][affinityRoll];
        }
        else {
            element = Spirit.RandomElement();
        }
        var hitEffect = 0;
        var projectile = 0;
        var aoe = 0;
        if (rangedRoll == 0) {
            hitEffect = baseHitEffects[element];
        }
        else {
            projectile = baseProjectiles[element];
        }
        int radiusRoll = Random.Range(0, 300);
        int radius;
        if (radiusRoll < 270) {
            radius = 0;
        }
        else if (radiusRoll < 291) {
            points = Mathf.Floor(points / 4f);
            radius = 2;
        }
        else if (radiusRoll < 297) {
            points = Mathf.Floor(points / 16f);
            radius = 4;
        }
        else if (radiusRoll < 299) {
            points = Mathf.Floor(points / 36f);
            radius = 6;
        }
        else {
            points = Mathf.Floor(points / 64f);
            radius = 8;
        }
        if (radius > 0) {
            aoe = baseAoes[element];
        }
        int dotRoll = Random.Range(1, 10000);
        bool isDot = false;
        float dotTime;
        if (dotRoll < 8500) {
            isDot = false;
            dotTime = 0;
        }
        else if (dotRoll < 8875) {
            isDot = true;
            dotTime = 4;
            points = Mathf.Floor(points * 1.5f);
        }
        else if (dotRoll < 9625) {
            isDot = true;
            dotTime = 8;
            points = Mathf.Floor(points * 3f);
        }
        else {
            isDot = true;
            dotTime = 12;
            points = Mathf.Floor(points * 4f);
        }
        int attributesRoll = Random.Range(0, 100);
        int numAttributes;
        if (attributesRoll < 20) {
            numAttributes = 0;
        }
        else if (attributesRoll < 81) {
            numAttributes = 1;
        }
        else if (attributesRoll < 93) {
            numAttributes = 2;
        }
        else if (attributesRoll < 98) {
            numAttributes = 3;
        }
        else {
            numAttributes = 4;
        }
        float cooldown = 0;
        int hasCDRoll = Random.Range(0, 100);
        if (hasCDRoll < 35) {
            int cooldownRoll = Random.Range(0, 7);
            switch (cooldownRoll) {
                case 0:
                    cooldown = 1.5f;
                    points = Mathf.Floor(points * 1.2f);
                    break;
                case 1:
                    cooldown = 3;
                    points = Mathf.Floor(points * 1.3f);
                    break;
                case 2:
                    cooldown = 8;
                    points = Mathf.Floor(points * 1.4f);
                    break;
                case 3:
                    cooldown = 15;
                    points = Mathf.Floor(points * 1.5f);
                    break;
                case 4:
                    cooldown = 30;
                    points = Mathf.Floor(points * 2f);
                    break;
                case 5:
                    cooldown = 90;
                    points = Mathf.Floor(points * 6f);
                    break;
                case 6:
                default:
                    cooldown = 150;
                    points = Mathf.Floor(points * 10f);
                    break;
            }
        }
        var attributes = new List<AbilityAttribute>();
        for (int i = 0; i < numAttributes; i++) {
            var attribute = AbilityAttribute.GetAttackAttribute(points, mp, isDot, radius, rangedRoll == 1, cooldown, element);
            if (attribute != null && !DuplicateStealthy(attribute, attributes)) {
                points -= attribute.points;
                attributes.Add(attribute);
                if (attribute.type == "createDamageZone") aoe = baseDamageZones[element];
            }
        }
        float baseDamage = CalculateDamage(points);
        float damage;
        float dotDamage;
        if (isDot) {
            damage = 0;
            dotDamage = baseDamage;
        }
        else {
            damage = baseDamage;
            dotDamage = 0;
        }

        float mostPoints = points;
        AbilityAttribute mostPointsAttribute = null;
        foreach (var attribute in attributes) {
            if (attribute.points >= mostPoints && attribute.type != "addedDot" && attribute.type != "offGCD" && attribute.type != "projectileSpread") {
                mostPoints = attribute.points;
                mostPointsAttribute = attribute;
            }
            else if (mostPointsAttribute != null && attribute.type == mostPointsAttribute.type) {
                mostPoints += attribute.points;
            }
        }
        int icon = 0;
        if (mostPointsAttribute != null) {
            if (attributeIcons.ContainsKey(mostPointsAttribute.type)) {
                icon = attributeIcons[mostPointsAttribute.type];
            }
            else {
                Debug.Log("APPROPRIATE ICON NOT FOUND: " + mostPoints.ToString() + " spent on " + mostPointsAttribute.type);
            }
        }
        else {
            if (radius > 0) {
                icon = basicAoeIcons[element];
            }
            else if (rangedRoll == 1) {
                icon = basicRangedIcons[element];
            }
            else {
                icon = basicMeleeIcons[element];
            }
        }

        var newAbility = new AttackAbility("", "", damage, element, baseStat, dotDamage: dotDamage, dotTime: dotTime, isRanged: rangedRoll == 1, cooldown: cooldown, mpUsage: mp, baseMpUsage: baseMp, radius: radius, icon: icon, hitEffect: hitEffect, rangedProjectile: projectile, aoe: aoe, attributes: attributes.ToArray());
        newAbility.name = NameAbility(newAbility);
        newAbility.points = (int)startingPoints;
        newAbility.description = DescribeAbility(newAbility);
        return newAbility;
    }

    public static float CalculateDamage(float points) {
        return 1f / 70f * points;
    }

    public float CalculateDotDamage(Character character) {
        float attackPower;
        if (character.GetComponent<PlayerCharacter>() != null) {
            attackPower = character.GetComponent<PlayerCharacter>().weapon.attackPower;
        }
        else {
            attackPower = character.GetComponent<Monster>().attackFactor;
        }
        if (baseStat == BaseStat.strength) {
            attackPower *= character.strength;
        }
        else if (baseStat == BaseStat.dexterity) {
            attackPower *= character.dexterity;
        }
        else {
            attackPower *= character.intelligence;
        }
        return attackPower * dotDamage / dotTime;
    }

    public static bool DuplicateStealthy(AbilityAttribute attribute, List<AbilityAttribute> attributes) {
        if (attribute.type != "stealthy") return false;
        foreach (var attribute2 in attributes) if (attribute2.type == "stealthy") return true;
        return false;
    }
}
