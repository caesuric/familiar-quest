using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(CacheGrabber))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Mana))]
[RequireComponent(typeof(ObjectSpawner))]
public class StatusEffectHost : MonoBehaviour {

    //public float physicalSave;
    //public float mentalSave;
    //public float luckDebuffDivisor;
    //public float luckBuffMultiplier;
    //public float luckDropFactor;
    public List<StatusEffect> statusEffects = new List<StatusEffect>();
    private List<StatusEffect> pruneList = new List<StatusEffect>();
    private Dictionary<string, Action> effectMaintainers;
    private Dictionary<Element, int> ddeSpellEffects = new Dictionary<Element, int>();
    private List<string> saveIsPhysical = new List<string>() { "paralysis", "blind", "poison" };
    private List<string> saveIsMental = new List<string>() { "blunting", "vulnerability" };
    
    // Use this for initialization
    void Start() {
        var data = TextReader.ReadSets("DdeSpellEffects");
        foreach (var item in data) ddeSpellEffects.Add(AbilityCalculator.StringToElement(item[0]), int.Parse(item[1]));
        Calculate();
    }

    // Update is called once per frame
    void Update() {
        //if (NetworkServer.active) {
            pruneList.Clear();
            StatusEffect[] statusEffectsCopy = new StatusEffect[statusEffects.Count];
            statusEffects.CopyTo(statusEffectsCopy);
            foreach (var effect in statusEffectsCopy) MaintainEffect(effect);
            foreach (var effect in statusEffectsCopy) if (effect.duration<=0) pruneList.Add(effect);
            bool updateStats = (pruneList.Count > 0);
            foreach (var effect in pruneList) PruneEffect(effect);
            if (updateStats) GetComponent<Character>().CalculateAll();
        //}
    }

    private void MaintainEffect(StatusEffect effect) {
        effect.Update();
        effectMaintainers = new Dictionary<string, Action>() {
            {"dot", () => MaintainDot(effect) },
            {"mpOverTime", () => MaintainMpOverTime(effect) },
            {"hot", () => MaintainHot(effect) },
            {"poison", () => MaintainPoison() },
        };
        if (effectMaintainers.ContainsKey(effect.type)) effectMaintainers[effect.type]();
    }

    private void MaintainDot(StatusEffect effect) {
        GetComponent<Health>().TakeDamageFromDot(effect.degree * Time.deltaTime, ((AttackAbility)(effect.ability)).element, effect.inflicter, silent: true, ability: (AttackAbility)effect.ability);
    }

    private void MaintainMpOverTime(StatusEffect effect) {
        GetComponent<Mana>().mp += effect.degree / effect.duration * Time.deltaTime;
    }

    private void MaintainHot(StatusEffect effect) {
        GetComponent<Health>().hp = Mathf.Min(GetComponent<Health>().hp + (effect.degree * Time.deltaTime), GetComponent<Health>().maxHP);
    }

    private void MaintainPoison() {
        GetComponent<Health>().hp -= 6f * Time.deltaTime;
    }

