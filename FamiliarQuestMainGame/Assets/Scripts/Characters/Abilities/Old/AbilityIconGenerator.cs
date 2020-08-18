//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public static class AbilityIconGenerator {
//    public static int Retrieve(UtilityAbility ability) {
//        foreach (var attribute in ability.attributes) {
//            switch (attribute.type) {
//                case "restoreMP":
//                    return 60;
//                case "shield":
//                    return 19;
//                case "elementalDamageBuff":
//                    var element = attribute.FindParameter("element").stringVal;
//                    switch (element) {
//                        case "slashing":
//                            return 33;
//                        case "piercing":
//                            return 3;
//                        case "bashing":
//                            return 32;
//                        case "fire":
//                            return 9;
//                        case "ice":
//                            return 34;
//                        case "acid":
//                            return 12;
//                        case "light":
//                            return 35;
//                        case "dark":
//                            return 36;
//                    }
//                    break;
//                case "heal":
//                    return 21;
//                case "hot":
//                    return 61;
//                case "mpOverTime":
//                    return 60;
//                case "stealthy":
//                    break;
//                case "usableWhileParalyzed":
//                    break;
//                case "offGCD":
//                    break;
//                case "disableDevice":
//                    return 62;
//                case "stealth":
//                    return 64;
//                case "grapple":
//                    return 66;
//                case "speed-":
//                    return 70;
//                case "speed+":
//                    return 5;
//                case "paralyze":
//                    return 20;
//                default:
//                    break;
//            }
//        }
//        return 0;
//    }

//    public static int Retrieve(PassiveAbility ability) {
//        foreach (var attribute in ability.attributes) {
//            switch (attribute.type) {
//                case "damageEnemiesOnScreen":
//                    return 68;
//                case "experienceBoost":
//                    return 69;
//                case "knockback":
//                    return 71;
//                case "charge":
//                    return 5;
//                case "pullEnemies":
//                    return 72;
//                case "goldBoost":
//                    return 73;
//                case "boostDamage":
//                    return 77;
//                case "boostElementalDamage":
//                    switch (attribute.FindParameter("element").stringVal) {
//                        default:
//                        case "piercing":
//                            return 26;
//                        case "slashing":
//                            return 25;
//                        case "bashing":
//                            return 24;
//                        case "fire":
//                            return 28;
//                        case "ice":
//                            return 29;
//                        case "acid":
//                            return 27;
//                        case "light":
//                            return 30;
//                        case "dark":
//                            return 31;
//                    }
//                case "reduceDamage":
//                    return 19;
//                case "reduceElementalDamage":
//                    return 19;
//                case "boostStat":
//                    switch (attribute.FindParameter("stat").stringVal) {
//                        case "strength":
//                            return 74;
//                        case "dexterity":
//                            return 5;
//                        case "constitution":
//                            return 48;
//                        case "intelligence":
//                            return 55;
//                        case "wisdom":
//                            return 75;
//                        case "luck":
//                            return 76;
//                    }
//                    break;
//                default:
//                    return 0;
//            }
//        }
//        return 0;
//    }
//}
