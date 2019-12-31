using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class GoapAgent : MonoBehaviour {
    public List<GoapGoal> goals = new List<GoapGoal>();
    public Dictionary<string, object> state = new Dictionary<string, object>();
    public Dictionary<string, object> memory = new Dictionary<string, object>();
    public bool busy = false;
    public bool planning = false;
    public List<GoapAction> availableActions = new List<GoapAction>();
    public List<GoapAction> currentActions = new List<GoapAction>();
    public List<GoapSensor> sensors = new List<GoapSensor>();
    public GoapPlanner planner = new GoapPlanner();
    public string currentAction = "";
    public Task plannerTask = null;

    public void Update() {
        foreach (var sensor in sensors) sensor.Run(this);
        if (!busy && !planning) {
            planning = true;
            plannerTask = new Task(() => planner.Plan(this));
            plannerTask.Start();
        }
        else if (!planning) {
            ExecutePlan();
        }
        else {
            Debug.Log("planning");
            currentAction = "Planning...";
        }
    }

    private void ExecutePlan() {
        if (currentActions.Count == 0) {
            busy = false;
            return;
        }
        Debug.Log("Executing action: " + currentActions[0].GetType().Name);
        currentAction = currentActions[0].GetType().Name;
        currentActions[0].Execute(this);
        if (currentActions[0].isDone) currentActions.RemoveAt(0);
    }
}
