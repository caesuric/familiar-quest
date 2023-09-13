using System.Collections.Generic;

namespace AI.Actions {
    public class HitPlayerWithBossRangedAttack : GoapAction {

        Boss boss = null;
        private MonsterAnimationController monsterAnimationController = null;

        public HitPlayerWithBossRangedAttack() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "bossRangedAttackAvailable", true },
                { "gcdReady", true },
                { "facingPlayer", true },
                { "facingPlayerPrecisely", true },
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
            UseRangedAbility(agent);
            ApplyEffects(agent);
        }

        private void UseRangedAbility(GoapAgent agent) {
            if (boss == null) return;
            foreach (var ability in boss.bossAbilities) {
                if (IsRangedAbility(ability) && ability.currentCooldown == 0) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
        }

        private bool IsRangedAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var attackAbility = ability as AttackAbility;
                if (attackAbility.isRanged) return true;
            }
            return false;
        }
    }
}
