using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [Timeout(10000)]
    public class AbilityScalerTest
    {
        [Test]
        [Description("When leveling an attack ability from level 1 to 2, base damage increases.")]
        public void CheckSimpleDamageIncrease() {
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 1, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute>(), new AbilitySkillTree());
            Assert.Greater(output.damage, 1);
        }

        [Test]
        [Description("When leveling an attack ability, base damage increases even if there is an attribute on the ability")]
        public void CheckSimpleDamageIncreaseWithAttribute() {
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 0.3f, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute> {
                new AbilityAttribute {
                    type = "projectileSpread",
                    priority = 100
                }
            }, new AbilitySkillTree());
            Assert.Greater(output.damage, 0.3f);
        }

        [Test]
        [Description("When leveling an attack ability, base damage increases even if there are two attributes on the ability")]
        public void CheckSimpleDamageIncreaseWithTwoAttributes() {
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 0.15f, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute> {
                new AbilityAttribute {
                    type = "projectileSpread",
                    priority = 100
                },
                new AbilityAttribute {
                    type = "offGCD",
                    priority = 100
                }
            }, new AbilitySkillTree());
            Assert.Greater(output.damage, 0.15f);
        }

        [Test]
        [Description("When leveling an attack ability, base damage increases when there is a negative attribute followed by a positive one")]
        public void CheckDamageIncreaseWithNegativeAndPositiveAttribute() {
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 0.75f, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute> {
                new AbilityAttribute {
                    type = "delay",
                    priority = 100
                },
                new AbilityAttribute {
                    type = "offGCD",
                    priority = 100
                }
            }, new AbilitySkillTree());
            Assert.Greater(output.damage, 0.75f);
        }

        [Test]
        [Description("When leveling an attack ability, base damage increases when there is a positive attribute followed by a negative one")]
        public void CheckDamageIncreaseWithPositiveAndNegativeAttribute() {
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 0.75f, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute> {
                new AbilityAttribute {
                    type = "offGCD",
                    priority = 100
                },
                new AbilityAttribute {
                    type = "delay",
                    priority = 100
                }
            }, new AbilitySkillTree());
            Assert.Greater(output.damage, 0.75f);
        }

        [Test]
        [Description("It increases an ability's level from 1 to 2.")]
        public void CheckSimpleLevelIncrease() {
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 1, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute>(), new AbilitySkillTree());
            Assert.AreEqual(output.level, 2);
        }

        [Test]
        [Description("When leveling a passive stat boost ability from 1 to 2, it boosts the stat increase by 5%.")]
        public void CheckLevelPassiveStatBoost() {
            var passive = new PassiveAbility {
                level = 1,
                xp = 0,
                points = 70f,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        priority = 100f,
                        type = "boostStat",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "stat",
                                value = "strength"
                            },
                            new AbilityAttributeParameter {
                                name = "degree",
                                value = 3f
                            }
                        }
                    }
                }
            };
            passive.GainExperience(ExperienceGainer.xpTable[0]);
            Assert.AreEqual(passive.level, 2);
            Assert.Less((float)passive.FindAttribute("boostStat").FindParameter("degree").value, 4f);
        }

        [Test]
        [Description("Levels an ability, then activates a reduce dot time skill tree node, then levels an ability")]
        public void LevelAbilityThenActivateReduceDotTimeNodeThenLevel() {
            AttackAbility ability;
            while (true) {
                ability = AttackAbilityGenerator.Generate();
                if (AbilityHasReduceDotTime(ability)) break;
            }
            var node = FindReduceDotTime(ability);
            ability.GainExperience(ExperienceGainer.xpTable[0]);
            node.active = true;
            node.Activate();
            var dotTimeBefore = ability.dotTime;
            ability.GainExperience(ExperienceGainer.xpTable[1]);
            var dotTimeAfter = ability.dotTime;
            Assert.AreEqual(dotTimeBefore, dotTimeAfter);
        }

        private bool AbilityHasReduceDotTime(AttackAbility ability) {
            foreach (var layer in ability.skillTree.nodesByLayer) {
                foreach (var node in layer) {
                    foreach (var effect in node.effects) {
                        if (effect.type == "reduceDotTime") return true;
                    }
                }
            }
            return false;
        }

        private AbilitySkillTreeNode FindReduceDotTime(AttackAbility ability) {
            foreach (var layer in ability.skillTree.nodesByLayer) {
                foreach (var node in layer) {
                    foreach (var effect in node.effects) {
                        if (effect.type == "reduceDotTime") return node;
                    }
                }
            }
            return null;
        }

        //[Test]
        //[Description("Temporary - creates AttackAbility instances and tries to break them by leveling them up")]
        //public void ExploratoryTestForAttackAbilityBreakOnLevelUp() {
        //    int dotCount = 0;
        //    int count = 0;
        //    int positiveDotCount = 0;
        //    int positiveCount = 0;
        //    for (int i=0; i<10000; i++) {
        //        var attackAbility = AttackAbilityGenerator.Generate();
        //        var damageBefore = attackAbility.damage + attackAbility.dotDamage;
        //        attackAbility.GainExperience(ExperienceGainer.xpTable[0]);
        //        var damageAfter = attackAbility.damage + attackAbility.dotDamage;
        //        if (damageAfter < damageBefore) {
        //            if (attackAbility.dotDamage > 0) dotCount++;
        //            Debug.Log("---------------------");
        //            Debug.Log(attackAbility.name + " had lowered attack after leveling up.");
        //            Debug.Log("Cooldown: " + attackAbility.cooldown.ToString());
        //            Debug.Log("MP Cost:" + attackAbility.mpUsage.ToString());
        //            Debug.Log("Went from " + damageBefore.ToString() + " to " + damageAfter.ToString());
        //            Debug.Log("Attributes:");
        //            int attributeCount = 0;
        //            foreach (var attribute in attackAbility.attributes) {
        //                if (attribute.priority >= 50 && attributeCount < 4) {
        //                    attributeCount++;
        //                    Debug.Log(attribute.type);
        //                }
        //            }
        //            Debug.Log((attackAbility.attributes.Count - attributeCount).ToString() + " latent attributes hidden.");
        //            count++;
        //        }
        //        else if (damageAfter > damageBefore) {
        //            //Debug.Log("---------------------");
        //            //Debug.Log(attackAbility.name + " had INCREASED attack after leveling up.");
        //            //Debug.Log("Points: " + attackAbility.points.ToString());
        //            //Debug.Log("Cooldown: " + attackAbility.cooldown.ToString());
        //            //Debug.Log("MP Cost:" + attackAbility.mpUsage.ToString());
        //            //Debug.Log("Went from " + damageBefore.ToString() + " to " + damageAfter.ToString());
        //            //Debug.Log("Attributes:");
        //            //foreach (var attribute in attackAbility.attributes) Debug.Log(attribute.type);
        //            positiveCount++;
        //            if (attackAbility.dotDamage > 0) positiveDotCount++;
        //        }
        //    }
        //    Debug.Log("---------------------");
        //    Debug.Log(count.ToString() + " failures found.");
        //    Debug.Log(dotCount.ToString() + " of those were DoTs.");
        //    Debug.Log(positiveCount.ToString() + " successes found.");
        //    Debug.Log(positiveDotCount.ToString() + " of those were DoTs.");
        //}
    }

}

