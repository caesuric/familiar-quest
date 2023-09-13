using UnityEngine;

namespace AI.Sensors {
    public class BossAbilityTracking : GoapSensor {

        Boss boss = null;

        public override void Run(GoapAgent agent) {
            if (boss == null) boss = agent.GetComponent<Boss>();
            agent.state["bossMeleeAttackAvailable"] = IsMeleeAttackAvailable();
            agent.state["bossRangedAttackAvailable"] = IsRangedAttackAvailable();
            agent.state["bossUtilityAbilityAvailable"] = IsUtilityAbilityAvailable();
        }

        private bool IsMeleeAttackAvailable() {
            foreach (var ability in boss.bossAbilities) if (IsMeleeAbility(ability) && ability.currentCooldown == 0) return true;
            return false;
        }

        private bool IsRangedAttackAvailable() {
            foreach (var ability in boss.bossAbilities) if (IsRangedAbility(ability) && ability.currentCooldown == 0) return true;
            return false;
        }

        private bool IsUtilityAbilityAvailable() {
            foreach (var ability in boss.bossAbilities) if (IsUtilityAbility(ability) && ability.currentCooldown == 0) return true;
            return false;
        }

        private bool IsMeleeAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var attackAbility = ability as AttackAbility;
                if (attackAbility.isRanged == false) return true;
            }
            return false;
        }

        private bool IsRangedAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var attackAbility = ability as AttackAbility;
                if (attackAbility.isRanged || attackAbility.FindAttribute("chargeTowards") != null) return true;
            }
            return false;
        }

        private bool IsUtilityAbility(ActiveAbility ability) {
            if (ability is UtilityAbility) return true;
            return false;
        }
    }
}
