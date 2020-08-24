using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


public class SkillTreeRenderer : MonoBehaviour {
    public GameObject nodePrefab;
    public GameObject linePrefab;
    public float width;
    public float height;
    public AbilitySkillTree skillTree = null;
    public RectTransform rectTransform;
    public Vector2 size;
    public GameObject canvas;
    public bool initialized = false;

    private void Update() {
        if (!initialized && skillTree != null) {
            canvas = transform.parent.gameObject;
            rectTransform = canvas.GetComponent<RectTransform>();
            width = GetComponent<RectTransform>().rect.width;
            height = GetComponent<RectTransform>().rect.height;
            for (int y = 0; y < skillTree.nodesByLayer.Count; y++) {
                for (int x = 0; x < skillTree.nodesByLayer[y].Count; x++) {
                    skillTree.nodesByLayer[y][x].position = new Vector2(width * (x + 1) / (skillTree.nodesByLayer[y].Count + 1), height - (height * (y + 1) / (skillTree.nodesByLayer.Count + 1)));
                }
            }
            GenerateLines(skillTree);
            GenerateIcons(skillTree);
            size = rectTransform.rect.size;
            initialized = true;
        }
        else if (skillTree == null) return;
        var newSize = rectTransform.rect.size;
        if (newSize != size) {
            foreach (Transform child in transform) Destroy(child.gameObject);
            width = GetComponent<RectTransform>().rect.width;
            height = GetComponent<RectTransform>().rect.height;
            for (int y = 0; y < skillTree.nodesByLayer.Count; y++) {
                for (int x = 0; x < skillTree.nodesByLayer[y].Count; x++) {
                    skillTree.nodesByLayer[y][x].position = new Vector2(width * (x + 1) / (skillTree.nodesByLayer[y].Count + 1), height - (height * (y + 1) / (skillTree.nodesByLayer.Count + 1)));
                }
            }
            GenerateLines(skillTree);
            GenerateIcons(skillTree);
            size = newSize;
        }
    }

    public void Initialize(AbilitySkillTree skillTree) {
        this.skillTree = skillTree;
    }

    private void GenerateLines(AbilitySkillTree skillTree) {
        foreach (var node in skillTree.baseNodes) foreach (var child in node.children) DrawLinesForNode(node, child);
    }

    private void GenerateIcons(AbilitySkillTree skillTree) {
        foreach (var line in skillTree.nodesByLayer) {
            foreach (var icon in line) {
                var node = Instantiate(nodePrefab);
                node.GetComponent<RectTransform>().localPosition = icon.position;
                node.GetComponent<SkillTreeNodeRenderer>().Initialize(icon);
                node.transform.SetParent(transform);
            }
        }
    }

    private void DrawLinesForNode(AbilitySkillTreeNode parent, AbilitySkillTreeNode node) {
        DrawLine(parent, node);
        foreach (var child in node.children) DrawLinesForNode(node, child);
    }

    private void DrawLine(AbilitySkillTreeNode parent, AbilitySkillTreeNode child) {
        var lineObj = Instantiate(linePrefab);
        lineObj.transform.SetParent(transform);
        var uiLine = lineObj.GetComponent<UILine>();
        var rectTransform = lineObj.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = GetComponent<RectTransform>().rect.size;
        rectTransform.localScale = new Vector2(0.5f, 0.5f);
        var size = 1000;
        var x1 = parent.position.x * size / width;
        var x2 = child.position.x * size / width;
        var y1 = parent.position.y * size / height;
        var y2 = child.position.y * size / height;
        uiLine.Initialize(1000, 1000, (int)x1, (int)y1, (int)x2, (int)y2);
    }
}