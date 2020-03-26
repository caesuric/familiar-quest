using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentNamer {
    public static void NameEquipment(Equipment item) {
        if (item is MeleeWeapon) item.name = "Sword";
        else if (item is RangedWeapon && ((RangedWeapon)item).usesInt) item.name = "Wand";
        else if (item is RangedWeapon) item.name = "Bow";
        else if (item is Armor) item.name = "Armor";
        else if (item is Necklace) item.name = "Necklace";
        else if (item is Belt) item.name = "Belt";
        else if (item is Bracelet) item.name = "Bracelet";
        else if (item is Cloak) item.name = "Cloak";
        else if (item is Earring) item.name = "Earring";
        else if (item is Hat) item.name = "Hat";
        else if (item is Shoes) item.name = "Shoes";
        AddHighestStatText(item);
    }

    private static void AddHighestStatText(Equipment item) {
        var stat = item.GetHighestStat();
        if (stat == "") item.name = "Common " + item.name;
        else {
            var lookups = new Dictionary<string, string>() {
                { "strength", "Power" },
                { "dexterity", "Grace" },
                { "constitution", "Endurance" },
                { "intelligence", "Acumen" },
                { "wisdom", "Divinity" },
                { "luck", "Fortune" }
            };
            if (lookups.ContainsKey(stat)) item.name = item.name + " of " + lookups[stat];
            else item.name = item.name + CharacterAttribute.attributes[stat].friendlyName;
        }
    }

    public static string GetHpAndMp(Armor item) {
        var character = GetCharacter();
        return "<b>HP</b>: " + Mathf.Floor(CharacterAttribute.attributes["bonusHp"].instances[character].TotalValue).ToString() + " <b>MP</b>: " + Mathf.Floor(CharacterAttribute.attributes["bonusHp"].instances[character].TotalValue).ToString();
    }

    public static string GetAttackPower(Weapon item) {
        var character = GetCharacter();
        if (item is MeleeWeapon) return "<b>Attack Power</b>: " + Mathf.Floor(item.attackPower * CharacterAttribute.attributes["strength"].instances[character].TotalValue).ToString();
        else if (item is RangedWeapon && ((RangedWeapon)item).usesInt) return "<b>Attack Power</b>: " + Mathf.Floor(item.attackPower * CharacterAttribute.attributes["intelligence"].instances[character].TotalValue).ToString();
        else if (item is RangedWeapon) return "<b>Attack Power</b>: " + Mathf.Floor(item.attackPower * CharacterAttribute.attributes["dexterity"].instances[character].TotalValue).ToString();
        return "";
    }

    public static string GetAttackPowerFromStat(float attackPower, string subtype) {
        if (subtype == "") return "Unknown";
        var character = GetCharacter();
        if (subtype == "strength") return "<b>Attack Power</b>: " + Mathf.Floor(attackPower * CharacterAttribute.attributes["strength"].instances[character].TotalValue).ToString();
        else if (subtype == "intelligence") return "<b>Attack Power</b>: " + Mathf.Floor(attackPower * CharacterAttribute.attributes["intelligence"].instances[character].TotalValue).ToString();
        else if (subtype == "dexterity") return "<b>Attack Power</b>: " + Mathf.Floor(attackPower * CharacterAttribute.attributes["dexterity"].instances[character].TotalValue).ToString();
        return "";
    }

    public static int GetAttackPowerNumberFromItem(Item item) {
        var weapon = item as Weapon;
        if (weapon == null) return 0;
        var character = GetCharacter();
        if (item is MeleeWeapon) return Mathf.FloorToInt(weapon.attackPower * CharacterAttribute.attributes["strength"].instances[character].TotalValue);
        else if (item is RangedWeapon && ((RangedWeapon)weapon).usesInt) return Mathf.FloorToInt(weapon.attackPower * CharacterAttribute.attributes["intelligence"].instances[character].TotalValue);
        else return Mathf.FloorToInt(weapon.attackPower * CharacterAttribute.attributes["dexterity"].instances[character].TotalValue);
        //if (subtype == "") return 0;
        //var character = GetCharacter();
        //if (subtype == "strength") return Mathf.FloorToInt(attackPower * CharacterAttribute.attributes["strength"].instances[character].TotalValue);
        //else if (subtype == "intelligence") return Mathf.FloorToInt(attackPower * CharacterAttribute.attributes["intelligence"].instances[character].TotalValue);
        //else if (subtype == "dexterity") return Mathf.FloorToInt(attackPower * CharacterAttribute.attributes["dexterity"].instances[character].TotalValue);
        //return 0;
    }

    public static int GetAttackPowerNumber(Weapon item) {
        var character = GetCharacter();
        if (character == null) return 0;
        if (item is MeleeWeapon) return (int)Mathf.Floor(item.attackPower * CharacterAttribute.attributes["strength"].instances[character].TotalValue);
        else if (item is RangedWeapon && ((RangedWeapon)item).usesInt) return (int)Mathf.Floor(item.attackPower * CharacterAttribute.attributes["intelligence"].instances[character].TotalValue);
        else if (item is RangedWeapon) return (int)Mathf.Floor(item.attackPower * CharacterAttribute.attributes["dexterity"].instances[character].TotalValue);
        return 0;
    }

    private static Character GetCharacter() {
        if (PlayerCharacter.localPlayer == null) return null;
        return PlayerCharacter.localPlayer.GetComponent<Character>();
    }
}
