using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ScalingTest
    {
        Character player = null;
        Character monster = null;

        [UnityTest]
        //[Timeout(1800000)]
        [Description("Player Character scales correctly based on loot acquired.")]
        public IEnumerator TestAllLootScaling() {
            var yieldCounter = 0;
            for (int i=1; i<=1; i++) {
                Debug.Log("----------");
                Debug.Log(i.ToString());
                float totalStats = 0;
                int count = 0;
                for (int j = 0; j < 100; j++) {
                    Debug.Log("Player number " + (j+1).ToString());
                    ResetPlayer();
                    var pc = player.GetComponent<PlayerCharacter>();
                    while (pc.GetComponent<ExperienceGainer>().level <= i) {
                        yieldCounter++;
                        if (monster==null || monster.GetComponent<MonsterScaler>().level != pc.GetComponent<ExperienceGainer>().level) CreateMonster(pc.GetComponent<ExperienceGainer>().level);
                        if (pc.GetComponent<ExperienceGainer>().level == i) {
                            count++;
                            totalStats += TotalCharacterStats(player);
                        }
                        pc.GetComponent<ExperienceGainer>().GainXP(monster.GetComponent<RewardGiver>().xpValue);
                        var roll = Random.Range(0, 1f);
                        if (roll <= 0.2f) {
                            var item = RewardGiver.GenerateItem(i);
                            EquipItemIfWanted(item);
                        }
                        if (yieldCounter >= 1000) {
                            yieldCounter = 0;
                            yield return null;
                        }
                    }
                }
                var average = totalStats / count / 6;
                Debug.Log(average);
                
            }
        }

        public void TestLootScaling(int targetLevel) {
            var pc = player.GetComponent<PlayerCharacter>();
            float totalStats = 0;
            int count = 0;
            for (int i=0; i<100; i++) {
                ResetPlayer();
                pc = player.GetComponent<PlayerCharacter>();
                while (pc.GetComponent<ExperienceGainer>().level <= targetLevel) {
                    if (monster.GetComponent<MonsterScaler>().level!=pc.GetComponent<ExperienceGainer>().level) CreateMonster(pc.GetComponent<ExperienceGainer>().level);
                    if (pc.GetComponent<ExperienceGainer>().level == targetLevel) {
                        count++;
                        totalStats += TotalCharacterStats(player);
                    }
                    pc.GetComponent<ExperienceGainer>().GainXP(monster.GetComponent<RewardGiver>().xpValue);
                    var roll = Random.Range(0, 1f);
                    if (roll <= 0.2f) {
                        var item = RewardGiver.GenerateItem(targetLevel);
                        EquipItemIfWanted(item);
                    }
                }
            }
            var average = totalStats / count / 6;
            Debug.Log(average);
        }

        public void EquipItemIfWanted(Item item) {
            var pc = player.GetComponent<PlayerCharacter>();
            if (!(item is Equipment)) return;
            var equipment = item as Equipment;
            if (item is Weapon && ((Weapon)item).attackPower > pc.weapon.attackPower) EquipWeapon((Weapon)item);
            else if (equipment is Armor && TotalGearStats(equipment) > TotalGearStats(pc.armor)) EquipArmor((Armor)equipment);
            else if (equipment is Necklace && TotalGearStats(equipment) > TotalGearStats(pc.necklace)) EquipNecklace((Necklace)equipment);
            else if (equipment is Bracelet) EquipBracelet((Bracelet)equipment);
            else if (equipment is Cloak && TotalGearStats(equipment) > TotalGearStats(pc.cloak)) EquipCloak((Cloak)equipment);
            else if (equipment is Earring && TotalGearStats(equipment) > TotalGearStats(pc.earring)) EquipEarring((Earring)equipment);
            else if (equipment is Hat && TotalGearStats(equipment) > TotalGearStats(pc.hat)) EquipHat((Hat)equipment);
            else if (equipment is Shoes && TotalGearStats(equipment) > TotalGearStats(pc.shoes)) EquipShoes((Shoes)equipment);
        }

        public void EquipWeapon(Weapon weapon) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevWeapon = pc.weapon;
            pc.ModifyStats(prevWeapon, weapon);
            pc.weapon = weapon;
        }

        public void EquipArmor(Armor armor) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevArmor = pc.armor;
            pc.ModifyStats(prevArmor, armor);
            pc.armor = armor;
        }

        public void EquipNecklace(Necklace necklace) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevNecklace = pc.necklace;
            pc.ModifyStats(prevNecklace, necklace);
            pc.necklace = necklace;
        }

        public void EquipBracelet(Bracelet bracelet) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevBraceletSlot = FindWorstBraceletSlot();
            var prevBracelet = pc.bracelets[prevBraceletSlot];
            if (TotalGearStats(prevBracelet) < TotalGearStats(bracelet)) {
                pc.ModifyStats(prevBracelet, bracelet);
                pc.bracelets[prevBraceletSlot] = bracelet;
            }
        }

        public int FindWorstBraceletSlot() {
            var pc = player.GetComponent<PlayerCharacter>();
            int output = 0;
            float lowest = 10000f;
            for (int i = 0; i < 4; i++) {
                var totalStats = TotalGearStats(pc.bracelets[i]);
                if (totalStats < lowest) {
                    output = i;
                    lowest = totalStats;
                }
            }
            return output;
        }

        public int TotalGearStats(Equipment equipment) {
            var totalStats = 0;
            if (equipment != null) foreach (var kvp in equipment.stats) totalStats += kvp.Value;
            return totalStats;            
        }

        public void EquipCloak(Cloak cloak) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevCloak = pc.cloak;
            pc.ModifyStats(prevCloak, cloak);
            pc.cloak = cloak;
        }

        public void EquipEarring(Earring earring) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevEarring = pc.earring;
            pc.ModifyStats(prevEarring, earring);
            pc.earring = earring;
        }

        public void EquipHat(Hat hat) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevHat = pc.hat;
            pc.ModifyStats(prevHat, hat);
            pc.hat = hat;
        }

        public void EquipShoes(Shoes shoes) {
            var pc = player.GetComponent<PlayerCharacter>();
            var prevShoes = pc.shoes;
            pc.ModifyStats(prevShoes, shoes);
            pc.shoes = shoes;
        }

        public int TotalCharacterStats(Character character) {
            int total = 0;
            foreach (var kvp in CharacterAttribute.attributes) if (!CharacterAttribute.attributes[kvp.Key].isSecondary) total += (int)CharacterAttribute.attributes[kvp.Key].instances[character].TotalValue;
            total += character.GetComponent<ExperienceGainer>().sparePoints;
            return total;
        }

        public void ResetPlayer() {
            if (player!=null) GameObject.Destroy(player.gameObject);
            player = CreatePlayer();
        }

        private Character CreatePlayer() {
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
            cgo.AddComponent<ConfigGrabber>();
            cgo.AddComponent<HotbarUser>();
            cgo.AddComponent<Health>();
            cgo.AddComponent<Mana>();
            cgo.AddComponent<PlayerCharacter>();
            var output = cgo.AddComponent<Character>();
            CharacterAttribute.attributes["strength"].instances[output].BaseValue = 10;
            CharacterAttribute.attributes["dexterity"].instances[output].BaseValue = 10;
            CharacterAttribute.attributes["constitution"].instances[output].BaseValue = 10;
            CharacterAttribute.attributes["intelligence"].instances[output].BaseValue = 10;
            CharacterAttribute.attributes["wisdom"].instances[output].BaseValue = 10;
            CharacterAttribute.attributes["luck"].instances[output].BaseValue = 10;
            return output;
        }

        private void CreateMonster(int level) {
            if (monster != null) GameObject.Destroy(monster.gameObject);
            var cgo = new GameObject();
            cgo.AddComponent<NoiseMaker>();
            cgo.AddComponent<SimulatedNoiseGenerator>();
            cgo.AddComponent<AudioGenerator>();
            cgo.AddComponent<AnimationController>();
            cgo.AddComponent<CacheGrabber>();
            cgo.AddComponent<Attacker>();
            cgo.AddComponent<ObjectSpawner>();
            cgo.AddComponent<StatusEffectHost>();
            cgo.AddComponent<AbilityUser>();
            cgo.AddComponent<ConfigGrabber>();
            cgo.AddComponent<Health>();
            cgo.AddComponent<Mana>();
            cgo.AddComponent<Monster>();
            cgo.AddComponent<RewardGiver>();
            var ms = cgo.AddComponent<MonsterScaler>();
            var c = cgo.AddComponent<Character>();
            ms.AdjustForLevel(level);
            ms.Scale();
            monster = c;
        }
    }
}
