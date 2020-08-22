using DuloGames.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectionFrame : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public float multiplier = 1f;
    private Vector3 originalPosition;
    public GameObject hotbarButton1;
    public GameObject hotbarButton2;

    public int number;
    public GameObject mouseOverCanvas;
    private Vector3 mouseOverCanvasOriginalPosition;
    public PlayerCharacter pc = null;
    public Text title;
    public Text description;
    private MeshRenderer hitbox = null;
    public MeshRenderer rangedHitbox;
    public MeshRenderer aoe;
    public bool isRightMouseButton = false;
    private Vector3 aoeScale;

    // Use this for initialization
    void Start() {
        number = 5;
        originalPosition = transform.position;
        mouseOverCanvasOriginalPosition = mouseOverCanvas.transform.position;
        aoeScale = aoe.transform.localScale;
    }

    // Update is called once per frame
    void Update() {
        if (PlayerCharacter.localPlayer == null) return;
        if (pc == null && PlayerCharacter.players.Count > 0) pc = PlayerCharacter.localPlayer;
        if (pc != null && hitbox == null) hitbox = pc.gameObject.GetComponentInChildren<HitboxDealDamage>().GetComponent<MeshRenderer>();
        if (PlayerCharacter.players.Count < 1) return;
        if (!isRightMouseButton) number = PlayerCharacter.localPlayer.GetComponent<InputController>().currentAbility;
        else number = PlayerCharacter.localPlayer.GetComponent<InputController>().currentAltAbility;
        //var num = number - 4;
        //transform.position = originalPosition + new Vector3((hotbarButton2.transform.position.x - hotbarButton1.transform.position.x) * multiplier * num, 0, 0);
        transform.position = PlayerCharacter.localPlayer.GetComponent<HotbarUser>().hotbarButtons[number].transform.position;
        transform.SetAsLastSibling();
        var tooltip = GetComponent<UITooltipShow>();
        tooltip.contentLines = new UITooltipLineContent[2] {
            new UITooltipLineContent() {
                LineStyle = UITooltipLines.LineStyle.Title,
                Content = pc.GetComponent<HotbarUser>().abilityNames[number]
            },
            new UITooltipLineContent() {
                LineStyle = UITooltipLines.LineStyle.Custom,
                CustomLineStyle = "ItemAttribute",
                Content = pc.GetComponent<HotbarUser>().abilityDescriptions[number]
            }
        };
    }

    public void OnMouseEnter() {
        //mouseOverCanvas.SetActive(true);
        //var mousePos = new Vector3(Input.mousePosition.x + 200, Input.mousePosition.y + 10, 0);
        //mouseOverCanvas.transform.position = mousePos;
        if (number < pc.GetComponent<HotbarUser>().abilityNames.Count) EnableTooltip();
        else DisableTooltip();
        pc.GetComponent<HotbarUser>().hotbarButtons[number].GetComponent<MouseOverHotbarButton>().hoverOverlay.SetActive(true);
    }

    private void EnableTooltip() {
        //if (number == 10 || number == 11) {
        //    var potionNumber = number - 10;
        //    if (pc.GetComponent<PlayerCharacter>().consumables.Count <= potionNumber) return;
        //    title.text = pc.GetComponent<PlayerCharacter>().consumables[potionNumber].name;
        //    description.text = pc.GetComponent<PlayerCharacter>().consumables[potionNumber].description;
        //}
        //if (pc.GetComponent<HotbarUser>().abilityIsRanged.Count <= number || pc.GetComponent<HotbarUser>().abilityIsAttack.Count <= number || pc.GetComponent<HotbarUser>().abilityNames.Count <= number || pc.GetComponent<HotbarUser>().abilityDescriptions.Count <= number) return;
        //title.text = pc.GetComponent<HotbarUser>().abilityNames[number];
        //description.text = pc.GetComponent<HotbarUser>().abilityDescriptions[number];
        if (!pc.GetComponent<HotbarUser>().abilityIsRanged[number] && pc.GetComponent<HotbarUser>().abilityIsAttack[number]) ShowMeleeAttackTooltip();
        else if (pc.GetComponent<HotbarUser>().abilityIsRanged[number]) ShowRangedAttackTooltip();
        if (pc.GetComponent<HotbarUser>().abilityRadii[number] > 0) ShowAoeRadius();
        else aoe.enabled = false;
    }

    private void ShowAoeRadius() {
        hitbox.enabled = false;
        aoe.enabled = true;
        aoe.transform.localScale = aoeScale * pc.GetComponent<HotbarUser>().abilityRadii[number];
        if (pc.GetComponent<HotbarUser>().abilityIsRanged[number]) {
            Physics.Raycast(pc.transform.position, pc.transform.forward, out RaycastHit hitInfo);
            aoe.transform.position = hitInfo.point;
        }
        else aoe.transform.position = pc.transform.position;
    }

    private void ShowMeleeAttackTooltip() {
        hitbox.enabled = true;
        rangedHitbox.enabled = false;
    }

    private void ShowRangedAttackTooltip() {
        hitbox.enabled = false;
        rangedHitbox.enabled = true;
        rangedHitbox.transform.position = pc.transform.position + (pc.transform.forward * 11.06f);
        rangedHitbox.transform.rotation = pc.transform.rotation;
    }

    private void DisableTooltip() {
        mouseOverCanvas.SetActive(false);
        hitbox.enabled = false;
        rangedHitbox.enabled = false;
        aoe.enabled = false;
    }

    public void OnMouseExit() {
        //mouseOverCanvas.SetActive(false);
        hitbox.enabled = false;
        rangedHitbox.enabled = false;
        aoe.enabled = false;
        pc.GetComponent<HotbarUser>().hotbarButtons[number].GetComponent<MouseOverHotbarButton>().hoverOverlay.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData data) {

    }

    public void OnPointerExit(PointerEventData data) {

    }

    public void OnDrop(PointerEventData data) {
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<AbilityScreenIcon>() != null) {
            var ability = data.pointerDrag.GetComponent<AbilityScreenIcon>().ability;
            ActiveAbility hotbarAbility;
            if (number > -1) hotbarAbility = PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActives[number];
            else hotbarAbility = null;
            PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActives[number] = (ActiveAbility)ability;
            if (hotbarAbility != null && !PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActivesOverflow.Contains(hotbarAbility)) PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActivesOverflow.Add(hotbarAbility);
            if (ability is ActiveAbility activeAbility) PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActivesOverflow.Remove(activeAbility);
            else if (ability is PassiveAbility passiveAbility) PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemPassivesOverflow.Remove(passiveAbility);
            PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
            GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>().UpdateAbilities();
        }
    }
}
