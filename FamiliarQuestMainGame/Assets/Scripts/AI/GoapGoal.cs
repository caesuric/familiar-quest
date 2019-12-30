using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class GoapGoal {
    public string key = "";
    public object value = null;
    public float weight = 1f;

    public GoapGoal(float weight = 1) {
        this.weight = weight;
    }
}
