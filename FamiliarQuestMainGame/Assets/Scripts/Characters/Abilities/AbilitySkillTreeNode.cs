using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AbilitySkillTreeNode {
    public List<AbilitySkillTreeNode> children = new List<AbilitySkillTreeNode>();
    public Vector2 position;
    public List<SoulGemEnhancement> effects = new List<SoulGemEnhancement>();
    public bool clickable = false;
    public bool active = false;
    public Ability ability = null;

    public AbilitySkillTreeNode() {

    }

    public AbilitySkillTreeNode(Ability ability, List<AbilitySkillTreeNode> otherNodes) {
        this.ability = ability;
        for (int i=0; i<100; i++) {
            effects = SoulGemEnhancementGenerator.Generate(ability);
            if (!DuplicateAbility(otherNodes)) return;
        }
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
}