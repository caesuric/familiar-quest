using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class WaitForGcd : GoapAction {

        private MonsterAnimationController mac = null;

        public WaitForGcd() {
            preconditions = new Dictionary<string, object>() {
                { "gcdReady", false }
            };
            effects = new Dictionary<string, object>() {
                { "gcdReady", true }
            };
        }

        public override void Execute(GoapAgent agent) {
            if (mac == null) mac = agent.GetComponent<MonsterAnimationController>();
            mac.attacking = true;
            if (!ActionPossible(agent)) {
                mac.attacking = false;
                Fail(agent);
            }
            else if (agent.state["gcdReady"].Equals(true)) ApplyEffects(agent);
        }
    }
}
