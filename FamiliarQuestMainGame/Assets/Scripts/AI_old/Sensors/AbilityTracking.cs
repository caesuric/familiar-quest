namespace AI.Sensors {
    public class AbilityTracking : GoapSensor {

        AbilityUser abilityUser = null;
        MonsterBaseAbilities monsterBaseAbilities = null;

        public override void Run(GoapAgent agent) {
            if (abilityUser == null) abilityUser = agent.GetComponent<AbilityUser>();
            if (monsterBaseAbilities == null) monsterBaseAbilities = agent.GetComponent<MonsterBaseAbilities>();
            agent.state["meleeAttackAvailable"] = IsMeleeAttackAvailable();
            agent.state["rangedAttackAvailable"] = IsRangedAttackAvailable();
        }

        private bool IsMeleeAttackAvailable() {
            foreach (var ability in abilityUser.soulGemActives) {
                if (IsMeleeAbility(ability)) return true;
            }
            foreach (var ability in monsterBaseAbilities.baseAbilities) {
                if (IsMeleeAbility(ability) && ability.currentCooldown == 0) return true;
            }
            return false;
        }

        private bool IsRangedAttackAvailable() {
            foreach (var ability in abilityUser.soulGemActives) {
                if (IsRangedAbility(ability)) return true;
            }
            foreach (var ability in monsterBaseAbilities.baseAbilities) {
                if (IsRangedAbility(ability) && ability.currentCooldown == 0) return true;
            }
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
    }
}
