//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class GraphNode {
//    public List<GraphNode> connections = new List<GraphNode>();
//    public int left;
//    public int right;
//    public int top;
//    public int bottom;
//    public bool dug = false;
//    public bool startingRoom = false;
//    public int mobs = 0;
//    public bool stairsDownRoom = false;
//    public bool treasureRoom = false;
//    public bool stairsDownPath = false;
//    public bool bossRoom = false;
//    public bool hasEncounter = false;
//    public List<string> dressing = new List<string> { "", "", "", "" };
//    public List<string> dressingPlacement = new List<string> { "", "", "", "" };
//    public List<int> dressingCount = new List<int> { 0, 0, 0, 0 };
//    public bool isHallway = false;
//    public RoomData roomData = null;

//    public GraphNode(params GraphNode[] neighbors) {
//        foreach (var item in neighbors) {
//            connections.Add(item);
//            item.connections.Add(this);
//        }
//    }
//    public GraphNode() {
//        // This space intentionally left blank.
//    }
//}
