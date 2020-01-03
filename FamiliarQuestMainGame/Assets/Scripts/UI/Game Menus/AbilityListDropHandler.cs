﻿using UnityEngine;
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
            var spiritUser = PlayerCharacter.localPlayer.GetComponent<SpiritUser>();
            var abilities = spiritUser.spirits[0].activeAbilities;
            if (mouseOverHotbarButton.number < 12) {
                spiritUser.overflowAbilities.Add(abilities[mouseOverHotbarButton.number]);
                abilities[mouseOverHotbarButton.number] = null;
            }
            else {
                spiritUser.RemovePassive(spiritUser.spirits[0].passiveAbilities[0]);
                spiritUser.overflowAbilities.Add(spiritUser.spirits[0].passiveAbilities[0]);
                spiritUser.spirits[0].passiveAbilities[0] = null;
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