    private void PruneEffect(StatusEffect effect) {
        if (effect.type == "dde") ApplyDde(effect);
        if (effect.type == "bossRage") RemoveBossRage();
        if (effect.type == "speed-" && GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed /= (1 - effect.degree);
        if (effect.icon != null) Destroy(effect.icon);
        if (effect.visual != null) Destroy(effect.visual);
        //if (effect.visual != null) NetworkServer.Destroy(effect.visual);
        statusEffects.Remove(effect);
    }

    public void Calculate() {
        //int level;
        //var experienceGainer = GetComponent<ExperienceGainer>();
        //var monsterScaler = GetComponent<MonsterScaler>();
        //if (experienceGainer != null) level = experienceGainer.level;
        //else level = monsterScaler.level;
        //physicalSave = SecondaryStatUtility.CalcPhysicalResist(GetComponent<Character>().constitution, level);
        //mentalSave = SecondaryStatUtility.CalcMentalResist(GetComponent<Character>().wisdom, level);
        //var luck = GetComponent<Character>().luck;
        //luckBuffMultiplier = 1 + SecondaryStatUtility.CalcStatusEffectDurationBonus(luck, level);
        //luckDebuffDivisor = 1 - SecondaryStatUtility.CalcStatusEffectDurationBonus(luck, level);
    }

    private void ApplyDde(StatusEffect effect) {
        GetComponent<Health>().TakeDamage(effect.degree, Element.none, effect.inflicter);
        var cache = GetComponent<CacheGrabber>().spellEffects;
        GameObject visual = cache[ddeSpellEffects[((AttackAbility)(effect.ability)).element]];
        GetComponent<ObjectSpawner>().CmdSpawnUnderParent(visual, gameObject);
    }

    public bool CheckForEffect(string type) {
        foreach (var effect in statusEffects) if (effect.type == type) return true;
        return false;
    }

    public void AddStatusEffect(string type, float duration, float degree = 0, Character inflicter = null, bool good = false, ActiveAbility ability = null) {
        var saveType = GetSaveType(type);
        if (inflicter != null && SavedOnEffect(saveType, type, inflicter)) return;
        var existingEffect = GetEffect(type);
        if (type!="dot" && type!="hot" && type!="mpOverTime" && existingEffect!=null) {
            if (degree>= existingEffect.degree) {
                existingEffect.duration = duration;
                existingEffect.degree = degree;
            }
            return;
        }
        if (type == "bossRage") ApplyBossRage();
        else if (type == "speed-" && GetComponent<NavMeshAgent>() != null) GetComponent<NavMeshAgent>().speed *= (1 - degree);
        var visual = GetComponent<CacheGrabber>().GetStatusEffectVisual(type, inflicter, ability);
        //if (visual != null) NetworkServer.Spawn(visual);
        //if (good) duration *= luckBuffMultiplier;
        //else duration *= luckDebuffDivisor;
        if (good) duration *= 1 + CharacterAttribute.attributes["statusEffectDuration"].instances[GetComponent<Character>()].TotalValue / 100f;
        else duration *= 1 - CharacterAttribute.attributes["statusEffectDuration"].instances[GetComponent<Character>()].TotalValue / 100f;
        var effect = new StatusEffect(type, duration, degree, inflicter: inflicter, good: good, ability: ability, visual: visual);
        statusEffects.Add(effect);
    }

    private void ApplyBossRage() {
        var c = GetComponent<Character>();
        //c.strength *= 2;
        //c.dexterity *= 2;
        //c.constitution *= 2;
        //c.intelligence *= 2;
        //c.wisdom *= 2;
        //c.luck *= 2;
        var attributes = new List<string>() { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        foreach (var attr in attributes) CharacterAttribute.attributes[attr].instances[c].BuffValue += CharacterAttribute.attributes[attr].instances[c].BaseValue;
    }

    private void RemoveBossRage() {
        var c = GetComponent<Character>();
        //c.strength /= 2;
        //c.dexterity /= 2;
        //c.constitution /= 2;
        //c.intelligence /= 2;
        //c.wisdom /= 2;
        //c.luck /= 2;
        var attributes = new List<string>() { "strength", "dexterity", "constitution", "intelligence", "wisdom", "luck" };
        foreach (var attr in attributes) CharacterAttribute.attributes[attr].instances[c].BuffValue -= CharacterAttribute.attributes[attr].instances[c].BaseValue;
    }

    private string GetSaveType(string type) {
        if (saveIsPhysical.Contains(type)) return "physical";
        else if (saveIsMental.Contains(type)) return "mental";
        return "none";
    }

    private bool SavedOnEffect(string saveType, string condition, Character inflicter) {
        var name = gameObject.name;
        if (name == "kittenCharacter(Clone)") name = "Player";
        //if (saveType == "mental" && UnityEngine.Random.Range(0f, 1f) < mentalSave) {
        if (saveType == "mental" && UnityEngine.Random.Range(0f, 1f) < CharacterAttribute.attributes["mentalResistance"].instances[GetComponent<Character>()].TotalValue) {
            GetComponent<ObjectSpawner>().CreateFloatingSaveText("RESISTED " + condition.ToUpper() + "!", name + " succeeded on mental resist vs. " + condition + "!");
            return true;
        }
        //else if (saveType == "physical" && UnityEngine.Random.Range(0f, 1f) < physicalSave) {
        if (saveType == "mental" && UnityEngine.Random.Range(0f, 1f) < CharacterAttribute.attributes["physicalResistance"].instances[GetComponent<Character>()].TotalValue) {
            GetComponent<ObjectSpawner>().CreateFloatingSaveText("RESISTED " + condition.ToUpper() + "!", name + " succeeded on physical resist vs. " + condition + "!");
            return true;
        }
        return false;
    }

    public StatusEffect GetEffect(string type) {
        foreach (var effect in statusEffects) if (effect.type == type) return effect;
        return null;
    }

    public void RemoveEffectByName(string name) {
        foreach (var effect in statusEffects) {
            if (effect.type == name) {
                if (effect.icon != null) Destroy(effect.icon);
                //if (effect.visual != null) NetworkServer.Destroy(effect.visual);
                statusEffects.Remove(effect);
                return;
            }
        }
    }

    public void RemoveAnyDebuff(ActiveAbility ability, bool eat = false, bool resetCDOnFail = true) {
        foreach (var effect in statusEffects) {
            if (!effect.good) {
                if (effect.icon != null) Destroy(effect.icon);
                //if (effect.visual != null) NetworkServer.Destroy(effect.visual);
                statusEffects.Remove(effect);
                if (eat) GetComponent<Mana>().mp = Mathf.Min(GetComponent<Mana>().maxMP, GetComponent<Mana>().mp + 50);
                return;
            }
        }
        if (resetCDOnFail) ability.currentCooldown = 0;
    }

    public void RemoveAllDebuffs(ActiveAbility ability) {
        var pruneList = new List<StatusEffect>();
        foreach (var effect in statusEffects) {
            if (!effect.good) {
                if (effect.icon != null) Destroy(effect.icon);
                //if (effect.visual != null) NetworkServer.Destroy(effect.visual);
                pruneList.Add(effect);
            }
        }
        foreach (var effect in pruneList) statusEffects.Remove(effect);
        if (pruneList.Count == 0) ability.currentCooldown = 0;
    }

    //[Command]
    public void CmdSetBlinded(bool value) {
        GetComponent<PlayerSyncer>().blinded = value;
    }

    public void RemoveStealth() {
        if (CheckForEffect("stealth")) RemoveEffectByName("stealth");
    }

    public void RemoveEffect(StatusEffect effect) {
        statusEffects.Remove(effect);
        if (effect.icon != null) Destroy(effect.icon);
        if (effect.visual != null) Destroy(effect.visual);
    }
}

