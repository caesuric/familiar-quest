using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityScreenIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public Ability ability = null;
    public bool draggable = true;
    private string title = "";
    private string description = "";
    private AbilityMenu abilityScreen;
    private float timer = 0f;
    private int clickCount = 0;
    private readonly Dictionary<int, RectTransform> m_DraggingPlanes = new Dictionary<int, RectTransform>();
    public static List<AbilityScreenIcon> instances = new List<AbilityScreenIcon>();

    // Use this for initialization
    void Start() {
        abilityScreen = GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>();
        instances.Add(this);
    }

    void Update() {
        if (clickCount > 0) timer += Time.deltaTime;
        if (timer >=0.5f) {
            timer = 0f;
            clickCount = 0;
        }
    }

    public void OnDestroy() {
        instances.Remove(this);
    }

    public static void UpdateAbilities() {
        foreach (var asi in instances) if (asi != null) asi.Initialize(asi.ability);
    }

    public void Click() {
        clickCount++;
        if (clickCount == 2) DoubleClickAbility();
    }

    private void DoubleClickAbility() {
        abilityScreen.skillTreeAbility = ability;
        abilityScreen.skillTreeDropSlot.Initialize(ability);
        abilityScreen.UpdateSkillTree();
        abilityScreen.tabs[0].isOn = false;
        abilityScreen.tabs[1].isOn = false;
        abilityScreen.tabs[2].isOn = true;
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
                //{ BaseStat.strength, PlayerCharacter.localPlayer.GetComponent<Character>().strength},
                //{ BaseStat.dexterity, PlayerCharacter.localPlayer.GetComponent<Character>().dexterity},
                //{ BaseStat.constitution, PlayerCharacter.localPlayer.GetComponent<Character>().constitution},
                //{ BaseStat.intelligence, PlayerCharacter.localPlayer.GetComponent<Character>().intelligence},
                //{ BaseStat.wisdom, PlayerCharacter.localPlayer.GetComponent<Character>().wisdom},
                //{ BaseStat.luck, PlayerCharacter.localPlayer.GetComponent<Character>().luck},
                { BaseStat.strength, (int)CharacterAttribute.attributes["strength"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
                { BaseStat.dexterity, (int)CharacterAttribute.attributes["dexterity"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
                { BaseStat.constitution, (int)CharacterAttribute.attributes["constitution"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
                { BaseStat.intelligence, (int)CharacterAttribute.attributes["intelligence"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
                { BaseStat.wisdom, (int)CharacterAttribute.attributes["wisdom"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
                { BaseStat.luck, (int)CharacterAttribute.attributes["luck"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
        };
        int baseAttributeScore = lookups[ability.baseStat];
        text = text.Replace("{{healing}}", GetHealingText(ability, baseAttributeScore));
        text = text.Replace("{{shield}}", GetShieldText(ability, ability.baseStat));
        text = text.Replace("{{restoreMP}}", GetRestoreMpText(ability, baseAttributeScore));
        text = text.Replace("{{hot}}", GetRestoreHpOverTimeText(ability, baseAttributeScore));
        return text.Replace("{{restoreMpOverTime}}", GetRestoreMpOverTimeText(ability, baseAttributeScore));
    }

    private string GetHealingText(ActiveAbility ability, int baseAttributeScore) {
        //float factor = PlayerCharacter.localPlayer.GetComponent<Character>().wisdom;
        float factor = CharacterAttribute.attributes["wisdom"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue;
        factor *= PlayerCharacter.localPlayer.GetComponent<PlayerCharacter>().weapon.attackPower;
        float healing = 0;
        //foreach (var attribute in ability.attributes) if (attribute.type == "heal") healing += (float)attribute.FindParameter("degree").value * factor * PlayerCharacter.localPlayer.GetComponent<Health>().healingMultiplier;
        foreach (var attribute in ability.attributes) if (attribute.type == "heal") healing += (float)attribute.FindParameter("degree").value * factor * CharacterAttribute.attributes["healingMultiplier"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue;
        return ((int)healing).ToString();
    }

    private string GetShieldText(ActiveAbility ability, BaseStat stat) {
        float shield = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "shield") shield += (float)attribute.FindParameter("degree").value * PlayerCharacter.localPlayer.GetComponent<Attacker>().GetBaseDamage(stat);
        return ((int)shield).ToString();
    }

    private string GetRestoreMpText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        foreach (var attribute in ability.attributes) if (attribute.type == "restoreMP") mp += (float)attribute.FindParameter("degree").value;
        return ((int)mp).ToString();
    }

    private string GetRestoreHpOverTimeText(ActiveAbility ability, int baseAttributeScore) {
        float healing = 0;
        float duration = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "hot") {
                healing += (float)attribute.FindParameter("degree").value;
                duration = Mathf.Max(duration, (float)attribute.FindParameter("duration").value);
            }
        }
        return ((int)healing).ToString() + " over " + ((int)duration).ToString() + " seconds";
    }

    private string GetRestoreMpOverTimeText(ActiveAbility ability, int baseAttributeScore) {
        float mp = 0;
        float duration = 0;
        foreach (var attribute in ability.attributes) {
            if (attribute.type == "mpOverTime") {
                mp += (float)attribute.FindParameter("degree").value;
                duration = Mathf.Max(duration, (float)attribute.FindParameter("duration").value);
            }
        }
        return ((int)mp).ToString() + " over " + ((int)duration).ToString() + " seconds";
    }

    private string AttackAbilityInterpolate(string text, ActiveAbility ability) {
        Dictionary<BaseStat, int> lookups = new Dictionary<BaseStat, int>() {
                //{ BaseStat.strength, PlayerCharacter.localPlayer.GetComponent<Character>().strength},
                //{ BaseStat.dexterity, PlayerCharacter.localPlayer.GetComponent<Character>().dexterity},
                //{ BaseStat.intelligence, PlayerCharacter.localPlayer.GetComponent<Character>().intelligence},
                { BaseStat.strength, (int)CharacterAttribute.attributes["strength"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
                { BaseStat.dexterity, (int)CharacterAttribute.attributes["dexterity"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue},
                { BaseStat.intelligence, (int)CharacterAttribute.attributes["intelligence"].instances[PlayerCharacter.localPlayer.GetComponent<Character>()].TotalValue}
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
        return (int)((float)attribute.FindParameter("degree").value * baseAttributeScore);
    }

    private int GetDotSeconds(AbilityAttribute attribute) {
        return (int)((float)attribute.FindParameter("duration").value);
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (!draggable || ability == null) return;
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
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlanes[eventData.pointerId], eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos)) {
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
