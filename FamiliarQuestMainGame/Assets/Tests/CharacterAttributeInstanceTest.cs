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
        [Test]
        [Timeout(10000)]
        [Description("It properly updates derived stats.")]
        public void CheckDerivedStatUpdates() {
            SetUpAttributes();
            var c = CreateCharacter();
            CharacterAttributeInstance.CreateAllAttributesForCharacter(c);
            CharacterAttribute.attributes["baseStat"].instances[c].BaseValue = 20;
            Assert.IsFalse(CharacterAttribute.attributes["derivedStat"].instances[c].TotalValue == 0);
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

