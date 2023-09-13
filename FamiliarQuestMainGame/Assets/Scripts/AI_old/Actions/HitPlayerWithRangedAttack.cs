using System.Collections.Generic;

namespace AI.Actions {
    public class HitPlayerWithRangedAttack : GoapAction {

        MonsterBaseAbilities monsterBaseAbilities = null;
        AbilityUser abilityUser = null;
        private MonsterAnimationController monsterAnimationController = null;

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
            UseRangedAbility(agent);
            ApplyEffects(agent);
        }

        private void UseRangedAbility(GoapAgent agent) {
            if (monsterBaseAbilities == null || abilityUser == null) return;
            foreach (var ability in monsterBaseAbilities.baseAbilities) {
                if (IsRangedAbility(ability) && ability.currentCooldown == 0) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
            foreach (var ability in abilityUser.soulGemActives) {
                if (IsRangedAbility(ability) && ability.currentCooldown == 0) {
                    agent.GetComponent<AbilityUser>().UseAbility(ability);
                    return;
                }
            }
        }

        private bool IsRangedAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var attackAbility = ability as AttackAbility;
                if (attackAbility.isRanged || attackAbility.FindAttribute("chargeTowards") != null) return true;
            }
            return false;
        }
    }
}
