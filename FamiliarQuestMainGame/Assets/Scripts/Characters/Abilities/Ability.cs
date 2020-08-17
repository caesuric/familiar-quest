using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Ability {
    public List<AbilityAttribute> attributes = new List<AbilityAttribute>();
    public int icon;
    public int points = 70;
    public long xp = 0;
    public int level = 1;
    public string name;
    public string description;
    public abstract Ability Copy();
    protected abstract void LevelUp(int originalLevel, int targetLevel);

    public AbilityAttribute FindAttribute(string attribute) {
        foreach (var item in attributes) if (item.type == attribute) return item;
        return null;
    }

    public void SortAttributes() {
        attributes.Sort((AbilityAttribute attr1, AbilityAttribute attr2) => { return attr1.priority.CompareTo(attr2.priority); });
        attributes.Reverse();
    }

    public bool IsValid() {
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
            if (experienceGained >= ExperienceGainer.xpTable[i]) targetLevel++;
            else break;
        }
        if (targetLevel > level) {
            LevelUp(level, targetLevel);
            ShowLevelUpFloatingText();
        }
        level = targetLevel;
    }

    private void ShowLevelUpFloatingText() {
        PlayerCharacter.localPlayer.GetComponent<ObjectSpawner>().CmdCreateFloatingText(name.ToUpper() + " GAINED A LEVEL!", Color.green, 90, name + " gained a level!");
    }

    public static int GetLevelFromPoints(float points) {
        var level = 1;
        float currentPoints = points;
        while (currentPoints > 70) {
            currentPoints /= 1.05f;
            level += 1;
        }
        return level;
    }
}
