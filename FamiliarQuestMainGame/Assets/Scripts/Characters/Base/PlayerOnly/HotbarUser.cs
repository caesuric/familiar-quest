﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
using DuloGames.UI;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PlayerCharacter))]
[RequireComponent(typeof(Mana))]
[RequireComponent(typeof(AbilityUser))]
[RequireComponent(typeof(CacheGrabber))]
public class HotbarUser : MonoBehaviour {

    //[SyncVar]
    //public float GCDTime = 0.0f; //seems to have been deprecated in favor of AbilityUser.GCDTime
    //public SyncListInt abilityIcons = new SyncListInt();
    //public SyncListFloat abilityCurrentCDs = new SyncListFloat();
    //public SyncListFloat abilityCDs = new SyncListFloat();
    //public SyncListBool abilityIsRanged = new SyncListBool();
    //public SyncListBool abilityIsAttack = new SyncListBool();
    //public SyncListFloat abilityRadii = new SyncListFloat();
    //public SyncListString abilityNames = new SyncListString();
    //public SyncListString abilityDescriptions = new SyncListString();
    //public SyncListInt consumableCounts = new SyncListInt();
    //public SyncListString spiritNames = new SyncListString();
    public List<int> abilityIcons = new List<int>();
    public List<float> abilityCurrentCDs = new List<float>();
    public List<float> abilityCDs = new List<float>();
    public List<bool> abilityIsRanged = new List<bool>();
    public List<bool> abilityIsAttack = new List<bool>();
    public List<float> abilityRadii = new List<float>();
    public List<string> abilityNames = new List<string>();
    public List<string> abilityDescriptions = new List<string>();
    public List<int> consumableCounts = new List<int>();
    public List<string> spiritNames = new List<string>();

    public List<GameObject> hotbarButtons = new List<GameObject>();
    public List<float> currentCooldownPercentages = new List<float>();
    public bool ready = false;

    private static Dictionary<string, Sprite> images = new Dictionary<string, Sprite>();

    // Use this for initialization
    void Start() {
        if (images.Count == 0) {
            images.Clear();
            var imagesTemp = Resources.LoadAll<Sprite>("Icons");
            foreach (var image in imagesTemp) {
                images[image.name] = image;
            }
        }
        var objs = GameObject.FindGameObjectsWithTag("HotbarButton");
        foreach (var obj in objs) hotbarButtons.Add(null);
        foreach (var obj in objs) AddHotbarButtonToList(obj);
    }

    // Update is called once per frame
    void Update() {
        if (hotbarButtons == null || hotbarButtons.Count == 0 || hotbarButtons[0] == null) {
            CmdEmptyAbilityIcons();
            hotbarButtons.RemoveRange(0, hotbarButtons.Count);
            var objs = GameObject.FindGameObjectsWithTag("HotbarButton");
            foreach (var obj in objs) hotbarButtons.Add(null);
            foreach (var obj in objs) AddHotbarButtonToList(obj);
            //if (isLocalPlayer) CmdRefreshAbilityInfo();
            CmdRefreshAbilityInfo();
        }
        if (!ready && GetComponent<CacheGrabber>().iconCache!=null && GetComponent<CacheGrabber>().iconCache.Count > 0 && GetComponent<AbilityUser>().soulGemActives.Count > 0) {
            hotbarButtons[0].GetComponent<MouseOverHotbarButton>().image.sprite = images[GetComponent<PlayerCharacter>().weapon.icon];
            hotbarButtons[0].GetComponent<MouseOverHotbarButton>().image.gameObject.SetActive(true);
            //if (isLocalPlayer) CmdRefreshAbilityInfo();
            CmdRefreshAbilityInfo();
            ready = true;
        }
        var pc = GetComponent<PlayerCharacter>();
        UpdateCooldowns(); //if (NetworkServer.active) UpdateCooldowns();
        if (pc.isMe && hotbarButtons.Count > 0) {
            if (abilityCurrentCDs.Count != currentCooldownPercentages.Count) {
                currentCooldownPercentages.Clear();
                for (var i = 0; i < abilityCurrentCDs.Count; i++) FullUpdateAbilityCD(i);
            }
            else for (var i = 0; i < abilityCurrentCDs.Count; i++) PartialUpdateAbilityCD(i);
            for (int i = 0; i < abilityIcons.Count; i++) {
                if (hotbarButtons[i] == null) continue;
                if (abilityIcons[i] == 65) {
                    hotbarButtons[i].GetComponent<MouseOverHotbarButton>().image.gameObject.SetActive(false);
                    continue;
                }
                hotbarButtons[i].GetComponent<MouseOverHotbarButton>().image.sprite = GetComponent<CacheGrabber>().iconCache[abilityIcons[i]];
                hotbarButtons[i].GetComponent<MouseOverHotbarButton>().image.gameObject.SetActive(true);
            }
        }
    }

