using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AI.Sensors {
    public class AbilityTracking : GoapSensor {

        SpiritUser spiritUser = null;
        MonsterBaseAbilities mba = null;

        public override void Run(GoapAgent agent) {
            if (spiritUser == null) spiritUser = agent.GetComponent<SpiritUser>();
            if (mba == null) mba = agent.GetComponent<MonsterBaseAbilities>();
            agent.state["meleeAttackAvailable"] = IsMeleeAttackAvailable();
            agent.state["rangedAttackAvailable"] = IsRangedAttackAvailable();
        }

        private bool IsMeleeAttackAvailable() {
            foreach (var spirit in spiritUser.spirits) {
                foreach (var ability in spirit.activeAbilities) {
                    if (IsMeleeAbility(ability)) return true;
                }
            }
            foreach (var ability in mba.baseAbilities) {
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
            foreach (var ability in mba.baseAbilities) {
                if (IsRangedAbility(ability)) return true;
            }
            return false;
        }

        private bool IsMeleeAbility(ActiveAbility ability) {
            if (ability is AttackAbility) {
                var aa = ability as AttackAbility;
                if (aa.isRanged == false) return true;
            }
            return false;
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
