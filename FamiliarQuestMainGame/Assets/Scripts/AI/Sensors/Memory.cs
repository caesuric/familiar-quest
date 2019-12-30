using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AI.Sensors {
    public class Memory : GoapSensor {

        public override void Run(GoapAgent agent) {
            if (!agent.memory.ContainsKey("characters")) agent.memory["characters"] = new Data.MemoryOfCharacters();
            var moc = agent.memory["characters"] as Data.MemoryOfCharacters;
            var rememberAlive = false;
            foreach (var memory in moc.memories) {
                if (memory.isEnemy && memory.character.GetComponent<Health>().hp > 0) {
                    rememberAlive = true;
                    break;
                }
            }
            if (!rememberAlive && !agent.state["seePlayer"].Equals(true)) agent.state["playerAlive"] = false;
        }
    }
}
