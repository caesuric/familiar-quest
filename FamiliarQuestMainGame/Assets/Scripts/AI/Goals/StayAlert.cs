using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Goals {
    public class StayAlert : GoapGoal {
        public StayAlert(float weight = 1) {
            key = "awareOfSurroundings";
            value = true;
            this.weight = weight;
        }
    }
}