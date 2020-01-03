using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityEnhancementDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public Image background;
    public int slot = 1;
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
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<AbilityScreenIcon>() != null) {
            var ability = data.pointerDrag.GetComponent<AbilityScreenIcon>().ability;
            abilityMenu.abilityToEnhance = ability;
            GetComponent<AbilityScreenIcon>().Initialize(ability);
            GetComponent<AbilityScreenIcon>().draggable = false;
            abilityMenu.UpdateAbilities();
        }
        else if (data.pointerDrag != null && data.pointerDrag.GetComponent<MouseOverHotbarButton>() != null) {
            var abilities = PlayerCharacter.localPlayer.GetComponent<SpiritUser>().spirits[0].activeAbilities;
            var num = data.pointerDrag.GetComponent<MouseOverHotbarButton>().number;
            var ability = abilities[num];
            abilityMenu.abilityToEnhance = ability;
            GetComponent<AbilityScreenIcon>().Initialize(ability);
            GetComponent<AbilityScreenIcon>().draggable = false;
            abilityMenu.UpdateAbilities();
        }
    }

    private bool IsValid(GameObject pointerDrag) {
        if (pointerDrag == null) return false;
        if (pointerDrag.GetComponent<AbilityScreenIcon>() != null) return true;
        if (pointerDrag.GetComponent<MouseOverHotbarButton>() != null) return true;
        return false;
    }
}
