using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class PassiveAbilityAttributeGenerator {
    private delegate AbilityAttribute AbilityAttributeDelegate(PassiveAbility ability);
    private static readonly List<string> simpleAttributes;
    private static readonly Dictionary<string, AbilityAttributeDelegate> attributes;

    static PassiveAbilityAttributeGenerator() {
        simpleAttributes = new List<string> {
            "knockback",
            "charge",
            "pullEnemies"
        };
        attributes = new Dictionary<string, AbilityAttributeDelegate> {
            ["damageEnemiesOnScreen"] = GetDamageEnemiesOnScreen,
            ["experienceBoost"] = GetExperienceBoost,
            ["goldBoost"] = GetGoldBoost,
            ["boostStat"] = GetBoostStat,
            ["boostDamage"] = GetBoostDamage,
            ["reduceDamage"] = GetReduceDamage,
            ["reduceElementalDamage"] = ReduceElementalDamage,
            ["boostElementalDamage"] = BoostElementalDamage
        };
    }

    public static AbilityAttribute Generate(PassiveAbility ability) {
        for (int i = 0; i < 10000; i++) {
            string roll = TableRoller.Roll("PassiveAttributes");
            AbilityAttribute attribute;
            if (simpleAttributes.Contains(roll)) {
                attribute = new AbilityAttribute {
                    type = roll
                };
                attribute.priority = RNG.Float(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
            attribute = attributes[roll](ability);
            if (attribute != null) {
                attribute.priority = RNG.Float(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
        }
        Debug.Log("FAILED TO FIND VALID ATTRIBUTE FOR ABILITY!");
        return null;
    }

    public static AbilityAttribute Generate(PassiveAbility ability, string attributeType) {
        for (int i = 0; i < 10000; i++) {
            AbilityAttribute attribute;
            if (simpleAttributes.Contains(attributeType)) {
                attribute = new AbilityAttribute {
                    type = attributeType
                };
                attribute.priority = RNG.Float(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
            attribute = attributes[attributeType](ability);
            if (attribute != null) {
                attribute.priority = RNG.Float(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
        }
        Debug.Log("FAILED TO FIND VALID ATTRIBUTE FOR ABILITY!");
        return null;
    }

    private static AbilityAttribute GetDamageEnemiesOnScreen(PassiveAbility ability) {
        int degree = RNG.Int(1, 23);
        return new AbilityAttribute {
            type = "damageEnemiesOnScreen",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = (float)degree
                }
            }
        };
    }

    private static AbilityAttribute GetExperienceBoost(PassiveAbility ability) {
        int degree = RNG.Int(5, 31);
        return new AbilityAttribute {
            type = "experienceBoost",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = (float)degree
                }
            }
        };
    }

    private static AbilityAttribute GetGoldBoost(PassiveAbility ability) {
        int degree = RNG.Int(5, 31);
        return new AbilityAttribute {
            type = "goldBoost",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = (float)degree
                }
            }
        };
    }

    private static AbilityAttribute GetBoostStat(PassiveAbility ability) {
        int degree = RNG.Int(3, 101);
        var stat = RNG.EnumValue<BaseStat>().ToString();
        return new AbilityAttribute {
            type = "boostStat",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = (float)degree
                },
                new AbilityAttributeParameter {
                    name = "stat",
                    value = stat
                }
            }
        };
    }

    private static AbilityAttribute GetBoostDamage(PassiveAbility ability) {
        int degree = RNG.Int(5, 31);
        return new AbilityAttribute {
            type = "boostDamage",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degree / 100f
                }
            }
        };
    }

    private static AbilityAttribute GetReduceDamage(PassiveAbility ability) {
        int degree = RNG.Int(15, 31);
        return new AbilityAttribute {
            type = "reduceDamage",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degree / 100f
                }
            }
        };
    }

    private static AbilityAttribute ReduceElementalDamage(PassiveAbility ability) {
        int degree = RNG.Int(60, 151);
        string element = RNG.EnumValue<Element>().ToString();
        while (element == "none") element = RNG.EnumValue<Element>().ToString();
        return new AbilityAttribute {
            type = "reduceElementalDamage",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degree / 100f
                },
                new AbilityAttributeParameter {
                    name = "element",
                    value = element
                }
            }
        };
    }

    private static AbilityAttribute BoostElementalDamage(PassiveAbility ability) {
        int degree = RNG.Int(60, 151);
        string element = RNG.EnumValue<Element>().ToString();
        while (element == "none") element = RNG.EnumValue<Element>().ToString();
        return new AbilityAttribute {
            type = "boostElementalDamage",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degree / 100f
                },
                new AbilityAttributeParameter {
                    name = "element",
                    value = element
                }
            }
        };
    }
}
