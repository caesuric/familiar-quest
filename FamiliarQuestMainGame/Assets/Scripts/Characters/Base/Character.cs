using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[NetworkSettings(sendInterval = 0.016f)]
public class Character : DependencyUser {

    //public int strength;
    //public int dexterity;
    //public int constitution;
    //public int intelligence;
    //public int wisdom;
    //public int luck;
    public string faction;
    public string oppositeFaction;

    static Character() {
        new CharacterAttribute("strength", "Strength", false);
        new CharacterAttribute("dexterity", "Dexterity", false);
        new CharacterAttribute("constitution", "Constitution", false);
        new CharacterAttribute("intelligence", "Intelligence", false);
        new CharacterAttribute("wisdom", "Wisdom", false);
        new CharacterAttribute("luck", "Luck", false);
        new CharacterAttribute("hpRating", "HP Rating", false);
        new CharacterAttribute("hpRegenRating", "HP Regen Rating", false);
        new CharacterAttribute("receivedHealingRating", "Received Healing Rating", false);
        new CharacterAttribute("armorMultiplierRating", "Armor Multiplier Rating", false);
        new CharacterAttribute("physicalResistRating", "Physical Resist Rating", false);
        new CharacterAttribute("mentalResistRating", "Mental Resist Rating", false);
        new CharacterAttribute("mpRating", "MP Rating", false);
        new CharacterAttribute("healingMultiplierRating", "Healing Multiplier Rating", false);
        new CharacterAttribute("moveSpeedRating", "Move Speed Rating", false);
        new CharacterAttribute("cooldownReductionRating", "Cooldown Reduction Rating", false);
        new CharacterAttribute("mpRegenRating", "MP Regen Rating", false);
        new CharacterAttribute("criticalHitChanceRating", "Critical Hit Chance Rating", false);
        new CharacterAttribute("criticalDamageRating", "Critical Damage Rating", false);
        new CharacterAttribute("statusEffectDurationRating", "Status Effect Duration Rating", false);
        new CharacterAttribute("itemFindRating", "Item Find Rating", false);
        new CharacterAttribute("fireResistRating", "Fire Resist Rating", false);
        new CharacterAttribute("iceResistRating", "Ice Resist Rating", false);
        new CharacterAttribute("acidResistRating", "Acid Resist Rating", false);
        new CharacterAttribute("lightResistRating", "Light Resist Rating", false);
        new CharacterAttribute("darkResistRating", "Dark Resist Rating", false);
        new CharacterAttribute("piercingResistRating", "Piercing Resist Rating", false);
        new CharacterAttribute("slashingResistRating", "Slashing Resist Rating", false);
        new CharacterAttribute("bashingResistRating", "Bashing Resist Rating", false);

        new CharacterAttribute("bonusHp", "Bonus HP", true, new List<string> { "constitution", "hpRating" }, 0, 145, 290, 0, 7500, 15000);
        new CharacterAttribute("hpRegen", "HP Regen", true, new List<string> { "constitution", "hpRegenRating" }, 0, 0, 2, 0, 0, 100);
        new CharacterAttribute("receivedHealing", "Received Healing", true, new List<string> { "strength", "receivedHealingRating" }, 25, 100, 200, 50, 100, 400, true);
        new CharacterAttribute("armorMultiplier", "Armor Multiplier", true, new List<string> { "constitution", "armorMultiplierRating" }, 25, 100, 200, 50, 100, 400, true);
        new CharacterAttribute("physicalResistance", "Physical Resistance", true, new List<string> { "constitution", "physicalResistRating" }, 0, 30, 60, 0, 50, 100, true);
        new CharacterAttribute("mentalResistance", "Mental Resistance", true, new List<string> { "wisdom", "mentalResistRating" }, 0, 30, 60, 0, 50, 100, true);
        new CharacterAttribute("bonusMp", "Bonus MP", true, new List<string> { "intelligence", "mpRating" }, 0, 145, 290, 0, 7500, 15000);
        new CharacterAttribute("healingMultiplier", "Healing Multiplier", true, new List<string> { "wisdom", "healingMultiplierRating" }, 25, 100, 200, 50, 100, 400, true);
        new CharacterAttribute("mpRegen", "MP Regen Rate", true, new List<string> { "wisdom", "mpRegenRating" }, 0, 5, 10, 10, 250, 500);
        new CharacterAttribute("moveSpeed", "Move Speed", true, new List<string> { "dexterity", "moveSpeedRating" }, 75, 100, 150, 75, 100, 200, true);
        new CharacterAttribute("cooldownReduction", "Cooldown Reduction", true, new List<string> { "wisdom", "cooldownReductionRating" }, 0, 0, 25, 0, 0, 50, true);
        new CharacterAttribute("criticalHitChance", "Critical Hit Chance", true, new List<string> { "luck", "criticalHitChanceRating" }, 0, 20, 30, 10, 20, 50, true);
        new CharacterAttribute("criticalDamage", "Critical Hit Damage", true, new List<string> { "luck", "criticalDamageRating" }, 100, 200, 250, 150, 200, 300, true);
        new CharacterAttribute("statusEffectDuration", "Status Effect Duration", true, new List<string> { "luck", "statusEffectDurationRating" }, -80, 0, 40, -40, 0, 80, true);
        new CharacterAttribute("itemFindRate", "Item Find Rate", true, new List<string> { "luck", "itemFindRating" }, 0, 20, 30, 10, 20, 50, true);
        new CharacterAttribute("fireResistance", "Fire Resistance", true, new List<string> { "luck", "fireResistRating" }, 0, 0, 20, 0, 0, 75, true);
        new CharacterAttribute("iceResistance", "Ice Resistance", true, new List<string> { "luck", "iceResistRating" }, 0, 0, 20, 0, 0, 75, true);
        new CharacterAttribute("acidResistance", "Acid Resistance", true, new List<string> { "luck", "acidResistRating" }, 0, 0, 20, 0, 0, 75, true);
        new CharacterAttribute("lightResistance", "Light Resistance", true, new List<string> { "luck", "lightResistRating" }, 0, 0, 20, 0, 0, 75, true);
        new CharacterAttribute("darkResistance", "Dark Resistance", true, new List<string> { "luck", "darkResistRating" }, 0, 0, 20, 0, 0, 75, true);
        new CharacterAttribute("piercingResistance", "Piercing Resistance", true, new List<string> { "piercingResistRating" }, 0, 10, 20, 0, 40, 75, true);
        new CharacterAttribute("slashingResistance", "Slashing Resistance", true, new List<string> { "slashingResistRating" }, 0, 10, 20, 0, 40, 75, true);
        new CharacterAttribute("bashingResistance", "Bashing Resistance", true, new List<string> { "bashingResistRating" }, 0, 10, 20, 0, 40, 75, true);
    }

