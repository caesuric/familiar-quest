using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions {
    public class PursuePlayer : GoapAction {

        private bool started = false;
        private NavMeshAgent navMeshAgent = null;
        private MonsterAnimationController mac = null;

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
            if (mac == null) mac = agent.GetComponent<MonsterAnimationController>();
            if (!ActionPossible(agent)) {
                started = false;
                Fail(agent);
                mac.moving = false;
                return;
            }
            if (!started) {
                if (navMeshAgent == null) navMeshAgent = agent.GetComponent<NavMeshAgent>();
                var charMems = agent.memory["characters"] as Data.MemoryOfCharacters;
                var playerMem = charMems.GetClosestPlayerMemory(agent);
                if (playerMem==null) {
                    Fail(agent);
                    return;
                }
                started = true;
                isDone = false;
                navMeshAgent.SetDestination(playerMem.position);
            }
            mac.moving = true;
            if (started && Vector3.Distance(agent.transform.position, navMeshAgent.destination)<1f) {
                agent.busy = false;
                started = false;
                mac.moving = false;
            }

            var distance = Vector3.Distance(agent.transform.position, navMeshAgent.destination);
            cost = 1f + (distance / 2f);
        }
    }
}
