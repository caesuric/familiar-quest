using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FusionAttributeDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public Image background;
    public int slot = 1;
    private Color originalColor;

    // Use this for initialization
    void Start() {
        originalColor = background.color;
    }

    public void OnPointerEnter(PointerEventData data) {
        if (IsValid(data.pointerDrag)) background.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData data) {
        background.color = originalColor;
    }

    public void OnDrop(PointerEventData data) {
        var abilityMenu = AbilityMenu.instance;
        InputMovement.isDragging = false;
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<FusionAttributeItem>() != null) {
            var attribute = data.pointerDrag.GetComponent<FusionAttributeItem>().attribute;
            if (slot == 1 && abilityMenu.fusionAbilityAttributesSelected.Contains(attribute)) {
                abilityMenu.fusionAbilityAttributesSelected.Remove(attribute);
                abilityMenu.fusionAbilityAttributes.Add(attribute);
                abilityMenu.UpdateFusionAttributeLists();
                FusionDropSlot.instance.FusionSettingsUpdated();
            }
            else if (slot == 2 && abilityMenu.fusionAbilityAttributes.Contains(attribute)) {
                abilityMenu.fusionAbilityAttributes.Remove(attribute);
                abilityMenu.fusionAbilityAttributesSelected.Add(attribute);
                abilityMenu.UpdateFusionAttributeLists();
                FusionDropSlot.instance.FusionSettingsUpdated();
            }
            else return;
        }
    }

    private bool IsValid(GameObject pointerDrag) {
        if (pointerDrag == null) return false;
        if (pointerDrag.GetComponent<FusionAttributeItem>() != null) return true;
        return false;
    }


}
