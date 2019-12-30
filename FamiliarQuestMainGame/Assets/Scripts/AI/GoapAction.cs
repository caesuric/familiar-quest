using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GoapAction {
    protected GameObject target = null;
    protected float cost = 1f;
    public bool isDone = false;
    protected Dictionary<string, object> preconditions = new Dictionary<string, object>();
    protected Dictionary<string, object> effects = new Dictionary<string, object>();

    public float GetCost() {
        return cost;
    }

    public GameObject GetTarget() {
        return target;
    }

    public void Reset() {
        target = null;
        isDone = false;
    }

    public abstract void Execute(GoapAgent agent);

    public bool ActionPossible(GoapAgent agent) {
        foreach (var kvp in preconditions) {
            var key = kvp.Key;
            var value = kvp.Value;
            if (!agent.state.ContainsKey(key)) return false;
            if (!agent.state[key].Equals(value)) return false;
        }
        return true;
    }

    protected void ApplyEffects(GoapAgent agent) {
        foreach (var kvp in effects) {
            var key = kvp.Key;
            var value = kvp.Value;
            agent.state[key] = value;
        }
        isDone = true;
        agent.busy = false;
    }

    public bool PlanExecution(GoapPlanner planner) {
        if (ActionPlanPossible(planner)) {
            ApplyPlanEffects(planner);
            return true;
        }
        else return false;
    }

    private bool ActionPlanPossible(GoapPlanner planner) {
        foreach (var kvp in preconditions) {
            var key = kvp.Key;
            var value = kvp.Value;
            if (!planner.state.ContainsKey(key) || !planner.state[key].Equals(value)) return false;
        }
        return true;
    }

    private void ApplyPlanEffects(GoapPlanner planner) {
        foreach (var kvp in effects) {
            var key = kvp.Key;
            var value = kvp.Value;
            planner.state[key] = value;
        }
    }

    public void ApplyEffectsDirectly(Dictionary<string, object> state) {
        foreach (var kvp in effects) {
            var key = kvp.Key;
            var value = kvp.Value;
            state[key] = value;
        }
    }

    public bool AStarActionPossible(Dictionary<string, object> state) {
        foreach (var kvp in preconditions) {
            var key = kvp.Key;
            var value = kvp.Value;
            if (!state.ContainsKey(key)) return false;
            if (!state[key].Equals(value)) return false;
        }
        return true;
    }

    protected void Fail(GoapAgent agent) {
        isDone = true;
        agent.busy = false;
    }
}
