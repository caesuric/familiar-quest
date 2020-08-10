using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class PursuePlayer : GoapAction {

        private bool started = false;
        private NavMeshAgent navMeshAgent = null;
        private MonsterAnimationController monsterAnimationController = null;

        public PursuePlayer() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", false },
                { "haveSeenPlayer", true },
                { "playerAlive", true }
            };
            effects = new Dictionary<string, object>() {
                { "seePlayer", true }
            };
        }

        public override void Execute(GoapAgent agent) {
            if (monsterAnimationController == null) monsterAnimationController = agent.GetComponent<MonsterAnimationController>();
            if (!ActionPossible(agent)) {
                FailAction(agent);
                return;
            }
            if (!started) StartAction(agent);
            ContinueAction(agent);
        }

        private void FailAction(GoapAgent agent) {
            started = false;
            Fail(agent);
            monsterAnimationController.moving = false;
        }

        private void StartAction(GoapAgent agent) {
            if (navMeshAgent == null) navMeshAgent = agent.GetComponent<NavMeshAgent>();
            var charMems = agent.memory["characters"] as Data.MemoryOfCharacters;
            var playerMem = charMems.GetClosestPlayerMemory(agent);
            if (playerMem == null) {
                Fail(agent);
                return;
            }
            started = true;
            isDone = false;
            if (navMeshAgent.isOnNavMesh) navMeshAgent.SetDestination(playerMem.position);
        }

        private void ContinueAction(GoapAgent agent) {
            monsterAnimationController.moving = true;
            if (started && Vector3.Distance(agent.transform.position, navMeshAgent.destination) < 1f) {
                agent.busy = false;
                started = false;
                monsterAnimationController.moving = false;
            }
            var distance = Vector3.Distance(agent.transform.position, navMeshAgent.destination);
            cost = 1f + (distance / 2f);
        }
    }
}
