using System.Collections.Generic;

namespace AI.Actions {
    public class HitPlayerWithBossMeleeAttack : GoapAction {

        private Boss boss = null;
        private MonsterAnimationController monsterAnimationController = null;

        public HitPlayerWithBossMeleeAttack() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "inMeleeRangeOfPlayer", true },
                { "bossMeleeAttackAvailable", true },
                { "gcdReady", true },
                { "facingPlayer", true },
                { "playerAlive", true }
            };
            effects = new Dictionary<string, object>() {
                { "playerHurt", true },
                { "gcdReady", false }
            };
            cost = 1f;
        }

        public override void Execute(GoapAgent agent) {
            if (monsterAnimationController == null) monsterAnimationController = agent.GetComponent<MonsterAnimationController>();
            if (boss == null) boss = agent.GetComponent<Boss>();
            if (!ActionPossible(agent)) {
                monsterAnimationController.attacking = false;
                Fail(agent);
            }
            else {
                monsterAnimationController.attacking = true;
                HitPlayer(agent);
            }
        }

        private void HitPlayer(GoapAgent agent) {
            UseMeleeAbility(agent);
            ApplyEffects(agent);
        }

        private void UseMeleeAbility(GoapAgent agent) {
            if (boss == null) return;
            foreach (var ability in boss.bossAbilities) {
                if (IsMeleeAbility(ability) && ability.currentCooldown == 0) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
        }

        private bool IsMeleeAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var attackAbility = ability as AttackAbility;
                if (attackAbility.isRanged == false) return true;
            }
            return false;
        }
    }
}
