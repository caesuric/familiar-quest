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
    public class AbilitySkillTreeNodeTest
    {
        [Test]
        [Description("Reduce MP Usage actually reduces MP usage.")]
        public void CheckReduceMpUsageReducesMpUsage() {
            var ability = new AttackAbility {
                baseMpUsage = 40,
                mpUsage = 40,
                attributes = new List<AbilityAttribute>(),
                damage = 1f,
                dotDamage = 0f,
                dotTime = 0f,
                element = Element.bashing,
                isRanged = true,
                level = 1,
                points = 70f,
                radius = 0,
                xp = 0
            };
            var skillTreeNode = new AbilitySkillTreeNode(ability, new List<AbilitySkillTreeNode>());
            skillTreeNode.effects.Add(new SoulGemEnhancement {
                type = "reduceMpUsage",
                effect = -5
            });
            skillTreeNode.Activate();
            Assert.Less(ability.mpUsage, 40);
            Assert.Less(ability.baseMpUsage, 40);
        }

        [SetUp]
        public void Setup() {
            
        }     
    }

}

