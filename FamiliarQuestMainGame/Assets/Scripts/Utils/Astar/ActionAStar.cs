using System.Collections.Generic;

public class ActionGraph {
    public List<GoapAction> actions;

    public float Cost(GoapActionNode a, GoapActionNode b) {
        if (b.action != null) return b.action.GetCost();
        else return 0;
    }

    public IEnumerable<GoapActionNode> Neighbors(GoapActionNode a) {
        foreach (var action in actions) {
            if (action.ActionPossible(a.state)) {
                var gan = new GoapActionNode() {
                    action = action,
                    state = CopyState(a.state)
                };
                if (action != null) action.ApplyEffectsDirectly(gan.state);
                yield return gan;
            }
        }
    }

    private Dictionary<string, object> CopyState(Dictionary<string, object> originalState) {
        var state = new Dictionary<string, object>();
        foreach (var kvp in originalState) state[kvp.Key] = kvp.Value;
        return state;
    }
}

public class GoapActionNode : Priority_Queue.FastPriorityQueueNode {
    public GoapAction action = null;
    public Dictionary<string, object> state = new Dictionary<string, object>();

    public static bool operator ==(GoapActionNode gan1, GoapActionNode gan2) {
        if (ReferenceEquals(gan1, null)) {
            return ReferenceEquals(gan2, null);
        }
        if (ReferenceEquals(gan2, null)) {
            return ReferenceEquals(gan1, null);
        }
        if (gan1.action == null || gan2.action == null) return (gan1.action == gan2.action) && gan1.StateMatches(gan2);
        return (gan1.action.Equals(gan2.action) && gan1.StateMatches(gan2));
    }

    public static bool operator !=(GoapActionNode gan1, GoapActionNode gan2) {
        if (ReferenceEquals(gan1, null)) {
            return !ReferenceEquals(gan2, null);
        }
        if (ReferenceEquals(gan2, null)) {
            return !ReferenceEquals(gan1, null);
        }
        return (!gan1.action.Equals(gan2.action) || !gan1.StateMatches(gan2));
    }

    public override bool Equals(object o) {
        var item = o as GoapActionNode;
        if (o == null) return false;
        return (this == item);
    }

    private bool StateMatches(GoapActionNode gan2) {
        foreach (var kvp in state) {
            if (!gan2.state.ContainsKey(kvp.Key)) return false;
            if (!gan2.state[kvp.Key].Equals(kvp.Value)) return false;
        }
        foreach (var kvp in gan2.state) {
            if (!state.ContainsKey(kvp.Key)) return false;
            if (!state[kvp.Key].Equals(kvp.Value)) return false;
        }
        return true;
    }

    public override int GetHashCode() {
        var hashCode = 629302477;
        hashCode = hashCode * -1521134295 + EqualityComparer<GoapAction>.Default.GetHashCode(action);
        hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, object>>.Default.GetHashCode(state);
        return hashCode;
    }
}

public class ActionAStar {
    public Dictionary<GoapActionNode, GoapActionNode> cameFrom = new Dictionary<GoapActionNode, GoapActionNode>();
    public Dictionary<GoapActionNode, float> costSoFar = new Dictionary<GoapActionNode, float>();
    public GoapActionNode finalPoint = null;

    public static float Heuristic(GoapActionNode a, Dictionary<string, object> b) {
        int wrongStates = 0;
        foreach (var kvp in b) if (!a.state[kvp.Key].Equals(kvp.Value)) wrongStates++;
        return wrongStates;
    }

    public ActionAStar(ActionGraph graph, GoapActionNode start, Dictionary<string, object> goal) {
        var frontier = new Priority_Queue.FastPriorityQueue<GoapActionNode>(10000);
        frontier.Enqueue(start, 0);
        cameFrom[start] = start;
        costSoFar[start] = 0;
        while (frontier.Count > 0) {
            var current = frontier.Dequeue();
            if (MeetsGoal(current, goal)) {
                finalPoint = current;
                break;
            }
            foreach (var next in graph.Neighbors(current)) {
                float newCost = costSoFar[current] + graph.Cost(current, next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                    costSoFar[next] = newCost;
                    float priority = newCost + Heuristic(next, goal);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }
    }

    private bool MeetsGoal(GoapActionNode current, Dictionary<string, object> goal) {
        foreach (var kvp in goal) if (!current.state.ContainsKey(kvp.Key) || !current.state[kvp.Key].Equals(kvp.Value)) return false;
        return true;
    }
}