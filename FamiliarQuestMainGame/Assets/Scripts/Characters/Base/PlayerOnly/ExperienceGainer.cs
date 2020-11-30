using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(PlayerCharacter))]
public class ExperienceGainer : MonoBehaviour {
    public static List<long> xpTable = new List<long>();
    public static List<int> xpPerMob = new List<int>();
    public GameObject levelUpEffect;
    public AudioClip levelUpSound;
    public float timer = 0f;
    public bool showLevelUpEffect = false;
    private GameObject levelUpEffectInstance = null;
    public float xpPercentage = 1.0f;
    public int xp = 0;
    public int level = 1;
    public long xpToLevel = 0;
    public int sparePoints = 0;
    bool initialCalculationComplete = false;

    static ExperienceGainer() {
        float killsNeededMultiplyFactor = 1.1f;
        float killsNeededComplexFactor = 1.0095f;
        int killsNeededComplexCutoff = 40;
        float xpPerMobMultiplier = 1.44f;
        float xpPerMobComplexFactor = 1.016755f;
        float xpPerMobMultiplierReducer = 0.01f;
        int xpPerMobLastNormalLevel = 16;

        List<float> killsNeeded = new List<float>();
        List<float> rawXpPerMob = new List<float>();
        List<float> rawXpTable = new List<float>();

        float killsNeededCursor = 8f;
        float xpPerMobCursor = 3f;
        float totalXp = 0f;

        for (int i=1; i<=50; i++) {
            killsNeeded.Add(killsNeededCursor);
            rawXpPerMob.Add(xpPerMobCursor);
            rawXpTable.Add(killsNeededCursor * xpPerMobCursor);
            xpPerMob.Add((int)(xpPerMobCursor));
            totalXp += (killsNeededCursor * xpPerMobCursor);
            xpTable.Add((long)totalXp);
            if (i < killsNeededComplexCutoff) killsNeededCursor *= killsNeededMultiplyFactor;
            else killsNeededCursor *= killsNeededMultiplyFactor / Mathf.Pow(killsNeededComplexFactor, (i - (killsNeededComplexCutoff - 1)));
            if (i <= xpPerMobLastNormalLevel) xpPerMobCursor *= xpPerMobMultiplier;
            else xpPerMobCursor *= (xpPerMobMultiplier - (xpPerMobMultiplierReducer * (xpPerMobLastNormalLevel - i))) / Mathf.Pow(xpPerMobComplexFactor, i - xpPerMobLastNormalLevel);
        }
    }

    void Update() {
        if (!initialCalculationComplete) Calculate();
        if (showLevelUpEffect) timer += Time.deltaTime;
        if (timer>10f) {
            showLevelUpEffect = false;
            timer = 0f;
            Destroy(levelUpEffectInstance);
        }
    }

    public void Calculate() {
        xpToLevel = xpTable[level - 1] - xp;
        xpPercentage = GetXPPercentage();
        if (xpPercentage != 0) initialCalculationComplete = true;
    }

    // Use this for initialization
    void Start() {
        Calculate();
    }

    public void GainXP(int amount) {
        if (GetComponent<AbilityUser>().HasPassive("experienceBoost")) {
            foreach (var attribute in GetComponent<AbilityUser>().soulGemPassive.attributes) {
                if (attribute.type == "experienceBoost") {
                    float amountFloat = amount;
                    amountFloat *= (1 + (float)attribute.FindParameter("degree").value);
                    amount = (int)amountFloat;
                }
            }
        }
        xp += amount;
        if (xp >= xpTable[level - 1]) LevelUp();
        Calculate();
        float halfAmount = amount / 2f;
        AddExperienceToSoulGems(Mathf.CeilToInt(halfAmount));
    }

    private void AddExperienceToSoulGems(int amount) {
        var au = GetComponent<AbilityUser>();
        if (au.soulGemPassive != null) au.soulGemPassive.GainExperience(amount);
        foreach (var active in au.soulGemActives) if (active != null) active.GainExperience(amount);
        foreach (var ability in au.soulGemActivesOverflow) if (ability!=null) ability.GainExperience(amount);
        foreach (var ability in au.soulGemPassivesOverflow) if (ability != null) ability.GainExperience(amount);
    }

    private void LevelUp() {
        int targetLevel = DetermineTargetLevel();
        for (int i = level; i < targetLevel; i++) ActuallyLevelUp();
        LevelUpTextUpdater.Trigger();
        if (levelUpEffect != null) {
            levelUpEffectInstance = Instantiate(levelUpEffect);
            showLevelUpEffect = true;
            timer = 0f;
        }
        if (GetComponent<AudioSource>()!=null) {
            GetComponent<AudioSource>().clip = levelUpSound;
            GetComponent<AudioSource>().Play();
        }
    }

    private int DetermineTargetLevel() {
        for (int i = 0; i < xpTable.Count; i++) if (xp < xpTable[i]) return i + 1;
        return 50;
    }

    private void ActuallyLevelUp() {
        level += 1;
        var attributes = new List<string>() { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        int totalStats = sparePoints;
        foreach (var attr in attributes) totalStats += (int)CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue;
        sparePoints += Mathf.Max((int)(totalStats * 0.02f), 1);
        foreach (var attr in attributes) CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue = Mathf.Max((int)(CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue * 1.08f), CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue + 1);
        GetComponent<Character>().CalculateAll();
        GetComponent<Health>().hp = GetComponent<Health>().maxHP;
        GetComponent<Mana>().mp = GetComponent<Mana>().maxMP;
    }

    public float GetXPPercentage() {
        long nextLevelXP = xpTable[level - 1];
        long xpProgress = xp;
        if (level > 1) {
            nextLevelXP -= xpTable[level - 2];
            xpProgress -= xpTable[level - 2];
        }
        return (float)xpProgress / (float)nextLevelXP;
    }
}
