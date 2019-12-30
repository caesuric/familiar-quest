using UnityEngine;
using System.Collections;

public class AbilityParameter {
    public string name;
    public DataType type;
    public int intVal;
    public float floatVal;
    public string stringVal;
    public AbilityParameter(string name, DataType type, int intVal = 0, float floatVal = 0, string stringVal = "") {
        this.name = name;
        this.type = type;
        this.intVal = intVal;
        this.floatVal = floatVal;
        this.stringVal = stringVal;
    }

    public AbilityParameter Copy() {
        return new AbilityParameter(name, type, intVal, floatVal, stringVal);
    }
}
