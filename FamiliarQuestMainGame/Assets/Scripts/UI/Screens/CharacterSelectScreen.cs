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
    public Text abilityScreenStrengthText;
    public Text abilityScreenDexterityText;
    public Text abilityScreenConstitutionText;
    public Text abilityScreenIntelligenceText;
    public Text abilityScreenWisdomText;
    public Text abilityScreenLuckText;
    public GameObject characterDeleteConfirmPanel;
    private bool generatedAbilities = false;
    private List<AttackAbility> attackAbilities1 = new List<AttackAbility>();
    private List<AttackAbility> attackAbilities2 = new List<AttackAbility>();
    private List<UtilityAbility> utilityAbilities = new List<UtilityAbility>();
    private List<PassiveAbility> passiveAbilities = new List<PassiveAbility>();
    private readonly List<Ability> chosenAbilities = new List<Ability>();
    private static Character fakeCharacter = null;
    // Use this for initialization
    void Start() {
        UpdateCharacterNameList();
        UpdateCharacters();
        MusicController.instance.PlayMusic(MusicController.instance.menuMusic);
        var cgo = new GameObject();
        cgo.AddComponent<NoiseMaker>();
        cgo.AddComponent<SimulatedNoiseGenerator>();
        cgo.AddComponent<AudioGenerator>();
        cgo.AddComponent<AnimationController>();
        cgo.AddComponent<ExperienceGainer>();
        cgo.AddComponent<CacheGrabber>();
        cgo.AddComponent<Attacker>();
        cgo.AddComponent<ObjectSpawner>();
        cgo.AddComponent<StatusEffectHost>();
        cgo.AddComponent<AbilityUser>();
        cgo.AddComponent<ConfigGrabber>();
        cgo.AddComponent<HotbarUser>();
        cgo.AddComponent<Health>();
        cgo.AddComponent<Mana>();
        cgo.AddComponent<PlayerCharacter>();
        cgo.GetComponent<PlayerCharacter>().weapon = new MeleeWeapon {
            attackPower = 0.8437f
        };
        fakeCharacter = cgo.AddComponent<Character>();
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
        sparePoints = 0;
        UpdateStats();
        subHeaderText.text = "Character Selection";
        kittenOuterModel.SetActive(false);
        foreach (var obj in contentObjects) {
            var ciu = obj.GetComponent<CharacterItemUpdater>();
            ciu.selectionFrame.SetActive(false);
        }
        foreach (var appearanceButton in appearanceButtons) appearanceButton.selectionFrame.SetActive(false);
        playButton.SetActive(false);
        finalizeAppearanceButton.SetActive(false);
        generatedAbilities = false;
    }

    public void MoveToStats() {
        characterAppearanceMenu.SetActive(false);
        characterStatMenu.SetActive(true);
        generatedAbilities = false;
    }

    public void MoveBackToStats() {
        characterAbilityMenu.SetActive(false);
        characterStatMenu.SetActive(true);
    }

    public void MoveToAbilities() {
        if (sparePoints > 0) return;
        characterStatMenu.SetActive(false);
        characterAbilityMenu.SetActive(true);
        if (!generatedAbilities) {
            GenerateAbilities();
            AddAbilitiesToBoxes();
        }
        generatedAbilities = true;
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
        character.strength = strength;
        character.dexterity = dexterity;
        character.constitution = constitution;
        character.intelligence = intelligence;
        character.wisdom = wisdom;
        character.luck = luck;
        foreach (var attribute in selectedAbilities[3].attributes) {
            if (attribute.type == "boostStat") {
                switch ((string)attribute.FindParameter("stat").value) {
                    case "strength":
                        character.strength += Mathf.FloorToInt((float)attribute.FindParameter("degree").value);
                        break;
                    case "dexterity":
                        character.dexterity += Mathf.FloorToInt((float)attribute.FindParameter("degree").value);
                        break;
                    case "constitution":
                        character.constitution += Mathf.FloorToInt((float)attribute.FindParameter("degree").value);
                        break;
                    case "intelligence":
                        character.intelligence += Mathf.FloorToInt((float)attribute.FindParameter("degree").value);
                        break;
                    case "wisdom":
                        character.wisdom += Mathf.FloorToInt((float)attribute.FindParameter("degree").value);
                        break;
                    case "luck":
                        character.luck += Mathf.FloorToInt((float)attribute.FindParameter("degree").value);
                        break;
                }
            }
        }
        character.soulGemActives.Add(SavedActiveAbility.ConvertFrom((ActiveAbility)selectedAbilities[0]));
        character.soulGemActives.Add(SavedActiveAbility.ConvertFrom((ActiveAbility)selectedAbilities[1]));
        character.soulGemActives.Add(SavedActiveAbility.ConvertFrom((ActiveAbility)selectedAbilities[2]));
        character.soulGemPassive = SavedPassiveAbility.ConvertFrom((PassiveAbility)selectedAbilities[3]);
        BinaryFormatter bf = new BinaryFormatter();
        if (!Directory.Exists(Application.persistentDataPath + "/characters")) Directory.CreateDirectory(Application.persistentDataPath + "/characters");
        FileStream file = File.Create(Application.persistentDataPath + "/characters/" + characterName + ".character");
        bf.Serialize(file, character);
        file.Close();
        FinishCharacterCreation();
        generatedAbilities = false;
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
            ability = AttackAbilityGenerator.Generate();
        }
        attackAbilities1.Add(ability);
    }

    private void GenerateCooldownAbility() {
        AttackAbility ability = null;
        while (ability == null || ability.cooldown == 0 || WrongStat(ability)) {
            ability = AttackAbilityGenerator.Generate();
        }
        attackAbilities2.Add(ability);
    }

    private void GenerateUtilityAbility() {
        UtilityAbility ability = null;
        while (ability == null) {
            ability = UtilityAbilityGenerator.Generate();
        }
        utilityAbilities.Add(ability);
    }

    private void GeneratePassiveAbility() {
        PassiveAbility ability = null;
        while (ability == null) {
            ability = PassiveAbilityGenerator.Generate();
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
        var ao = SceneManager.LoadSceneAsync("Lobby");
        LoadingProgressBar.StartLoad(ao, 0);
        characterByteArray = File.ReadAllBytes(Application.persistentDataPath + "/characters/" + selectedCharacterName + ".character");
    }

    public static void DeserializeCharacter(byte[] data) {
        if (data == null) {
            PlayerCharacter.players.Remove(fakeCharacter.GetComponent<PlayerCharacter>());
            PlayerCharacter.localPlayer = null;
            return;
        }
        if (fakeCharacter!=null && fakeCharacter.gameObject != null) Destroy(fakeCharacter.gameObject);
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
        strengthText.text = abilityScreenStrengthText.text = strength.ToString();
        dexterityText.text = abilityScreenDexterityText.text = dexterity.ToString();
        constitutionText.text = abilityScreenConstitutionText.text = constitution.ToString();
        intelligenceText.text = abilityScreenIntelligenceText.text = intelligence.ToString();
        wisdomText.text = abilityScreenWisdomText.text = wisdom.ToString();
        luckText.text = abilityScreenLuckText.text = luck.ToString();
        sparePointsText.text = sparePoints.ToString();
        CharacterAttribute.attributes["strength"].instances[fakeCharacter].BaseValue = strength;
        CharacterAttribute.attributes["dexterity"].instances[fakeCharacter].BaseValue = dexterity;
        CharacterAttribute.attributes["constitution"].instances[fakeCharacter].BaseValue = constitution;
        CharacterAttribute.attributes["intelligence"].instances[fakeCharacter].BaseValue = intelligence;
        CharacterAttribute.attributes["wisdom"].instances[fakeCharacter].BaseValue = wisdom;
        CharacterAttribute.attributes["luck"].instances[fakeCharacter].BaseValue = luck;
        hpText.text = GetStatAsString("bonusHp");
        hpRegenRateText.text = GetStatAsString("hpRegen") + " HP/sec.";
        receivedHealingBonusText.text = GetStatAsString("receivedHealing") + "%";
        armorBonusText.text = GetStatAsString("armorMultiplier") + "%";
        physicalResistText.text = GetStatAsString("physicalResistance") + "%";
        mentalResistText.text = GetStatAsString("mentalResistance") + "%";
        mpText.text = GetStatAsString("bonusMp");
        mpRegenRateText.text = GetStatAsString("mpRegen") + " MP/sec.";
        healingBonusText.text = GetStatAsString("healingMultiplier") + "%";
        moveSpeedText.text = GetStatAsString("moveSpeed") + "%";
        cooldownReductionText.text = GetStatAsString("cooldownReduction") + "%";
        criticalHitRateText.text = GetStatAsString("criticalHitChance") + "%";
        criticalDamageBonusText.text = GetStatAsString("criticalDamage") + "%";
        statusEffectDurationBonusText.text = GetStatAsString("statusEffectDuration") + "%";
        itemFindRateText.text = GetStatAsString("itemFindRate") + "%";
        elementalResistanceText.text = GetStatAsString("fireResistance") + "%";
    }

    private string GetStatAsString(string stat) {
        var number = CharacterAttribute.attributes[stat].instances[fakeCharacter].TotalValue;
        var rounded = (int)number;
        return rounded.ToString();
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
        SceneManager.LoadScene("Lobby");
    }

    public void PressDelete() {
        characterDeleteConfirmPanel.SetActive(true);
    }

    public void ActuallyPressDelete() {
        characterDeleteConfirmPanel.SetActive(false);
        File.Delete(Application.persistentDataPath + "/characters/" + selectedCharacterName + ".character");
        File.Delete(Application.persistentDataPath + "/worlds/" + selectedCharacterName + ".world");
        UpdateCharacterNameList();
        UpdateCharacters();
        kittenOuterModel.SetActive(false);
    }

    public void NoToDelete() {
        characterDeleteConfirmPanel.SetActive(false);
    }
}
