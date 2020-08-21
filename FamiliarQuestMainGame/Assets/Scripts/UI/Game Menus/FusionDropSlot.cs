using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FusionDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public Image background;
    public AbilityScreenIcon fusionDestination;
    public int slot = 1;
    private Color originalColor;
    private AbilityMenu abilityMenu;
    public static FusionDropSlot instance = null;

    // Use this for initialization
    void Start() {
        originalColor = background.color;
        abilityMenu = GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>();
        if (slot == 2) instance = this;
    }


    public void OnPointerEnter(PointerEventData data) {
        if (IsValid(data.pointerDrag)) background.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData data) {
        background.color = originalColor;
    }

    public void OnDrop(PointerEventData data) {
        InputMovement.isDragging = false;
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<AbilityScreenIcon>() != null) {
            var ability = data.pointerDrag.GetComponent<AbilityScreenIcon>().ability;
            if (slot == 1 && abilityMenu.fusionSource2 != ability) {
                abilityMenu.fusionSource1 = (ActiveAbility)ability;
                abilityMenu.fusionSourceSlot1 = -1;
            }
            else if (slot == 2 && abilityMenu.fusionSource1 != ability) {
                abilityMenu.fusionSource2 = (ActiveAbility)ability;
                abilityMenu.fusionSourceSlot2 = -1;
            }
            else return;
            GetComponent<AbilityScreenIcon>().Initialize(ability);
            CheckAndUpdate();
            abilityMenu.UpdateAbilities();
            abilityMenu.ResetFusionChoices();
        }
        else if (data.pointerDrag != null && data.pointerDrag.GetComponent<MouseOverHotbarButton>() != null) {
            var abilities = PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActives;
            var num = data.pointerDrag.GetComponent<MouseOverHotbarButton>().number;
            var ability = abilities[num];
            if (slot == 1) {
                abilityMenu.fusionSource1 = ability;
                abilityMenu.fusionSourceSlot1 = num;
            }
            else if (slot == 2) {
                abilityMenu.fusionSource2 = ability;
                abilityMenu.fusionSourceSlot2 = num;
            }
            else return;
            GetComponent<AbilityScreenIcon>().Initialize(ability);
            CheckAndUpdate();
            abilityMenu.UpdateAbilities();
            abilityMenu.ResetFusionChoices();
        }
    }

    private void CheckAndUpdate() {
        AbilityMenu.instance.ResetFusionChoices();
        UpdateFusionChoices();
    }

    public void UpdateFusionChoices() {
        FusionSettingsUpdated();
        abilityMenu.UpdateFusionChoicesMenu();
    }

    public void FusionSettingsUpdated() {
        if (abilityMenu.fusionSource1 == null || abilityMenu.fusionSource2 == null) return;
        var newAbility = AbilityFusion.Fuse(abilityMenu.fusionSource1, abilityMenu.fusionSource2);
        fusionDestination.Initialize(newAbility);
        abilityMenu.fusionResult = newAbility;
    }

    private bool IsValid(GameObject pointerDrag) {
        if (pointerDrag == null) return false;
        if (pointerDrag.GetComponent<AbilityScreenIcon>() != null) return true;
        if (pointerDrag.GetComponent<MouseOverHotbarButton>() != null) return true;
        return false;
    }
}
