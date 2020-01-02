using UnityEngine;
using System.Collections;

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
        var highest = Mathf.Max(item.strength, item.dexterity, item.constitution, item.intelligence, item.wisdom, item.luck);
        if (highest == 0) item.name = "Common " + item.name;
        else {
            if (highest == item.strength) item.name = item.name + " of Power";
            if (highest == item.dexterity) item.name = item.name + " of Grace";
            if (highest == item.constitution) item.name = item.name + " of Endurance";
            if (highest == item.intelligence) item.name = item.name + " of Acumen";
            if (highest == item.wisdom) item.name = item.name + " of Divinity";
            if (highest == item.luck) item.name = item.name + " of Fortune";
        }
    }

    public static string GetHpAndMp(Armor item) {
        var character = GetCharacter();
        return "<b>HP</b>: " + Mathf.Floor(character.constitution).ToString() + " <b>MP</b>: " + Mathf.Floor(character.intelligence).ToString();
    }

    public static string GetAttackPower(Weapon item) {
        var character = GetCharacter();
        if (item is MeleeWeapon) return "<b>Attack Power</b>: " + Mathf.Floor(item.attackPower * character.strength).ToString();
        else if (item is RangedWeapon && ((RangedWeapon)item).usesInt) return "<b>Attack Power</b>: " + Mathf.Floor(item.attackPower * character.intelligence).ToString();
        else if (item is RangedWeapon) return "<b>Attack Power</b>: " + Mathf.Floor(item.attackPower * character.dexterity).ToString();
        return "";
    }

    public static string GetAttackPowerFromStat(float attackPower, string subtype) {
        if (subtype == "") return "Unknown";
        var character = GetCharacter();
        if (subtype == "strength") return "<b>Attack Power</b>: " + Mathf.Floor(attackPower * character.strength).ToString();
        else if (subtype == "intelligence") return "<b>Attack Power</b>: " + Mathf.Floor(attackPower * character.intelligence).ToString();
        else if (subtype == "dexterity") return "<b>Attack Power</b>: " + Mathf.Floor(attackPower * character.dexterity).ToString();
        return "";
    }

    public static int GetAttackPowerNumberFromStat(float attackPower, string subtype) {
        if (subtype == "") return 0;
        var character = GetCharacter();
        if (subtype == "strength") return Mathf.FloorToInt(attackPower * character.strength);
        else if (subtype == "intelligence") return Mathf.FloorToInt(attackPower * character.intelligence);
        else if (subtype == "dexterity") return Mathf.FloorToInt(attackPower * character.dexterity);
        return 0;
    }

    public static int GetAttackPowerNumber(Weapon item) {
        var character = GetCharacter();
        if (character == null) return 0;
        if (item is MeleeWeapon) return (int)Mathf.Floor(item.attackPower * character.strength);
        else if (item is RangedWeapon && ((RangedWeapon)item).usesInt) return (int)Mathf.Floor(item.attackPower * character.intelligence);
        else if (item is RangedWeapon) return (int)Mathf.Floor(item.attackPower * character.dexterity);
        return 0;
    }

    private static Character GetCharacter() {
        if (PlayerCharacter.localPlayer == null) return null;
        return PlayerCharacter.localPlayer.GetComponent<Character>();
    }
}
