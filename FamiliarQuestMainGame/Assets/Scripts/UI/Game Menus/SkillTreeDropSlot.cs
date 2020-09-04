using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public Image background;
    private Color originalColor;
    private AbilityMenu abilityMenu;
    public Ability ability = null;
    public static SkillTreeDropSlot instance;
    
    // Use this for initialization
    void Start() {
        instance = this;
        originalColor = background.color;
        abilityMenu = GameObject.FindGameObjectWithTag("AbilityScreen").GetComponent<AbilityMenu>();
    }

    public static void UpdateAbility() {
        if (instance!=null && instance.ability!=null) instance.GetComponent<AbilityScreenIcon>().Initialize(instance.ability);
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
            abilityMenu.skillTreeAbility = ability;
            GetComponent<AbilityScreenIcon>().Initialize(ability);
            abilityMenu.UpdateSkillTree();
        }
        else if (data.pointerDrag != null && data.pointerDrag.GetComponent<MouseOverHotbarButton>() != null) {
            var abilities = PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActives;
            var num = data.pointerDrag.GetComponent<MouseOverHotbarButton>().number;
            ability = abilities[num];
            abilityMenu.skillTreeAbility = ability;
            GetComponent<AbilityScreenIcon>().Initialize(ability);
            abilityMenu.UpdateSkillTree();
        }
    }

    private bool IsValid(GameObject pointerDrag) {
        if (pointerDrag == null) return false;
        if (pointerDrag.GetComponent<AbilityScreenIcon>() != null) return true;
        if (pointerDrag.GetComponent<MouseOverHotbarButton>() != null) return true;
        return false;
    }
}
