//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public static class AbilityNamer {
//    private static readonly Dictionary<Element, Dictionary<bool, string>> baseAttackNames;

//    static AbilityNamer() {
//        baseAttackNames = new Dictionary<Element, Dictionary<bool, string>>() {
//            {
//                Element.acid, new Dictionary<bool, string>() {
//                    { true, "Acid Bolt" },
//                    { false, "Acid Strike" }
//                }
//            },
//            {
//                Element.fire, new Dictionary<bool, string>() {
//                    { true, "Firebolt" },
//                    { false, "Cinderstrike" }
//                }
//            },
//            {
//                Element.ice, new Dictionary<bool, string>() {
//                    { true, "Frostbolt" },
//                    { false, "Froststrike" }
//                }
//             },
//            {
//                Element.dark, new Dictionary<bool, string>() {
//                    { true, "Voidbolt" },
//                    { false, "Voidstrike" }
//                }
//            },
//            {
//                Element.light, new Dictionary<bool, string>() {
//                    { true, "Sunbolt" },
//                    { false, "Sunstrike" }
//                }
//            },
//            {
//                Element.piercing, new Dictionary<bool, string>() {
//                    { true, "Arrow" },
//                    { false, "Stab" }
//                }
//            },
//            {
//                Element.bashing, new Dictionary<bool, string>() {
//                    { true, "Stone" },
//                    { false, "Bash" }
//                }
//            },
//            {
//                Element.slashing, new Dictionary<bool, string>() {
//                    { true, "Shuriken" },
//                    { false, "Slash" }
//                }
//            },
//            {
//                Element.none, new Dictionary<bool, string>() {
//                    { true, "Nullbolt" },
//                    { false, "Nullstrike" }
//                }
//            }
//        };
//    }

//    public static string Name(UtilityAbility ability) {
//        string baseName = "";
//        string nameMod = "";
//        int count = 0;
//        foreach (var attribute in ability.attributes) {
//            if (attribute.priority < 50 || count > 3) continue;
//            switch (attribute.type) {
//                case "restoreMP":
//                    baseName = "Concentration";
//                    break;
//                case "shield":
//                    baseName = "Shield";
//                    break;
//                case "elementalDamageBuff":
//                    var element = attribute.FindParameter("element").stringVal;
//                    baseName = element[0].ToString().ToUpper() + element.Substring(1) + " " + "Boost";
//                    break;
//                case "heal":
//                    baseName = "Healing";
//                    break;
//                case "hot":
//                    baseName = "Regeneration";
//                    break;
//                case "mpOverTime":
//                    baseName = "Meditation";
//                    break;
//                case "stealthy":
//                    nameMod = "Subtle";
//                    break;
//                case "usableWhileParalyzed":
//                    nameMod = "Cognitive";
//                    break;
//                case "offGCD":
//                    nameMod = "Swift";
//                    break;
//                case "disableDevice":
//                    baseName = "Disable Device";
//                    break;
//                case "stealth":
//                    baseName = "Stealth";
//                    break;
//                case "grapple":
//                    baseName = "Grappling Hook";
//                    break;
//                case "speed-":
//                    baseName = "Slow";
//                    break;
//                case "paralyze":
//                    baseName = "Paralyze";
//                    break;
//                case "immobilizeSelf":
//                    nameMod = "Turret";
//                    break;
//                case "removeDebuff":
//                    baseName = "Cleanse";
//                    break;
//                case "removeAllDebuffs":
//                    baseName = "Greater Cleanse";
//                    break;
//                case "eatDebuff":
//                    baseName = "Consume Ailment";
//                    break;
//                case "speed+":
//                    baseName = "Swiftness";
//                    break;
//                default:
//                    break;
//            }
//            count++;
//        }
//        if (nameMod == "") return baseName;
//        return nameMod + " " + baseName;
//    }

