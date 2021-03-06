﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySelectionScreenButton : MonoBehaviour {

    public GameObject selectionFrame;
    public Ability ability = null;
    public CharacterSelectScreen characterSelectScreen = null;
    public int abilitySlot = 0;
    private bool initialized = false;

    // Use this for initialization
    void Start() {
        selectionFrame.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (!initialized && ability != null && GetComponent<CacheGrabber>().iconCache.Count > 0) {
            GetComponent<Image>().sprite = GetComponent<CacheGrabber>().iconCache[ability.icon];
            var tooltip = GetComponent<DuloGames.UI.UITooltipShow>();
            tooltip.contentLines = new DuloGames.UI.UITooltipLineContent[] { new DuloGames.UI.UITooltipLineContent(), new DuloGames.UI.UITooltipLineContent() };
            tooltip.contentLines[0].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Title;
            tooltip.contentLines[0].Content = ability.name;
            tooltip.contentLines[1].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
            tooltip.contentLines[1].CustomLineStyle = "ItemAttribute";
            var description = AbilityInterpolate(ability);
            tooltip.contentLines[1].Content = description;
            initialized = true;
        }
    }

    private string AbilityInterpolate(Ability ability) {
        if (ability is ActiveAbility) return ActiveAbilityInterpolate((ActiveAbility)ability);
        var text = ability.description;
        return text;
    }

    private string ActiveAbilityInterpolate(ActiveAbility ability) {
        var text = ability.description;
        text = text.Replace("{{baseStat}}", ability.baseStat.ToString());
        if (ability is AttackAbility) text = AttackAbilityInterpolate(text, ability);
        text = UtilityAbilityInterpolate(text, ability);
        return text;
    }

    private string AttackAbilityInterpolate(string text, ActiveAbility ability) {
        Dictionary<BaseStat, int> lookups = new Dictionary<BaseStat, int>() {
                { BaseStat.strength, characterSelectScreen.strength},
                { BaseStat.dexterity, characterSelectScreen.dexterity},
                { BaseStat.intelligence, characterSelectScreen.intelligence},
            };
        int baseAttributeScore = lookups[((AttackAbility)ability).baseStat];
        text = text.Replace("{{damage}}", (GetAttackText(ability, baseAttributeScore)));
        text = text.Replace("{{blunting}}", GetBluntingText(ability, baseAttributeScore));
        text = text.Replace("{{shield}}", GetDamageShieldText(ability, ((AttackAbility)ability).baseStat));
        return text.Replace("{{dotDamage}}", (Mathf.Floor(0.8437f * baseAttributeScore * ((AttackAbility)ability).dotDamage).ToString()));
    }

    private string UtilityAbilityInterpolate(string text, ActiveAbility ability) {
        Dictionary<BaseStat, int> lookups = new Dictionary<BaseStat, int>() {
                { BaseStat.strength, characterSelectScreen.strength},
                { BaseStat.dexterity, characterSelectScreen.dexterity},
                { BaseStat.constitution, characterSelectScreen.constitution},
                { BaseStat.intelligence, characterSelectScreen.intelligence},
                { BaseStat.wisdom, characterSelectScreen.wisdom},
                { BaseStat.luck, characterSelectScreen.luck},
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
        float factor = 0;
        if (ability.baseStat == BaseStat.strength) factor = 0.8437f * characterSelectScreen.strength;
        else if (ability.baseStat == BaseStat.dexterity) factor = 0.8437f * characterSelectScreen.dexterity;
        else if (ability.baseStat == BaseStat.constitution) factor = 0.8437f * characterSelectScreen.constitution;
        else if (ability.baseStat == BaseStat.intelligence) factor = 0.8437f * characterSelectScreen.intelligence;
        else if (ability.baseStat == BaseStat.wisdom) factor = 0.8437f * characterSelectScreen.wisdom;
        else factor = 0.8437f * characterSelectScreen.luck;
        foreach (var attribute in ability.attributes) if (attribute.type == "blunting") blunting += ((float)attribute.FindParameter("degree").value) * factor;
        return ((int)blunting).ToString();
    }

    private string GetHealingText(ActiveAbility ability, int baseAttributeScore) {
        float factor = characterSelectScreen.wisdom;
        factor *= 0.8437f;
        float healing = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "heal") healing += (float)attribute.FindParameter("degree").value * factor * SecondaryStatUtility.CalcHealingMultiplier(characterSelectScreen.strength, 1);
        return Mathf.FloorToInt(healing).ToString();
    }

    private string GetShieldText(ActiveAbility ability, BaseStat stat) {
        float shield = 0;
        float factor = 0;
        if (stat == BaseStat.strength) factor = 0.8437f * characterSelectScreen.strength;
        else if (stat == BaseStat.dexterity) factor = 0.8437f * characterSelectScreen.dexterity;
        else if (stat == BaseStat.constitution) factor = 0.8437f * characterSelectScreen.constitution;
        else if (stat == BaseStat.intelligence) factor = 0.8437f * characterSelectScreen.intelligence;
        else if (stat == BaseStat.wisdom) factor = 0.8437f * characterSelectScreen.wisdom;
        else factor = 0.8437f * characterSelectScreen.luck;
        foreach (var attribute in ability.attributes) if (attribute.type == "shield") shield += (float)attribute.FindParameter("degree").value * factor;
        return Mathf.FloorToInt(shield).ToString();
    }

    private string GetDamageShieldText(ActiveAbility ability, BaseStat stat) {
        float shield = 0;
        float factor = 0;
        if (stat == BaseStat.strength) factor = 0.8437f * characterSelectScreen.strength;
        else if (stat == BaseStat.dexterity) factor = 0.8437f * characterSelectScreen.dexterity;
        else if (stat == BaseStat.constitution) factor = 0.8437f * characterSelectScreen.constitution;
        else if (stat == BaseStat.intelligence) factor = 0.8437f * characterSelectScreen.intelligence;
        else if (stat == BaseStat.wisdom) factor = 0.8437f * characterSelectScreen.wisdom;
        else factor = 0.8437f * characterSelectScreen.luck;
        foreach (var attribute in ability.attributes) if (attribute.type == "damageShield") shield += (float)attribute.FindParameter("degree").value * factor;
        return Mathf.FloorToInt(shield).ToString();
    }

    private string GetRestoreMpText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "restoreMP") mp += (float)attribute.FindParameter("degree").value;
        return Mathf.FloorToInt(mp).ToString();
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
        return Mathf.FloorToInt(healing).ToString() + " over " + Mathf.FloorToInt(duration).ToString() + " seconds";
    }

    private string GetRestoreMpOverTimeText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        float duration = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "mpOverTime") {
                mp += Mathf.FloorToInt((float)attribute.FindParameter("degree").value);
                duration = Mathf.Max(duration, Mathf.FloorToInt((float)attribute.FindParameter("duration").value));
            }
        }
        return Mathf.FloorToInt(mp).ToString() + " over " + Mathf.FloorToInt(duration).ToString() + " seconds";
    }

    private string GetAttackText(ActiveAbility ability, int baseAttributeScore) {
        var text = Mathf.Floor(0.8437f * baseAttributeScore * ((AttackAbility)ability).damage).ToString();
        text += GetTextFromDots(ability, baseAttributeScore);
        return text;
    }

    private string GetTextFromDots(ActiveAbility ability, int baseAttributeScore) {
        var text = "";
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "addedDot") {
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
        return Mathf.FloorToInt((float)attribute.FindParameter("degree").value * baseAttributeScore);
    }

    private int GetDotSeconds(AbilityAttribute attribute) {
        return Mathf.FloorToInt((float)attribute.FindParameter("duration").value);
    }

    public void Click() {
        characterSelectScreen.selectedAbilities[abilitySlot] = ability;
        characterSelectScreen.UpdateAbilitySelectionFrames();
    }
}
