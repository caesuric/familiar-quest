using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class MonsterScaler : MonoBehaviour {
    public int level = 1;
    public int quality = 1;
    public float multiplier = 0.5f;
    public int numPlayers = 1;
    private int oldNumPlayers = 1;
    private bool sizeIncreased = false;
    public TextMesh barTextObj;
    //private static readonly float scaleFactor = 1.2f; //1.1f;
    //private static readonly float extraHpScaleFactor = 1.05f;
    private bool unitFrameConfigured = false;
    public float colliderSize = 0f;
    private static readonly List<float> scaleFactor = new List<float>() {
        1.2f,
        1.166666667f,
        1.071428571f,
        1.2f,
        1.166666667f,
        1.142857143f,
        1.166666667f,
        1.214285714f,
        1.176470588f,
        1.15f,
        1.152173913f,
        1.188679245f,
        1.063492063f,
        1.208955224f,
        1.135802469f,
        1.217391304f,
        1.125f,
        1.111111111f,
        1.192857143f,
        1.137724551f,
        1.089473684f,
        1.178743961f,
        1.139344262f,
        1.129496403f,
        1.133757962f,
        1.115168539f,
        1.143576826f,
        1.121145374f,
        1.139489194f,
        1.117241379f,
        1.132716049f,
        1.133514986f,
        1.109375f,
        1.122426869f,
        1.137065637f,
        1.108658744f,
        1.130168453f,
        1.109078591f,
        1.140500916f,
        1.09051955f,
        1.114931238f,
        1.120704846f,
        1.119496855f,
        1.099719101f,
        1.12962963f,
        1.106840023f,
        1.102655771f,
        1.100046318f,
        1.099789474f
    };

    private void Update() {
        if (!unitFrameConfigured) {
            var unitFrame = GetComponentInChildren<UnitFrame>();
            if (unitFrame!=null) {
                unitFrameConfigured = true;
                unitFrame.SetLevel(level);
                unitFrame.SetLevelCircleType(quality);
                unitFrame.SetScale(gameObject);
            }
        }
    }

    public void Scale() {
        AdjustForQuality();
        AdjustForMultiplier();
        AdjustForPlayers();
        GetComponent<Character>().CalculateAll();
        AdjustColliderSize();
        //if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().stoppingDistance = 2.5f + GetComponent<MonsterCombatant>().colliderSize;
        if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().stoppingDistance = 2.5f + colliderSize;
    }

    public void AdjustForLevel(int level) {
        var originalLevel = level;
        //if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed /= SecondaryStatUtility.CalcMoveSpeed(GetComponent<Character>().dexterity, originalLevel);
        if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed /= (CharacterAttribute.attributes["moveSpeed"].instances[GetComponent<Character>()].TotalValue / 100f);
        if (level > this.level) {
            level -= this.level;
            this.level = originalLevel;
            //float tempStrength = GetComponent<Character>().strength;
            //float tempDexterity = GetComponent<Character>().dexterity;
            //float tempConstitution = GetComponent<Character>().constitution;
            //float tempIntelligence = GetComponent<Character>().intelligence;
            //float tempWisdom = GetComponent<Character>().wisdom;
            //float tempLuck = GetComponent<Character>().luck;
            float tempStrength = CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].BaseValue;
            float tempDexterity = CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].BaseValue;
            float tempConstitution = CharacterAttribute.attributes["constitution"].instances[GetComponent<Character>()].BaseValue;
            float tempIntelligence = CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].BaseValue;
            float tempWisdom = CharacterAttribute.attributes["wisdom"].instances[GetComponent<Character>()].BaseValue;
            float tempLuck = CharacterAttribute.attributes["luck"].instances[GetComponent<Character>()].BaseValue;
            for (int i = 0; i < level; i++) {
                tempStrength *= scaleFactor[i];
                tempDexterity *= scaleFactor[i];
                tempConstitution *= scaleFactor[i];
                tempIntelligence *= scaleFactor[i];
                tempLuck *= scaleFactor[i];
                GetComponent<Monster>().attackFactor *= scaleFactor[i];
                GetComponent<Monster>().hpFactor *= scaleFactor[i]; //* extraHpScaleFactor;
                GetComponent<Monster>().mpFactor *= scaleFactor[i];
                multiplier += 0.01f;
            }
            GetComponent<Health>().hp = GetComponent<Health>().maxHP = (int)GetComponent<Health>().maxHP;
            //GetComponent<Character>().strength = (int)tempStrength;
            //GetComponent<Character>().dexterity = (int)tempDexterity;
            //GetComponent<Character>().constitution = (int)tempConstitution;
            //GetComponent<Character>().intelligence = (int)tempIntelligence;
            //GetComponent<Character>().wisdom = (int)tempWisdom;
            //GetComponent<Character>().luck = (int)tempLuck;
            CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].BaseValue = (int)tempStrength;
            CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].BaseValue = (int)tempDexterity;
            CharacterAttribute.attributes["constitution"].instances[GetComponent<Character>()].BaseValue = (int)tempConstitution;
            CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].BaseValue = (int)tempIntelligence;
            CharacterAttribute.attributes["wisdom"].instances[GetComponent<Character>()].BaseValue = (int)tempWisdom;
            CharacterAttribute.attributes["luck"].instances[GetComponent<Character>()].BaseValue = (int)tempLuck;
            if (GetComponent<NavMeshAgent>()!=null) GetComponent<NavMeshAgent>().speed *= CharacterAttribute.attributes["moveSpeed"].instances[GetComponent<Character>()].TotalValue / 100f;
        }
        else if (level < this.level) {
            level = this.level - level;
            this.level = originalLevel;
            //float tempStrength = GetComponent<Character>().strength;
            //float tempDexterity = GetComponent<Character>().dexterity;
            //float tempConstitution = GetComponent<Character>().constitution;
            //float tempIntelligence = GetComponent<Character>().intelligence;
            //float tempWisdom = GetComponent<Character>().wisdom;
            //float tempLuck = GetComponent<Character>().luck;
            float tempStrength = CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].BaseValue;
            float tempDexterity = CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].BaseValue;
            float tempConstitution = CharacterAttribute.attributes["constitution"].instances[GetComponent<Character>()].BaseValue;
            float tempIntelligence = CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].BaseValue;
            float tempWisdom = CharacterAttribute.attributes["wisdom"].instances[GetComponent<Character>()].BaseValue;
            float tempLuck = CharacterAttribute.attributes["luck"].instances[GetComponent<Character>()].BaseValue;
            for (int i = level; i > 0; i--) {
                tempStrength /= scaleFactor[i];
                tempDexterity /= scaleFactor[i];
                tempConstitution /= scaleFactor[i];
                tempIntelligence /= scaleFactor[i];
                tempLuck /= scaleFactor[i];
                GetComponent<Monster>().attackFactor /= scaleFactor[i];
                GetComponent<Monster>().hpFactor /= scaleFactor[i]; //* extraHpScaleFactor);
                GetComponent<Monster>().mpFactor /= scaleFactor[i];
                multiplier -= 0.01f;
            }
            GetComponent<Health>().hp = GetComponent<Health>().maxHP = (int)GetComponent<Health>().maxHP;
            //GetComponent<Character>().strength = (int)tempStrength;
            //GetComponent<Character>().dexterity = (int)tempDexterity;
            //GetComponent<Character>().constitution = (int)tempConstitution;
            //GetComponent<Character>().intelligence = (int)tempIntelligence;
            //GetComponent<Character>().wisdom = (int)tempWisdom;
            //GetComponent<Character>().luck = (int)tempLuck;
            CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].BaseValue = (int)tempStrength;
            CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].BaseValue = (int)tempDexterity;
            CharacterAttribute.attributes["constitution"].instances[GetComponent<Character>()].BaseValue = (int)tempConstitution;
            CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].BaseValue = (int)tempIntelligence;
            CharacterAttribute.attributes["wisdom"].instances[GetComponent<Character>()].BaseValue = (int)tempWisdom;
            CharacterAttribute.attributes["luck"].instances[GetComponent<Character>()].BaseValue = (int)tempLuck;
            //GetComponent<NavMeshAgent>().speed *= SecondaryStatUtility.CalcMoveSpeed(GetComponent<Character>().dexterity, level);
            if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed *= CharacterAttribute.attributes["moveSpeed"].instances[GetComponent<Character>()].TotalValue / 100f;
        }
        var effectiveLevel = level;
        if (effectiveLevel > 50) effectiveLevel = 50;
        GetComponent<RewardGiver>().xpValue = ExperienceGainer.xpPerMob[effectiveLevel - 1];
    }

    private void AdjustForQuality() {
        switch (quality) {
            case 0:
            default:
                break;
            case 1:
                multiplier = 1 - ((1 - multiplier) / 2);
                GetComponent<RewardGiver>().xpValue = (int)(((float)GetComponent<RewardGiver>().xpValue) * 1.12f * 1.12f);
                break;
            case 2:
                multiplier = 1;
                GetComponent<RewardGiver>().xpValue = (int)(((float)GetComponent<RewardGiver>().xpValue) * 1.25f * 1.25f);
                break;
            case 3:
                ScaleByFactor(1.26f);
                GetComponent<RewardGiver>().xpValue = (int)(((float)GetComponent<RewardGiver>().xpValue) * 1.41f * 1.41f);
                multiplier = 1 - ((1 - multiplier) / 2);
                break;
            case 4:
                ScaleByFactor(1.1f, 60f);
                GetComponent<RewardGiver>().xpValue = (int)(((float)GetComponent<RewardGiver>().xpValue) * 6f);
                //multiplier = 1;
                if (!sizeIncreased) {
                    sizeIncreased = true;
                    gameObject.transform.localScale *= 2;
                    GetComponent<NavMeshAgent>().stoppingDistance *= 2;
                }
                break;
        }
    }
    public void UpdateBarText(string text) {
        var obj = barTextObj;
        if (obj != null) obj.text = text;
        RpcUpdateBarText(text);
    }

    //[ClientRpc]
    private void RpcUpdateBarText(string text) {
        var obj = barTextObj;
        if (obj != null) obj.text = text;
    }

    private void AdjustForMultiplier() {
        var overallMultiplier = multiplier / 2f;
        ScaleByFactor(overallMultiplier);
    }

    private void AdjustForPlayers() {
        ScaleByFactor(PlayerFactor(numPlayers) / PlayerFactor(oldNumPlayers));
        oldNumPlayers = numPlayers;
    }

    private float PlayerFactor(int number) {
        var value = Mathf.Sqrt(number);
        if (number >= 10) return value * 3.8125f;
        else if (number >= 8) return value * 2.95f;
        else if (number >= 6) return value * 2.4625f;
        else if (number == 5) return value * 1.99f;
        else if (number == 4) return value * 1.585f;
        else if (number == 3) return value * 1.27f;
        else return value;
    }

    private void ScaleByFactor(float x, float hpFactor=1) {
        //if (GetComponent<NavMeshAgent>()!=null) GetComponent<NavMeshAgent>().speed /= SecondaryStatUtility.CalcMoveSpeed(GetComponent<Character>().dexterity, level);
        if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed /= (CharacterAttribute.attributes["moveSpeed"].instances[GetComponent<Character>()].TotalValue / 100f);
        GetComponent<Health>().maxHP *= x * hpFactor;
        GetComponent<Health>().hp = GetComponent<Health>().maxHP;
        GetComponent<Monster>().attackFactor *= x;
        GetComponent<Monster>().hpFactor *= x * hpFactor;
        GetComponent<Monster>().mpFactor *= x;
        //GetComponent<Character>().strength = (int)(((float)GetComponent<Character>().strength) * x);
        //GetComponent<Character>().dexterity = (int)(((float)GetComponent<Character>().dexterity) * x);
        //GetComponent<Character>().constitution = (int)(((float)GetComponent<Character>().constitution) * x);
        //GetComponent<Character>().intelligence = (int)(((float)GetComponent<Character>().intelligence) * x);
        //GetComponent<Character>().wisdom = (int)(((float)GetComponent<Character>().wisdom) * x);
        //GetComponent<Character>().luck = (int)(((float)GetComponent<Character>().luck) * x);
        var attributes = new List<string>() { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        if (GetComponent<Character>() == null) return;
        foreach (var attr in attributes) CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue = CharacterAttribute.attributes[attr].instances[GetComponent<Character>()].BaseValue * x;
        if (hpFactor > 1) GetComponent<Health>().bossHpFactor = 60f;
        //GetComponent<RewardGiver>().xpValue = (int)(((float)GetComponent<RewardGiver>().xpValue) * x * x);
        GetComponent<RewardGiver>().baseGoldValue = (int)(((float)GetComponent<RewardGiver>().baseGoldValue) * x * x);
        //if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed *= SecondaryStatUtility.CalcMoveSpeed(GetComponent<Character>().dexterity, level);
        if (GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed *= CharacterAttribute.attributes["moveSpeed"].instances[GetComponent<Character>()].TotalValue / 100f;
    }

    private void AdjustColliderSize() {
        var cCollider = GetComponent<CapsuleCollider>();
        if (cCollider != null) {
            colliderSize = cCollider.radius;
            if (GetComponent<Spider>() != null) colliderSize /= 2;
            if (quality == 4) colliderSize *= 2;
        }
    }

    public static void ScaleToPlayers(int number) {
        foreach (var monster in Monster.monsters) {
            monster.GetComponent<MonsterScaler>().oldNumPlayers = monster.GetComponent<MonsterScaler>().numPlayers;
            monster.GetComponent<MonsterScaler>().numPlayers = number;
            monster.GetComponent<MonsterScaler>().AdjustForPlayers();
        }
    }

    public static void ScaleToLevel() {
        float average = 0;
        foreach (var player in PlayerCharacter.players) average += player.GetComponent<ExperienceGainer>().level;
        average /= PlayerCharacter.players.Count;
        int rounded = (int)average;
        foreach (var monster in Monster.monsters) monster.GetComponent<MonsterScaler>().AdjustForLevel(rounded + LevelGen.instance.floor - 1);
    }
}