    private void FullUpdateAbilityCD(int i) {
        if (abilityCurrentCDs.Count > i && abilityCDs.Count > i && abilityCurrentCDs[i] == -1) currentCooldownPercentages.Add(1);
        else if (abilityCurrentCDs.Count > i && abilityCDs.Count > i) currentCooldownPercentages.Add(Mathf.Max(abilityCurrentCDs[i] / abilityCDs[i], GetComponent<AbilityUser>().GCDTime / AbilityUser.maxGCDTime));
    }

    private void PartialUpdateAbilityCD(int i) {
        if (abilityCurrentCDs[i] == -1) currentCooldownPercentages[i] = 1;
        else currentCooldownPercentages[i] = Mathf.Max(abilityCurrentCDs[i] / abilityCDs[i], GetComponent<AbilityUser>().GCDTime / AbilityUser.maxGCDTime);
    }

    private void UpdateCooldowns() {
        //if (abilityCurrentCDs.Count < 1) abilityCurrentCDs.Add(0);
        //abilityCurrentCDs[0] = 0;
        int number = -1;
        foreach (var ability in GetComponent<AbilityUser>().soulGemActives) {
            number++;
            if (ability == null) abilityCurrentCDs[number] = -1;
            else if (ability.mpUsage > GetComponent<Mana>().mp) abilityCurrentCDs[number] = -1;
            //else if (ability.FindAttribute("stealth") != null && MonsterCombatant.AnyInCombat()) abilityCurrentCDs[number] = -1;
            else abilityCurrentCDs[number] = ability.currentCooldown;
        }
    }

    private void AddHotbarButtonToList(GameObject obj) {
        var numberString = obj.name.Substring(2);
        int number;
        try {
            number = int.Parse(numberString) - 1;
            hotbarButtons[number] = obj;
        }
        catch {
            // pass
        }
    }

    public void UseHotbarAbility(int deprecated, int abilitySelected) {
        var ability = GetComponent<AbilityUser>().soulGemActives[abilitySelected];
        if (ability != null) GetComponent<AbilityUser>().UseAbility(ability);
    }

    public void UseItem(int number) {
        if (GetComponent<PlayerCharacter>().consumables.Count > number) {
            GetComponent<AbilityUser>().GCDTime = AbilityUser.maxGCDTime;
            var item = GetComponent<PlayerCharacter>().consumables[number];
            if (item == null) return;
            if (item.quantity > 0) {
                if (item.type == ConsumableType.health && GetComponent<Health>().hp < GetComponent<Health>().maxHP) UseHealthPotion(item);
                else if (item.type == ConsumableType.mana && GetComponent<Mana>().mp < GetComponent<Mana>().maxMP) UseManaPotion(item);
                if (item.quantity == 0) GetComponent<PlayerCharacter>().consumables[number] = null;
                CmdRefreshAbilityInfo();
            }
        }
    }

    private void UseHealthPotion(Consumable item) {
        if (item.quantity <= 0) return;
        GetComponent<Health>().hp = Mathf.Min(GetComponent<Health>().hp + item.degree, GetComponent<Health>().maxHP);
        item.quantity -= 1;
    }

    private void UseManaPotion(Consumable item) {
        if (item.quantity <= 0) return;
        GetComponent<Mana>().mp = Mathf.Min(GetComponent<Mana>().mp + item.degree, GetComponent<Mana>().maxMP);
        item.quantity -= 1;
    }

