using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RewardGiver : MonoBehaviour {
    public int xpValue;
    public int baseGoldValue;
    public int goldValue;
    public bool generatedMonster = false;
    private int intendedLevel = 1;

    void Start() {
        //if (NetworkServer.active) {
            float tempGoldValue = Random.Range((float)baseGoldValue / 2.0f, (float)baseGoldValue * 1.5f);
            goldValue = (int)tempGoldValue;
        //}
    }

    public void DropLoot(Character attacker, bool guaranteed = false) {
        if (generatedMonster) return;
        if (GetComponent<Gremlin>()!=null && GetComponent<Gremlin>().item!=null) {
            attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("GOT ITEM BACK", "Got item back from Gremlin!");
            PlayerCharacter.localPlayer.inventory.items.Add(GetComponent<Gremlin>().item);
            return;
        }
        //var lootChance = SecondaryStatUtility.CalcItemFindRate(attacker.luck, attacker.GetComponent<ExperienceGainer>().level);
        var lootChance = CharacterAttribute.attributes["itemFindRate"].instances[attacker.GetComponent<Character>()].TotalValue / 100f;
        if (GetComponent<Boss>()!=null) guaranteed = true;
        if (GetComponent<EnergyWisplet>() != null) guaranteed = true;
        float roll = Random.Range(0f, 1f);
        if (roll > lootChance && !guaranteed) return;
        var exactTreasureRoll = Random.Range(0, 1f);
        if (guaranteed) exactTreasureRoll = Random.Range(0.875f, 1f);
        if (exactTreasureRoll <= 0.25f) DropConsumable(attacker);
        else if (exactTreasureRoll <= 0.5f) DropEquipment(attacker, 0);
        else if (exactTreasureRoll <= 0.75f) DropEquipment(attacker, 1);
        else if (exactTreasureRoll <= 0.875f) DropEquipment(attacker, 2);
        else if (exactTreasureRoll <= 0.9375f) DropEquipment(attacker, 3);
        else if (exactTreasureRoll <= 0.96875f) DropEquipment(attacker, 4);
        else if (exactTreasureRoll <= 0.984375f) DropEquipment(attacker, 5);
        else if (exactTreasureRoll <= 0.9947917f) DropEquipment(attacker, 6);
        else DropEquipment(attacker, 7);
    }

    public static Item GenerateItem(int level) {
        var lootChance = 1f;
        float roll = Random.Range(0f, lootChance);
        if (roll <= 0.25f) return GenerateConsumable();
        else if (roll <= 0.5f) return GenerateEquipment(level, 0);
        else if (roll <= 0.75f) return GenerateEquipment(level, 1);
        else if (roll <= 0.875f) return GenerateEquipment(level, 2);
        else if (roll <= 0.9375f) return GenerateEquipment(level, 3);
        else if (roll <= 0.96875f) return GenerateEquipment(level, 4);
        else if (roll <= 0.984375f) return GenerateEquipment(level, 5);
        else if (roll <= 0.9947917f) return GenerateEquipment(level, 6);
        else return GenerateEquipment(level, 7);
    }
    
    public void DropEquipment(Character attacker, int quality) {
        int[] stat1 = { 0, 2, 3, 5, 7, 9, 12, 15 };
        int[] stat2 = { 0, 0, 2, 3, 4, 5, 6, 7 };
        int[] stat3 = { 0, 0, 0, 0, 2, 2, 3, 4 };
        var statPreAdjusted1 = (float)stat1[quality];
        var statPreAdjusted2 = (float)stat2[quality];
        var statPreAdjusted3 = (float)stat3[quality];
        intendedLevel = 1;
        if (GetComponent<MonsterScaler>()!=null && GetComponent<MonsterScaler>().level> 1) intendedLevel = GetComponent<MonsterScaler>().level;
        else if (GetComponent<Chest>()!=null && attacker.GetComponent<ExperienceGainer>().level>1) intendedLevel = attacker.GetComponent<ExperienceGainer>().level;
        for (int i = 1; i < intendedLevel; i++) {
            statPreAdjusted1 *= 1.1f;
            statPreAdjusted2 *= 1.1f;
            statPreAdjusted3 *= 1.1f;
        }
        var statAdjusted1 = (int)statPreAdjusted1;
        var statAdjusted2 = (int)statPreAdjusted2;
        var statAdjusted3 = (int)statPreAdjusted3;
        int armor = Random.Range(67 + (14 * intendedLevel), 135 + (29 * intendedLevel));
        Equipment item = null;
        if (SceneInitializer.instance != null && SceneInitializer.instance.inside && LevelGen.dungeonData != null) item = DropItemForLootAffinities(attacker, armor, quality);
        else item = DropItemDefault(attacker, armor, quality);
        if (statAdjusted1 > 0) BuffRandomStat(item, statAdjusted1);
        if (statAdjusted2 > 0) BuffRandomStat(item, statAdjusted2);
        if (statAdjusted3 > 0) BuffRandomStat(item, statAdjusted3);
        if (quality >= 2) BuffRandomSecondaryStat(item, statAdjusted1);
        if (quality >= 3) BuffRandomSecondaryStat(item, statAdjusted2);
        if (item == null) return;

        item.description += GetDescriptionText(item);
        item.level = intendedLevel;
        item.quality = quality;
        EquipmentNamer.NameEquipment(item);
        var pc = attacker.GetComponent<PlayerCharacter>();
        if (item is Armor && pc.armor == null) {
            pc.armor = (Armor)item;
            pc.ModifyStats(null, item);
            pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (item is Hat && pc.hat == null) {
            pc.hat = (Hat)item;
            pc.ModifyStats(null, item);
            pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (item is Shoes && pc.shoes == null) {
            pc.shoes = (Shoes)item;
            pc.ModifyStats(null, item);
            pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (item is Belt && pc.belt == null) {
            pc.belt = (Belt)item;
            pc.ModifyStats(null, item);
            pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (item is Cloak && pc.cloak == null) {
            pc.cloak = (Cloak)item;
            pc.ModifyStats(null, item);
            pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (item is Earring && pc.earring == null) {
            pc.earring = (Earring)item;
            pc.ModifyStats(null, item);
            pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (item is Necklace && pc.necklace == null) {
            pc.necklace = (Necklace)item;
            pc.ModifyStats(null, item);
            pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (item is Bracelet) {
            EquipBraceletIfPossible(item, pc);
        }
        else {
            pc.inventory.items.Add(item);
            DropsArea.AddItemDrop(item);
            StartCoroutine(pc.inventory.RefreshInABit());
        }
    }

    private Equipment DropItemForLootAffinities(Character attacker, int armor, int quality) {
        try {
            var slotAffinities = LevelGen.dungeonData.dungeonData.lootSlotAffinities;
            if (GetComponent<Boss>() != null) slotAffinities = LevelGen.dungeonData.dungeonData.bossLootSlotAffinities;
            Equipment item = null;
            int roll = Random.Range(0, 9);
            var c = attacker.GetComponent<Character>();
            var strength = CharacterAttribute.attributes["strength"].instances[c].TotalValue;
            var dexterity = CharacterAttribute.attributes["dexterity"].instances[c].TotalValue;
            var intelligence = CharacterAttribute.attributes["intelligence"].instances[c].TotalValue;
            if (roll == 1 && slotAffinities.Contains("armor")) item = DropArmor(attacker, armor);
            else if (roll == 2 && quality > 0 && slotAffinities.Contains("necklace")) item = DropNecklace(attacker);
            else if (roll == 3 && quality > 0 && slotAffinities.Contains("belt")) item = DropBelt(attacker);
            else if (roll == 4 && quality > 0 && slotAffinities.Contains("cloak")) item = DropCloak(attacker);
            else if (roll == 5 && quality > 0 && slotAffinities.Contains("earring")) item = DropEarring(attacker);
            else if (roll == 6 && slotAffinities.Contains("hat")) item = DropHat(attacker, armor);
            else if (roll == 7 && slotAffinities.Contains("shoes")) item = DropShoes(attacker, armor);
            else if (roll == 8 && quality > 0 && slotAffinities.Contains("bracelet")) item = DropBracelet(attacker);
            else if (roll == 0 && intelligence >= dexterity && intelligence >= strength && slotAffinities.Contains("weapon")) item = DropWand(attacker);
            else if (roll == 0 && dexterity >= intelligence && dexterity >= strength && slotAffinities.Contains("weapon")) item = DropBow(attacker);
            else if (roll == 0 && strength >= dexterity && strength >= intelligence && slotAffinities.Contains("weapon")) item = DropSword(attacker);
            else return DropItemForLootAffinities(attacker, armor, quality);
            return item;
        }
        catch {
            return DropItemDefault(attacker, armor, quality);
        }
    }

    private Equipment DropItemDefault(Character attacker, int armor, int quality) {
        Equipment item = null;
        int roll = Random.Range(0, 12);
        var c = attacker.GetComponent<Character>();
        var strength = CharacterAttribute.attributes["strength"].instances[c].TotalValue;
        var dexterity = CharacterAttribute.attributes["dexterity"].instances[c].TotalValue;
        var intelligence = CharacterAttribute.attributes["intelligence"].instances[c].TotalValue;
        if (roll == 1) item = DropArmor(attacker, armor);
        else if (roll == 2 && quality > 0) item = DropNecklace(attacker);
        else if (roll == 3 && quality > 0) item = DropBelt(attacker);
        else if (roll == 4 && quality > 0) item = DropCloak(attacker);
        else if (roll == 5 && quality > 0) item = DropEarring(attacker);
        else if (roll == 6) item = DropHat(attacker, armor);
        else if (roll == 7) item = DropShoes(attacker, armor);
        else if (roll > 7 && quality > 0) item = DropBracelet(attacker);
        else if (roll == 0 && intelligence >= dexterity && intelligence >= strength) item = DropWand(attacker);
        else if (roll == 0 && dexterity >= intelligence && dexterity >= strength) item = DropBow(attacker);
        else if (roll == 0 && strength >= dexterity && strength >= intelligence) item = DropSword(attacker);
        else return null;
        return item;
    }

    private void EquipBraceletIfPossible(Equipment item, PlayerCharacter pc) {
        if (!(item is Bracelet bracelet)) return;
        for (int i=0; i<pc.bracelets.Length; i++) {
            if (pc.bracelets[i]==null) {
                pc.bracelets[i] = bracelet;
                pc.ModifyStats(null, item);
                pc.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
                return;
            }
        }
        PlayerCharacter.localPlayer.inventory.items.Add(item);
        DropsArea.AddItemDrop(item);
        StartCoroutine(pc.inventory.RefreshInABit());
    }

    public static Equipment GenerateEquipment(int level, int quality) {
        int[] stat1 = { 0, 2, 3, 5, 7, 9, 12, 15 };
        int[] stat2 = { 0, 0, 2, 3, 4, 5, 6, 7 };
        int[] stat3 = { 0, 0, 0, 0, 2, 2, 3, 4 };
        var statPreAdjusted1 = (float)stat1[quality];
        var statPreAdjusted2 = (float)stat2[quality];
        var statPreAdjusted3 = (float)stat3[quality];
        for (int i = 1; i < level; i++) {
            statPreAdjusted1 *= 1.1f;
            statPreAdjusted2 *= 1.1f;
            statPreAdjusted3 *= 1.1f;
        }
        var statAdjusted1 = (int)statPreAdjusted1;
        var statAdjusted2 = (int)statPreAdjusted2;
        var statAdjusted3 = (int)statPreAdjusted3;
        int armor = Random.Range(67 + (14 * level), 135 + (29 * level));
        Equipment item = null;
        int roll = Random.Range(0, 12);
        if (roll == 1) item = GenerateArmorForShop(level, armor);
        else if (roll == 2 && quality > 0) item = GenerateNecklace();
        else if (roll == 3 && quality > 0) item = GenerateBelt();
        else if (roll == 4 && quality > 0) item = GenerateCloak();
        else if (roll == 5 && quality > 0) item = GenerateEarring();
        else if (roll == 6) item = GenerateHat(armor);
        else if (roll == 7) item = GenerateShoes(armor);
        else if (roll > 7 && quality > 0) item = GenerateBracelet();
        else { // if (roll == 0) 
            int roll2 = Random.Range(0, 3);
            switch (roll2) {
                case 0:
                    item = GenerateWand(level);
                    break;
                case 1:
                default:
                    item = GenerateSword(level);
                    break;
                case 2:
                    item = GenerateBow(level);
                    break;
            }
        }
        if (statAdjusted1 > 0) BuffRandomStatForShop(item, statAdjusted1);
        if (statAdjusted2 > 0) BuffRandomStatForShop(item, statAdjusted2);
        if (statAdjusted3 > 0) BuffRandomStatForShop(item, statAdjusted3);
        if (quality >= 2) BuffRandomSecondaryStatForShop(item, statAdjusted1);
        if (quality >= 3) BuffRandomSecondaryStatForShop(item, statAdjusted2);

        item.description += GetDescriptionText(item);
        item.level = level;
        item.quality = quality;
        EquipmentNamer.NameEquipment(item);
        return item;
    }

    private static string GetDescriptionText(Equipment item) {
        var output = "";
        foreach (var kvp in item.stats) {
            var key = kvp.Key;
            var value = kvp.Value;
            output += "+" + value.ToString() + " " + CharacterAttribute.attributes[key].friendlyName + "\n";
        }
        return output;
    }

    private static void BuffRandomStatForShop(Equipment item, int amount) {
        var lookups = new List<string>() { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        int roll = Random.Range(0, lookups.Count);
        item.AddStat(lookups[roll], amount);
    }

    private static void BuffRandomSecondaryStatForShop(Equipment item, int amount) {
        var lookups = new List<string>() {
            "hpRating",
            "hpRegenRating",
            "receivedHealingRating",
            "armorMultiplierRating",
            "physicalResistRating",
            "mentalResistRating",
            "mpRating",
            "healingMultiplierRating",
            "moveSpeedRating",
            "cooldownReductionRating",
            "mpRegenRating",
            "criticalHitChanceRating",
            "criticalDamageRating",
            "statusEffectDurationRating",
            "itemFindRating",
            "fireResistRating",
            "iceResistRating",
            "acidResistRating",
            "lightResistRating",
            "darkResistRating",
            "piercingResistRating",
            "slashingResistRating",
            "bashingResistRating"
        };
        int roll = Random.Range(0, lookups.Count);
        item.AddStat(lookups[roll], amount);
    }

    private void BuffRandomStat(Equipment item, int amount) {
        List<string> lookups = new List<string>();
        var baseLookups = new List<string>() { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        if (!SceneInitializer.instance.inside || LevelGen.dungeonData == null) lookups = baseLookups;
        else if (GetComponent<Boss>() != null) lookups = LevelGen.dungeonData.dungeonData.lootPrimaryStatAffinities;
        else {
            for (int i = 0; i < 4; i++) foreach (var entry in LevelGen.dungeonData.dungeonData.lootPrimaryStatAffinities) lookups.Add(entry);
            foreach (var entry in baseLookups) lookups.Add(entry);
        }
        int roll = Random.Range(0, lookups.Count);
        item.AddStat(lookups[roll], amount);
    }

    private void BuffRandomSecondaryStat(Equipment item, int amount) {
        List<string> lookups = new List<string>();
        var baseLookups = new List<string>() {
            "hpRating",
            "hpRegenRating",
            "receivedHealingRating",
            "armorMultiplierRating",
            "physicalResistRating",
            "mentalResistRating",
            "mpRating",
            "healingMultiplierRating",
            "moveSpeedRating",
            "cooldownReductionRating",
            "mpRegenRating",
            "criticalHitChanceRating",
            "criticalDamageRating",
            "statusEffectDurationRating",
            "itemFindRating",
            "fireResistRating",
            "iceResistRating",
            "acidResistRating",
            "lightResistRating",
            "darkResistRating",
            "piercingResistRating",
            "slashingResistRating",
            "bashingResistRating"
        };
        if (!SceneInitializer.instance.inside || LevelGen.dungeonData == null) lookups = baseLookups;
        else if (GetComponent<Boss>() != null) lookups = LevelGen.dungeonData.dungeonData.lootSecondaryStatAffinities;
        else {
            for (int i = 0; i < 4; i++) foreach (var entry in LevelGen.dungeonData.dungeonData.lootSecondaryStatAffinities) lookups.Add(entry);
            foreach (var entry in baseLookups) lookups.Add(entry);
        }
        int roll = Random.Range(0, lookups.Count);
        item.AddStat(lookups[roll], amount);
    }

    public Weapon DropWand(Character attacker) {
        float baseDamage = 0.8437f;
        if (intendedLevel > 1) for (int i = 1; i < intendedLevel; i++) baseDamage *= 1.1f;
        float damageRoll = Random.Range(0.8f, 1.2f);
        var weapon = GetRandomWand(baseDamage, damageRoll);
        weapon.description += "{{AttackPower}}\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("WEAPON FOUND", "Weapon found.");
        return weapon;
    }

    public static Weapon GenerateWand(int level) {
        float baseDamage = 0.8437f;
        if (level > 1) for (int i = 1; i < level; i++) baseDamage *= 1.1f;
        float damageRoll = Random.Range(0.8f, 1.2f);
        var weapon = GetRandomWand(baseDamage, damageRoll);
        weapon.description += "{{AttackPower}}\n";
        return weapon;
    }

    public Weapon DropBow(Character attacker) {
        float baseDamage = 0.8437f;
        if (intendedLevel > 1) for (int i = 1; i < intendedLevel; i++) baseDamage *= 1.1f;
        float damageRoll = Random.Range(0.8f, 1.2f);
        var weapon = GetRandomBow(baseDamage, damageRoll);
        weapon.description += "{{AttackPower}}\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("WEAPON FOUND", "Weapon found.");
        return weapon;
    }

    public static Weapon GenerateBow(int level) {
        float baseDamage = 0.8437f;
        if (level > 1) for (int i = 1; i < level; i++) baseDamage *= 1.1f;
        float damageRoll = Random.Range(0.8f, 1.2f);
        var weapon = GetRandomBow(baseDamage, damageRoll);
        weapon.description += "{{AttackPower}}\n";
        return weapon;
    }

    public Weapon DropSword(Character attacker) {
        float baseDamage = 0.8437f;
        if (intendedLevel > 1) for (int i = 1; i < intendedLevel; i++) baseDamage *= 1.1f;
        float damageRoll = Random.Range(0.8f, 1.2f);
        var weapon = GetRandomSword(baseDamage, damageRoll);
        weapon.description += "{{AttackPower}}\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("WEAPON FOUND", "Weapon found.");
        return weapon;
    }

    public static Weapon GenerateSword(int level) {
        float baseDamage = 0.8437f;
        if (level > 1) for (int i = 1; i < level; i++) baseDamage *= 1.1f;
        float damageRoll = Random.Range(0.8f, 1.2f);
        var weapon = GetRandomSword(baseDamage, damageRoll);
        weapon.description += "{{AttackPower}}\n";
        return weapon;
    }

    public Weapon DropWeapon(Character attacker) {
        float baseDamage = 0.8437f;
        if (intendedLevel > 1) for (int i = 1; i < intendedLevel; i++) baseDamage *= 1.1f;
        Weapon weapon = GetRandomWeapon(baseDamage);
        weapon.description += "{{AttackPower}}\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("WEAPON FOUND", "Weapon found.");
        return weapon;
    }

    private static Weapon GetRandomWeapon(float baseDamage) {
        int roll = Random.Range(0, 3);
        float damageRoll = Random.Range(0.8f, 1.2f);
        if (roll == 0) return GetRandomSword(baseDamage, damageRoll);
        else if (roll == 1) return GetRandomBow(baseDamage, damageRoll);
        else return GetRandomWand(baseDamage, damageRoll);
    }

    private static Weapon GetRandomSword(float baseDamage, float damageRoll) {
        Weapon weapon = new MeleeWeapon {
            attackPower = baseDamage * damageRoll,
            name = "Random Sword",
            description = "Melee weapon:\n"
        };
        return weapon;
    }

    private static Weapon GetRandomBow(float baseDamage, float damageRoll) {
        Weapon weapon = new RangedWeapon {
            attackPower = baseDamage * damageRoll,
            name = "Random Bow",
            description = "Ranged weapon:\n"
        };
        return weapon;
    }

    private static Weapon GetRandomWand(float baseDamage, float damageRoll) {
        Weapon weapon = RangedWeapon.Wand();
        weapon.attackPower = baseDamage * damageRoll;
        weapon.name = "Random Wand";
        weapon.description = "Ranged weapon:\n";
        return weapon;
    }

    public Armor DropArmor(Character attacker, int armor) {
        var armorObj = GenerateArmor(attacker);
        armorObj.armor = armor;
        armorObj.description += "Armor: " + armor.ToString() + "\n";
        return armorObj;
    }

    public static Armor GenerateArmorForShop(int level, int armor) {
        var armorObj = new Armor() { name = "Random Armor", description = "" };
        armorObj.armor = armor;
        armorObj.description += "Armor: " + armor.ToString() + "\n";
        return armorObj;
    }

    private Armor GenerateArmor(Character attacker) {
        var armor = new Armor {
            name = "Random Armor",
            description = ""
        };
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("ARMOR FOUND", "Armor found.");
        return armor;
    }

    public Necklace DropNecklace(Character attacker) {
        var necklace = new Necklace {
            name = "Random Necklace",
            description = ""
        };
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("NECKLACE FOUND", "Necklace found.");
        return necklace;
    }

    public static Necklace GenerateNecklace() {
        var necklace = new Necklace {
            name = "Random Necklace",
            description = ""
        };
        return necklace;
    }

    public Belt DropBelt(Character attacker) {
        var belt = new Belt {
            name = "Random Belt",
            description = ""
        };
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("BELT FOUND", "Belt found.");
        return belt;
    }

    public static Belt GenerateBelt() {
        var belt = new Belt {
            name = "Random Belt",
            description = ""
        };
        return belt;
    }

    public Bracelet DropBracelet(Character attacker) {
        var bracelet = new Bracelet {
            name = "Random Bracelet",
            description = ""
        };
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("BRACELET FOUND", "Bracelet found.");
        return bracelet;
    }

    public static Bracelet GenerateBracelet() {
        var bracelet = new Bracelet {
            name = "Random Bracelet",
            description = ""
        };
        return bracelet;
    }

    public Cloak DropCloak(Character attacker) {
        var cloak = new Cloak {
            name = "Random Cloak",
            description = ""
        };
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("CLOAK FOUND", "Cloak found.");
        return cloak;
    }

    public static Cloak GenerateCloak() {
        var cloak = new Cloak {
            name = "Random Cloak",
            description = ""
        };
        return cloak;
    }

    public Earring DropEarring(Character attacker) {
        var earring = new Earring {
            name = "Random Earring",
            description = ""
        };
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("EARRING FOUND", "Earring found.");
        return earring;
    }

    public static Earring GenerateEarring() {
        var earring = new Earring {
            name = "Random Earring",
            description = ""
        };
        return earring;
    }

    public Hat DropHat(Character attacker, int armor) {
        var hat = new Hat {
            name = "Random Hat",
            description = "",
            armor = armor
        };
        hat.description += "Armor: " + armor.ToString() + "\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("HAT FOUND", "Hat found.");
        return hat;
    }

    public static Hat GenerateHat(int armor) {
        var hat = new Hat {
            name = "Random Hat",
            description = "",
            armor = armor
        };
        hat.description += "Armor: " + armor.ToString() + "\n";
        return hat;
    }

    public Shoes DropShoes(Character attacker, int armor) {
        var shoes = new Shoes {
            name = "Random Shoes",
            description = "",
            armor = armor
        };
        shoes.description += "Armor: " + armor.ToString() + "\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("SHOES FOUND", "Shoes found.");
        return shoes;
    }

    public static Shoes GenerateShoes(int armor) {
        var shoes = new Shoes {
            name = "Random Shoes",
            description = "",
            armor = armor
        };
        shoes.description += "Armor: " + armor.ToString() + "\n";
        return shoes;
    }

    private static float ScaleArmorValue(float value, int intendedLevel) {
        if (intendedLevel > 1) for (int i = 1; i < intendedLevel; i++) value *= 1.1f;
        return value;
    }

    public void DropConsumable(Character attacker) {
        AddConsumable(attacker);
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("POTION FOUND", "Potion found.");
        attacker.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    public static Item GenerateConsumable() {
        int roll = Random.Range(0, 2);
        if (roll == 0) return new Consumable(ConsumableType.health, 100, 1);
        else return new Consumable(ConsumableType.mana, 100, 1);
    }

    private void AddConsumable(Character attacker) {
        int roll = Random.Range(0, 2);
        if (roll == 0) AddHealthPotion(attacker);
        else AddManaPotion(attacker);
    }

    private void AddHealthPotion(Character attacker) {
        foreach (var item in attacker.GetComponent<PlayerCharacter>().consumables) {
            if (item == null) continue;
            if (item.type == ConsumableType.health) {
                item.quantity += 1;
                return;
            }
        }
        attacker.GetComponent<PlayerCharacter>().consumables.Add(new Consumable(ConsumableType.health, 100, 1));
    }

    private void AddManaPotion(Character attacker) {
        foreach (var item in attacker.GetComponent<PlayerCharacter>().consumables) {
            if (item == null) continue;
            if (item.type == ConsumableType.mana) {
                item.quantity += 1;
                return;
            }
        }
        attacker.GetComponent<PlayerCharacter>().consumables.Add(new Consumable(ConsumableType.mana, 100, 1));
    }
}
