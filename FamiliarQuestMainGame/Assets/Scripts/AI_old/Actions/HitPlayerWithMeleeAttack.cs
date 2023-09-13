using System.Collections.Generic;

namespace AI.Actions {
    public class HitPlayerWithMeleeAttack : GoapAction {

        MonsterBaseAbilities monsterBaseAbilities = null;
        AbilityUser abilityUser = null;
        private MonsterAnimationController monsterAnimationController = null;

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
            if (monsterAnimationController == null) monsterAnimationController = agent.GetComponent<MonsterAnimationController>();
            if (monsterBaseAbilities == null) monsterBaseAbilities = agent.GetComponent<MonsterBaseAbilities>();
            if (abilityUser == null) abilityUser = agent.GetComponent<AbilityUser>();
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
            if (monsterBaseAbilities == null || abilityUser == null) return;
            foreach (var ability in monsterBaseAbilities.baseAbilities) {
                if (IsMeleeAbility(ability) && ability.currentCooldown == 0) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
            foreach (var ability in abilityUser.soulGemActives) {
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