    public void UpdateWeapon() {
        hotbarButtons[0].GetComponent<MouseOverHotbarButton>().image.sprite = images[GetComponent<PlayerCharacter>().weapon.icon];
        hotbarButtons[0].GetComponent<MouseOverHotbarButton>().image.gameObject.SetActive(true);
    }

    //[Command]
    public void CmdEmptyAbilityIcons() {
        while (abilityIcons.Count > 0) abilityIcons.RemoveAt(0);
    }

    //[Command]
    public void CmdRefreshAbilityInfo() {
        ClearAbilityLists();
        //AddWeaponAbilityData();
        //SortAbilities();
        PutAbilitiesIntoOverflow();
        var au = GetComponent<AbilityUser>();
        //foreach (var spirit in su.spirits) foreach (var ability in spirit.activeAbilities) AddAbilityInfo(ability);
        foreach (var ability in au.soulGemActives) AddAbilityInfo(ability);
        AddAbilityBlankPadding(10);
        SortConsumables();
        foreach (var consumable in GetComponent<PlayerCharacter>().consumables) AddHotbarConsumable(consumable);
        AddAbilityBlankPadding(13);
        AddPassiveInfo(au.soulGemPassive);
        AddTooltipData();
        SkillTreeDropSlot.UpdateAbility();
        AbilityScreenIcon.UpdateAbilities();
        //RefreshSpiritInfo();
    }

    private void AddTooltipData() {
        for (int i=0; i<hotbarButtons.Count; i++) {
            if (hotbarButtons[i] == null) continue;
            if (abilityNames.Count <= i) continue;
            var tooltip = hotbarButtons[i].GetComponent<UITooltipShow>();
            tooltip.contentLines = new UITooltipLineContent[2];
            tooltip.contentLines[0] = new UITooltipLineContent {
                LineStyle = UITooltipLines.LineStyle.Title,
                Content = abilityNames[i]
            };
            tooltip.contentLines[1] = new UITooltipLineContent {
                LineStyle = UITooltipLines.LineStyle.Description,
                Content = abilityDescriptions[i]
            };
        }
    }

    private void PutAbilitiesIntoOverflow() {
        var au = GetComponent<AbilityUser>();
        if (au!=null) {
            var abilities = au.soulGemActives;
            var overflowAbilities = au.soulGemActivesOverflow;
            while (abilities.Count > 10 && HasNullSpots(abilities)) MoveToNullSpots(abilities);
            if (abilities.Count > 10) {
                for (int i = 10; i < abilities.Count; i++) overflowAbilities.Add(abilities[i]);
                abilities.RemoveRange(10, abilities.Count - 10);
            }
        }
        
        //while (abilities.Count < 8) abilities.Add(null);
    }

    private void MoveToNullSpots(List<ActiveAbility> abilities) {
        for (int i = 8; i < abilities.Count; i++) {
            var ability = abilities[i];
            for (int j = 0; j < 8; j++) {
                if (abilities[j] == null) {
                    abilities[j] = ability;
                    abilities.RemoveAt(i);
                    return;
                }
            }
        }
    }

    private bool HasNullSpots(List<ActiveAbility> abilities) {
        for (int i = 0; i < 8; i++) if (abilities[i] == null) return true;
        return false;
    }

    private void SortAbilities() {
        var abilities = GetComponent<AbilityUser>().soulGemActives;
        var newAbilitiesCooldowns = new List<ActiveAbility>();
        var newAbilitiesConstant = new List<ActiveAbility>();
        var newAbilitiesAll = new List<ActiveAbility>();
        foreach (var ability in abilities) {
            if (ability != null) {
                if (ability.cooldown == 0) newAbilitiesConstant.Add(ability);
                else newAbilitiesCooldowns.Add(ability);
            }
        }
        for (int i = 0; i < 4 && i < newAbilitiesCooldowns.Count; i++) newAbilitiesAll.Add(newAbilitiesCooldowns[i]);
        while (newAbilitiesAll.Count < 4) newAbilitiesAll.Add(null);
        for (int i = 0; i < 4 && i < newAbilitiesConstant.Count; i++) newAbilitiesAll.Add(newAbilitiesConstant[i]);
        while (newAbilitiesAll.Count < 8) newAbilitiesAll.Add(null);
        GetComponent<AbilityUser>().soulGemActives = newAbilitiesAll;
    }

