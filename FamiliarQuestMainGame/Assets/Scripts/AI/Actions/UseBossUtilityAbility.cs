using System.Collections.Generic;

namespace AI.Actions {
    public class UseBossUtilityAbility : GoapAction {

        Boss boss = null;
        private MonsterAnimationController monsterAnimationController = null;

        public UseBossUtilityAbility() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "bossUtilityAbilityAvailable", true },
                { "gcdReady", true },
                { "playerAlive", true }
            };
            effects = new Dictionary<string, object>() {
                { "playerHurt", true },
                { "gcdReady", false }
            };
            cost = 0.5f;
        }

        public override void Execute(GoapAgent agent) {
            if (monsterAnimationController == null) monsterAnimationController = agent.GetComponent<MonsterAnimationController>();
            if (boss == null) boss = agent.GetComponent<Boss>();
            if (!ActionPossible(agent)) Fail(agent);
            else UseAbility(agent);
        }

        private void UseAbility(GoapAgent agent) {
            UseUtilityAbility(agent);
            ApplyEffects(agent);
        }

        private void UseUtilityAbility(GoapAgent agent) {
            if (boss == null) return;
            foreach (var ability in boss.bossAbilities) {
                if (IsUtilityAbility(ability) && ability.currentCooldown == 0) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
        }

        private bool IsUtilityAbility(ActiveAbility ability) {
            if (ability is UtilityAbility) return true;
            return false;
        }
    }
}
