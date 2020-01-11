using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectScreen : MonoBehaviour {

    public GameObject abilityIconPrefab;
    public GameObject attackAbilitiesBox1;
    public GameObject attackAbilitiesBox2;
    public GameObject utilityAbilitiesBox;
    public GameObject passiveAbilitiesBox;
    public GameObject content;
    public ScrollRect scrollRect;
    public GameObject characterButtonPrefab;
    public GameObject playButton;
    public GameObject kittenModel;
    public GameObject kittenOuterModel;
    public GameObject characterSelectMenu;
    public GameObject characterAppearanceMenu;
    public GameObject characterStatMenu;
    public GameObject characterAbilityMenu;
    public GameObject characterNameMenu;
    public GameObject finalizeAppearanceButton;
    public List<AppearanceSelectorButton> appearanceButtons;
    public Text subHeaderText;
    public List<Material> furMaterials;
    private List<GameObject> contentObjects = new List<GameObject>();
    public List<string> characterNames = new List<string>();
    public int selectedFurType = -1;
    public int strength = 10;
    public int dexterity = 10;
    public int constitution = 10;
    public int intelligence = 10;
    public int wisdom = 10;
    public int luck = 10;
    public int sparePoints = 0;
    public int statMinimum = 1;
    public int statMaximum = 20;
    public List<Ability> selectedAbilities = new List<Ability> { null, null, null, null };
    public Text strengthText, dexterityText, constitutionText, intelligenceText, wisdomText, luckText, sparePointsText, hpText, hpRegenRateText;
    public Text receivedHealingBonusText, armorBonusText, physicalResistText, mentalResistText, mpText, mpRegenRateText, healingBonusText, moveSpeedText;
    public Text cooldownReductionText, criticalHitRateText, criticalDamageBonusText, statusEffectDurationBonusText, itemFindRateText, elementalResistanceText;
    public static SavedCharacter loadedCharacter;
    public static string selectedCharacterName;
    public static byte[] characterByteArray = null;
    private List<AttackAbility> attackAbilities1 = new List<AttackAbility>();
    private List<AttackAbility> attackAbilities2 = new List<AttackAbility>();
    private List<UtilityAbility> utilityAbilities = new List<UtilityAbility>();
    private List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    private readonly List<Ability> chosenAbilities = new List<Ability>();
    // Use this for initialization
    void Start() {
        UpdateCharacterNameList();
        UpdateCharacters();
        MusicController.instance.PlayMusic(MusicController.instance.menuMusic);
    }

    private void UpdateCharacterNameList() {
        characterNames.Clear();
        if (!Directory.Exists(Application.persistentDataPath + "/characters")) Directory.CreateDirectory(Application.persistentDataPath + "/characters");
        if (File.Exists(Application.persistentDataPath + "/characters/.character")) File.Delete(Application.persistentDataPath + "/characters/.character");
        var files = Directory.GetFiles(Application.persistentDataPath + "/characters");
        foreach (var filename in files) {
            var fi = new FileInfo(filename);
            characterNames.Add(fi.Name);
        }
    }

    private void UpdateCharacters() {
        foreach (var co in contentObjects) Destroy(co);
        contentObjects.RemoveRange(0, contentObjects.Count);
        foreach (var name in characterNames) UpdateCharacter(name);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void UpdateCharacter(string name) {
        var obj = Instantiate(characterButtonPrefab, content.transform);
        contentObjects.Add(obj);
        var characterItemUpdater = obj.GetComponent<CharacterItemUpdater>();
        var characterName = name.Replace(".character", "");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/characters/" + name, FileMode.Open);
        var loaded = (SavedCharacter)bf.Deserialize(file);
        file.Close();
        //characterItemUpdater.Initialize(loaded.level, "Str " + loaded.strength + ", Dex " + loaded.dexterity + ", Con " + loaded.constitution + ",\n Int " + loaded.intelligence + ", Wis " + loaded.wisdom + ", Luc " + loaded.luck, loaded.selectedClass, this, loaded.furType);
        characterItemUpdater.Initialize(characterName, loaded.level, loaded.furType, this);
    }

    public void NewCharacter() {
        //SceneManager.LoadScene("Class Select Screen");
        characterSelectMenu.SetActive(false);
        characterAppearanceMenu.SetActive(true);
        subHeaderText.text = "Character Creation";
    }

    public void CancelCharacterCreation() {
        characterSelectMenu.SetActive(true);
        characterAppearanceMenu.SetActive(false);
        characterStatMenu.SetActive(false);
        characterAbilityMenu.SetActive(false);
        characterNameMenu.SetActive(false);
        characterNameMenu.GetComponentInChildren<InputField>().text = "";
        attackAbilities1.Clear();
        attackAbilities2.Clear();
        utilityAbilities.Clear();
        passiveAbilities.Clear();
        strength = 10;
        dexterity = 10;
        constitution = 10;
        intelligence = 10;
        wisdom = 10;
        luck = 10;
        subHeaderText.text = "Character Selection";
        kittenOuterModel.SetActive(false);
        foreach (var obj in contentObjects) {
            var ciu = obj.GetComponent<CharacterItemUpdater>();
            ciu.selectionFrame.SetActive(false);
        }
        foreach (var appearanceButton in appearanceButtons) appearanceButton.selectionFrame.SetActive(false);
        playButton.SetActive(false);
        finalizeAppearanceButton.SetActive(false);
    }

    public void MoveToStats() {
        characterAppearanceMenu.SetActive(false);
        characterStatMenu.SetActive(true);
    }

    public void MoveToAbilities() {
        characterStatMenu.SetActive(false);
        characterAbilityMenu.SetActive(true);
        GenerateAbilities();
        AddAbilitiesToBoxes();
    }

    public void MoveToName() {
        if (selectedAbilities.Contains(null)) return;
        characterAbilityMenu.SetActive(false);
        characterNameMenu.SetActive(true);
    }

    public void FinalizeCharacter() {
        var characterName = characterNameMenu.GetComponentInChildren<InputField>().text;
        if (characterName == "") return;
        var character = SavedCharacter.BrandNewCharacter(characterName, selectedFurType);
        var spirit = new SavedSpirit();
        character.strength = strength;
        character.dexterity = dexterity;
        character.constitution = constitution;
        character.intelligence = intelligence;
        character.wisdom = wisdom;
        character.luck = luck;
        foreach (var attribute in selectedAbilities[3].attributes) {
            if (attribute.type == "boostStat") {
                switch (attribute.FindParameter("stat").stringVal) {
                    case "strength":
                        character.strength += attribute.FindParameter("degree").intVal;
                        break;
                    case "dexterity":
                        character.dexterity += attribute.FindParameter("degree").intVal;
                        break;
                    case "constitution":
                        character.constitution += attribute.FindParameter("degree").intVal;
                        break;
                    case "intelligence":
                        character.intelligence += attribute.FindParameter("degree").intVal;
                        break;
                    case "wisdom":
                        character.wisdom += attribute.FindParameter("degree").intVal;
                        break;
                    case "luck":
                        character.luck += attribute.FindParameter("degree").intVal;
                        break;
                }
            }
        }
        spirit.activeAbilities.Add(SavedActiveAbility.ConvertFrom((ActiveAbility)selectedAbilities[0]));
        spirit.activeAbilities.Add(SavedActiveAbility.ConvertFrom((ActiveAbility)selectedAbilities[1]));
        spirit.activeAbilities.Add(SavedActiveAbility.ConvertFrom((ActiveAbility)selectedAbilities[2]));
        spirit.passiveAbilities.Add(SavedPassiveAbility.ConvertFrom((PassiveAbility)selectedAbilities[3]));
        character.spirits.Add(spirit);
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "/characters")) Directory.CreateDirectory(Application.persistentDataPath + "/characters");
        FileStream file = File.Create(Application.persistentDataPath + "/characters/" + characterName + ".character");
        bf.Serialize(file, character);
        file.Close();
        FinishCharacterCreation();
    }

    private void FinishCharacterCreation() {
        CancelCharacterCreation();
        UpdateCharacterNameList();
        UpdateCharacters();
    }

    private void GenerateAbilities() {
        while (attackAbilities1.Count < 3) GenerateNonCooldownAbility();
        while (attackAbilities2.Count < 3) GenerateCooldownAbility();
        while (utilityAbilities.Count < 3) GenerateUtilityAbility();
        while (passiveAbilities.Count < 3) GeneratePassiveAbility();
    }

    private void AddAbilitiesToBoxes() {
        foreach (Transform child in attackAbilitiesBox1.transform) Destroy(child.gameObject);
        foreach (Transform child in attackAbilitiesBox2.transform) Destroy(child.gameObject);
        foreach (Transform child in utilityAbilitiesBox.transform) Destroy(child.gameObject);
        foreach (Transform child in passiveAbilitiesBox.transform) Destroy(child.gameObject);
        foreach (var ability in attackAbilities1) {
            var go = Instantiate(abilityIconPrefab, attackAbilitiesBox1.transform);
            go.GetComponent<AbilitySelectionScreenButton>().ability = ability;
            go.GetComponent<AbilitySelectionScreenButton>().characterSelectScreen = this;
            go.GetComponent<AbilitySelectionScreenButton>().abilitySlot = 0;
        }
        foreach (var ability in attackAbilities2) {
            var go = Instantiate(abilityIconPrefab, attackAbilitiesBox2.transform);
            go.GetComponent<AbilitySelectionScreenButton>().ability = ability;
            go.GetComponent<AbilitySelectionScreenButton>().characterSelectScreen = this;
            go.GetComponent<AbilitySelectionScreenButton>().abilitySlot = 1;
        }
        foreach (var ability in utilityAbilities) {
            var go = Instantiate(abilityIconPrefab, utilityAbilitiesBox.transform);
            go.GetComponent<AbilitySelectionScreenButton>().ability = ability;
            go.GetComponent<AbilitySelectionScreenButton>().characterSelectScreen = this;
            go.GetComponent<AbilitySelectionScreenButton>().abilitySlot = 2;
        }
        foreach (var ability in passiveAbilities) {
            var go = Instantiate(abilityIconPrefab, passiveAbilitiesBox.transform);
            go.GetComponent<AbilitySelectionScreenButton>().ability = ability;
            go.GetComponent<AbilitySelectionScreenButton>().characterSelectScreen = this;
            go.GetComponent<AbilitySelectionScreenButton>().abilitySlot = 3;
        }
    }

    private void GenerateNonCooldownAbility() {
        AttackAbility ability = null;
        while (ability == null || ability.cooldown > 0 || WrongStat(ability)) {
            var element = Spirit.RandomElement();
            ability = AttackAbility.Generate(new List<Element> { element });
        }
        attackAbilities1.Add(ability);
    }

    private void GenerateCooldownAbility() {
        AttackAbility ability = null;
        while (ability == null || ability.cooldown == 0 || WrongStat(ability)) {
            var element = Spirit.RandomElement();
            ability = AttackAbility.Generate(new List<Element> { element });
        }
        attackAbilities2.Add(ability);
    }

    private void GenerateUtilityAbility() {
        UtilityAbility ability = null;
        while (ability == null) {
            var element = Spirit.RandomElement();
            ability = UtilityAbility.Generate(new List<Element> { element }, 80f);
        }
        utilityAbilities.Add(ability);
    }

    private void GeneratePassiveAbility() {
        PassiveAbility ability = null;
        while (ability == null) {
            ability = PassiveAbility.Generate(70f);
        }
        passiveAbilities.Add(ability);
    }

    public void UpdateAbilitySelectionFrames() {
        UpdateAbilitySelectionFrameForBox(attackAbilitiesBox1);
        UpdateAbilitySelectionFrameForBox(attackAbilitiesBox2);
        UpdateAbilitySelectionFrameForBox(utilityAbilitiesBox);
        UpdateAbilitySelectionFrameForBox(passiveAbilitiesBox);
    }

    public void UpdateAbilitySelectionFrameForBox(GameObject go) {
        foreach (Transform child in go.transform) {
            var assb = child.GetComponent<AbilitySelectionScreenButton>();
            if (assb == null) continue;
            if (assb.ability is AttackAbility && (selectedAbilities.Contains(assb.ability as AttackAbility)) || (assb.ability is UtilityAbility && selectedAbilities.Contains(assb.ability as UtilityAbility)) || (assb.ability is PassiveAbility && selectedAbilities.Contains(assb.ability as PassiveAbility))) assb.selectionFrame.SetActive(true);
            else assb.selectionFrame.SetActive(false);
        }
    }

    private bool WrongStat(AttackAbility ability) {
        if (ability.baseStat == BaseStat.strength && (strength < dexterity || strength < intelligence)) return true;
        if (ability.baseStat == BaseStat.dexterity && (dexterity < strength || dexterity < intelligence)) return true;
        if (ability.baseStat == BaseStat.intelligence && (intelligence < strength || intelligence < dexterity)) return true;
        return false;
    }

    public void ChooseCharacter(string name, int furType) {
        foreach (var obj in contentObjects) {
            var ciu = obj.GetComponent<CharacterItemUpdater>();
            if (ciu.characterName == name) ciu.selectionFrame.SetActive(true);
            else ciu.selectionFrame.SetActive(false);
        }
        selectedCharacterName = name;
        playButton.SetActive(true);
        SetFurType(furType);
    }

    public void ClickAppearanceButton(AppearanceSelectorButton button) {
        foreach (var appearanceButton in appearanceButtons) appearanceButton.selectionFrame.SetActive(false);
        button.selectionFrame.SetActive(true);
        SetFurType(button.number);
        selectedFurType = button.number;
        finalizeAppearanceButton.SetActive(true);
    }

    public void SetFurType(int furType) {
        kittenModel.GetComponent<Renderer>().material = furMaterials[furType];
        kittenOuterModel.SetActive(true);
    }

    public void LoadCharacter() {
        SceneManager.LoadScene("New Lobby");
        characterByteArray = File.ReadAllBytes(Application.persistentDataPath + "/characters/" + selectedCharacterName + ".character");
    }

    public static void DeserializeCharacter(byte[] data) {
        BinaryFormatter bf = new BinaryFormatter();
        loadedCharacter = (SavedCharacter)bf.Deserialize(new MemoryStream(data));
    }

    public void StrengthDown() {
        if (strength > statMinimum) {
            strength -= 1;
            sparePoints += 1;
            UpdateStats();
        }
    }

    public void StrengthUp() {
        if (strength < statMaximum && sparePoints > 0) {
            strength += 1;
            sparePoints -= 1;
            UpdateStats();
        }
    }

    public void DexterityDown() {
        if (dexterity > statMinimum) {
            dexterity -= 1;
            sparePoints += 1;
            UpdateStats();
        }
    }

    public void DexterityUp() {
        if (dexterity < statMaximum && sparePoints > 0) {
            dexterity += 1;
            sparePoints -= 1;
            UpdateStats();
        }
    }

    public void ConstitutionDown() {
        if (constitution > statMinimum) {
            constitution -= 1;
            sparePoints += 1;
            UpdateStats();
        }
    }

    public void ConstitutionUp() {
        if (constitution < statMaximum && sparePoints > 0) {
            constitution += 1;
            sparePoints -= 1;
            UpdateStats();
        }
    }

    public void IntelligenceDown() {
        if (intelligence > statMinimum) {
            intelligence -= 1;
            sparePoints += 1;
            UpdateStats();
        }
    }

    public void IntelligenceUp() {
        if (intelligence < statMaximum && sparePoints > 0) {
            intelligence += 1;
            sparePoints -= 1;
            UpdateStats();
        }
    }

    public void WisdomDown() {
        if (wisdom > statMinimum) {
            wisdom -= 1;
            sparePoints += 1;
            UpdateStats();
        }
    }

    public void WisdomUp() {
        if (wisdom < statMaximum && sparePoints > 0) {
            wisdom += 1;
            sparePoints -= 1;
            UpdateStats();
        }
    }

    public void LuckDown() {
        if (luck > statMinimum) {
            luck -= 1;
            sparePoints += 1;
            UpdateStats();
        }
    }

    public void LuckUp() {
        if (luck < statMaximum && sparePoints > 0) {
            luck += 1;
            sparePoints -= 1;
            UpdateStats();
        }
    }

    private void UpdateStats() {
        strengthText.text = strength.ToString();
        dexterityText.text = dexterity.ToString();
        constitutionText.text = constitution.ToString();
        intelligenceText.text = intelligence.ToString();
        wisdomText.text = wisdom.ToString();
        luckText.text = luck.ToString();
        sparePointsText.text = sparePoints.ToString();
        hpText.text = SecondaryStatUtility.CalcHp(constitution, 1).ToString();
        hpRegenRateText.text = Mathf.Floor(SecondaryStatUtility.CalcHpRegen(constitution, 1)).ToString() + " HP/sec.";
        receivedHealingBonusText.text = Percentify(SecondaryStatUtility.CalcReceivedHealing(strength, 1));
        armorBonusText.text = Percentify(SecondaryStatUtility.CalcArmorMultiplier(constitution, 1));
        physicalResistText.text = Percentify(SecondaryStatUtility.CalcPhysicalResist(constitution, 1));
        mentalResistText.text = Percentify(SecondaryStatUtility.CalcMentalResist(wisdom, 1));
        mpText.text = SecondaryStatUtility.CalcMp(intelligence, 1).ToString();
        mpRegenRateText.text = Mathf.Floor(SecondaryStatUtility.CalcMpRegenRate(wisdom, 1)).ToString() + " MP/sec.";
        healingBonusText.text = Percentify(SecondaryStatUtility.CalcHealingMultiplier(wisdom, 1));
        moveSpeedText.text = Percentify(SecondaryStatUtility.CalcMoveSpeed(dexterity, 1));
        cooldownReductionText.text = Percentify(SecondaryStatUtility.CalcCooldownReduction(wisdom, 1));
        criticalHitRateText.text = Percentify(SecondaryStatUtility.CalcCriticalHitRate(luck, 1));
        criticalDamageBonusText.text = Percentify(SecondaryStatUtility.CalcCriticalDamage(luck, 1));
        statusEffectDurationBonusText.text = Percentify(SecondaryStatUtility.CalcStatusEffectDurationBonus(luck, 1));
        itemFindRateText.text = Percentify(SecondaryStatUtility.CalcItemFindRate(luck, 1));
        elementalResistanceText.text = Percentify(SecondaryStatUtility.CalcElementalResistanceFromLuck(luck, 1));
    }

    private string Percentify(float number) {
        var newNumber = number * 100f;
        newNumber = Mathf.Floor(newNumber);
        return newNumber.ToString() + "%";
    }

    public void PressPlay() {
        var config = GameObject.FindGameObjectWithTag("ConfigObject");
        Destroy(config);
        characterByteArray = File.ReadAllBytes(Application.persistentDataPath + "/characters/" + selectedCharacterName + ".character");
        SceneManager.LoadScene("New Lobby");
    }

    public void PressDelete() {
        File.Delete(Application.persistentDataPath + "/characters/" + selectedCharacterName + ".character");
        File.Delete(Application.persistentDataPath + "/worlds/" + selectedCharacterName + ".world");
        UpdateCharacterNameList();
        UpdateCharacters();
    }
}
