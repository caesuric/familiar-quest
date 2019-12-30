using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenu : MonoBehaviour {

    public GameObject abilityPane;
    public GameObject iconPrefab;
    public GameObject mouseOverPanel;
    public GameObject fusionPanel;
    public GameObject dustingPanel;
    public GameObject dustUsagePanel;
    public Sprite placeholder;
    public Text mouseOverTitle;
    public Text mouseOverDescription;
    public AbilityScreenIcon fusionSourceObj1;
    public AbilityScreenIcon fusionSourceObj2;
    public AbilityScreenIcon fusionResultObj;
    public ActiveAbility fusionSource1 = null;
    public ActiveAbility fusionSource2 = null;
    public ActiveAbility fusionResult = null;
    public int fusionSourceSlot1 = -1;
    public int fusionSourceSlot2 = -1;
    private List<GameObject> abilityIcons = new List<GameObject>();
    private int lastAbilityCount = 0;
    private List<Ability> abilities;
    private bool initialized = false;
    public int fusionAbilityTypeChoice = 0;
    public int fusionElementChoice = 0;
    public int fusionBaseStatChoice = 0;
    public int fusionDotChoice = 0;
    public bool fusionIsRangedChoice = false;
    public int fusionCooldownChoice = 0;
    public int fusionMpUsageChoice = 0;
    public int fusionRadiusChoice = 0;
    public GameObject fusionAbilityTypePane;
    public GameObject fusionElementPane;
    public Text fusionElementChoice1;
    public Text fusionElementChoice2;
    public GameObject fusionBaseStatPane;
    public Text fusionBaseStatChoice1;
    public Text fusionBaseStatChoice2;
    public GameObject fusionDamageDotSplitPane;
    public GameObject fusionIsRangedPane;
    public GameObject fusionCooldownPane;
    public Text fusionCooldownChoice1;
    public Text fusionCooldownChoice2;
    public GameObject fusionMpPane;
    public Text fusionMpChoice1;
    public Text fusionMpChoice2;
    public GameObject fusionRadiusPane;
    public Text fusionRadiusChoice1;
    public Text fusionRadiusChoice2;
    public List<AbilityAttribute> fusionAbilityAttributes = new List<AbilityAttribute>();
    public List<AbilityAttribute> fusionAbilityAttributesSelected = new List<AbilityAttribute>();
    public GameObject fusionAttributesList;
    public GameObject fusionAttributesSelectedList;
    public List<GameObject> fusionAttributeItems = new List<GameObject>();
    public List<GameObject> fusionSelectedAttributeItems = new List<GameObject>();
    public GameObject attributePrefab;
    public List<Ability> abilitiesToDust = new List<Ability>();
    public List<GameObject> dustAbilityObjects = new List<GameObject>();
    public GameObject dustingAbilityPane;
    public GameObject dustPrefab;
    public List<GameObject> dustItems = new List<GameObject>();
    public List<GameObject> dustToUseItems = new List<GameObject>();
    public Ability abilityToEnhance = null;
    public Ability enhancedAbility = null;
    public List<Dust> dustToUse = new List<Dust>();
    public GameObject dustToUsePane;
    public GameObject dustStoragePane;
    public AbilityScreenIcon abilityToEnhanceObj;
    public AbilityScreenIcon enhancedAbilityObj;
    public InputField filterInput;
    public static AbilityMenu instance = null;

    void Start() {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    // Update is called once per frame
    void Update() {
        if (!initialized && PlayerCharacter.players.Count > 0 && PlayerCharacter.localPlayer != null) {
            abilities = PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities;
            initialized = true;
        }
        if (initialized && lastAbilityCount != abilities.Count) {
            UpdateAbilities();
            RefreshDustPanel();
            RefreshDustUsagePanel();
        }
    }

    public void ResetFusionChoices() {
        fusionAbilityTypeChoice = 0;
        fusionElementChoice = 0;
        fusionBaseStatChoice = 0;
        fusionDotChoice = 0;
        fusionIsRangedChoice = false;
        fusionCooldownChoice = 0;
        fusionMpUsageChoice = 0;
        fusionRadiusChoice = 0;
        fusionAbilityTypePane.GetComponentInChildren<Slider>().value = 0;
        fusionElementPane.GetComponentInChildren<Slider>().value = 0;
        fusionBaseStatPane.GetComponentInChildren<Slider>().value = 0;
        fusionDamageDotSplitPane.GetComponentInChildren<Slider>().value = 0;
        fusionIsRangedPane.GetComponentInChildren<Slider>().value = 0;
        fusionCooldownPane.GetComponentInChildren<Slider>().value = 0;
        fusionMpPane.GetComponentInChildren<Slider>().value = 0;
        fusionRadiusPane.GetComponentInChildren<Slider>().value = 0;
        UpdateFusionChoicesMenu();
    }

    public void UpdateAbilities() {
        foreach (var icon in abilityIcons) Destroy(icon);
        abilityIcons.RemoveRange(0, abilityIcons.Count);
        foreach (var ability in abilities) {
            if (FilteredOut(ability)) continue;
            var go = Instantiate(iconPrefab);
            abilityIcons.Add(go);
            go.transform.SetParent(abilityPane.transform);
            go.GetComponent<AbilityScreenIcon>().Initialize(ability);
        }
        lastAbilityCount = abilities.Count;
        fusionSourceObj1.Initialize(fusionSource1);
        fusionSourceObj2.Initialize(fusionSource2);
        fusionResultObj.Initialize(fusionResult);
        fusionSourceObj1.draggable = false;
        fusionSourceObj2.draggable = false;
        fusionResultObj.draggable = false;
        UpdateEnhancementResult();
        enhancedAbilityObj.Initialize(enhancedAbility);
    }

    public void Fuse() {
        if (!ValidFusionSources()) return;
        var su = PlayerCharacter.localPlayer.GetComponent<SpiritUser>();
        if (fusionSourceSlot1 != -1) fusionSourceSlot1 = su.spirits[0].activeAbilities.IndexOf(fusionSource1);
        if (fusionSourceSlot2 != -1) fusionSourceSlot2 = su.spirits[0].activeAbilities.IndexOf(fusionSource2);
        if (fusionSourceSlot1 == -1) su.overflowAbilities.Remove(fusionSource1);
        if (fusionSourceSlot1 > -1) su.spirits[0].activeAbilities[fusionSourceSlot1] = null;
        if (fusionSourceSlot2 == -1) su.overflowAbilities.Remove(fusionSource2);
        if (fusionSourceSlot2 > -1) su.spirits[0].activeAbilities[fusionSourceSlot2] = null;
        su.overflowAbilities.Add(fusionResult);
        UpdateAbilities();
        PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        fusionSource1 = null;
        fusionSource2 = null;
        fusionResult = null;
        fusionSourceObj1.ability = null;
        fusionSourceObj2.ability = null;
        fusionResultObj.ability = null;
        fusionSourceObj1.GetComponent<Image>().sprite = placeholder;
        fusionSourceObj2.GetComponent<Image>().sprite = placeholder;
        fusionResultObj.GetComponent<Image>().sprite = placeholder;
    }

    private bool ValidFusionSources() {
        var su = PlayerCharacter.localPlayer.GetComponent<SpiritUser>();
        if (fusionSource1 == null || fusionSource2 == null) return false;
        if (fusionSourceSlot1 == -1 && !su.overflowAbilities.Contains(fusionSource1)) return false;
        if (fusionSourceSlot1 > -1 && !su.spirits[0].activeAbilities.Contains(fusionSource1)) return false;
        if (fusionSourceSlot2 == -1 && !su.overflowAbilities.Contains(fusionSource2)) return false;
        if (fusionSourceSlot2 > -1 && !su.spirits[0].activeAbilities.Contains(fusionSource2)) return false;
        return true;
    }

    public void ToggleFusionPanel() {
        fusionPanel.SetActive(!fusionPanel.activeSelf);
        if (fusionPanel.activeSelf) {
            dustingPanel.SetActive(false);
            dustUsagePanel.SetActive(false);
        }
    }

    public void ToggleDustingPanel() {
        dustingPanel.SetActive(!dustingPanel.activeSelf);
        if (dustingPanel.activeSelf) {
            fusionPanel.SetActive(false);
            dustUsagePanel.SetActive(false);
        }
    }

    public void ToggleDustUsagePanel() {
        dustUsagePanel.SetActive(!dustUsagePanel.activeSelf);
        if (dustUsagePanel.activeSelf) {
            fusionPanel.SetActive(false);
            dustingPanel.SetActive(false);
        }
    }

    public void UpdateFusionResult() {
        FusionDropSlot.instance.UpdateFusionChoices();
    }

    public void UpdateFusionChoicesMenu() {
        if (!ValidFusionSources()) {
            fusionAbilityTypePane.SetActive(false);
            fusionElementPane.SetActive(false);
            fusionBaseStatPane.SetActive(false);
            fusionDamageDotSplitPane.SetActive(false);
            fusionIsRangedPane.SetActive(false);
            fusionCooldownPane.SetActive(false);
            fusionMpPane.SetActive(false);
            fusionRadiusPane.SetActive(false);
            return;
        }
        if (fusionSource1 is AttackAbility && fusionSource2 is AttackAbility) fusionAbilityTypePane.SetActive(false);
        else if (fusionSource1 is UtilityAbility && fusionSource2 is UtilityAbility) {
            fusionAbilityTypeChoice = 1;
            fusionAbilityTypePane.SetActive(false);
        }
        else fusionAbilityTypePane.SetActive(true);
        if (fusionSource1 is AttackAbility && fusionSource2 is UtilityAbility) fusionElementPane.SetActive(false);
        else if (fusionSource1 is UtilityAbility && fusionSource2 is AttackAbility) {
            fusionElementChoice = 1;
            fusionElementPane.SetActive(false);
        }
        else if (fusionSource1 is AttackAbility && fusionSource2 is AttackAbility && ((AttackAbility)fusionSource1).element != ((AttackAbility)fusionSource2).element) {
            fusionElementPane.SetActive(true);
            fusionElementChoice1.text = ((AttackAbility)fusionSource1).element.ToString();
            fusionElementChoice2.text = ((AttackAbility)fusionSource2).element.ToString();
        }
        else fusionElementPane.SetActive(false);
        if (fusionSource1.baseStat != fusionSource2.baseStat) {
            fusionBaseStatPane.SetActive(true);
            fusionBaseStatChoice1.text = fusionSource1.baseStat.ToString();
            fusionBaseStatChoice2.text = fusionSource2.baseStat.ToString();
        }
        else fusionBaseStatPane.SetActive(false);
        if (fusionSource1 is AttackAbility && fusionSource2 is UtilityAbility) fusionDamageDotSplitPane.SetActive(false);
        else if (fusionSource1 is UtilityAbility && fusionSource2 is AttackAbility) {
            fusionDotChoice = 1;
            fusionDamageDotSplitPane.SetActive(false);
        }
        else if (fusionSource1 is AttackAbility && fusionSource2 is AttackAbility && !DotSplitSame((AttackAbility)fusionSource1, (AttackAbility)fusionSource2)) fusionDamageDotSplitPane.SetActive(true);
        else fusionDamageDotSplitPane.SetActive(false);
        if (fusionSource1 is AttackAbility && fusionSource2 is UtilityAbility) {
            fusionIsRangedChoice = ((AttackAbility)fusionSource1).isRanged;
            fusionIsRangedPane.SetActive(false);
        }
        else if (fusionSource1 is UtilityAbility && fusionSource2 is AttackAbility) {
            fusionIsRangedChoice = ((AttackAbility)fusionSource2).isRanged;
            fusionIsRangedPane.SetActive(false);
        }
        else if (fusionSource1 is AttackAbility && fusionSource2 is AttackAbility && ((AttackAbility)fusionSource1).isRanged != ((AttackAbility)fusionSource2).isRanged) fusionIsRangedPane.SetActive(true);
        else if (fusionSource1 is AttackAbility && fusionSource2 is AttackAbility) {
            fusionIsRangedChoice = ((AttackAbility)fusionSource1).isRanged;
            fusionIsRangedPane.SetActive(false);
        }
        else fusionDamageDotSplitPane.SetActive(false);
        if (fusionSource1.cooldown != fusionSource2.cooldown) {
            fusionCooldownPane.SetActive(true);
            fusionCooldownChoice1.text = fusionSource1.cooldown.ToString() + "s";
            fusionCooldownChoice2.text = fusionSource2.cooldown.ToString() + "s";
        }
        else fusionCooldownPane.SetActive(false);
        if (fusionSource1.mpUsage != fusionSource2.mpUsage) {
            fusionMpPane.SetActive(true);
            fusionMpChoice1.text = fusionSource1.mpUsage.ToString();
            fusionMpChoice2.text = fusionSource2.mpUsage.ToString();
        }
        else fusionMpPane.SetActive(false);

        if (fusionSource1 is AttackAbility && fusionSource2 is UtilityAbility) fusionRadiusPane.SetActive(false);
        else if (fusionSource1 is UtilityAbility && fusionSource2 is AttackAbility) {
            fusionRadiusChoice = 1;
            fusionRadiusPane.SetActive(false);
        }
        else if (fusionSource1 is AttackAbility && fusionSource2 is AttackAbility && ((AttackAbility)fusionSource1).radius != ((AttackAbility)fusionSource2).radius) {
            fusionRadiusPane.SetActive(true);
            fusionRadiusChoice1.text = ((AttackAbility)fusionSource1).radius.ToString();
            fusionRadiusChoice2.text = ((AttackAbility)fusionSource2).radius.ToString();
        }
        else fusionRadiusPane.SetActive(false);
        UpdateFusionAbilityAttributes();
    }

    public void UpdateFusionAbilityAttributes() {
        fusionAbilityAttributes.Clear();
        fusionAbilityAttributesSelected.Clear();
        foreach (var attribute in fusionSource1.attributes) fusionAbilityAttributes.Add(attribute.Copy());
        foreach (var attribute in fusionSource2.attributes) fusionAbilityAttributes.Add(attribute.Copy());
        UpdateFusionAttributeLists();
    }

    public void UpdateFusionAttributeLists() {
        foreach (var attribute in fusionAttributeItems) Destroy(attribute);
        foreach (var attribute in fusionSelectedAttributeItems) Destroy(attribute);
        fusionAttributeItems.Clear();
        fusionSelectedAttributeItems.Clear();
        foreach (var attribute in fusionAbilityAttributes) {
            var go = Instantiate(attributePrefab);
            fusionAttributeItems.Add(go);
            go.transform.SetParent(fusionAttributesList.transform);
            go.GetComponent<FusionAttributeItem>().Initialize(attribute);
        }
        foreach (var attribute in fusionAbilityAttributesSelected) {
            var go = Instantiate(attributePrefab);
            fusionSelectedAttributeItems.Add(go);
            go.transform.SetParent(fusionAttributesSelectedList.transform);
            go.GetComponent<FusionAttributeItem>().Initialize(attribute);
        }
    }

    private static bool DotSplitSame(AttackAbility ability1, AttackAbility ability2) {
        var damage1 = ability1.damage;
        var damage2 = ability2.damage;
        var dotDamage1 = ability1.dotDamage;
        var dotDamage2 = ability2.dotDamage;
        if (damage1 / (damage1 + dotDamage1) != damage2 / (damage2 + dotDamage2)) return false;
        return true;
    }

    public void FusionAbilityTypeSliderChange() {
        fusionAbilityTypeChoice = (int)fusionAbilityTypePane.GetComponentInChildren<Slider>().value;
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void FusionElementSliderChange() {
        fusionElementChoice = (int)fusionElementPane.GetComponentInChildren<Slider>().value;
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void FusionBaseStatSliderChange() {
        fusionBaseStatChoice = (int)fusionBaseStatPane.GetComponentInChildren<Slider>().value;
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void FusionDamageDotSplitSliderChange() {
        fusionDotChoice = (int)fusionDamageDotSplitPane.GetComponentInChildren<Slider>().value;
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void FusionIsRangedSliderChange() {
        var isRangedChoice = (int)fusionIsRangedPane.GetComponentInChildren<Slider>().value;
        fusionIsRangedChoice = (isRangedChoice == 1);
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void FusionCooldownSliderChange() {
        fusionCooldownChoice = (int)fusionCooldownPane.GetComponentInChildren<Slider>().value;
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void FusionMpSliderChange() {
        fusionMpUsageChoice = (int)fusionMpPane.GetComponentInChildren<Slider>().value;
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void FusionRadiusSliderChange() {
        fusionRadiusChoice = (int)fusionRadiusPane.GetComponentInChildren<Slider>().value;
        FusionDropSlot.instance.FusionSettingsUpdated();
    }

    public void RefreshDustPanel() {
        foreach (var icon in dustAbilityObjects) Destroy(icon);
        dustAbilityObjects.Clear();
        foreach (var ability in abilitiesToDust) {
            var go = Instantiate(iconPrefab);
            dustAbilityObjects.Add(go);
            go.transform.SetParent(dustingAbilityPane.transform);
            go.GetComponent<AbilityScreenIcon>().Initialize(ability);
        }
    }

    public void DustAbilities() {
        foreach (var ability in abilitiesToDust) {
            var activeAbilities = PlayerCharacter.localPlayer.GetComponent<SpiritUser>().spirits[0].activeAbilities;
            if (ability is ActiveAbility && activeAbilities.Contains((ActiveAbility)ability)) {
                activeAbilities[activeAbilities.IndexOf((ActiveAbility)ability)] = null;
                PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
            }
            else if (abilities.Contains(ability)) {
                abilities.Remove(ability);
                UpdateAbilities();
            }
            MakeDust(ability);
        }
        abilitiesToDust.Clear();
        RefreshDustPanel();
        RefreshDustUsagePanel();
    }

    public void MakeDust(Ability ability) {
        if (ability.attributes.Count == 0) MakePowerDust(ability);
        else {
            int chosenNum = Random.Range(0, ability.attributes.Count);
            var attribute = ability.attributes[chosenNum];
            var degree = attribute.FindParameter("degree");
            if (degree != null && degree.floatVal != 0) AddDust(attribute.type, degree.floatVal);
            else if (degree != null && degree.intVal != 0) AddDust(attribute.type, degree.intVal);
            else AddDust(attribute.type, 1f);
        }
    }

    public void MakePowerDust(Ability ability) {
        AddDust("power", ((float)ability.points) / 20f);
    }

    public void AddDust(string type, float amount) {
        foreach (var dustItem in PlayerCharacter.localPlayer.GetComponent<DustUser>().dust) {
            if (dustItem.type == type) {
                dustItem.quantity += amount;
                return;
            }
        }
        PlayerCharacter.localPlayer.GetComponent<DustUser>().dust.Add(new Dust(type, amount));
    }

    public void RefreshDustUsagePanel() {
        foreach (var icon in dustItems) Destroy(icon);
        foreach (var icon in dustToUseItems) Destroy(icon);
        dustItems.Clear();
        dustToUseItems.Clear();
        foreach (var dust in PlayerCharacter.localPlayer.GetComponent<DustUser>().dust) {
            var go = Instantiate(dustPrefab);
            if (dustToUse.Contains(dust)) {
                dustToUseItems.Add(go);
                go.transform.SetParent(dustToUsePane.transform);
            }
            else {
                dustItems.Add(go);
                go.transform.SetParent(dustStoragePane.transform);
            }
            go.GetComponent<DustItem>().Initialize(dust);
        }
    }

    public void UpdateEnhancementResult() {
        if (abilityToEnhance == null) return;
        enhancedAbility = abilityToEnhance.Copy();
        ApplyEnhancements();
    }

    public void ApplyEnhancements() {
        if (enhancedAbility is ActiveAbility) ActiveAbility.Enhance((ActiveAbility)enhancedAbility, dustToUse);
    }

    public void Enhance() {
        if (abilityToEnhance == null || enhancedAbility == null) return;
        if (abilityToEnhance is PassiveAbility) return;
        var su = PlayerCharacter.localPlayer.GetComponent<SpiritUser>();
        if (su.spirits[0].activeAbilities.Contains((ActiveAbility)abilityToEnhance)) {
            su.spirits[0].activeAbilities[su.spirits[0].activeAbilities.IndexOf((ActiveAbility)abilityToEnhance)] = (ActiveAbility)enhancedAbility;
            PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
        else if (abilities.Contains(abilityToEnhance)) {
            abilities[abilities.IndexOf(abilityToEnhance)] = enhancedAbility;
        }
        abilityToEnhance = null;
        enhancedAbility = null;
        abilityToEnhanceObj.GetComponent<Image>().sprite = placeholder;
        enhancedAbilityObj.GetComponent<Image>().sprite = placeholder;
        UpdateAbilities();
    }

    public void ClearFusionSlot1() {
        fusionSource1 = null;
        fusionResult = null;
        fusionSourceObj1.ability = null;
        fusionResultObj.ability = null;
        fusionSourceObj1.GetComponent<Image>().sprite = placeholder;
        fusionResultObj.GetComponent<Image>().sprite = placeholder;
        UpdateAbilities();
        UpdateFusionChoicesMenu();
    }

    public void ClearFusionSlot2() {
        fusionSource2 = null;
        fusionResult = null;
        fusionSourceObj2.ability = null;
        fusionResultObj.ability = null;
        fusionSourceObj2.GetComponent<Image>().sprite = placeholder;
        fusionResultObj.GetComponent<Image>().sprite = placeholder;
        UpdateAbilities();
        UpdateFusionChoicesMenu();
    }

    public bool FilteredOut(Ability ability) {
        var filterText = filterInput.text.ToUpper();
        if (filterText == null || filterText == "") return false;
        if (!ability.description.ToUpper().Contains(filterText)) return true;
        return false;
    }
}