    public Character() {
        CharacterAttributeInstance.CreateAllAttributesForCharacter(this);
    }

    // Use this for initialization
    void Start() {
        dependencies = new List<string>() { "{{PLAYER_OR_MONSTER}}", "Health", "Mana", "StatusEffectHost", "Attacker" };
        Dependencies.Check(this);
        DetermineFaction();
    }

    public void CalculateAll() {
        GetComponent<Health>().Calculate();
        GetComponent<StatusEffectHost>().Calculate();
        GetComponent<Mana>().Calculate();
        GetComponent<Attacker>().Calculate();
    }

    public void DetermineFaction() {
        if (GetComponent<PlayerCharacter>()!=null) {
            faction = "Player";
            oppositeFaction = "Enemy";
        }
        else {
            faction = "Enemy";
            oppositeFaction = "Player";
        }
    }

    //[Command]
    public void CmdSetStats(int strength, int dexterity, int constitution, int intelligence, int wisdom, int luck)
    {
        CharacterAttribute.attributes["strength"].instances[this].BaseValue = strength;
        CharacterAttribute.attributes["dexterity"].instances[this].BaseValue = dexterity;
        CharacterAttribute.attributes["constitution"].instances[this].BaseValue = constitution;
        CharacterAttribute.attributes["intelligence"].instances[this].BaseValue = intelligence;
        CharacterAttribute.attributes["wisdom"].instances[this].BaseValue = wisdom;
        CharacterAttribute.attributes["luck"].instances[this].BaseValue = luck;
        GetComponent<ExperienceGainer>().sparePoints = 0;
        CalculateAll();
    }
}