    private void SortConsumables() {
        var consumables = GetComponent<PlayerCharacter>().consumables;
        var newConsumables = new List<Consumable>();
        foreach (var consumable in consumables) if (consumable!=null && consumable.type == ConsumableType.health) newConsumables.Add(consumable);
        if (newConsumables.Count < 1) newConsumables.Add(null);
        foreach (var consumable in consumables) if (consumable != null && consumable.type == ConsumableType.mana) newConsumables.Add(consumable);
        if (newConsumables.Count < 2) newConsumables.Add(null);
        GetComponent<PlayerCharacter>().consumables = newConsumables;
    }

    private void ClearAbilityLists() {
        abilityNames.Clear();
        abilityIcons.Clear();
        abilityCDs.Clear();
        abilityDescriptions.Clear();
        abilityIsRanged.Clear();
        abilityIsAttack.Clear();
        abilityRadii.Clear();
        abilityCurrentCDs.Clear();
        consumableCounts.Clear();
    }

    private void AddWeaponAbilityData() {
        abilityNames.Add("Attack");
        abilityIcons.Add(0);
        abilityCDs.Add(0);
        abilityCurrentCDs.Add(0);
        abilityDescriptions.Add("Attack with your weapon.");
        abilityIsRanged.Add(GetComponent<PlayerCharacter>().weapon is RangedWeapon);
        abilityIsAttack.Add(true);
        abilityRadii.Add(0);
    }

    private void AddAbilityInfo(ActiveAbility ability) {
        if (ability == null) {
            abilityNames.Add("");
            abilityIcons.Add(65);
            abilityCDs.Add(0);
            abilityDescriptions.Add("");
            abilityIsRanged.Add(false);
            abilityIsAttack.Add(false);
            abilityRadii.Add(0);
            abilityCurrentCDs.Add(0);
        }
        else {
            abilityNames.Add(ability.name);
            abilityIcons.Add(ability.icon);
            abilityCDs.Add(ability.cooldown);
            abilityDescriptions.Add(AbilityInterpolate(ability));
            abilityIsRanged.Add(ability is AttackAbility && ((AttackAbility)ability).isRanged);
            abilityIsAttack.Add(ability is AttackAbility);
            if (ability is AttackAbility) abilityRadii.Add(((AttackAbility)ability).radius);
            else if (ability.FindAttribute("disableDevice") != null) abilityRadii.Add(2);
            else abilityRadii.Add(0);
            abilityCurrentCDs.Add(0);
        }
    }

    private void AddPassiveInfo(PassiveAbility ability) {
        if (ability == null) {
            abilityNames[12] = "";
            abilityIcons[12] = 65;
            abilityCDs[12] = 0;
            abilityDescriptions[12] = "";
            abilityIsRanged[12] = false;
            abilityIsAttack[12] = false;
            abilityRadii[12] = 0;
            abilityCurrentCDs[12] = 0;
        }
        else {
            abilityNames[12] = ability.name;
            abilityIcons[12] = ability.icon;
            abilityCDs[12] = 0;
            abilityDescriptions[12] = ability.description;
            abilityIsRanged[12] = false;
            abilityIsAttack[12] = false;
            abilityRadii[12] = 0;
            abilityCurrentCDs[12] = 0;
        }
    }

    private void AddAbilityBlankPadding(int count) {
        while (abilityNames.Count < count) {
            abilityNames.Add("");
            abilityIcons.Add(65);
            abilityCDs.Add(0);
            abilityCurrentCDs.Add(0);
            abilityDescriptions.Add("");
            abilityIsRanged.Add(false);
            abilityIsAttack.Add(false);
            abilityRadii.Add(0);
        }
    }

