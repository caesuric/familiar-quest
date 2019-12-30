using DuloGames.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseOverEffect : MonoBehaviour {

    private string type = "";
    private Dictionary<string, string> titles = new Dictionary<string, string>();
    private Dictionary<string, string> descriptions = new Dictionary<string, string>();
    private GameObject mouseOverCanvas = null;
    private Text title;
    private Text description;
	// Use this for initialization
	void Start () {
        var items = TextReader.ReadSets("EffectFriendlyNames");
        foreach (var item in items) titles.Add(item[0], item[1]);
        items = TextReader.ReadSets("EffectDescriptions");
        foreach (var item in items) descriptions.Add(item[0], item[1]);
	}
	
	// Update is called once per frame
	void Update () {
        if (type == null || type=="" || mouseOverCanvas==null)
        {
            var effect = GetComponentInChildren<EffectIconUpdater>();
            type = effect.icon;
            var tooltip = GetComponent<UITooltipShow>();
            tooltip.contentLines = new UITooltipLineContent[] { new UITooltipLineContent(), new UITooltipLineContent() };
            tooltip.contentLines[0].LineStyle = UITooltipLines.LineStyle.Title;
            tooltip.contentLines[0].Content = titles[type];
            tooltip.contentLines[1].LineStyle = UITooltipLines.LineStyle.Description;
            tooltip.contentLines[1].Content = descriptions[type];
        }
	}
}