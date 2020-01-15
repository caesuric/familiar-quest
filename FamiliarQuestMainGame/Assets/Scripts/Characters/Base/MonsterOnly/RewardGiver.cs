using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

class RewardGiver : MonoBehaviour {
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
            attacker.GetComponent<ConfigGrabber>().sharedInventory.inventory.Add(GetComponent<Gremlin>().item);
            attacker.GetComponent<ConfigGrabber>().sharedInventory.CmdRefresh();
            return;
        }
        var lootChance = SecondaryStatUtility.CalcItemFindRate(attacker.luck, attacker.GetComponent<ExperienceGainer>().level);
        if (GetComponent<Boss>()!=null) {
            guaranteed = true;
            lootChance = lootChance * 0.017752f / 0.2f;
        }
        if (GetComponent<EnergyWisplet>() != null) guaranteed = true;
        float roll = Random.Range(0f, 1f);
        if (guaranteed) roll = Random.Range(0f, lootChance);
        if (roll > lootChance) return;
        else if (roll < lootChance * 0.001667f / 0.2f) DropEquipment(attacker, 7);
        else if (roll < lootChance * 0.004519f / 0.2f) DropEquipment(attacker, 6);
        else if (roll < lootChance * 0.0094f / 0.2f) DropEquipment(attacker, 5);
        else if (roll < lootChance * 0.017752f / 0.2f) DropEquipment(attacker, 4);
        else if (roll < lootChance * 0.032046f / 0.2f) DropEquipment(attacker, 3);
        else if (roll < lootChance * 0.056508f / 0.2f) DropEquipment(attacker, 2);
        else if (roll < lootChance * 0.098368f / 0.2f) DropEquipment(attacker, 1);
        else if (roll < lootChance * 0.170004f / 0.2f) DropEquipment(attacker, 0);
        else if (roll < lootChance) DropConsumable(attacker);
    }

    public static Item GenerateItem(int level) {
        var lootChance = 1f;
        float roll = Random.Range(0f, lootChance);
        if (roll < lootChance * 0.001667f / 0.2f) return GenerateEquipment(level, 7);
        else if (roll < lootChance * 0.004519f / 0.2f) return GenerateEquipment(level, 6);
        else if (roll < lootChance * 0.0094f / 0.2f) return GenerateEquipment(level, 5);
        else if (roll < lootChance * 0.017752f / 0.2f) return GenerateEquipment(level, 4);
        else if (roll < lootChance * 0.032046f / 0.2f) return GenerateEquipment(level, 3);
        else if (roll < lootChance * 0.056508f / 0.2f) return GenerateEquipment(level, 2);
        else if (roll < lootChance * 0.098368f / 0.2f) return GenerateEquipment(level, 1);
        else if (roll < lootChance * 0.170004f / 0.2f) return GenerateEquipment(level, 0);
        else return GenerateConsumable();
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
        int roll = Random.Range(0, 12);
        if (roll == 1) item = DropArmor(attacker, armor);
        else if (roll == 2 && quality > 0) item = DropNecklace(attacker);
        else if (roll == 3 && quality > 0) item = DropBelt(attacker);
        else if (roll == 4 && quality > 0) item = DropCloak(attacker);
        else if (roll == 5 && quality > 0) item = DropEarring(attacker);
        else if (roll == 6) item = DropHat(attacker, armor);
        else if (roll == 7) item = DropShoes(attacker, armor);
        else if (roll > 7 && quality > 0) item = DropBracelet(attacker);
        else if (roll == 0 && attacker.intelligence >= attacker.dexterity && attacker.intelligence >= attacker.strength) item = DropWand(attacker);
        else if (roll == 0 && attacker.dexterity >= attacker.intelligence && attacker.dexterity >= attacker.strength) item = DropBow(attacker);
        else if (roll == 0 && attacker.strength >= attacker.dexterity && attacker.strength >= attacker.intelligence) item = DropSword(attacker);
        else return;
        if (statAdjusted1>0) {
            if (attacker.intelligence >= attacker.dexterity && attacker.intelligence >= attacker.strength) item.intelligence += statAdjusted1;
            else if (attacker.dexterity >= attacker.intelligence && attacker.dexterity >= attacker.strength) item.dexterity += statAdjusted1;
            else item.strength += statAdjusted1;
            item.constitution += (statAdjusted1 / 2);
        }
        if (statAdjusted2 > 0) BuffRandomStat(item, statAdjusted2);
        if (statAdjusted3 > 0) BuffRandomStat(item, statAdjusted3);

        if (item.strength > 0) item.description += GetDescriptionText(item.strength, "Strength");
        if (item.dexterity > 0) item.description += GetDescriptionText(item.dexterity, "Dexterity");
        if (item.constitution > 0) item.description += GetDescriptionText(item.constitution, "Constitution");
        if (item.intelligence > 0) item.description += GetDescriptionText(item.intelligence, "Intelligence");
        if (item.wisdom > 0) item.description += GetDescriptionText(item.wisdom, "Wisdom");
        if (item.luck > 0) item.description += GetDescriptionText(item.luck, "Luck");

        EquipmentNamer.NameEquipment(item);
        item.quality = quality;
        var pc = attacker.GetComponent<PlayerCharacter>();
        if (item is Armor && pc.armor == null) pc.armor = (Armor)item;
        else if (item is Hat && pc.hat == null) pc.hat = (Hat)item;
        else if (item is Shoes && pc.shoes == null) pc.shoes = (Shoes)item;
        else if (item is Belt && pc.belt == null) pc.belt = (Belt)item;
        else if (item is Cloak && pc.cloak == null) pc.cloak = (Cloak)item;
        else if (item is Earring && pc.earring == null) pc.earring = (Earring)item;
        else if (item is Necklace && pc.necklace == null) pc.necklace = (Necklace)item;
        else {
            SharedInventory.instance.inventory.Add(item);
            DropsArea.AddItemDrop(item);
        }
        SharedInventory.instance.CmdRefresh();
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
        else if (roll == 0) {
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
        int roll3 = Random.Range(0, 3);
        switch (roll3) {
            case 0:
            default:
                item.intelligence += statAdjusted1;
                break;
            case 1:
                item.dexterity += statAdjusted1;
                break;
            case 2:
                item.strength += statAdjusted1;
                break;
        }

        if (statAdjusted2 > 0) BuffRandomStat(item, statAdjusted2);
        if (statAdjusted3 > 0) BuffRandomStat(item, statAdjusted3);

        if (item.strength > 0) item.description += GetDescriptionText(item.strength, "Strength");
        if (item.dexterity > 0) item.description += GetDescriptionText(item.dexterity, "Dexterity");
        if (item.constitution > 0) item.description += GetDescriptionText(item.constitution, "Constitution");
        if (item.intelligence > 0) item.description += GetDescriptionText(item.intelligence, "Intelligence");
        if (item.wisdom > 0) item.description += GetDescriptionText(item.wisdom, "Wisdom");
        if (item.luck > 0) item.description += GetDescriptionText(item.luck, "Luck");

        EquipmentNamer.NameEquipment(item);
        item.quality = quality;
        return item;
    }

    private static string GetDescriptionText(int value, string attribute) {
        return "+" + value.ToString() + " " + attribute + "\n";
    }

    private static void BuffRandomStat(Equipment item, int amount) {
        int roll = Random.Range(0, 6);
        switch (roll) {
            case 0:
                item.strength += amount;
                break;
            case 1:
                item.dexterity += amount;
                break;
            case 2:
                item.constitution += amount;
                break;
            case 3:
                item.intelligence += amount;
                break;
            case 4:
                item.wisdom += amount;
                break;
            case 5:
            default:
                item.luck += amount;
                break;
        }
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
        Weapon weapon = new MeleeWeapon();
        weapon.attackPower = baseDamage * damageRoll;
        weapon.name = "Random Sword";
        weapon.description = "Melee weapon:\n";
        return weapon;
    }

    private static Weapon GetRandomBow(float baseDamage, float damageRoll) {
        Weapon weapon = new RangedWeapon();
        weapon.attackPower = baseDamage * damageRoll;
        weapon.name = "Random Bow";
        weapon.description = "Ranged weapon:\n";
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
        float hp = Random.Range(1, 39);
        float mp = 39 - hp;
        float hpRoll = Random.Range(0.8f, 1.2f);
        float mpRoll = Random.Range(0.8f, 1.2f);
        hp = ScaleArmorValue(hp, intendedLevel) * hpRoll;
        mp = ScaleArmorValue(mp, intendedLevel) * mpRoll;
        var armorObj = GenerateArmor(attacker, hp, mp);
        armorObj.armor = armor;
        armorObj.description += "Armor: " + armor.ToString() + "\n";
        return armorObj;
    }

    public static Armor GenerateArmorForShop(int level, int armor) {
        float hp = Random.Range(1, 39);
        float mp = 39 - hp;
        float hpRoll = Random.Range(0.8f, 1.2f);
        float mpRoll = Random.Range(0.8f, 1.2f);
        hp = ScaleArmorValue(hp, level) * hpRoll;
        mp = ScaleArmorValue(mp, level) * mpRoll;
        var armorObj = new Armor() { hp = hp, mp = mp, name = "Random Armor", description = "" };
        armorObj.armor = armor;
        armorObj.description += "Armor: " + armor.ToString() + "\n";
        return armorObj;
    }

    private Armor GenerateArmor(Character attacker, float hp, float mp) {
        var armor = new Armor();
        armor.hp = hp;
        armor.mp = mp;
        armor.name = "Random Armor";
        //armor.description = "{{HpAndMp}}";
        armor.description = "";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("ARMOR FOUND", "Armor found.");
        return armor;
    }

    public Necklace DropNecklace(Character attacker) {
        var necklace = new Necklace();
        necklace.name = "Random Necklace";
        necklace.description = "";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("NECKLACE FOUND", "Necklace found.");
        return necklace;
    }

    public static Necklace GenerateNecklace() {
        var necklace = new Necklace();
        necklace.name = "Random Necklace";
        necklace.description = "";
        return necklace;
    }

    public Belt DropBelt(Character attacker) {
        var belt = new Belt();
        belt.name = "Random Belt";
        belt.description = "";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("BELT FOUND", "Belt found.");
        return belt;
    }

    public static Belt GenerateBelt() {
        var belt = new Belt();
        belt.name = "Random Belt";
        belt.description = "";
        return belt;
    }

    public Bracelet DropBracelet(Character attacker) {
        var bracelet = new Bracelet();
        bracelet.name = "Random Bracelet";
        bracelet.description = "";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("BRACELET FOUND", "Bracelet found.");
        return bracelet;
    }

    public static Bracelet GenerateBracelet() {
        var bracelet = new Bracelet();
        bracelet.name = "Random Bracelet";
        bracelet.description = "";
        return bracelet;
    }

    public Cloak DropCloak(Character attacker) {
        var cloak = new Cloak();
        cloak.name = "Random Cloak";
        cloak.description = "";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("CLOAK FOUND", "Cloak found.");
        return cloak;
    }

    public static Cloak GenerateCloak() {
        var cloak = new Cloak();
        cloak.name = "Random Cloak";
        cloak.description = "";
        return cloak;
    }

    public Earring DropEarring(Character attacker) {
        var earring = new Earring();
        earring.name = "Random Earring";
        earring.description = "";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("EARRING FOUND", "Earring found.");
        return earring;
    }

    public static Earring GenerateEarring() {
        var earring = new Earring();
        earring.name = "Random Earring";
        earring.description = "";
        return earring;
    }

    public Hat DropHat(Character attacker, int armor) {
        var hat = new Hat();
        hat.name = "Random Hat";
        hat.description = "";
        hat.armor = armor;
        hat.description += "Armor: " + armor.ToString() + "\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("HAT FOUND", "Hat found.");
        return hat;
    }

    public static Hat GenerateHat(int armor) {
        var hat = new Hat();
        hat.name = "Random Hat";
        hat.description = "";
        hat.armor = armor;
        hat.description += "Armor: " + armor.ToString() + "\n";
        return hat;
    }

    public Shoes DropShoes(Character attacker, int armor) {
        var shoes = new Shoes();
        shoes.name = "Random Shoes";
        shoes.description = "";
        shoes.armor = armor;
        shoes.description += "Armor: " + armor.ToString() + "\n";
        attacker.GetComponent<ObjectSpawner>().CreateFloatingStatusText("SHOES FOUND", "Shoes found.");
        return shoes;
    }

    public static Shoes GenerateShoes(int armor) {
        var shoes = new Shoes();
        shoes.name = "Random Shoes";
        shoes.description = "";
        shoes.armor = armor;
        shoes.description += "Armor: " + armor.ToString() + "\n";
        return shoes;
    }

    private static float ScaleArmorValue(float value, int intendedLevel) {
        if (intendedLevel > 1) for (int i = 1; i < intendedLevel; i++) value *= 1.1f;
        return value;
    }

    public void DropConsumable(Character attacker) {
        AddConsumable(attacker);
        SharedInventory.instance.CmdRefresh();
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
