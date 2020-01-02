using System.Collections.Generic;

public class GoapPlanner {
    public Dictionary<string, object> state = new Dictionary<string, object>();

    public void Plan(GoapAgent agent) {
        float cheapestPlan = 1000000f;
        foreach (var goal in agent.goals) {
            var graph = new ActionGraph() { actions = agent.availableActions };
            foreach (var kvp in agent.state) state[kvp.Key] = kvp.Value;
            var start = new GoapActionNode() { action = null, state = state };
            var aas = new ActionAStar(graph, start, new Dictionary<string, object>() { { goal.key, goal.value } });
            var cursor = aas.finalPoint;
            if (cursor != null && cursor.action != null && aas.costSoFar.ContainsKey(cursor) && aas.costSoFar[cursor] / goal.weight < cheapestPlan) {
                cheapestPlan = aas.costSoFar[cursor] / goal.weight;
                UpdateAgentActionList(cursor, aas, agent);
            }
        }
        agent.planning = false;
    }

    private void UpdateAgentActionList(GoapActionNode cursor, ActionAStar aas, GoapAgent agent) {
        var i = 0;
        var limit = 100;
        var actionList = new List<GoapAction>();
        while (cursor != null && cursor.action != null && aas.cameFrom.ContainsKey(cursor) && i < limit) {
            i++;
            actionList.Add(cursor.action);
            cursor = aas.cameFrom[cursor];
        }
        actionList.Reverse();
        agent.currentActions = actionList;
        agent.busy = true;
        agent.planning = false;
    }
}
