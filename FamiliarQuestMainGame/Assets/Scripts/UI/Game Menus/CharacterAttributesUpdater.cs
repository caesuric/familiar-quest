using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributesUpdater : MonoBehaviour {

    private Character character;
    private ExperienceGainer eg;
    public Text strength;
    public Text dexterity;
    public Text constitution;
    public Text intelligence;
    public Text wisdom;
    public Text luck;
    public Text sparePointsText;
    public GameObject strengthDownButton;
    public GameObject strengthUpButton;
    public GameObject dexterityDownButton;
    public GameObject dexterityUpButton;
    public GameObject constitutionDownButton;
    public GameObject constitutionUpButton;
    public GameObject intelligenceDownButton;
    public GameObject intelligenceUpButton;
    public GameObject wisdomDownButton;
    public GameObject wisdomUpButton;
    public GameObject luckDownButton;
    public GameObject luckUpButton;
    public Text currentReceivedHealing;
    public Text newReceivedHealing;
    public Text currentMoveSpeed;
    public Text newMoveSpeed;
    public Text currentHp;
    public Text newHp;
    public Text currentHpRegenRate;
    public Text newHpRegenRate;
    public Text currentArmorMultiplier;
    public Text newArmorMultiplier;
    public Text currentPhysicalResist;
    public Text newPhysicalResist;
    public Text currentMp;
    public Text newMp;
    public Text currentMentalResist;
    public Text newMentalResist;
    public Text currentMpRegenRate;
    public Text newMpRegenRate;
    public Text currentHealingMultiplier;
    public Text newHealingMultiplier;
    public Text currentCooldownReduction;
    public Text newCooldownReduction;
    public Text currentCriticalHitRate;
    public Text newCriticalHitRate;
    public Text currentCriticalDamageBonus;
    public Text newCriticalDamageBonus;
    public Text currentStatusEffectDuration;
    public Text newStatusEffectDuration;
    public Text currentItemFindRate;
    public Text newItemFindRate;
    public Text currentFireResist;
    public Text newFireResist;
    public Text currentIceResist;
    public Text newIceResist;
    public Text currentAcidResist;
    public Text newAcidResist;
    public Text currentLightResist;
    public Text newLightResist;
    public Text currentDarkResist;
    public Text newDarkResist;
    private float newReceivedHealingValue;
    private float newMoveSpeedValue;
    private float newHpValue;
    private float newHpRegenRateValue;
    private float newArmorMultiplierValue;
    private float newPhysicalResistValue;
    private float newMpValue;
    private float newMentalResistValue;
    private float newMpRegenRateValue;
    private float newHealingMultiplierValue;
    private float newCooldownReductionValue;
    private float newCriticalHitRateValue;
    private float newCriticalDamageBonusValue;
    private float newStatusEffectDurationValue;
    private float newItemFindRateValue;
    private float newFireResistValue;
    private float newIceResistValue;
    private float newAcidResistValue;
    private float newLightResistValue;
    private float newDarkResistValue;
    private int currentStrength, minStrength;
    private int currentDexterity, minDexterity;
    private int currentConstitution, minConstitution;
    private int currentIntelligence, minIntelligence;
    private int currentWisdom, minWisdom;
    private int currentLuck, minLuck;
    private int sparePoints;
    public GameObject strengthAdvanced;
    public GameObject dexterityAdvanced;
    public GameObject constitutionAdvanced;
    public GameObject intelligenceAdvanced;
    public GameObject wisdomAdvanced;
    public GameObject luckAdvanced;
    private GameObject currentAdvancedPanel = null;
    public List<GameObject> sparePointsInterface = new List<GameObject>();
    public List<GameObject> advancedStatIncreaseInterface = new List<GameObject>();
    private bool levelUpModeActive = false;

    // Use this for initialization
    void Start() {
        character = PlayerCharacter.localPlayer.GetComponent<Character>();
        eg = character.GetComponent<ExperienceGainer>();
        currentAdvancedPanel = strengthAdvanced;
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(eg.sparePoints);
        if (eg.sparePoints == 0) {
            strength.text = CharacterAttribute.attributes["strength"].instances[character].TotalValue.ToString();
            dexterity.text = CharacterAttribute.attributes["dexterity"].instances[character].TotalValue.ToString();
            constitution.text = CharacterAttribute.attributes["constitution"].instances[character].TotalValue.ToString();
            intelligence.text = CharacterAttribute.attributes["intelligence"].instances[character].TotalValue.ToString();
            wisdom.text = CharacterAttribute.attributes["wisdom"].instances[character].TotalValue.ToString();
            luck.text = CharacterAttribute.attributes["luck"].instances[character].TotalValue.ToString();
            strengthDownButton.GetComponent<Image>().enabled = false;
            strengthUpButton.GetComponent<Image>().enabled = false;
            dexterityDownButton.GetComponent<Image>().enabled = false;
            dexterityUpButton.GetComponent<Image>().enabled = false;
            constitutionDownButton.GetComponent<Image>().enabled = false;
            constitutionUpButton.GetComponent<Image>().enabled = false;
            intelligenceDownButton.GetComponent<Image>().enabled = false;
            intelligenceUpButton.GetComponent<Image>().enabled = false;
            wisdomDownButton.GetComponent<Image>().enabled = false;
            wisdomUpButton.GetComponent<Image>().enabled = false;
            luckDownButton.GetComponent<Image>().enabled = false;
            luckUpButton.GetComponent<Image>().enabled = false;
        }
        else if (!levelUpModeActive) {
            levelUpModeActive = true;
            currentStrength = minStrength = (int)CharacterAttribute.attributes["strength"].instances[character].BaseValue;
            currentDexterity = minDexterity = (int)CharacterAttribute.attributes["dexterity"].instances[character].BaseValue;
            currentConstitution = minConstitution = (int)CharacterAttribute.attributes["constitution"].instances[character].BaseValue;
            currentIntelligence = minIntelligence = (int)CharacterAttribute.attributes["intelligence"].instances[character].BaseValue;
            currentWisdom = minWisdom = (int)CharacterAttribute.attributes["wisdom"].instances[character].BaseValue;
            currentLuck = minLuck = (int)CharacterAttribute.attributes["luck"].instances[character].BaseValue;
            sparePoints = eg.sparePoints;
            UpdateAdvancedStats();
        }
        else {
            strength.text = currentStrength.ToString();
            dexterity.text = currentDexterity.ToString();
            constitution.text = currentConstitution.ToString();
            intelligence.text = currentIntelligence.ToString();
            wisdom.text = currentWisdom.ToString();
            luck.text = currentLuck.ToString();
            sparePointsText.text = "Points left: " + sparePoints.ToString();
            strengthDownButton.GetComponent<Image>().enabled = (currentStrength > minStrength);
            strengthUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            dexterityDownButton.GetComponent<Image>().enabled = (currentDexterity > minDexterity);
            dexterityUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            constitutionDownButton.GetComponent<Image>().enabled = (currentConstitution > minConstitution);
            constitutionUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            intelligenceDownButton.GetComponent<Image>().enabled = (currentIntelligence > minIntelligence);
            intelligenceUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            wisdomDownButton.GetComponent<Image>().enabled = (currentWisdom > minWisdom);
            wisdomUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            luckDownButton.GetComponent<Image>().enabled = (currentLuck > minLuck);
            luckUpButton.GetComponent<Image>().enabled = (sparePoints > 0);
            newReceivedHealing.text = newReceivedHealingValue.ToString() + "%";
            newMoveSpeed.text = newMoveSpeedValue.ToString() + "%";
            newHp.text = newHpValue.ToString();
            newHpRegenRate.text = newHpRegenRateValue.ToString() + "HP / sec.";
            newArmorMultiplier.text = newArmorMultiplierValue.ToString() + "%";
            newPhysicalResist.text = newPhysicalResistValue.ToString() + "%";
            newMp.text = newMpValue.ToString();
            newMentalResist.text = newMentalResistValue.ToString() + "%";
            newMpRegenRate.text = newMpRegenRateValue.ToString() + "MP / sec.";
            newHealingMultiplier.text = newHealingMultiplierValue.ToString() + "%";
            newCooldownReduction.text = newCooldownReductionValue.ToString() + "%";
            newCriticalHitRate.text = newCriticalHitRateValue.ToString() + "%";
            newCriticalDamageBonus.text = newCriticalDamageBonusValue.ToString() + "%";
            newStatusEffectDuration.text = newStatusEffectDurationValue.ToString() + "%";
            newItemFindRate.text = newItemFindRateValue.ToString() + "%";
            newFireResist.text = newFireResistValue.ToString() + "%";
            newIceResist.text = newIceResistValue.ToString() + "%";
            newAcidResist.text = newAcidResistValue.ToString() + "%";
            newLightResist.text = newLightResistValue.ToString() + "%";
            newDarkResist.text = newDarkResistValue.ToString() + "%";
        }
        currentReceivedHealing.text = ((int)CharacterAttribute.attributes["receivedHealing"].instances[character].TotalValue).ToString() + "%";
        currentMoveSpeed.text = ((int)CharacterAttribute.attributes["moveSpeed"].instances[character].TotalValue).ToString() + "%";
        currentHp.text = ((int)CharacterAttribute.attributes["bonusHp"].instances[character].TotalValue).ToString();
        currentHpRegenRate.text = ((int)CharacterAttribute.attributes["hpRegen"].instances[character].TotalValue).ToString() + "HP / sec.";
        currentArmorMultiplier.text = ((int)CharacterAttribute.attributes["armorMultiplier"].instances[character].TotalValue).ToString() + "%";
        currentPhysicalResist.text = ((int)CharacterAttribute.attributes["physicalResistance"].instances[character].TotalValue).ToString() + "%";
        currentMp.text = ((int)CharacterAttribute.attributes["bonusMp"].instances[character].TotalValue).ToString();
        currentMentalResist.text = ((int)CharacterAttribute.attributes["mentalResistance"].instances[character].TotalValue).ToString() + "%";
        currentMpRegenRate.text = ((int)CharacterAttribute.attributes["mpRegen"].instances[character].TotalValue).ToString() + "MP / sec.";
        currentHealingMultiplier.text = ((int)CharacterAttribute.attributes["healingMultiplier"].instances[character].TotalValue).ToString() + "%";
        currentCooldownReduction.text = ((int)CharacterAttribute.attributes["cooldownReduction"].instances[character].TotalValue).ToString() + "%";
        currentCriticalHitRate.text = ((int)CharacterAttribute.attributes["criticalHitChance"].instances[character].TotalValue).ToString() + "%";
        currentCriticalDamageBonus.text = ((int)CharacterAttribute.attributes["criticalDamage"].instances[character].TotalValue).ToString() + "%";
        currentStatusEffectDuration.text = ((int)CharacterAttribute.attributes["statusEffectDuration"].instances[character].TotalValue).ToString() + "%";
        currentItemFindRate.text = ((int)CharacterAttribute.attributes["itemFindRate"].instances[character].TotalValue).ToString() + "%";
        currentFireResist.text = ((int)CharacterAttribute.attributes["fireResistance"].instances[character].TotalValue).ToString() + "%";
        currentIceResist.text = ((int)CharacterAttribute.attributes["iceResistance"].instances[character].TotalValue).ToString() + "%";
        currentAcidResist.text = ((int)CharacterAttribute.attributes["acidResistance"].instances[character].TotalValue).ToString() + "%";
        currentLightResist.text = ((int)CharacterAttribute.attributes["lightResistance"].instances[character].TotalValue).ToString() + "%";
        currentDarkResist.text = ((int)CharacterAttribute.attributes["darkResistance"].instances[character].TotalValue).ToString() + "%";
        foreach (var item in sparePointsInterface) item.SetActive(eg.sparePoints > 0);
        foreach (var item in advancedStatIncreaseInterface) item.SetActive(eg.sparePoints > 0);
    }

    public void StrengthDown() {
        if (currentStrength > minStrength) {
            currentStrength -= 1;
            sparePoints += 1;
            UpdateAdvancedStats();
        }
    }

    public void StrengthUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentStrength += 1;
            UpdateAdvancedStats();
        }
    }

    public void DexterityDown() {
        if (currentDexterity > minDexterity) {
            currentDexterity -= 1;
            sparePoints += 1;
            UpdateAdvancedStats();
        }
    }

    public void DexterityUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentDexterity += 1;
            UpdateAdvancedStats();
        }
    }

    public void ConstitutionDown() {
        if (currentConstitution > minConstitution) {
            currentConstitution -= 1;
            sparePoints += 1;
            UpdateAdvancedStats();
        }
    }

    public void ConstitutionUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentConstitution += 1;
            UpdateAdvancedStats();
        }
    }

    public void IntelligenceDown() {
        if (currentIntelligence > minIntelligence) {
            currentIntelligence -= 1;
            sparePoints += 1;
            UpdateAdvancedStats();
        }
    }

    public void IntelligenceUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentIntelligence += 1;
            UpdateAdvancedStats();
        }
    }

    public void WisdomDown() {
        if (currentWisdom > minWisdom) {
            currentWisdom -= 1;
            sparePoints += 1;
            UpdateAdvancedStats();
        }
    }

    public void WisdomUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentWisdom += 1;
            UpdateAdvancedStats();
        }
    }

    public void LuckDown() {
        if (currentLuck > minLuck) {
            currentLuck -= 1;
            sparePoints += 1;
            UpdateAdvancedStats();
        }
    }

    public void LuckUp() {
        if (sparePoints > 0) {
            sparePoints -= 1;
            currentLuck += 1;
            UpdateAdvancedStats();
        }
    }

    private void UpdateAdvancedStats() {
        int oldStrength = (int)CharacterAttribute.attributes["strength"].instances[character].BaseValue;
        int oldDexterity = (int)CharacterAttribute.attributes["dexterity"].instances[character].BaseValue;
        int oldConstitution = (int)CharacterAttribute.attributes["constitution"].instances[character].BaseValue;
        int oldIntelligence = (int)CharacterAttribute.attributes["intelligence"].instances[character].BaseValue;
        int oldWisdom = (int)CharacterAttribute.attributes["wisdom"].instances[character].BaseValue;
        int oldLuck = (int)CharacterAttribute.attributes["luck"].instances[character].BaseValue;
        character.CmdSetStats(currentStrength, currentDexterity, currentConstitution, currentIntelligence, currentWisdom, currentLuck);
        newReceivedHealingValue = (int)CharacterAttribute.attributes["receivedHealing"].instances[character].TotalValue;
        newMoveSpeedValue = (int)CharacterAttribute.attributes["moveSpeed"].instances[character].TotalValue;
        newHpValue = (int)CharacterAttribute.attributes["bonusHp"].instances[character].TotalValue;
        newHpRegenRateValue = (int)CharacterAttribute.attributes["hpRegen"].instances[character].TotalValue;
        newArmorMultiplierValue = (int)CharacterAttribute.attributes["armorMultiplier"].instances[character].TotalValue;
        newPhysicalResistValue = (int)CharacterAttribute.attributes["physicalResistance"].instances[character].TotalValue;
        newMpValue = (int)CharacterAttribute.attributes["bonusMp"].instances[character].TotalValue;
        newMentalResistValue = (int)CharacterAttribute.attributes["mentalResistance"].instances[character].TotalValue;
        newMpRegenRateValue = (int)CharacterAttribute.attributes["mpRegen"].instances[character].TotalValue;
        newHealingMultiplierValue = (int)CharacterAttribute.attributes["healingMultiplier"].instances[character].TotalValue;
        newCooldownReductionValue = (int)CharacterAttribute.attributes["cooldownReduction"].instances[character].TotalValue;
        newCriticalHitRateValue = (int)CharacterAttribute.attributes["criticalHitChance"].instances[character].TotalValue;
        newCriticalDamageBonusValue = (int)CharacterAttribute.attributes["criticalDamage"].instances[character].TotalValue;
        newStatusEffectDurationValue = (int)CharacterAttribute.attributes["statusEffectDuration"].instances[character].TotalValue;
        newItemFindRateValue = (int)CharacterAttribute.attributes["itemFindRate"].instances[character].TotalValue;
        newFireResistValue = (int)CharacterAttribute.attributes["fireResistance"].instances[character].TotalValue;
        newIceResistValue = (int)CharacterAttribute.attributes["iceResistance"].instances[character].TotalValue;
        newAcidResistValue = (int)CharacterAttribute.attributes["acidResistance"].instances[character].TotalValue;
        newLightResistValue = (int)CharacterAttribute.attributes["lightResistance"].instances[character].TotalValue;
        newDarkResistValue = (int)CharacterAttribute.attributes["darkResistance"].instances[character].TotalValue;
        character.CmdSetStats(oldStrength, oldDexterity, oldConstitution, oldIntelligence, oldWisdom, oldLuck);
    }

    public void ConfirmPoints() {
        eg.sparePoints = sparePoints;
        character.CmdSetStats(currentStrength, currentDexterity, currentConstitution, currentIntelligence, currentWisdom, currentLuck);
        levelUpModeActive = false;
    }

    public void ActivateStrengthAdvanced() {
        ActivatePanel(strengthAdvanced);
    }

    public void ActivateDexterityAdvanced() {
        ActivatePanel(dexterityAdvanced);
    }

    public void ActivateConstitutionAdvanced() {
        ActivatePanel(constitutionAdvanced);
    }

    public void ActivateIntelligenceAdvanced() {
        ActivatePanel(intelligenceAdvanced);
    }

    public void ActivateWisdomAdvanced() {
        ActivatePanel(wisdomAdvanced);
    }

    public void ActivateLuckAdvanced() {
        ActivatePanel(luckAdvanced);
    }

    private void ActivatePanel(GameObject panel) {
        if (currentAdvancedPanel != null) currentAdvancedPanel.SetActive(false);
        panel.SetActive(true);
        currentAdvancedPanel = panel;
    }
}