namespace AI.Sensors {
    public class AbilityTracking : GoapSensor {

        SpiritUser spiritUser = null;
        MonsterBaseAbilities monsterBaseAbilities = null;

        public override void Run(GoapAgent agent) {
            if (spiritUser == null) spiritUser = agent.GetComponent<SpiritUser>();
            if (monsterBaseAbilities == null) monsterBaseAbilities = agent.GetComponent<MonsterBaseAbilities>();
            agent.state["meleeAttackAvailable"] = IsMeleeAttackAvailable();
            agent.state["rangedAttackAvailable"] = IsRangedAttackAvailable();
        }

        private bool IsMeleeAttackAvailable() {
            foreach (var spirit in spiritUser.spirits) {
                foreach (var ability in spirit.activeAbilities) {
                    if (IsMeleeAbility(ability)) return true;
                }
            }
            foreach (var ability in monsterBaseAbilities.baseAbilities) {
                if (IsMeleeAbility(ability)) return true;
            }
            return false;
        }

        private bool IsRangedAttackAvailable() {
            foreach (var spirit in spiritUser.spirits) {
                foreach (var ability in spirit.activeAbilities) {
                    if (IsRangedAbility(ability)) return true;
                }
            }
            foreach (var ability in monsterBaseAbilities.baseAbilities) {
                if (IsRangedAbility(ability)) return true;
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
                if (attackAbility.isRanged) return true;
            }
            return false;
        }
    }
}
