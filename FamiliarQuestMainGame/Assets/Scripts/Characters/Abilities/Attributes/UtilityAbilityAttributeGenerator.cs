using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

public static class UtilityAbilityAttributeGenerator {
    private delegate AbilityAttribute AbilityAttributeDelegate(UtilityAbility ability);
    private static readonly List<string> simpleAttributes;
    private static readonly Dictionary<string, AbilityAttributeDelegate> attributes;

    static UtilityAbilityAttributeGenerator() {
        simpleAttributes = new List<string> {
            "offGCD",
            "usableWhileParalyzed",
            "stealth",
            "stealthy",
            "removeDebuff",
            "removeAllDebuffs",
            "eatDebuff",
            "grapplingHook"
        };
        attributes = new Dictionary<string, AbilityAttributeDelegate> {
            ["mpOverTime"] = GetMpOverTime,
            ["elementalDamageBuff"] = GetElementalDamageBuff,
            ["heal"] = GetHeal,
            ["hot"] = GetHot,
            ["shield"] = GetShield,
            ["restoreMP"] = GetRestoreMp,
            ["disableDevice"] = GetDisableDevice,
            ["speed-"] = GetSpeedMinus,
            ["paralyze"] = GetParalyze,
            ["immobilizeSelf"] = GetImmobilizeSelf,
            ["speed+"] = GetSpeedPlus
        };
    }

    public static AbilityAttribute Generate(UtilityAbility ability) {
        for (int i = 0; i < 10000; i++) {
            string roll = TableRoller.Roll("UtilityAttributes_" + ability.targetType);
            AbilityAttribute attribute;
            if (simpleAttributes.Contains(roll)) {
                attribute = new AbilityAttribute {
                    type = roll
                };
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
            attribute = attributes[roll](ability);
            if (attribute != null) {
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
        }
        Debug.Log("FAILED TO FIND VALID ATTRIBUTE FOR ABILITY!");
        return null;
    }

    public static AbilityAttribute Generate(UtilityAbility ability, string attributeType) {
        for (int i = 0; i < 10000; i++) {
            AbilityAttribute attribute;
            if (simpleAttributes.Contains(attributeType)) {
                attribute = new AbilityAttribute {
                    type = attributeType
                };
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
            attribute = attributes[attributeType](ability);
            if (attribute != null) {
                attribute.priority = UnityEngine.Random.Range(12.5f, 100f);
                attribute.points = AbilityAttributeAppraiser.Appraise(ability, attribute);
                return attribute;
            }
        }
        Debug.Log("FAILED TO FIND VALID ATTRIBUTE FOR ABILITY!");
        return null;
    }

    private static AbilityAttribute GetMpOverTime(UtilityAbility ability) {
        return new AbilityAttribute {
            type = "mpOverTime",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = 80f * ability.points / 70f
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = 8f
                }
            }
        };
    }

    private static AbilityAttribute GetElementalDamageBuff(UtilityAbility ability) {
        var roll = UnityEngine.Random.Range(0, 3);
        var damageBoostTable = new List<float> { 100, 50, 25 };
        var damageBoost = damageBoostTable[roll];
        var element = RNG.EnumValue<Element>();
        while (element == Element.none) element = RNG.EnumValue<Element>();
        return new AbilityAttribute {
            type = "elementalDamageBuff",
            parameters = new List<AbilityAttributeParameter>
            {
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

    private static AbilityAttribute GetHeal(UtilityAbility ability) {
        return new AbilityAttribute {
            type = "heal",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = ability.points / 20f
                }
            }
        };
    }

    private static AbilityAttribute GetHot(UtilityAbility ability) {
        return new AbilityAttribute {
            type = "hot",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = ability.points / 10f
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = 8f
                }
            }
        };
    }

    private static AbilityAttribute GetShield(UtilityAbility ability) {
        return new AbilityAttribute {
            type = "shield",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = ability.points / 40f
                },
                new AbilityAttributeParameter {
                    name = "stat",
                    value = "strength"
                }
            }
        };
    }

    private static AbilityAttribute GetRestoreMp(UtilityAbility ability) {
        return new AbilityAttribute {
            type = "restoreMP",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = ability.points * 0.29f
                }
            }
        };
    }

    private static AbilityAttribute GetDisableDevice(UtilityAbility ability) {
        return new AbilityAttribute {
            type = "disableDevice",
            parameters = new List<AbilityAttributeParameter>
            {
                new AbilityAttributeParameter {
                    name = "radius",
                    value = 2f
                }
            }
        };
    }

    private static AbilityAttribute GetSpeedMinus(UtilityAbility ability) {
        int degreeRoll = UnityEngine.Random.Range(20, 70);
        int durationRoll = UnityEngine.Random.Range(2, 9);
        float radius = 2f;
        bool rollForRadius = false;
        if (ability.targetType == "none") rollForRadius = true;
        else {
            int rollForRoll = UnityEngine.Random.Range(0, 2);
            if (rollForRoll == 0) rollForRadius = true;
        }
        if (rollForRadius) {
            int radiusRoll = UnityEngine.Random.Range(0, 30);
            if (radiusRoll < 21) radius = 2;
            else if (radiusRoll < 27) radius = 4;
            else if (radiusRoll < 29) radius = 6;
            else radius = 8;
        }
        if (rollForRadius) return new AbilityAttribute {
            type = "speed-",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degreeRoll / 100f
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = (float)durationRoll
                },
                new AbilityAttributeParameter {
                    name = "radius",
                    value = radius
                }
            }
        };
        else return new AbilityAttribute {
            type = "speed-",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degreeRoll / 100f
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = (float)durationRoll
                }
            }
        };
    }

    private static AbilityAttribute GetParalyze(UtilityAbility ability) {
        int duration = RNG.Int(2, 9);
        bool rollForRadius = false;
        float radius = 2f;
        if (ability.targetType == "none") rollForRadius = true;
        else rollForRadius = RNG.Bool();
        if (rollForRadius) {
            int radiusRoll = RNG.Int(0, 30);
            if (radiusRoll < 21) radius = 2;
            else if (radiusRoll < 27) radius = 4;
            else if (radiusRoll < 29) radius = 6;
            else radius = 8;
        }
        if (rollForRadius) return new AbilityAttribute {
            type = "paralyze",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "radius",
                    value = radius
                },
                new AbilityAttributeParameter {
                    name = "duration",
                    value = (float)duration
                }
            }
        };
        else return new AbilityAttribute {
            type = "paralyze",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "duration",
                    value = (float)duration
                }
            }
        };
    }

    private static AbilityAttribute GetImmobilizeSelf(UtilityAbility ability) {
        int duration = RNG.Int(2, 9);
        return new AbilityAttribute {
            type = "immobilizeSelf",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "duration",
                    value = (float)duration
                }
            }
        };
    }

    private static AbilityAttribute GetSpeedPlus(UtilityAbility ability) {
        int duration = RNG.Int(2, 11);
        int degree = RNG.Int(30, 120);
        if (degree > 100) degree = 100;
        return new AbilityAttribute {
            type = "speed+",
            parameters = new List<AbilityAttributeParameter> {
                new AbilityAttributeParameter {
                    name = "duration",
                    value = (float)duration
                },
                new AbilityAttributeParameter {
                    name = "degree",
                    value = degree / 100f
                }
            }
        };
    }
}
