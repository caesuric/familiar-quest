using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AI.Sensors {
    public class GcdTracking : GoapSensor {
        public override void Run(GoapAgent agent) {
            agent.state["gcdReady"] = GetGcdState(agent);
        }

        private bool GetGcdState(GoapAgent agent) {
            if (agent.GetComponent<AbilityUser>().GCDTime > 0) return false;
            return true;
        }
    }
}
