using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityScreenIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public Ability ability;
    public bool draggable = true;
    private string title = "";
    private string description = "";
    private AbilityMenu abilityScreen;
    private Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();

    // Use this for initialization
    void Start () {
        abilityScreen = GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(Ability abilityParam) {
        ability = abilityParam;
        if (ability == null) return;
        GetComponent<Image>().sprite = PlayerCharacter.localPlayer.GetComponent<CacheGrabber>().iconCache[ability.icon];
        title = ability.name;
        description = abilityParam.description;
        if (ability is ActiveAbility) description = AbilityInterpolate((ActiveAbility)abilityParam);
        transform.localScale = new Vector3(1, 1, 1);
        var tooltip = GetComponent<DuloGames.UI.UITooltipShow>();
        tooltip.contentLines = new DuloGames.UI.UITooltipLineContent[] { new DuloGames.UI.UITooltipLineContent(), new DuloGames.UI.UITooltipLineContent() };
        tooltip.contentLines[0].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Title;
        tooltip.contentLines[0].Content = ability.name;
        tooltip.contentLines[1].LineStyle = DuloGames.UI.UITooltipLines.LineStyle.Custom;
        tooltip.contentLines[1].CustomLineStyle = "ItemAttribute";
        tooltip.contentLines[1].Content = description;
    }

    public void OnMouseEnter() {
        abilityScreen.mouseOverTitle.text = title;
        if (ability is ActiveAbility) abilityScreen.mouseOverDescription.text = AbilityInterpolate((ActiveAbility)ability);
        var mousePos = new Vector3(Input.mousePosition.x + 200, Input.mousePosition.y + 10, 0);
        abilityScreen.mouseOverPanel.SetActive(true);
        abilityScreen.mouseOverPanel.transform.position = mousePos;
    }

    public void OnMouseExit() {
        abilityScreen.mouseOverPanel.SetActive(false);
    }

    private string AbilityInterpolate(ActiveAbility ability) {
        if (ability == null) return "";
        var text = ability.description;
        text = text.Replace("{{baseStat}}", ability.baseStat.ToString());
        if (ability is UtilityAbility) text = UtilityAbilityInterpolate(text, ability);
        if (ability is AttackAbility) text = AttackAbilityInterpolate(text, ability);
        return text;
    }

    private string UtilityAbilityInterpolate(string text, ActiveAbility ability) {
        Dictionary<BaseStat, int> lookups = new Dictionary<BaseStat, int>() {
                { BaseStat.strength, PlayerCharacter.localPlayer.GetComponent<Character>().strength},
                { BaseStat.dexterity, PlayerCharacter.localPlayer.GetComponent<Character>().dexterity},
                { BaseStat.constitution, PlayerCharacter.localPlayer.GetComponent<Character>().constitution},
                { BaseStat.intelligence, PlayerCharacter.localPlayer.GetComponent<Character>().intelligence},
                { BaseStat.wisdom, PlayerCharacter.localPlayer.GetComponent<Character>().wisdom},
                { BaseStat.luck, PlayerCharacter.localPlayer.GetComponent<Character>().luck},
        };
        int baseAttributeScore = lookups[ability.baseStat];
        text = text.Replace("{{healing}}", GetHealingText(ability, baseAttributeScore));
        text = text.Replace("{{shield}}", GetShieldText(ability, ability.baseStat));
        text = text.Replace("{{restoreMP}}", GetRestoreMpText(ability, baseAttributeScore));
        text = text.Replace("{{hot}}", GetRestoreHpOverTimeText(ability, baseAttributeScore));
        return text.Replace("{{restoreMpOverTime}}", GetRestoreMpOverTimeText(ability, baseAttributeScore));
    }

    private string GetHealingText(ActiveAbility ability, int baseAttributeScore) {
        float factor = PlayerCharacter.localPlayer.GetComponent<Character>().wisdom;
        factor *= PlayerCharacter.localPlayer.GetComponent<PlayerCharacter>().weapon.attackPower;
        float healing = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "heal") healing += attribute.FindParameter("degree").floatVal * factor * PlayerCharacter.localPlayer.GetComponent<Health>().healingMultiplier;
        return ((int)healing).ToString();
    }

    private string GetShieldText(ActiveAbility ability, BaseStat stat) {
        float shield = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "shield") shield += attribute.FindParameter("degree").floatVal * PlayerCharacter.localPlayer.GetComponent<Attacker>().GetBaseDamage(stat);
        return ((int)shield).ToString();
    }

    private string GetRestoreMpText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "restoreMP") mp += attribute.FindParameter("degree").floatVal;
        return ((int)mp).ToString();
    }

    private string GetRestoreHpOverTimeText(ActiveAbility ability, int baseAttributeScore) {
        float healing = 0;
        float duration = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "hot") {
                healing += attribute.FindParameter("degree").floatVal;
                duration = Mathf.Max(duration, attribute.FindParameter("duration").floatVal);
            }
        }
        return ((int)healing).ToString() + " over " + ((int)duration).ToString() + " seconds";
    }

    private string GetRestoreMpOverTimeText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        float duration = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "mpOverTime") {
                mp += attribute.FindParameter("degree").floatVal;
                duration = Mathf.Max(duration, attribute.FindParameter("duration").floatVal);
            }
        }
        return ((int)mp).ToString() + " over " + ((int)duration).ToString() + " seconds";
    }

    private string AttackAbilityInterpolate(string text, ActiveAbility ability) {
        Dictionary<BaseStat, int> lookups = new Dictionary<BaseStat, int>() {
                { BaseStat.strength, PlayerCharacter.localPlayer.GetComponent<Character>().strength},
                { BaseStat.dexterity, PlayerCharacter.localPlayer.GetComponent<Character>().dexterity},
                { BaseStat.intelligence, PlayerCharacter.localPlayer.GetComponent<Character>().intelligence},
            };
        int baseAttributeScore = lookups[((AttackAbility)ability).baseStat];
        text = text.Replace("{{damage}}", (GetAttackText(ability, baseAttributeScore)));
        return text.Replace("{{dotDamage}}", (Mathf.Floor(PlayerCharacter.localPlayer.weapon.attackPower * baseAttributeScore * ((AttackAbility)ability).dotDamage).ToString()));
    }

    private string GetAttackText(ActiveAbility ability, int baseAttributeScore) {
        var text = Mathf.Floor(PlayerCharacter.localPlayer.weapon.attackPower * baseAttributeScore * ((AttackAbility)ability).damage).ToString();
        text += GetTextFromDots(ability, baseAttributeScore);
        return text;
    }

    private string GetTextFromDots(ActiveAbility ability, int baseAttributeScore) {
        var text = "";
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "addedDot") {
                text += " + ";
                text += GetDotText(attribute, baseAttributeScore);
            }
        }
        return text;
    }

    private string GetDotText(AbilityAttribute attribute, int baseAttributeScore) {
        var text = GetDotDamage(attribute, baseAttributeScore).ToString();
        text += " over ";
        text += GetDotSeconds(attribute).ToString() + " s";
        return text;
    }

    private int GetDotDamage(AbilityAttribute attribute, int baseAttributeScore) {
        return (int)(attribute.FindParameter("degree").floatVal * baseAttributeScore);
    }

    private int GetDotSeconds(AbilityAttribute attribute) {
        return (int)(attribute.FindParameter("duration").floatVal);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!draggable) return;
        var canvas = FindInParents<Canvas>(gameObject);
        if (canvas == null)
            return;
        InputMovement.isDragging = true;
        transform.SetParent(canvas.transform);
        m_DraggingPlanes[eventData.pointerId] = canvas.transform as RectTransform;
        var group = gameObject.AddComponent<CanvasGroup>();
        group.blocksRaycasts = false;
        abilityScreen.mouseOverPanel.SetActive(false);
        SetDraggedPosition(eventData);
        if (ability is ActiveAbility && AbilityMenu.instance.abilitiesToDust.Contains((ActiveAbility)ability)) AbilityMenu.instance.abilitiesToDust.Remove((ActiveAbility)ability);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!draggable) return;
        abilityScreen.mouseOverPanel.SetActive(false);
        SetDraggedPosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData) {
        InputMovement.isDragging = false;
        if (!draggable) return;
        PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        abilityScreen.UpdateAbilities();
        Destroy(gameObject);
    }

    public void SetDraggedPosition(PointerEventData eventData) {
        var rt = GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out globalMousePos)) {
            rt.position = globalMousePos;
        }
    }

    static public T FindInParents<T>(GameObject go) where T : Component {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        var t = go.transform.parent;
        while (t != null && comp == null) {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