    private void AddHotbarConsumable(Consumable consumable) {
        if (consumable == null) {
            abilityIcons.Add(65);
            abilityNames.Add("");
            abilityDescriptions.Add("");
            abilityCDs.Add(0);
            abilityIsRanged.Add(false);
            abilityIsAttack.Add(false);
            abilityRadii.Add(0);
            abilityCurrentCDs.Add(0);
            consumableCounts.Add(0);
        }
        else {
            switch (consumable.type) {
                case ConsumableType.health:
                    AddHealthConsumable(consumable);
                    break;
                case ConsumableType.mana:
                    AddManaConsumable(consumable);
                    break;
            }
        }
    }

    private void AddHealthConsumable(Consumable consumable) {
        abilityIcons.Add(58);
        abilityNames.Add("Health Potion");
        abilityDescriptions.Add("Restores 100 health.");
        AddGenericConsumableInfo(consumable);
    }

    private void AddManaConsumable(Consumable consumable) {
        abilityIcons.Add(59);
        abilityNames.Add("Mana Potion");
        abilityDescriptions.Add("Restores 100 MP.");
        AddGenericConsumableInfo(consumable);
    }

    private void AddGenericConsumableInfo(Consumable consumable) {
        abilityCDs.Add(0);
        abilityIsRanged.Add(false);
        abilityIsAttack.Add(false);
        abilityRadii.Add(0);
        abilityCurrentCDs.Add(0);
        consumableCounts.Add(consumable.quantity);
    }

    private string AbilityInterpolate(ActiveAbility ability) {
        var text = ability.description;
        text = text.Replace("{{baseStat}}", ability.baseStat.ToString());
        if (ability is AttackAbility) text = AttackAbilityInterpolate(text, ability);
        text = UtilityAbilityInterpolate(text, ability);
        return text;
    }

