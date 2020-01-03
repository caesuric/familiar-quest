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
        hp.text = SecondaryStatUtility.CalcHp(c.constitution, eg.level).ToString();
        hpRegen.text = SecondaryStatUtility.CalcHpRegen(c.constitution, eg.level).ToString() + " HP/sec.";
        receivedHealing.text = Percentify(SecondaryStatUtility.CalcReceivedHealing(c.strength, eg.level));
        armorMultiplier.text = Percentify(SecondaryStatUtility.CalcArmorMultiplier(c.constitution, eg.level));
        physicalResist.text = Percentify(SecondaryStatUtility.CalcPhysicalResist(c.constitution, eg.level));
        mentalResist.text = Percentify(SecondaryStatUtility.CalcMentalResist(c.wisdom, eg.level));
        mp.text = SecondaryStatUtility.CalcMp(c.intelligence, eg.level).ToString();
        mpRegen.text = SecondaryStatUtility.CalcMpRegenRate(c.wisdom, eg.level).ToString() + " MP/sec.";
        healingMultiplier.text = Percentify(SecondaryStatUtility.CalcHealingMultiplier(c.wisdom, eg.level));
        moveSpeed.text = Percentify(SecondaryStatUtility.CalcMoveSpeed(c.dexterity, eg.level));
        cooldownReduction.text = Percentify(SecondaryStatUtility.CalcCooldownReduction(c.wisdom, eg.level));
        criticalHitRate.text = Percentify(SecondaryStatUtility.CalcCriticalHitRate(c.luck, eg.level));
        criticalDamageBonus.text = Percentify(SecondaryStatUtility.CalcCriticalDamage(c.luck, eg.level));
        statusEffectDuration.text = Percentify(SecondaryStatUtility.CalcStatusEffectDurationBonus(c.luck, eg.level));
        itemFindRate.text = Percentify(SecondaryStatUtility.CalcItemFindRate(c.luck, eg.level));
        elementalResistance.text = Percentify(SecondaryStatUtility.CalcElementalResistanceFromLuck(c.luck, eg.level));
    }

    private string Percentify(float number) {
        var newNumber = number * 100f;
        newNumber = Mathf.Floor(newNumber);
        return newNumber.ToString() + "%";
    }
}
