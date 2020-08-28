using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AbilitySkillTreeNode {
    private delegate void SoulGemEnhancementDelegate(SoulGemEnhancement effect);
    public List<AbilitySkillTreeNode> children = new List<AbilitySkillTreeNode>();
    public Vector2 position;
    public List<SoulGemEnhancement> effects = new List<SoulGemEnhancement>();
    public bool clickable = false;
    public bool active = false;
    public Ability ability = null;
    private Dictionary<string, SoulGemEnhancementDelegate> enhancementLookup;

    public AbilitySkillTreeNode() {
        SetUpEnhancementDictionary();
    }

    public AbilitySkillTreeNode(Ability ability, List<AbilitySkillTreeNode> otherNodes) {
        SetUpEnhancementDictionary();
        this.ability = ability;
        for (int i=0; i<100; i++) {
            effects = SoulGemEnhancementGenerator.Generate(ability);
            if (!DuplicateAbility(otherNodes)) return;
        }
    }

    private void SetUpEnhancementDictionary() {
        enhancementLookup = new Dictionary<string, SoulGemEnhancementDelegate> {
            ["reduceDotTime"] = GetReduceDotTime,
            ["reduceCooldown"] = GetReduceCooldown,
            ["reduceMpUsage"] = GetReduceMpUsage,
            ["increaseRadius"] = GetIncreasedAttackRadius,
            ["increaseDamage"] = GetIncreaseDamage,
            ["increaseDotDamage"] = GetIncreaseDotDamage,
            ["increasedDegree"] = GetIncreasedParam,
            ["increasedDuration"] = GetIncreasedParam,
            ["increasedRadius"] = GetIncreasedParam,
            ["decreaseDelay"] = GetDecreasedParam,
            ["addAttribute"] = GetAddAttribute,
            ["activeLatentAttribute"] = GetActivateAttribute,
            ["removeDrawback"] = GetRemoveDrawback
        };
    }

    private bool DuplicateAbility(List<AbilitySkillTreeNode> otherNodes) {
        foreach (var node in otherNodes) {
            if (effects.Count != node.effects.Count) continue;
            else {
                for (int i=0; i<effects.Count; i++) {
                    if (!SameEffect(effects[i], node.effects[i])) continue;
                    return true;
                }
            }
        }
        return false;
    }

    private bool SameEffect(SoulGemEnhancement effect1, SoulGemEnhancement effect2) {
        if (effect1.effect != effect2.effect) return false;
        if (effect1.generalType != effect2.generalType) return false;
        if (effect1.subTarget != effect2.subTarget) return false;
        if (effect1.target != effect2.target) return false;
        if (effect1.type != effect2.type) return false;
        return true;
    }

    public void Activate() {
        foreach (var effect in effects) if (enhancementLookup.ContainsKey(effect.type)) enhancementLookup[effect.type](effect);
        ability.description = AbilityDescriber.Describe(ability);
        PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void GetReduceDotTime(SoulGemEnhancement effect) {
        if (ability is AttackAbility attackAbility) attackAbility.dotTime += effect.effect;
    }

    private void GetReduceCooldown(SoulGemEnhancement effect) {
        if (ability is ActiveAbility activeAbility) activeAbility.cooldown += effect.effect;
    }

    private void GetReduceMpUsage(SoulGemEnhancement effect) {
        if (ability is ActiveAbility activeAbility) {
            var oldMpUsage = activeAbility.mpUsage;
            activeAbility.mpUsage -= effect.effect;
            activeAbility.baseMpUsage *= activeAbility.mpUsage / oldMpUsage;
        }
    }

    private void GetIncreasedAttackRadius(SoulGemEnhancement effect) {
        if (ability is AttackAbility attackAbility) attackAbility.radius += effect.effect;
    }

    private void GetIncreaseDamage(SoulGemEnhancement effect) {
        if (ability is AttackAbility attackAbility) attackAbility.damage += effect.effect;
    }

    private void GetIncreaseDotDamage(SoulGemEnhancement effect) {
        if (ability is AttackAbility attackAbility) attackAbility.dotDamage += effect.effect;
    }

    private void GetIncreasedParam(SoulGemEnhancement effect) {
        var attribute = ability.FindAttribute(effect.target);
        if (attribute == null) return;
        var parameter = attribute.FindParameter(effect.subTarget);
        if (parameter == null) return;
        parameter.value = (float)parameter.value + effect.effect;
    }

    private void GetDecreasedParam(SoulGemEnhancement effect) {
        var attribute = ability.FindAttribute(effect.target);
        if (attribute == null) return;
        var parameter = attribute.FindParameter(effect.subTarget);
        if (parameter == null) return;
        parameter.value = (float)parameter.value - effect.effect;
    }

    private void GetAddAttribute(SoulGemEnhancement effect) {
        for (int i=0; i<10000; i++) {
            var attribute = AbilityAttributeGenerator.Generate(ability);
            if (attribute.type==effect.target && attribute.priority >= 50) {
                ability.attributes.Add(attribute);
                return;
            }
        }
    }

    private void GetActivateAttribute(SoulGemEnhancement effect) {
        var attribute = ability.FindAttribute(effect.target);
        if (attribute != null) {
            attribute.priority = RNG.Float(50f, 100f);
            ability.SortAttributes();
        }
    }

    private void GetRemoveDrawback(SoulGemEnhancement effect) {
        var attribute = ability.FindAttribute(effect.target);
        if (attribute!=null) ability.attributes.Remove(attribute);
    }
}