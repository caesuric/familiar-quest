using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ability {
    public List<AbilityAttribute> attributes = new List<AbilityAttribute>();
    public int icon;
    public float points = 70f;
    public long xp = 0;
    public int level = 1;
    public int skillPoints = 0;
    public string name;
    public string description;
    public abstract Ability Copy();
    public AbilitySkillTree skillTree = null;
    protected abstract void LevelUp(int originalLevel, int targetLevel);

    public AbilityAttribute FindAttribute(string attribute) {
        foreach (var item in attributes) if (item.type == attribute && item.priority >= 50) return item;
        return null;
    }

    public void SortAttributes() {
        attributes.Sort((AbilityAttribute attr1, AbilityAttribute attr2) => { return attr1.priority.CompareTo(attr2.priority); });
        attributes.Reverse();
    }

    public bool IsValid() {
        for (int i=0; i<attributes.Count-1; i++) {
            var attr = attributes[i];
            for (int j=i+1; j<attributes.Count; j++) {
                var attr2 = attributes[j];
                if (attr.type == "stealthy" && attr2.type == "stealthy") return false;
            }
        }
        foreach (var attr in attributes) if (attr.priority >= 50) return true;
        return false;
    }

    public void SetLevel(int level) {
        this.level = level;
        if (level == 1) xp = 0;
        else xp = ExperienceGainer.xpTable[level - 2];
    }

    public void GainExperience(long experienceGained) {
        xp += experienceGained;
        var targetLevel = 1;
        for (int i = 0; i < 50; i++) {
            if (xp >= ExperienceGainer.xpTable[i]) targetLevel++;
            else break;
        }
        if (targetLevel > level) {
            LevelUp(level, targetLevel);
            ShowLevelUpFloatingText();
        }
        level = targetLevel;
    }

    public bool CanAddNewAttribute() {
        int validCount = 0;
        int allCount = 0;
        foreach (var attr in attributes) {
            if (allCount < 4 && attr.priority > 50) validCount++;
            else if (allCount >= 4) break;
            allCount++;
        }
        if (validCount < 4) return true;
        else return false;
    }

    private void ShowLevelUpFloatingText() {
        PlayerCharacter.localPlayer.GetComponent<ObjectSpawner>().CreateFloatingText(name.ToUpper() + " GAINED A LEVEL!", Color.green, 90, name + " gained a level!");
    }

    protected void LevelUp () {
        skillPoints++;
    }
}
