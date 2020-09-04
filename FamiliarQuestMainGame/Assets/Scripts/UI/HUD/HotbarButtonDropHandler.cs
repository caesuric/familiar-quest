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
            if (GetComponent<MouseOverHotbarButton>().number > -1 && GetComponent<MouseOverHotbarButton>().number != 12) hotbarAbility = PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActives[GetComponent<MouseOverHotbarButton>().number];
            else hotbarAbility = null;
            PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActives[number] = (ActiveAbility)ability;
            if (hotbarAbility != null && !PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActivesOverflow.Contains(hotbarAbility)) PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActivesOverflow.Add(hotbarAbility);
            PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActivesOverflow.Remove((ActiveAbility)ability);
            PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
            GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>().UpdateAbilities();
        }
        else if (data.pointerDrag != null && data.pointerDrag.GetComponent<MouseOverHotbarButton>() != null) {
            var abilities = PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActives;
            var num1 = data.pointerDrag.GetComponent<MouseOverHotbarButton>().number;
            var num2 = GetComponent<MouseOverHotbarButton>().number;
            if (num1 == 12 || num2 == 12) return;
            while (abilities.Count < 10 && (num1 >= abilities.Count || num2 >= abilities.Count)) abilities.Add(null);
            var temp = abilities[num1];
            abilities[num1] = abilities[num2];
            abilities[num2] = temp;
            PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        }
    }

    private void OnDropToPassiveSlot(PointerEventData data) {
        var ability = (PassiveAbility)(data.pointerDrag.GetComponent<AbilityScreenIcon>().ability);
        PassiveAbility slotAbility;
        slotAbility = PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemPassive;
        PlayerCharacter.localPlayer.GetComponent<AbilityUser>().RemovePassive(slotAbility);
        PlayerCharacter.localPlayer.GetComponent<AbilityUser>().AddPassive(ability);
        PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemPassive = ability;
        if (slotAbility != null && !PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemPassivesOverflow.Contains(slotAbility)) PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemPassivesOverflow.Add(slotAbility);
        PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemPassivesOverflow.Remove(ability);
        PlayerCharacter.localPlayer.GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
        GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>().UpdateAbilities();
    }
}
