using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ExperienceGainer : DependencyUser {
    public static List<long> xpTable = new List<long>();
    public static List<int> xpPerMob = new List<int>();
    public GameObject levelUpEffect;
    public AudioClip levelUpSound;
    public float timer = 0f;
    public bool showLevelUpEffect = false;
    private GameObject levelUpEffectInstance = null;
    //  int[] xpTable = new int[] { 200, 462, 805, 1254, 1842, 2613, 3624, 4950, 6689, 8969, 11959, 15880, 21022, 27766, 36611, 48212, 63428, 83385, 109561, 143894, 188926, 247991, 325463, 427078, 560360, 735178, 964476, 1265232, 1659715, 2177134, 2855801, 3745967, 4913544, 6444984, 8453681, 11088368, 14544128, 19076841, 25022128, 32820204, 43048472, 56464277, 74060983, 97141526, 127414889, 167122642, 219204919, 287518116 };
    //[SyncVar]
    public float xpPercentage = 1.0f;
    //[SyncVar]
    public int xp = 0;
    //[SyncVar]
    public int level = 1;
    //[SyncVar]
    public long xpToLevel = 0;
    //[SyncVar]
    public int strength;
    //[SyncVar]
    public int dexterity;
    //[SyncVar]
    public int constitution;
    //[SyncVar]
    public int intelligence;
    //[SyncVar]
    public int wisdom;
    //[SyncVar]
    public int luck;
    //[SyncVar]
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

    private void Calculate() {
        xpToLevel = xpTable[level - 1] - xp;
        xpPercentage = GetXPPercentage();
        if (xpPercentage != 0) initialCalculationComplete = true;
    }

    // Use this for initialization
    void Start() {
        dependencies = new List<string>() { "PlayerCharacter", "Character" };
        Dependencies.Check(this);
        Calculate();
    }

    public void GainXP(int amount) {
        if (GetComponent<SpiritUser>().HasPassive("experienceBoost")) {
            foreach (var passive in GetComponent<SpiritUser>().spirits[0].passiveAbilities) {
                {
                    foreach (var attribute in passive.attributes) {
                        if (attribute.type == "experienceBoost") {
                            float amountFloat = amount;
                            amountFloat *= (1 + attribute.FindParameter("degree").floatVal);
                            amount = (int)amountFloat;
                        }
                    }
                }
            }
        }
        xp += amount;
        if (xp >= xpTable[level - 1]) LevelUp();
        Calculate();
    }

    private void LevelUp() {
        int targetLevel = DetermineTargetLevel();
        for (int i = level; i < targetLevel; i++) ActuallyLevelUp();
        LevelUpTextUpdater.Trigger();
        levelUpEffectInstance = Instantiate(levelUpEffect, transform);
        showLevelUpEffect = true;
        timer = 0f;
        GetComponent<AudioSource>().clip = levelUpSound;
        GetComponent<AudioSource>().Play();
    }

    private int DetermineTargetLevel() {
        for (int i = 0; i < xpTable.Count; i++) if (xp < xpTable[i]) return i + 1;
        return 50;
    }

    private void ActuallyLevelUp() {
        level += 1;
        //RemoveAllEquipmentBonuses();
        var attributes = new List<string>() { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        int totalStats = sparePoints;
        foreach (var attr in attributes) totalStats += (int)CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue;
        //sparePoints += Mathf.Max((int)((GetComponent<Character>().strength + GetComponent<Character>().dexterity + GetComponent<Character>().constitution + GetComponent<Character>().intelligence + GetComponent<Character>().wisdom + GetComponent<Character>().luck + sparePoints) * 0.02), 1);
        sparePoints += Mathf.Max((int)(totalStats * 0.02f), 1);
        //GetComponent<Character>().strength = Mathf.Max((int)(GetComponent<Character>().strength * 1.08f), GetComponent<Character>().strength + 1);
        //GetComponent<Character>().dexterity = Mathf.Max((int)(GetComponent<Character>().dexterity * 1.08f), GetComponent<Character>().dexterity + 1);
        //GetComponent<Character>().constitution = Mathf.Max((int)(GetComponent<Character>().constitution * 1.08f), GetComponent<Character>().constitution + 1);
        //GetComponent<Character>().intelligence = Mathf.Max((int)(GetComponent<Character>().intelligence * 1.08f), GetComponent<Character>().intelligence + 1);
        //GetComponent<Character>().wisdom = Mathf.Max((int)(GetComponent<Character>().wisdom * 1.08f), GetComponent<Character>().wisdom + 1);
        //GetComponent<Character>().luck = Mathf.Max((int)(GetComponent<Character>().luck * 1.08f), GetComponent<Character>().luck + 1);
        foreach (var attr in attributes) CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue = Mathf.Max((int)(CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue * 1.08f), CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue + 1);
        //AddAllEquipmentBonuses();
        GetComponent<Character>().CalculateAll();
        GetComponent<Health>().hp = GetComponent<Health>().maxHP;
    }

    //private void RemoveAllEquipmentBonuses() {
    //    ToggleAllEquipmentBonuses(false);
    //}

    //private void AddAllEquipmentBonuses() {
    //    ToggleAllEquipmentBonuses(true);
    //}

    //private void ToggleAllEquipmentBonuses(bool add) {
    //    var pc = GetComponent<PlayerCharacter>();
    //    var c = GetComponent<Character>();
    //    var equipment = new List<Equipment>() { pc.weapon, pc.armor, pc.necklace, pc.belt, pc.bracelets[0], pc.bracelets[1], pc.bracelets[2], pc.bracelets[3], pc.cloak, pc.earring, pc.hat, pc.shoes };
    //    foreach (var item in equipment) {
    //        if (item == null) continue;
    //        if (add) {
    //            c.strength += item.strength;
    //            c.dexterity += item.dexterity;
    //            c.constitution += item.constitution;
    //            c.intelligence += item.intelligence;
    //            c.wisdom += item.wisdom;
    //            c.luck += item.luck;
    //        }
    //        else {
    //            c.strength -= item.strength;
    //            c.dexterity -= item.dexterity;
    //            c.constitution -= item.constitution;
    //            c.intelligence -= item.intelligence;
    //            c.wisdom -= item.wisdom;
    //            c.luck -= item.luck;
    //        }
    //    }
    //}
    
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
