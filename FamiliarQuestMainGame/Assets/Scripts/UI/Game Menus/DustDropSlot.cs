using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DustDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public Image background;
    private Color originalColor;
    private AbilityMenu abilityMenu;

    // Use this for initialization
    void Start() {
        originalColor = background.color;
        abilityMenu = GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>();
    }

    public void OnPointerEnter(PointerEventData data) {
        if (IsValid(data.pointerDrag)) background.color = Color.yellow;
    }

    public void OnPointerExit(PointerEventData data) {
        background.color = originalColor;
    }

    public void OnDrop(PointerEventData data) {
        InputMovement.isDragging = false;
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<DustItem>() != null) {
            var dust = data.pointerDrag.GetComponent<DustItem>().dust;
            abilityMenu.dustToUse.Add(dust);
            abilityMenu.RefreshDustUsagePanel();
            abilityMenu.UpdateAbilities();
        }
    }

    private bool IsValid(GameObject pointerDrag) {
        if (pointerDrag == null) return false;
        if (pointerDrag.GetComponent<DustItem>() != null) return true;
        return false;
    }
}
