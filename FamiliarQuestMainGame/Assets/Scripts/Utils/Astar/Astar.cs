using System;
using System.Collections.Generic;
using UnityEngine;

class Astar {

    public Dictionary<Coordinates, Coordinates> cameFrom = new Dictionary<Coordinates, Coordinates>();
    public Dictionary<Coordinates, float> costSoFar = new Dictionary<Coordinates, float>();
    public Coordinates end = null;

    public Dictionary<Coordinates, Coordinates> SearchUnfilled(Graph graph, Coordinates start, Coordinates end) {
        var frontier = new Priority_Queue.FastPriorityQueue<Coordinates>(10000);
        frontier.Enqueue(start, 0);
        cameFrom[start] = start;
        costSoFar[start] = 0;
        while (frontier.Count > 0) {
            var current = frontier.Dequeue();
            if (current == end) {
                this.end = current;
                break;
            }
            foreach (var next in graph.Neighbors(current)) {
                float newCost = costSoFar[current] + graph.Cost(next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                    costSoFar[next] = newCost;
                    float priority = newCost + Heuristic(next, end);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }
        return cameFrom;
    }

    public Dictionary<Coordinates, Coordinates> SearchThroughCorridors(InverseGraph graph, Coordinates start, Coordinates end) {
        var frontier = new Priority_Queue.FastPriorityQueue<Coordinates>(10000);
        frontier.Enqueue(start, 0);
        cameFrom[start] = start;
        costSoFar[start] = 0;
        while (frontier.Count > 0) {
            var current = frontier.Dequeue();
            if (current == end) {
                this.end = current;
                break;
            }
            foreach (var next in graph.Neighbors(current)) {
                float newCost = costSoFar[current] + graph.Cost(next);
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
                    costSoFar[next] = newCost;
                    float priority = newCost + Heuristic(next, end);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }
        return cameFrom;
    }

    public static bool ReachedEnd(Coordinates c, Room room) {
        if (c.x == room.x - 1 && c.y >= room.y && c.y < room.y + room.ySize) return true;
        if (c.x == room.x + room.xSize && c.y >= room.y && c.y < room.y + room.ySize) return true;
        if (c.y == room.y - 1 && c.x >= room.x && c.x < room.x + room.xSize) return true;
        if (c.y == room.y + room.ySize && c.x >= room.x && c.x < room.x + room.xSize) return true;
        return false;
    }

    static float Heuristic(Coordinates a, Coordinates b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }
}

public class Coordinates : Priority_Queue.FastPriorityQueueNode {

    public readonly int x, y;
    public Coordinates(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public static bool operator <(Coordinates c1, Coordinates c2) {
        if (c1.x != c2.x) return (c1.x < c2.x);
        return (c1.y < c2.y);
    }

    public static bool operator >(Coordinates c1, Coordinates c2) {
        if (c1.x != c2.x) return (c1.x > c2.x);
        return (c1.y > c2.y);
    }

    public static bool operator ==(Coordinates c1, Coordinates c2) {
        if (ReferenceEquals(c1, null)) {
            return ReferenceEquals(c2, null);
        }
        if (ReferenceEquals(c2, null)) {
            return ReferenceEquals(c1, null);
        }
        return (c1.x == c2.x && c1.y == c2.y);
    }

    public static bool operator !=(Coordinates c1, Coordinates c2) {
        if (ReferenceEquals(c1, null)) {
            return !ReferenceEquals(c2, null);
        }
        if (ReferenceEquals(c2, null)) {
            return !ReferenceEquals(c1, null);
        }
        return (c1.x != c2.x || c1.y != c2.y);
    }

    public override bool Equals(object o) {
        var item = o as Coordinates;
        if (o == null) return false;
        return (this == item);
    }

    public override int GetHashCode() {
        int hash = 13;
        hash = (hash * 7) + x.GetHashCode();
        hash = (hash * 7) + y.GetHashCode();
        return hash;
    }
}

public class Graph {
    protected int width = 0;
    protected int height = 0;
    protected int floor = 0;
    protected string[,,] grid;

    public Graph(int floor, int width, int height, string[,,] grid) {
        this.width = width;
        this.height = height;
        this.grid = grid;
        this.floor = floor;
    }

    public bool InBounds(int x, int y) {
        return 0 <= x && x < width && 0 <= y && y < height;
    }

    public bool Passable(int x, int y) {
        if (grid[floor, x, y] == "x" || grid[floor, x, y] == "X") return false;
        return true;
    }

    public static readonly Coordinates[] DIRS = new[]
       {
            new Coordinates(1, 0),
            new Coordinates(0, -1),
            new Coordinates(-1, 0),
            new Coordinates(0, 1)
       };

    public IEnumerable<Coordinates> Neighbors(Coordinates id) {
        foreach (var dir in DIRS) {
            Coordinates next = new Coordinates(id.x + dir.x, id.y + dir.y);
            if (InBounds(next.x, next.y) && Passable(next.x, next.y)) {
                yield return next;
            }
        }
    }

    public int Cost(Coordinates c) {
        var cost = 1;
        var neighbors = Neighbors(c);
        var neighborsCount = 0;
        foreach (var neighbor in neighbors) neighborsCount++;
        if (neighborsCount < 4) cost += 50;
        return cost;
    }
};

public class InverseGraph {
    protected int width = 0;
    protected int height = 0;
    protected int floor = 0;
    protected string[,,] grid;

    public InverseGraph(int floor, int width, int height, string[,,] grid) {
        this.width = width;
        this.height = height;
        this.grid = grid;
        this.floor = floor;
    }

    public bool InBounds(int x, int y) {
        return 0 <= x && x < width && 0 <= y && y < height;
    }

    public bool Passable(int x, int y) {
        if (grid[floor, x, y] == "x" || grid[floor, x, y] == "X" || grid[floor, x, y] == "*" || grid[floor, x, y] == ">" || grid[floor, x, y] == "<" || grid[floor, x, y] == "E") return true;
        return false;
    }

    public static readonly Coordinates[] DIRS = new[]
       {
            new Coordinates(1, 0),
            new Coordinates(0, -1),
            new Coordinates(-1, 0),
            new Coordinates(0, 1)
       };

    public IEnumerable<Coordinates> Neighbors(Coordinates id) {
        foreach (var dir in DIRS) {
            Coordinates next = new Coordinates(id.x + dir.x, id.y + dir.y);
            if (InBounds(next.x, next.y) && Passable(next.x, next.y)) {
                yield return next;
            }
        }
    }

    public int Cost(Coordinates c) {
        var cost = 1;
        return cost;
    }
};

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
                if (action!=null) action.ApplyEffectsDirectly(gan.state);
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

public class GoapActionNode: Priority_Queue.FastPriorityQueueNode {
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
        while (frontier.Count>0) {
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