    private string AttackAbilityInterpolate(string text, ActiveAbility ability) {
        Dictionary<BaseStat, int> lookups = new Dictionary<BaseStat, int>() {
            //{ BaseStat.strength, GetComponent<Character>().strength},
            //{ BaseStat.dexterity, GetComponent<Character>().dexterity},
            //{ BaseStat.intelligence, GetComponent<Character>().intelligence},
            { BaseStat.strength, (int)CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].TotalValue },
            { BaseStat.dexterity, (int)CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].TotalValue },
            { BaseStat.intelligence, (int)CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].TotalValue }
        };
        int baseAttributeScore = lookups[((AttackAbility)ability).baseStat];
        text = text.Replace("{{damage}}", (GetAttackText(ability, baseAttributeScore)));
        text = text.Replace("{{blunting}}", GetBluntingText(ability, baseAttributeScore));
        text = text.Replace("{{shield}}", GetShieldText(ability, ((AttackAbility)ability).baseStat));
        return text.Replace("{{dotDamage}}", (Mathf.Floor(GetComponent<PlayerCharacter>().weapon.attackPower * baseAttributeScore * ((AttackAbility)ability).dotDamage).ToString()));
    }

    private string UtilityAbilityInterpolate(string text, ActiveAbility ability) {
        Dictionary<BaseStat, int> lookups = new Dictionary<BaseStat, int>() {
                //{ BaseStat.strength, GetComponent<Character>().strength},
                //{ BaseStat.dexterity, GetComponent<Character>().dexterity},
                //{ BaseStat.constitution, GetComponent<Character>().constitution},
                //{ BaseStat.intelligence, GetComponent<Character>().intelligence},
                //{ BaseStat.wisdom, GetComponent<Character>().wisdom},
                //{ BaseStat.luck, GetComponent<Character>().luck},
                { BaseStat.strength, (int)CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].TotalValue },
                { BaseStat.dexterity, (int)CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].TotalValue },
                { BaseStat.intelligence, (int)CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].TotalValue },
                { BaseStat.constitution, (int)CharacterAttribute.attributes["constitution"].instances[GetComponent<Character>()].TotalValue },
                { BaseStat.wisdom, (int)CharacterAttribute.attributes["wisdom"].instances[GetComponent<Character>()].TotalValue },
                { BaseStat.luck, (int)CharacterAttribute.attributes["luck"].instances[GetComponent<Character>()].TotalValue }
        };
        int baseAttributeScore = lookups[ability.baseStat];
        text = text.Replace("{{healing}}", GetHealingText(ability, baseAttributeScore));
        text = text.Replace("{{shield}}", GetShieldText(ability, ability.baseStat));
        text = text.Replace("{{restoreMP}}", GetRestoreMpText(ability, baseAttributeScore));
        text = text.Replace("{{hot}}", GetRestoreHpOverTimeText(ability, baseAttributeScore));
        return text.Replace("{{restoreMpOverTime}}", GetRestoreMpOverTimeText(ability, baseAttributeScore));
    }

    private string GetBluntingText(ActiveAbility ability, int baseAttributeScore) {
        float blunting = 0;
        float multiplier;
        if (GetComponent<PlayerCharacter>() != null) multiplier = GetComponent<PlayerCharacter>().weapon.attackPower;
        else multiplier = GetComponent<Monster>().attackFactor;
        foreach (var attribute in ability.attributes) if (attribute.type == "blunting") blunting += ((float)attribute.FindParameter("degree").value) * multiplier;
        return ((int)blunting).ToString();
    }

    private string GetHealingText(ActiveAbility ability, int baseAttributeScore) {
        float factor = 10;
        factor *= GetComponent<PlayerCharacter>().weapon.attackPower;
        int level = GetComponent<ExperienceGainer>().level;
        factor *= CharacterAttribute.attributes["healingMultiplier"].instances[GetComponent<Character>()].TotalValue / 100f;
        float healing = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "heal") healing += ((float)attribute.FindParameter("degree").value) * factor * CharacterAttribute.attributes["receivedHealing"].instances[GetComponent<Character>()].TotalValue / 100f;
        return ((int)healing).ToString();
    }

    private string GetShieldText(ActiveAbility ability, BaseStat stat) {
        float shield = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "shield") shield += (float)attribute.FindParameter("degree").value * GetComponent<Attacker>().GetBaseDamage(stat);
        return ((int)shield).ToString();
    }

    private string GetRestoreMpText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "restoreMP") mp += (float)attribute.FindParameter("degree").value;
        return ((int)mp).ToString();
    }

    private string GetRestoreHpOverTimeText(ActiveAbility ability, int baseAttributeScore) {
        float healing = 0;
        float duration = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "hot") {
                healing += (float)attribute.FindParameter("degree").value;
                duration = Mathf.Max(duration, (float)attribute.FindParameter("duration").value);
            }
        }
        return ((int)healing).ToString() + " over " + ((int)duration).ToString() + " seconds";
    }

    private string GetRestoreMpOverTimeText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        float duration = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "mpOverTime") {
                mp += (float)attribute.FindParameter("degree").value;
                duration = Mathf.Max(duration, (float)attribute.FindParameter("duration").value);
            }
        }
        return ((int)mp).ToString() + " over " + ((int)duration).ToString() + " seconds";
    }

    private string GetAttackText(ActiveAbility ability, int baseAttributeScore) {
        if (GetComponent<PlayerCharacter>() == null) return "";
        var text = Mathf.Floor(GetComponent<PlayerCharacter>().weapon.attackPower * baseAttributeScore * ((AttackAbility)ability).damage).ToString();
        text += GetTextFromDots(ability, baseAttributeScore);
        return text;
    }

    private string GetTextFromDots(ActiveAbility ability, int baseAttributeScore) {
        var text = "";
        foreach (var attribute in ability.attributes) {
            if (attribute.type== "addedDot") {
                text += " + ";
                text += GetDotText(attribute, baseAttributeScore);
            }
        }
        return text;
    }

    private string GetDotText(AbilityAttribute attribute, int baseAttributeScore) {
        var text = GetDotDamage(attribute, baseAttributeScore).ToString();
        text += " over ";
        text += GetDotSeconds(attribute).ToString() + " s";
        return text;
    }

    private int GetDotDamage(AbilityAttribute attribute, int baseAttributeScore) {
        return (int)((float)attribute.FindParameter("degree").value * baseAttributeScore);
    }

    private int GetDotSeconds(AbilityAttribute attribute) {
        return (int)((float)attribute.FindParameter("duration").value);
    }
}
