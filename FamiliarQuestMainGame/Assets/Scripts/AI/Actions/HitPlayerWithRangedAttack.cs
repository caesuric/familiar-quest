using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class HitPlayerWithRangedAttack : GoapAction {

        MonsterBaseAbilities mba = null;
        SpiritUser su = null;
        private MonsterAnimationController mac = null;

        public HitPlayerWithRangedAttack() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "rangedAttackAvailable", true },
                { "gcdReady", true },
                { "facingPlayer", true },
                { "facingPlayerPrecisely", true },
                { "playerAlive", true }
            };
            effects = new Dictionary<string, object>() {
                { "playerHurt", true },
                { "gcdReady", false }
            };
            cost = 2f;
        }

        public override void Execute(GoapAgent agent) {
            if (mac == null) mac = agent.GetComponent<MonsterAnimationController>();
            if (mba==null) mba = agent.GetComponent<MonsterBaseAbilities>();
            if (su == null) su = agent.GetComponent<SpiritUser>();
            if (!ActionPossible(agent)) {
                mac.attacking = false;
                Fail(agent);
            }
            else {
                mac.attacking = true;
                HitPlayer(agent);
            }
        }

        private void HitPlayer(GoapAgent agent) {
            UseRangedAbility(agent);
            ApplyEffects(agent);
        }

        private void UseRangedAbility(GoapAgent agent) {
            if (mba == null || su == null) return;
            foreach (var ability in mba.baseAbilities) {
                if (IsRangedAbility(ability)) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
            foreach (var spirit in su.spirits) {
                foreach (var ability in spirit.activeAbilities) {
                    if (IsRangedAbility(ability)) {
                        agent.GetComponent<AbilityUser>().UseAbility(ability);
                        return;
                    }
                }
            }
        }

        private bool IsRangedAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var aa = ability as AttackAbility;
                if (aa.isRanged) return true;
            }
            return false;
        }
    }
}
