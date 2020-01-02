using System.Collections.Generic;

namespace AI.Actions {
    public class WaitForGcd : GoapAction {

        private MonsterAnimationController monsterAnimationController = null;

        public WaitForGcd() {
            preconditions = new Dictionary<string, object>() {
                { "gcdReady", false }
            };
            effects = new Dictionary<string, object>() {
                { "gcdReady", true }
            };
        }

        public override void Execute(GoapAgent agent) {
            if (monsterAnimationController == null) monsterAnimationController = agent.GetComponent<MonsterAnimationController>();
            monsterAnimationController.attacking = true;
            if (!ActionPossible(agent)) {
                monsterAnimationController.attacking = false;
                Fail(agent);
            }
            else if (agent.state["gcdReady"].Equals(true)) ApplyEffects(agent);
        }
    }
}
