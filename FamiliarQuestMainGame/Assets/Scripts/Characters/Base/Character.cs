using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[NetworkSettings(sendInterval = 0.016f)]
public class Character : DependencyUser {

    public int strength;
    public int dexterity;
    public int constitution;
    public int intelligence;
    public int wisdom;
    public int luck;
    public string faction;
    public string oppositeFaction;

    static Character() {
        var temp = new CharacterAttribute("strength", "Strength", false);
        temp = new CharacterAttribute("dexterity", "Dexterity", false);
        temp = new CharacterAttribute("constitution", "Constitution", false);
        temp = new CharacterAttribute("intelligence", "Intelligence", false);
        temp = new CharacterAttribute("wisdom", "Wisdom", false);
        temp = new CharacterAttribute("luck", "Luck", false);
        temp = new CharacterAttribute("hpRating", "HP Rating", false);
        temp = new CharacterAttribute("hpRegenRating", "HP Regen Rating", false);
        temp = new CharacterAttribute("receivedHealingRating", "Received Healing Rating", false);
        temp = new CharacterAttribute("armorMultiplierRating", "Armor Multiplier Rating", false);
        temp = new CharacterAttribute("physicalResistRating", "Physical Resist Rating", false);
        temp = new CharacterAttribute("mentalResistRating", "Mental Resist Rating", false);
        temp = new CharacterAttribute("mpRating", "MP Rating", false);
        temp = new CharacterAttribute("healingMultiplierRating", "Healing Multiplier Rating", false);
        temp = new CharacterAttribute("moveSpeedRating", "Move Speed Rating", false);
        temp = new CharacterAttribute("cooldownReductionRating", "Cooldown Reduction Rating", false);
        temp = new CharacterAttribute("criticalHitChanceRating", "Critical Hit Chance Rating", false);
        temp = new CharacterAttribute("criticalDamageRating", "Critical Damage Rating", false);
        temp = new CharacterAttribute("statusEffectDurationRating", "Status Effect Duration Rating", false);
        temp = new CharacterAttribute("itemFindRating", "Item Find Rating", false);
        temp = new CharacterAttribute("fireResistRating", "Fire Resist Rating", false);
        temp = new CharacterAttribute("iceResistRating", "Ice Resist Rating", false);
        temp = new CharacterAttribute("acidResistRating", "Acid Resist Rating", false);
        temp = new CharacterAttribute("lightResistRating", "Light Resist Rating", false);
        temp = new CharacterAttribute("darkResistRating", "Dark Resist Rating", false);
        temp = new CharacterAttribute("piercingResistRating", "Piercing Resist Rating", false);
        temp = new CharacterAttribute("slashingResistRating", "Slashing Resist Rating", false);
        temp = new CharacterAttribute("bashingResistRating", "Bashing Resist Rating", false);

        temp = new CharacterAttribute("bonusHp", "Bonus HP", true, new List<string> { "hpRating" }, 0, 145, 290, 0, 7500, 15000);
        temp = new CharacterAttribute("hpRegen", "HP Regen", true, new List<string> { "constitution", "hpRegenRating" }, 0, 0, 2, 0, 0, 100);
        temp = new CharacterAttribute("receivedHealing", "Received Healing", true, new List<string> { "strength", "receivedHealingRating" }, 25, 100, 200, 50, 100, 400, true);
        temp = new CharacterAttribute("armorMultiplier", "Armor Multiplier", true, new List<string> { "constitution", "armorMultiplierRating" }, 25, 100, 200, 50, 100, 400, true);
        temp = new CharacterAttribute("physicalResistance", "Physical Resistance", true, new List<string> { "constitution", "physicalResistRating" }, 0, 30, 60, 0, 50, 100, true);
        temp = new CharacterAttribute("mentalResistance", "Mental Resistance", true, new List<string> { "wisdom", "mentalResistRating" }, 0, 30, 60, 0, 50, 100, true);
        temp = new CharacterAttribute("bonusMp", "Bonus MP", true, new List<string> { "mpRating" }, 0, 145, 290, 0, 7500, 15000);
        temp = new CharacterAttribute("healingMultiplier", "Healing Multiplier", true, new List<string> { "wisdom", "healingMultiplierRating" }, 25, 100, 200, 50, 100, 400, true);
        temp = new CharacterAttribute("moveSpeed", "Move Speed", true, new List<string> { "dexterity", "moveSpeedRating" }, 75, 100, 150, 75, 100, 200, true);
        temp = new CharacterAttribute("cooldownReduction", "Cooldown Reduction", true, new List<string> { "wisdom", "cooldownReductionRating" }, 0, 0, 25, 0, 0, 50, true);
        temp = new CharacterAttribute("criticalHitChance", "Critical Hit Chance", true, new List<string> { "luck", "criticalHitChanceRating" }, 0, 20, 30, 10, 20, 50, true);
        temp = new CharacterAttribute("criticalDamage", "Critical Hit Damage", true, new List<string> { "luck", "criticalDamageRating" }, 100, 200, 250, 150, 200, 300, true);
        temp = new CharacterAttribute("statusEffectDuration", "Status Effect Duration", true, new List<string> { "luck", "statusEffectDurationRating" }, -80, 0, 40, -40, 0, 80, true);
        temp = new CharacterAttribute("itemFindRate", "Item Find Rate", true, new List<string> { "luck", "itemFindRate" }, 0, 20, 30, 10, 20, 50, true);
        temp = new CharacterAttribute("fireResistance", "Fire Resistance", true, new List<string> { "luck", "fireResistRating" }, 0, 0, 20, 0, 0, 75, true);
        temp = new CharacterAttribute("iceResistance", "Ice Resistance", true, new List<string> { "luck", "iceResistRating" }, 0, 0, 20, 0, 0, 75, true);
        temp = new CharacterAttribute("acidResistance", "Acid Resistance", true, new List<string> { "luck", "acidResistRating" }, 0, 0, 20, 0, 0, 75, true);
        temp = new CharacterAttribute("lightResistance", "Light Resistance", true, new List<string> { "luck", "lightResistRating" }, 0, 0, 20, 0, 0, 75, true);
        temp = new CharacterAttribute("darkResistance", "Dark Resistance", true, new List<string> { "luck", "darkResistRating" }, 0, 0, 20, 0, 0, 75, true);
        temp = new CharacterAttribute("piercingResistance", "Piercing Resistance", true, new List<string> { "piercingResistRating" }, 0, 10, 20, 0, 40, 75, true);
        temp = new CharacterAttribute("slashingResistance", "Slashing Resistance", true, new List<string> { "slashingResistRating" }, 0, 10, 20, 0, 40, 75, true);
        temp = new CharacterAttribute("bashingResistance", "Bashing Resistance", true, new List<string> { "bashingResistRating" }, 0, 10, 20, 0, 40, 75, true);
    }

    // Use this for initialization
    void Start() {
        dependencies = new List<string>() { "{{PLAYER_OR_MONSTER}}", "Health", "Mana", "StatusEffectHost", "Attacker" };
        Dependencies.Check(this);
        DetermineFaction();
        CharacterAttributeInstance.CreateAllAttributesForCharacter(this);
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
        this.strength = strength;
        this.dexterity = dexterity;
        this.constitution = constitution;
        this.intelligence = intelligence;
        this.wisdom = wisdom;
        this.luck = luck;
        GetComponent<ExperienceGainer>().sparePoints = 0;
        CalculateAll();
    }
}