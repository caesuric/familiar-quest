using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class HitPlayerWithMeleeAttack : GoapAction {

        MonsterBaseAbilities mba = null;
        SpiritUser su = null;
        private MonsterAnimationController mac = null;

        public HitPlayerWithMeleeAttack() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "inMeleeRangeOfPlayer", true },
                { "meleeAttackAvailable", true },
                { "gcdReady", true },
                { "facingPlayer", true },
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
            UseMeleeAbility(agent);
            ApplyEffects(agent);
        }

        private void UseMeleeAbility(GoapAgent agent) {
            if (mba == null || su == null) return;
            foreach (var ability in mba.baseAbilities) {
                if (IsMeleeAbility(ability)) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
            foreach (var spirit in su.spirits) {
                foreach (var ability in spirit.activeAbilities) {
                    if (IsMeleeAbility(ability)) {
                        agent.GetComponent<AbilityUser>().UseAbility(ability);
                        return;
                    }
                }
            }
        }

        private bool IsMeleeAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var aa = ability as AttackAbility;
                if (aa.isRanged == false) return true;
            }
            return false;
        }
    }
}
