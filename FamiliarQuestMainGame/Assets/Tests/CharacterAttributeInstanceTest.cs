using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CharacterAttributeInstanceTest
    {
        Character character;

        [Test]
        [Timeout(10000)]
        [Description("It properly updates derived stats so they don't remain at 0.")]
        public void CheckDerivedStatUpdates() {
            CharacterAttributeInstance.CreateAllAttributesForCharacter(character);
            CharacterAttribute.attributes["baseStat"].instances[character].BaseValue = 20;
            Assert.IsFalse(CharacterAttribute.attributes["derivedStat"].instances[character].TotalValue == 0);
        }

        [Test]
        [Timeout(10000)]
        [Description("It properly scales derived stats gradually")]
        public void CheckThatDerivedStatScalingIsGradual() {
            CharacterAttribute.attributes["baseStat"].instances[character].BaseValue = 1;
            Assert.IsTrue(CharacterAttribute.attributes["derivedStat"].instances[character].TotalValue == 0f);
            CharacterAttribute.attributes["baseStat"].instances[character].BaseValue = 9;
            Assert.IsTrue(CharacterAttribute.attributes["derivedStat"].instances[character].TotalValue > 50f * 7f / 9f);
            CharacterAttribute.attributes["baseStat"].instances[character].BaseValue = 10;
            Assert.IsTrue(CharacterAttribute.attributes["derivedStat"].instances[character].TotalValue == 50f);
            CharacterAttribute.attributes["baseStat"].instances[character].BaseValue = 15;
            Assert.IsTrue(CharacterAttribute.attributes["derivedStat"].instances[character].TotalValue == 75f);
            CharacterAttribute.attributes["baseStat"].instances[character].BaseValue = 20;
            Assert.IsTrue(CharacterAttribute.attributes["derivedStat"].instances[character].TotalValue == 100f);
        }

        [SetUp]
        public void Setup() {
            SetUpAttributes();
            character = CreateCharacter();
        }

        private Character CreateCharacter() {
            var cgo = new GameObject();
            cgo.AddComponent<NoiseMaker>();
            cgo.AddComponent<SimulatedNoiseGenerator>();
            cgo.AddComponent<AudioGenerator>();
            cgo.AddComponent<AnimationController>();
            cgo.AddComponent<ExperienceGainer>();
            cgo.AddComponent<CacheGrabber>();
            cgo.AddComponent<Attacker>();
            cgo.AddComponent<ObjectSpawner>();
            cgo.AddComponent<StatusEffectHost>();
            cgo.AddComponent<AbilityUser>();
            cgo.AddComponent<SpiritUser>();
            cgo.AddComponent<ConfigGrabber>();
            cgo.AddComponent<HotbarUser>();
            cgo.AddComponent<Health>();
            cgo.AddComponent<Mana>();
            cgo.AddComponent<PlayerCharacter>();
            return cgo.AddComponent<Character>();
        }
                
        private void SetUpAttributes() {
            ClearAttributes();
            new CharacterAttribute("baseStat", "Base Stat", false);
            new CharacterAttribute("derivedStat", "Derived Stat", true, new List<string> { "baseStat" }, 0, 50, 100, 0, 500, 1000);
        }

        private void ClearAttributes() {
            CharacterAttribute.attributes.Clear();
        }
    }

}

