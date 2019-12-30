using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI.Goals {
    public class HurtPlayer : GoapGoal {
        public HurtPlayer(float weight = 1) {
            key = "playerHurt";
            value = true;
            this.weight = weight;
        }
    }
}