using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class AttackAbilityAttributeGenerator {
    private delegate AbilityAttribute AbilityAttributeDelegate(AttackAbility ability);
    private static readonly List<string> simpleAttributes;
    private static readonly Dictionary<string, AbilityAttributeDelegate> attributes;
    private static readonly List<string> negativeAttributes;

    static AttackAbilityAttributeGenerator() {
        simpleAttributes = new List<string> {
            "usableWhileParalyzed",
            "removeDebuff",
            "stealthy"
        };
        attributes = new Dictionary<string, AbilityAttributeDelegate> {
            ["createDamageZone"] = GetCreateDamageZone,
            ["projectileSpread"] = GetProjectileSpread,
            ["jumpBack"] = GetJumpBack,
            ["chargeTowards"] = GetChargeTowards,
            ["pullTowards"] = GetPullTowards,
            ["knockback"] = GetKnockback,
            ["offGCD"] = GetOffGcd,
            ["paralyze"] = GetParalyze,
            ["lifeleech"] = GetLifeleech,
            ["mpOverTime"] = GetMpOverTime,
            ["elementalDamageBuff"] = GetElementalDamageBuff,
            ["blunting"] = GetBlunting,
            ["inflictVulnerability"] = GetInflictVulnerability,
            ["delay"] = GetDelay,
            ["damageShield"] = GetDamageShield,
            ["restoreMP"] = GetRestoreMP,
            ["addedDot"] = GetAddedDot,
            ["backstab"] = GetBackstab,
            ["increasedCritChance"] = GetIncreasedCritChance,
            ["increasedCritDamage"] = GetIncreasedCritDamage,
            ["speed-"] = GetSpeedMinus,
            ["immobilizeSelf"] = GetImmobilizeSelf,
        };
        negativeAttributes = new List<string> {
            "immobilizeSelf",
            "delay"
        };
    }

    public static AbilityAttribute Generate(AttackAbility ability) {
        for (int i = 0; i < 10000; i++) {
            string roll = TableRoller.Roll("AttackAttributes");
            AbilityAttribute attribute;
            if (simpleAttributes.Contains(roll)) {
                attribute = new AbilityAttribute {
                    type = roll
                };
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                if (negativeAttributes.Contains(attribute.type) && attribute.priority < 50) continue;
                return attribute;
            }
            attribute = attributes[roll](ability);
            if (attribute != null) {
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                if (negativeAttributes.Contains(attribute.type) && attribute.priority < 50) continue;
                return attribute;
            }
        }
        Debug.Log("FAILED TO FIND VALID ATTRIBUTE FOR ABILITY!");
        return null;
    }

    public static AbilityAttribute Generate(AttackAbility ability, string attributeType) {
        for (int i = 0; i < 10000; i++) {
            AbilityAttribute attribute;
            if (simpleAttributes.Contains(attributeType)) {
                attribute = new AbilityAttribute {
                    type = attributeType
                };
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                if (negativeAttributes.Contains(attribute.type) && attribute.priority < 50) continue;
                return attribute;
            }
            attribute = attributes[attributeType](ability);
            if (attribute != null) {
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                if (negativeAttributes.Contains(attribute.type) && attribute.priority < 50) continue;
                return attribute;
            }
        }
        Debug.Log("FAILED TO FIND VALID ATTRIBUTE FOR ABILITY!");
        return null;
    }

    private static AbilityAttribute GetCreateDamageZone(AttackAbility ability) {
        if (ability.radius > 0 && ability.dotDamage > 0) return new AbilityAttribute {
            type = "createDamageZone"
        };
        else return null;
    }

    private static AbilityAttribute GetProjectileSpread(AttackAbility ability) {
        if (ability.isRanged) return new AbilityAttribute {
            type = "projectileSpread"
        };
        else return null;
    }

    private static AbilityAttribute GetJumpBack(AttackAbility ability) {
        return new AbilityAttribute {
            type = "jumpBack",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = 5f
                }
            }
        };
    }

    private static AbilityAttribute GetChargeTowards(AttackAbility ability) {
        if (!ability.isRanged) return new AbilityAttribute {
            type = "chargeTowards"
        };
        else return null;
    }

    private static AbilityAttribute GetPullTowards(AttackAbility ability) {
        if (ability.isRanged) return new AbilityAttribute {
            type = "pullTowards"
        };
        else return null;
    }

    private static AbilityAttribute GetKnockback(AttackAbility ability) {
        return new AbilityAttribute {
            type = "knockback",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = 5f
                }
            }
        };
    }

    private static AbilityAttribute GetOffGcd(AttackAbility ability) {
        if (ability.cooldown > 0) return new AbilityAttribute {
            type = "offGCD"
        };
        else return null;
    }

    private static AbilityAttribute GetParalyze(AttackAbility ability) {
        return new AbilityAttribute {
            type = "paralyze",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "duration",
                    value = 3f
                }
            }
        };
    }

    private static AbilityAttribute GetLifeleech(AttackAbility ability) {
        int leechAmountInt = UnityEngine.Random.Range(5, 120);
        leechAmountInt = Mathf.Min(leechAmountInt, 100);
        float leechAmount = leechAmountInt / 100f;
        return new AbilityAttribute {
            type = "lifeleech",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = leechAmount
                }
            }
        };
    }

    private static AbilityAttribute GetMpOverTime(AttackAbility ability) {
        int mpAmount = (int)(ability.points * 80f / 70f);
        return new AbilityAttribute {
            type = "mpOverTime",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = (float)mpAmount
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = 8f
                }
            }
        };
    }

    private static AbilityAttribute GetElementalDamageBuff(AttackAbility ability) {
        var degreeRoll = UnityEngine.Random.Range(0, 3);
        float damageBoost;
        var boostNumbers = new List<float> { 100, 50, 25 };
        damageBoost = boostNumbers[degreeRoll];
        Element element;
        int elementRoll = UnityEngine.Random.Range(0, 2);
        if (elementRoll == 0) element = ability.element;
        else element = RNG.EnumValue<Element>();
        while (element == Element.none) element = RNG.EnumValue<Element>();
        return new AbilityAttribute {
            type = "elementalDamageBuff",
            parameters=new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = damageBoost
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = 12f
                },
                new AbilityAttributeParameter {
                    name = "element",
                    value = element.ToString()
                }
            }
        };
    }

    private static AbilityAttribute GetBlunting(AttackAbility ability) {
        return new AbilityAttribute {
            type = "blunting",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = AttackAbilityGenerator.CalculateDamage(ability.points * 0.5f)
                }
            }
        };
    }

    private static AbilityAttribute GetInflictVulnerability(AttackAbility ability) {
        var degreeRoll = UnityEngine.Random.Range(0, 3);
        var damageBoostNumbers = new List<float> { 100, 50, 25 };
        float damageBoost = damageBoostNumbers[degreeRoll];
        return new AbilityAttribute {
            type = "inflictVulnerability",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = damageBoost
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = 12f
                }
            }
        };
    }

    private static AbilityAttribute GetDelay(AttackAbility ability) {
        return new AbilityAttribute {
            type = "delay",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "time",
                    value = 4f
                }
            }
        };
    }

    private static AbilityAttribute GetDamageShield(AttackAbility ability) {
        return new AbilityAttribute {
            type = "damageShield",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = ability.points * 0.5f / 32
                }
            }
        };
    }

    private static AbilityAttribute GetRestoreMP(AttackAbility ability) {
        return new AbilityAttribute {
            type = "restoreMP",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = ability.points / 70f * 40f
                }
            }
        };
    }

    private static AbilityAttribute GetAddedDot(AttackAbility ability) {
        int roll = UnityEngine.Random.Range(0, 4);
        var dotDurations = new List<float> { 4f, 8f, 8f, 12f };
        var dotMultipliers = new List<float> { 1.5f, 3f, 3f, 4f };
        var dotDuration = dotDurations[roll];
        var dotMultiplier = dotMultipliers[roll];
        return new AbilityAttribute {
            type = "addedDot",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = AttackAbilityGenerator.CalculateDamage(ability.points * 0.5f * dotMultiplier)
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = dotDuration
                }
            }
        };
    }

    private static AbilityAttribute GetBackstab(AttackAbility ability) {
        return new AbilityAttribute {
            type = "backstab",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = 4f
                }
            }
        };
    }

    private static AbilityAttribute GetIncreasedCritChance(AttackAbility ability) {
        int roll = UnityEngine.Random.Range(10, 50);
        float increasedChance = roll / 100f;
        return new AbilityAttribute {
            type = "increasedCritChance",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = increasedChance
                }
            }
        };
    }

    private static AbilityAttribute GetIncreasedCritDamage(AttackAbility ability) {
        int roll = UnityEngine.Random.Range(100, 200);
        float increasedDamage = roll / 100f;
        return new AbilityAttribute {
            type = "increasedCritDamage",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = increasedDamage
                }
            }
        };
    }

    private static AbilityAttribute GetSpeedMinus(AttackAbility ability) {
        int degreeRoll = UnityEngine.Random.Range(20, 70);
        int durationRoll = UnityEngine.Random.Range(2, 10);
        float degree = degreeRoll / 100f;
        float duration = durationRoll;
        return new AbilityAttribute {
            type = "speed-",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degree
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = duration
                }
            }
        };
    }

    private static AbilityAttribute GetImmobilizeSelf(AttackAbility ability) {
        int roll = UnityEngine.Random.Range(2, 9);
        float duration = roll;
        return new AbilityAttribute {
            type = "immobilizeSelf",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "duration",
                    value = duration
                }
            }
        };
    }
}
