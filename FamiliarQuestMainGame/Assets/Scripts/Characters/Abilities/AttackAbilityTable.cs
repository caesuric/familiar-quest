using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class AttackAbilityTable {
    private static readonly Dictionary<Element, int> baseHitEffects;
    private static readonly Dictionary<Element, int> baseProjectiles;
    private static readonly Dictionary<Element, int> baseAoes;
    private static readonly Dictionary<Element, int> baseDamageZones;
    private static readonly Dictionary<Element, int> basicMeleeIcons;
    private static readonly Dictionary<Element, int> basicRangedIcons;
    private static readonly Dictionary<Element, int> basicAoeIcons;
    private static readonly Dictionary<string, int> attributeIcons;

    static AttackAbilityTable() {
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

    public static AttackAbility Retrieve(float points = 70f) {
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
        Element element = Spirit.RandomElement();
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
        float baseDamage = AttackAbility.CalculateDamage(points);
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
        newAbility.name = AbilityNamer.Name(newAbility);
        newAbility.points = (int)startingPoints;
        newAbility.description = AbilityDescriber.Describe(newAbility);
        return newAbility;
    }

    public static bool DuplicateStealthy(AbilityAttribute attribute, List<AbilityAttribute> attributes) {
        if (attribute.type != "stealthy") return false;
        foreach (var attribute2 in attributes) if (attribute2.type == "stealthy") return true;
        return false;
    }
}
