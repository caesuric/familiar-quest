using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarButtonDropHandler : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public void OnPointerEnter(PointerEventData data) {

    }

    public void OnPointerExit(PointerEventData data) {

    }

    public void OnDrop(PointerEventData data) {
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<AbilityScreenIcon>() != null) {
            var ability = data.pointerDrag.GetComponent<AbilityScreenIcon>().ability;
            ActiveAbility hotbarAbility;
            int number = GetComponent<MouseOverHotbarButton>().number;
            if (ability is PassiveAbility && number != 12) return;
            if (number == 12) {
                if (ability is ActiveAbility) return;
                OnDropToPassiveSlot(data);
                return;
            }
            if (GetComponent<MouseOverHotbarButton>().number > -1 && GetComponent<MouseOverHotbarButton>().number != 12) hotbarAbility = PlayerCharacter.localPlayer.GetComponent<SpiritUser>().spirits[0].activeAbilities[GetComponent<MouseOverHotbarButton>().number];
            else hotbarAbility = null;
            PlayerCharacter.localPlayer.GetComponent<SpiritUser>().spirits[0].activeAbilities[number] = (ActiveAbility)ability;
            if (hotbarAbility != null && !PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities.Contains(hotbarAbility)) PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities.Add(hotbarAbility);
            PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities.Remove((ActiveAbility)ability);
            PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
            GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>().UpdateAbilities();
        }
        else if (data.pointerDrag != null && data.pointerDrag.GetComponent<MouseOverHotbarButton>() != null) {
            var abilities = PlayerCharacter.localPlayer.GetComponent<SpiritUser>().spirits[0].activeAbilities;
            var num1 = data.pointerDrag.GetComponent<MouseOverHotbarButton>().number;
            var num2 = GetComponent<MouseOverHotbarButton>().number;
            if (num1 == 12 || num2 == 12) return;
            var temp = abilities[num1];
            abilities[num1] = abilities[num2];
            abilities[num2] = temp;
            PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
    }

    private void OnDropToPassiveSlot(PointerEventData data) {
        var ability = (PassiveAbility)(data.pointerDrag.GetComponent<AbilityScreenIcon>().ability);
        PassiveAbility slotAbility;
        slotAbility = PlayerCharacter.localPlayer.GetComponent<SpiritUser>().spirits[0].passiveAbilities[0];
        PlayerCharacter.localPlayer.GetComponent<SpiritUser>().RemovePassive(slotAbility);
        PlayerCharacter.localPlayer.GetComponent<SpiritUser>().AddPassive(ability);
        PlayerCharacter.localPlayer.GetComponent<SpiritUser>().spirits[0].passiveAbilities[0] = ability;
        if (slotAbility != null && !PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities.Contains(slotAbility)) PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities.Add(slotAbility);
        PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities.Remove(ability);
        PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>().UpdateAbilities();
    }
}
