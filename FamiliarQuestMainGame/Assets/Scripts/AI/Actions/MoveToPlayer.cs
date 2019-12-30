using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class MoveToPlayer : GoapAction {

        private bool started = false;
        private NavMeshAgent navMeshAgent = null;
        private MonsterAnimationController mac = null;

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
            if (mac == null) mac = agent.GetComponent<MonsterAnimationController>();
            if (!ActionPossible(agent)) {
                started = false;
                Fail(agent);
                mac.moving = false;
                return;
            }
            if (!started) {
                SelectTarget(agent);
                if (target == null) return;
                started = true;
                isDone = false;
                if (navMeshAgent == null) navMeshAgent = agent.GetComponent<NavMeshAgent>();
                SetDestination();
            }
            mac.moving = true;
            var distanceBetweenDestinationAndTarget = Vector3.Distance(navMeshAgent.destination, target.transform.position);
            var distanceToTarget = Vector3.Distance(agent.transform.position, navMeshAgent.destination);
            if (!navMeshAgent.pathPending && distanceBetweenDestinationAndTarget > 0.25f) SetDestination();
            if (agent.state["inMeleeRangeOfPlayer"].Equals(true)) {
                ApplyEffects(agent);
                started = false;
                mac.moving = false;
            }
            cost = 1f + (distanceToTarget / 2f);
        }

        private void SelectTarget(GoapAgent agent) {
            var fov = agent.memory["fieldOfVision"] as Data.FieldOfVision;
            if (fov.players.Count > 0) target = fov.players[0];
            else target = null;
        }

        private void SetDestination() {
            navMeshAgent.SetDestination(target.transform.position);
        }
    }
}
