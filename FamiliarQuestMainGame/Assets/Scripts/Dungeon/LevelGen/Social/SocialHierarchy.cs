using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SocialHierarchy {

    public List<SocialNode> leaders = new List<SocialNode>();
    public List<SocialNode> allNodes = new List<SocialNode>();

    public SocialHierarchy(List<string> mainTypes, List<string> pets, List<string> servants) {
        int numLeadersRoll = Random.Range(1, 11);
        int differentTypeRoll, randomNodeRoll;
        if (numLeadersRoll > 3) numLeadersRoll = 1;
        for (int i = 0; i < numLeadersRoll; i++) {
            differentTypeRoll = Random.Range(0, 5);
            var leader = new SocialNode(mainTypes, (differentTypeRoll == 0), null);
            leaders.Add(leader);
            allNodes.Add(leader);
        }
        int numChildNodesRoll = Random.Range(2, 10);
        for (int i = 0; i < numChildNodesRoll; i++) {
            randomNodeRoll = Random.Range(0, allNodes.Count);
            differentTypeRoll = Random.Range(0, 5);
            allNodes.Add(new SocialNode(mainTypes, (differentTypeRoll == 0), allNodes[randomNodeRoll]));
        }
        var nodeCount = allNodes.Count;
        if (servants.Count > 0) {
            foreach (var servant in servants) {
                randomNodeRoll = Random.Range(0, nodeCount);
                allNodes.Add(new SocialNode(new List<string> { servant }, false, allNodes[randomNodeRoll]));
            }
        }
        nodeCount = allNodes.Count;
        if (pets.Count > 0) {
            foreach (var pet in pets) {
                randomNodeRoll = Random.Range(0, nodeCount);
                allNodes.Add(new SocialNode(new List<string> { pet }, false, allNodes[randomNodeRoll]));
            }
        }
    }
}