using UnityEngine;
using UnityEngine.UI;

public class CharacterStatisticsUpdater : MonoBehaviour {

    private Character c;
    private ExperienceGainer eg;
    public Text hp;
    public Text hpRegen;
    public Text receivedHealing;
    public Text armorMultiplier;
    public Text physicalResist;
    public Text mentalResist;
    public Text mp;
    public Text mpRegen;
    public Text healingMultiplier;
    public Text moveSpeed;
    public Text cooldownReduction;
    public Text criticalHitRate;
    public Text criticalDamageBonus;
    public Text statusEffectDuration;
    public Text itemFindRate;
    public Text elementalResistance;

    // Use this for initialization
    void Start() {
        c = PlayerCharacter.players[0].GetComponent<Character>();
        eg = c.GetComponent<ExperienceGainer>();
    }

    // Update is called once per frame
    void Update() {
        //hp.text = SecondaryStatUtility.CalcHp(c.constitution, eg.level).ToString();
        //hpRegen.text = SecondaryStatUtility.CalcHpRegen(c.constitution, eg.level).ToString() + " HP/sec.";
        //receivedHealing.text = Percentify(SecondaryStatUtility.CalcReceivedHealing(c.strength, eg.level));
        //armorMultiplier.text = Percentify(SecondaryStatUtility.CalcArmorMultiplier(c.constitution, eg.level));
        //physicalResist.text = Percentify(SecondaryStatUtility.CalcPhysicalResist(c.constitution, eg.level));
        //mentalResist.text = Percentify(SecondaryStatUtility.CalcMentalResist(c.wisdom, eg.level));
        //mp.text = SecondaryStatUtility.CalcMp(c.intelligence, eg.level).ToString();
        //mpRegen.text = SecondaryStatUtility.CalcMpRegenRate(c.wisdom, eg.level).ToString() + " MP/sec.";
        //healingMultiplier.text = Percentify(SecondaryStatUtility.CalcHealingMultiplier(c.wisdom, eg.level));
        //moveSpeed.text = Percentify(SecondaryStatUtility.CalcMoveSpeed(c.dexterity, eg.level));
        //cooldownReduction.text = Percentify(SecondaryStatUtility.CalcCooldownReduction(c.wisdom, eg.level));
        //criticalHitRate.text = Percentify(SecondaryStatUtility.CalcCriticalHitRate(c.luck, eg.level));
        //criticalDamageBonus.text = Percentify(SecondaryStatUtility.CalcCriticalDamage(c.luck, eg.level));
        //statusEffectDuration.text = Percentify(SecondaryStatUtility.CalcStatusEffectDurationBonus(c.luck, eg.level));
        //itemFindRate.text = Percentify(SecondaryStatUtility.CalcItemFindRate(c.luck, eg.level));
        //elementalResistance.text = Percentify(SecondaryStatUtility.CalcElementalResistanceFromLuck(c.luck, eg.level));
        hp.text = CharacterAttribute.attributes["bonusHp"].instances[c].TotalValue.ToString();
        hpRegen.text = CharacterAttribute.attributes["hpRegen"].instances[c].TotalValue.ToString() + " HP/sec.";
        receivedHealing.text = CharacterAttribute.attributes["receivedHealing"].instances[c].TotalValue.ToString() + "%";
        armorMultiplier.text = CharacterAttribute.attributes["armorMultiplier"].instances[c].TotalValue.ToString() + "%";
        physicalResist.text = CharacterAttribute.attributes["physicalResistance"].instances[c].TotalValue.ToString() + "%";
        mentalResist.text = CharacterAttribute.attributes["mentalResistance"].instances[c].TotalValue.ToString() + "%";
        mp.text = CharacterAttribute.attributes["bonusMp"].instances[c].TotalValue.ToString();
        mpRegen.text = CharacterAttribute.attributes["mpRegen"].instances[c].TotalValue.ToString() + " MP/sec.";
        healingMultiplier.text = CharacterAttribute.attributes["healingMultiplier"].instances[c].TotalValue.ToString() + "%";
        moveSpeed.text = CharacterAttribute.attributes["moveSpeed"].instances[c].TotalValue.ToString() + "%";
        cooldownReduction.text = CharacterAttribute.attributes["cooldownReduction"].instances[c].TotalValue.ToString() + "%";
        criticalHitRate.text = CharacterAttribute.attributes["criticalHitChance"].instances[c].TotalValue.ToString() + "%";
        criticalDamageBonus.text = CharacterAttribute.attributes["criticalDamage"].instances[c].TotalValue.ToString() + "%";
        statusEffectDuration.text = CharacterAttribute.attributes["statusEffectDuration"].instances[c].TotalValue.ToString() + "%";
        itemFindRate.text = CharacterAttribute.attributes["itemFindRate"].instances[c].TotalValue.ToString() + "%";
        elementalResistance.text = CharacterAttribute.attributes["fireResistance"].instances[c].TotalValue.ToString() + "%";
    }

    private string Percentify(float number) {
        var newNumber = number * 100f;
        newNumber = Mathf.Floor(newNumber);
        return newNumber.ToString() + "%";
    }
}
