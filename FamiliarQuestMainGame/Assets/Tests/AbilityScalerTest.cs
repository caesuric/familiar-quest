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
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 1, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute>());
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
            });
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
            });
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
            });
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
            });
            Assert.Greater(output.damage, 0.75f);
        }

        [Test]
        [Description("It increases an ability's level from 1 to 2.")]
        public void CheckSimpleLevelIncrease() {
            var output = AbilityScaler.ScaleAttackAbility(AbilityCalculator.GetPointsFromLevel(2), Element.bashing, BaseStat.strength, 1, 0, 0, true, 0, 0, 0, 0, 0, 0, 0, 0, new List<AbilityAttribute>());
            Assert.AreEqual(output.level, 2);
        }
    }

}

