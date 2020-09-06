using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class SkillTreeNodeRenderer : MonoBehaviour {
    public List<SoulGemEnhancement> enhancements = new List<SoulGemEnhancement>();
    public AbilitySkillTreeNode node;
    public int initStatus = 0;
    
    public void Update() {
        if (initStatus == 1) {
            var config = GameObject.FindGameObjectWithTag("ConfigObject");
            if (config == null) return;
            if (config.GetComponent<IconCache>().newIcons.ContainsKey(enhancements[0].icon)) GetComponent<Image>().sprite = config.GetComponent<IconCache>().newIcons[enhancements[0].icon];
            else GetComponent<Image>().sprite = config.GetComponent<IconCache>().icons[int.Parse(enhancements[0].icon)];
            if (!node.active) GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
            else GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            var tooltip = GetComponent<DuloGames.UI.UITooltipShow>();
            tooltip.contentLines = new DuloGames.UI.UITooltipLineContent[] { new DuloGames.UI.UITooltipLineContent(), new DuloGames.UI.UITooltipLineContent() };
            tooltip.contentLines[0].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Title;
            tooltip.contentLines[0].Content = enhancements[0].name;
            tooltip.contentLines[1].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
            tooltip.contentLines[1].CustomLineStyle = "ItemAttribute";
            tooltip.contentLines[1].Content = enhancements[0].description;
            initStatus = 2;
        }
    }

    public void Initialize(AbilitySkillTreeNode node) {
        enhancements = node.effects;
        this.node = node;
        initStatus = 1;
    }

    public void Click() {
        if (node.clickable && node.ability.skillPoints > 0 && !node.active) {
            node.active = true;
            node.Activate();
            node.ability.skillPoints--;
            foreach (var child in node.children) child.clickable = true;
            initStatus = 1;
            AbilityMenu.instance.skillTreePointsText.text = "Skill points: " + node.ability.skillPoints.ToString();
        }
    }
}

