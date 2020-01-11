using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AbilityDustDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    private Color startingColor;

    // Use this for initialization
    void Start() {
        startingColor = GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData data) {
        if (IsValid(data.pointerDrag)) {
            GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnPointerExit(PointerEventData data) {
        GetComponent<Image>().color = startingColor;
    }

    public void OnDrop(PointerEventData data) {
        InputMovement.isDragging = false;
        GetComponent<Image>().color = startingColor;
        var mouseOverHotbarButton = data.pointerDrag.GetComponent<MouseOverHotbarButton>();
        var abilityScreenIcon = data.pointerDrag.GetComponent<AbilityScreenIcon>();
        var spiritUser = PlayerCharacter.localPlayer.GetComponent<SpiritUser>();
        var abilities = spiritUser.spirits[0].activeAbilities;
        if (mouseOverHotbarButton != null) {
            AbilityMenu.instance.abilitiesToDust.Add(abilities[mouseOverHotbarButton.number]);
            AbilityMenu.instance.RefreshDustPanel();
        }
        else if (abilityScreenIcon != null) {
            if (!AbilityMenu.instance.abilitiesToDust.Contains(abilityScreenIcon.ability)) AbilityMenu.instance.abilitiesToDust.Add(abilityScreenIcon.ability);
            AbilityMenu.instance.RefreshDustPanel();
        }
    }

    private bool IsValid(GameObject pointerDrag) {
        if (pointerDrag == null) return false;
        if (pointerDrag.GetComponent<AbilityScreenIcon>() != null) return true;
        if (pointerDrag.GetComponent<MouseOverHotbarButton>() != null) return true;
        return false;
    }
}