//    public static string Name(AttackAbility ability) {
//        var name = baseAttackNames[ability.element][ability.isRanged];
//        int prefixes = 0;
//        int suffixes = 0;
//        if (ability.mpUsage > 0) {
//            name = "Arcane " + name;
//            prefixes++;
//        }
//        if (ability.radius > 0) {
//            name = "Exploding " + name;
//            prefixes++;
//        }
//        if (ability.dotDamage > 0 && prefixes < 2) {
//            name = "Draining " + name;
//            prefixes++;
//        }
//        int count = 0;
//        foreach (var attribute in ability.attributes) {
//            if (attribute.priority < 50 || count > 3) continue;
//            switch (attribute.type) {
//                case "createDamageZone":
//                    if (suffixes < 1) {
//                        name = name + " of Danger";
//                        suffixes++;
//                    }
//                    break;
//                case "projectileSpread":
//                    if (suffixes < 1) name = name + " Fusillade";
//                    break;
//                case "jumpBack":
//                    if (prefixes < 2) {
//                        name = "Retreating " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "chargeTowards":
//                    if (prefixes < 2) {
//                        name = "Charging " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "pulltowards":
//                    if (suffixes < 1) {
//                        name = name + " of Grappling";
//                        suffixes++;
//                    }
//                    break;
//                case "knockback":
//                    if (prefixes < 2) {
//                        name = "Pushing " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "offGCD":
//                    if (prefixes < 2) {
//                        name = "Swift " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "paralyze":
//                    if (prefixes < 2) {
//                        name = "Paralyzing " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "usableWhileParalyzed":
//                    if (prefixes < 2) {
//                        name = "Cognitive " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "backstab":
//                    if (prefixes < 2) {
//                        name = "Treacherous " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "stealthy":
//                    if (prefixes < 2) {
//                        name = "Subtle " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "lifeleech":
//                    if (prefixes < 2) {
//                        name = "Vampiric " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "mpOverTime":
//                    if (prefixes < 2) {
//                        name = "Meditative " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "elementalDamageBuff":
//                    var type = attribute.FindParameter("element").stringVal;
//                    if (suffixes < 1) {
//                        switch (type) {
//                            case "bashing":
//                                name = name + " of Achilles";
//                                break;
//                            case "piercing":
//                                name = name + " of Achilles";
//                                break;
//                            case "slashing":
//                                name = name + " of Achilles";
//                                break;
//                            case "fire":
//                                name = name + " of the Inferno";
//                                break;
//                            case "ice":
//                                name = name + " of Frostbite";
//                                break;
//                            case "acid":
//                                name = name + " of Solvency";
//                                break;
//                            case "light":
//                                name = name + " of Sunburn";
//                                break;
//                            case "dark":
//                                name = name + " of the Voidgaze";
//                                break;
//                            case "none":
//                                name = name + " of Nullity";
//                                break;
//                        }
//                        suffixes++;
//                    }
//                    break;
//                case "blunting":
//                    if (suffixes < 1) {
//                        name = name + " of Blunting";
//                        suffixes++;
//                    }
//                    break;
//                case "inflictVulnerability":
//                    if (prefixes < 2) {
//                        name = "Tactical " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "delay":
//                    if (prefixes < 2) {
//                        name = "Delayed " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "damageShield":
//                    if (prefixes < 2) {
//                        name = "Aegis " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "restoreMP":
//                    if (suffixes < 1) {
//                        name = name + " of Concentration";
//                        suffixes++;
//                    }
//                    break;
//                case "removeDebuff":
//                    if (prefixes < 2) {
//                        name = "Cleansing " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "addedDot":
//                    if (prefixes < 2) {
//                        name = "Draining " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "increasedCritChance":
//                    if (prefixes < 2) {
//                        name = "Sharp " + name;
//                        prefixes++;
//                    }
//                    break;
//                case "increasedCritDamage":
//                    if (suffixes < 1) {
//                        name = name + " of Piercing";
//                        suffixes++;
//                    }
//                    break;
//                case "speed-":
//                    if (suffixes < 1) {
//                        name = name + " of Slowing";
//                        suffixes++;
//                    }
//                    break;
//                case "immobilizeSelf":
//                    if (suffixes < 1) {
//                        name = name + " of the Turret";
//                        suffixes++;
//                    }
//                    break;
//                default:
//                    break;
//            }
//            count++;
//        }
//        return name;
//    }

//    public static string Name(PassiveAbility ability) {
//        string baseName = "";
//        string nameMod = "";
//        int count = 0;
//        foreach (var attribute in ability.attributes) {
//            if (attribute.priority < 50 || count > 3) continue;
//            switch (attribute.type) {
//                case "damageEnemiesOnScreen":
//                default:
//                    baseName = "Burning Aura";
//                    break;
//                case "experienceBoost":
//                    baseName = "Fast Learner";
//                    break;
//                case "knockback":
//                    baseName = "Knockback";
//                    break;
//                case "charge":
//                    baseName = "Charge";
//                    break;
//                case "pullEnemies":
//                    baseName = "Grapple Gun";
//                    break;
//                case "goldBoost":
//                    baseName = "Miser";
//                    break;
//                case "boostDamage":
//                    baseName = "Damage Up";
//                    break;
//                case "reduceDamage":
//                    baseName = "Protection";
//                    break;
//                case "reduceElementalDamage":
//                    baseName = attribute.FindParameter("element").stringVal[0].ToString().ToUpper() + attribute.FindParameter("element").stringVal.Substring(1) + " Ward";
//                    break;
//                case "boostElementalDamage":
//                    baseName = attribute.FindParameter("element").stringVal[0].ToString().ToUpper() + attribute.FindParameter("element").stringVal.Substring(1) + " Boost";
//                    break;
//                case "boostStat":
//                    switch (attribute.FindParameter("stat").stringVal) {
//                        case "strength":
//                            baseName = "Strength Up";
//                            break;
//                        case "dexterity":
//                            baseName = "Dexterity Up";
//                            break;
//                        case "constitution":
//                            baseName = "Constitution Up";
//                            break;
//                        case "intelligence":
//                            baseName = "Intelligence Up";
//                            break;
//                        case "wisdom":
//                            baseName = "Wisdom Up";
//                            break;
//                        case "luck":
//                            baseName = "Luck Up";
//                            break;
//                    }
//                    break;
//            }
//            count++;
//        }
//        if (nameMod == "") return baseName;
//        return nameMod + " " + baseName;
//    }



//}
