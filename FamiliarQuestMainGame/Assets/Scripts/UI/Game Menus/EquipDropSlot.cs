using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public string slotType;
    public int equipNumber = 0;
    public Color startingColor;
    // Use this for initialization
    void Start() {
        startingColor = GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData data) {
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<InventoryItemUpdater>() != null) {
            var itemUpdater = data.pointerDrag.GetComponent<InventoryItemUpdater>();
            if (MatchesSlotType(itemUpdater)) GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnPointerExit(PointerEventData data) {
        GetComponent<Image>().color = startingColor;
    }

    public void OnDrop(PointerEventData data) {
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<InventoryItemUpdater>() != null) {
            var itemUpdater = data.pointerDrag.GetComponent<InventoryItemUpdater>();
            if (itemUpdater.item is Bracelet && itemUpdater.number < 0) itemUpdater.inventory.SwapBracelet(itemUpdater.item, equipNumber);
            else if (MatchesSlotType(itemUpdater)) {
                itemUpdater.inventory.EquipItem(itemUpdater.number, equipNumber: equipNumber);
            }
        }
    }

    private bool MatchesSlotType(InventoryItemUpdater inventoryItemUpdater) {
        if (inventoryItemUpdater.type == slotType) return true;
        return false;
    }
}