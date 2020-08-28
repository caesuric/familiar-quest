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
    public GameObject parentObject;
    public bool initialized = false;

    private void Start() {
        //Initialize(new AbilitySkillTree(AbilityGenerator.Generate())); //temporary until integrated into main game
    }

    private void Update() {
        if (!initialized && skillTree != null) {
            parentObject = transform.parent.gameObject;
            rectTransform = parentObject.GetComponent<RectTransform>();
            //var scaler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScaler>();
            width = GetComponent<RectTransform>().rect.width; //* scaler.transform.localScale.x;
            height = GetComponent<RectTransform>().rect.height; //* scaler.transform.localScale.y;
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
            //var scaler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasScaler>();
            width = GetComponent<RectTransform>().rect.width; //* scaler.transform.localScale.x;
            height = GetComponent<RectTransform>().rect.height; //* scaler.transform.localScale.y;
            for (int y = 0; y < skillTree.nodesByLayer.Count; y++) {
                for (int x = 0; x < skillTree.nodesByLayer[y].Count; x++) {
                    skillTree.nodesByLayer[y][x].position = new Vector2((width * (x + 1) / (skillTree.nodesByLayer[y].Count + 1)), height - (height * (y + 1) / (skillTree.nodesByLayer.Count + 1)));
                }
            }
            GenerateLines(skillTree);
            GenerateIcons(skillTree);
            size = newSize;
        }
    }

    public void Initialize(AbilitySkillTree skillTree) {
        foreach (Transform child in transform) Destroy(child.gameObject);
        this.skillTree = skillTree;
        initialized = false;
    }

    private void GenerateLines(AbilitySkillTree skillTree) {
        foreach (var node in skillTree.baseNodes) foreach (var child in node.children) DrawLinesForNode(node, child);
    }

    private void GenerateIcons(AbilitySkillTree skillTree) {
        foreach (var line in skillTree.nodesByLayer) {
            foreach (var icon in line) {
                var node = Instantiate(nodePrefab);
                node.transform.SetParent(transform);
                node.GetComponent<RectTransform>().localPosition = icon.position;
                node.GetComponent<RectTransform>().anchorMin -= new Vector2(0.5f, 0.5f);
                node.GetComponent<RectTransform>().anchorMax -= new Vector2(0.5f, 0.5f);
                node.GetComponent<SkillTreeNodeRenderer>().Initialize(icon);
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
        var sizeX = 1000 - this.rectTransform.offsetMin.x - this.rectTransform.offsetMax.x;
        var sizeY = 1000 - this.rectTransform.offsetMin.y - this.rectTransform.offsetMax.y;
        var x1 = (parent.position.x * sizeX / width) - this.rectTransform.offsetMin.x;
        var x2 = (child.position.x * sizeX / width) - this.rectTransform.offsetMin.x;
        var y1 = (parent.position.y * sizeY / height) - this.rectTransform.offsetMin.y;
        var y2 = (child.position.y * sizeY / height) - this.rectTransform.offsetMin.y;
        uiLine.Initialize((int)sizeX, (int)sizeY, (int)x1, (int)y1, (int)x2, (int)y2);
    }
}