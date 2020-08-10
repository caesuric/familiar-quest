using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class MoveToPlayer : GoapAction {

        private bool started = false;
        private NavMeshAgent navMeshAgent = null;
        private MonsterAnimationController monsterAnimationController = null;

        public MoveToPlayer() {
            preconditions = new Dictionary<string, object>() {
                { "seePlayer", true },
                { "inMeleeRangeOfPlayer", false },
                { "playerAlive", true }
            };
            effects = new Dictionary<string, object>() {
                { "inMeleeRangeOfPlayer", true },
                { "facingPlayer", true }
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
            SelectTarget(agent);
            if (target == null) return;
            started = true;
            isDone = false;
            if (navMeshAgent == null) navMeshAgent = agent.GetComponent<NavMeshAgent>();
            SetDestination();
        }

        private void ContinueAction(GoapAgent agent) {
            monsterAnimationController.moving = true;
            var distanceBetweenDestinationAndTarget = Vector3.Distance(navMeshAgent.destination, target.transform.position);
            var distanceToTarget = Vector3.Distance(agent.transform.position, navMeshAgent.destination);
            if (!navMeshAgent.pathPending && distanceBetweenDestinationAndTarget > 0.25f) SetDestination();
            if (agent.state["inMeleeRangeOfPlayer"].Equals(true)) {
                ApplyEffects(agent);
                started = false;
                monsterAnimationController.moving = false;
            }
            cost = 1f + (distanceToTarget / 2f);
        }

        private void SelectTarget(GoapAgent agent) {
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            if (fov.players.Count > 0) target = fov.players[0];
            else target = null;
        }

        private void SetDestination() {
            if (navMeshAgent.isOnNavMesh) navMeshAgent.SetDestination(target.transform.position);
        }
    }
}
