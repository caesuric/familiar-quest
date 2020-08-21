using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityListDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    private Color startingColor;

    // Use this for initialization
    void Start() {
        startingColor = GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData data) {
        if (IsValid(data.pointerDrag)) {
            GetComponent<Image>().color = new Color(93, 66, 46);
        }
    }

    public void OnPointerExit(PointerEventData data) {
        GetComponent<Image>().color = startingColor;
    }

    public void OnDrop(PointerEventData data) {
        InputMovement.isDragging = false;
        GetComponent<Image>().color = startingColor;
        var mouseOverHotbarButton = data.pointerDrag.GetComponent<MouseOverHotbarButton>();
        if (mouseOverHotbarButton != null) {
            var au = PlayerCharacter.localPlayer.GetComponent<AbilityUser>();
            var activeAbilities = au.soulGemActives;
            var passive = au.soulGemPassive;
            if (mouseOverHotbarButton.number < 12) {
                au.soulGemActivesOverflow.Add(activeAbilities[mouseOverHotbarButton.number]);
                activeAbilities[mouseOverHotbarButton.number] = null;
            }
            else {
                au.RemovePassive(passive);
                au.soulGemPassivesOverflow.Add(passive);
                au.soulGemPassive = null;
            }
            PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
    }

    private bool IsValid(GameObject pointerDrag) {
        if (pointerDrag == null) return false;
        if (pointerDrag.GetComponent<AbilityScreenIcon>() != null) return true;
        if (pointerDrag.GetComponent<MouseOverHotbarButton>() != null) return true;
        return false;
    }
}