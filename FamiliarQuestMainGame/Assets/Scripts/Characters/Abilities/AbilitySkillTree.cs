using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AbilitySkillTree {
    public List<AbilitySkillTreeNode> baseNodes = new List<AbilitySkillTreeNode>();
    public List<List<AbilitySkillTreeNode>> nodesByLayer = new List<List<AbilitySkillTreeNode>>();

    public AbilitySkillTree() {

    }

    public AbilitySkillTree(Ability ability) {
        int layers = RNG.Int(2, 4);
        int baseNodeCount = RNG.Int(2, 4);
        nodesByLayer.Add(new List<AbilitySkillTreeNode>());
        var allNodes = new List<AbilitySkillTreeNode>();
        for (int i = 0; i < baseNodeCount; i++) {
            var newNode = new AbilitySkillTreeNode(ability, allNodes);
            allNodes.Add(newNode);
            baseNodes.Add(newNode);
            newNode.clickable = true;
            nodesByLayer[0].Add(newNode);
        }
        int layerCounter = 1;
        while (layerCounter < layers) {
            int nodeCount = RNG.Int(2, 4);
            nodesByLayer.Add(new List<AbilitySkillTreeNode>());
            for (int i=0; i<nodeCount; i++) {
                var newNode = new AbilitySkillTreeNode(ability, allNodes);
                allNodes.Add(newNode);
                var parentNumber = RNG.Int(0, nodesByLayer[layerCounter - 1].Count);
                var parent = nodesByLayer[layerCounter - 1][parentNumber];
                parent.children.Add(newNode);
                nodesByLayer[layerCounter].Add(newNode);
            }
            layerCounter++;
        }
        CleanUpNodeList();
    }

    private void CleanUpNodeList() {
        nodesByLayer = new List<List<AbilitySkillTreeNode>>();
        foreach (var node in baseNodes) {
            AddNodeData(node, 0);
        }
    }

    private void AddNodeData(AbilitySkillTreeNode node, int level) {
        if (nodesByLayer.Count <= level) nodesByLayer.Add(new List<AbilitySkillTreeNode>());
        nodesByLayer[level].Add(node);
        foreach (var subnode in node.children) {
            AddNodeData(subnode, level + 1);
        }
    }
}