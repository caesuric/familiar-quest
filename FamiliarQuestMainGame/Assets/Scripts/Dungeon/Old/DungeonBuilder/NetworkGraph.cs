//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class NetworkGraph {
//    public List<GraphNode> members = new List<GraphNode>();

//    public NetworkGraph(int branches, bool finalFloor = false) {
//        var start = new GraphNode();
//        members.Add(start);
//        for (int i = 0; i < branches; i++) AddNode(finalFloor, i);
//    }

//    private void AddNode(bool finalFloor, int i) {
//        var currentNode = members[Random.Range(0, members.Count)];
//        for (int j = 0; j < 8; j++) {
//            var newNode = new GraphNode(currentNode);
//            members.Add(newNode);
//            currentNode = newNode;
//            if (i == 0 && j == 7) {
//                if (!finalFloor) currentNode.stairsDownRoom = true;
//                else currentNode.bossRoom = true;
//            }
//            if (i == 0) currentNode.stairsDownPath = true;
//            if (i != 0 && j == 7) currentNode.treasureRoom = true;
//        }
//    }
//}

