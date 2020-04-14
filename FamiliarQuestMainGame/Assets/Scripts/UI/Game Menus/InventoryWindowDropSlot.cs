using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryWindowDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    private Color startingColor;
    // Use this for initialization
    void Start() {
        startingColor = GetComponent<Image>().color;
    }

    public void OnPointerEnter(PointerEventData data) {
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<InventoryItemUpdater>() != null) {
            GetComponent<Image>().color = Color.yellow;
        }
    }

    public void OnPointerExit(PointerEventData data) {
        GetComponent<Image>().color = startingColor;
    }

    public void OnDrop(PointerEventData data) {
        GetComponent<Image>().color = startingColor;
        if (data.pointerDrag != null && data.pointerDrag.GetComponent<InventoryItemUpdater>() != null) {
            var itemUpdater = data.pointerDrag.GetComponent<InventoryItemUpdater>();
            var pc = PlayerCharacter.localPlayer;
            if (pc.weapon == itemUpdater.item) return;
            else if (pc.armor == itemUpdater.item) pc.armor = null;
            else if (pc.hat == itemUpdater.item) pc.hat = null;
            else if (pc.shoes == itemUpdater.item) pc.shoes = null;
            else if (pc.earring == itemUpdater.item) pc.earring = null;
            else if (pc.necklace == itemUpdater.item) pc.necklace = null;
            else if (pc.belt == itemUpdater.item) pc.belt = null;
            else if (pc.cloak == itemUpdater.item) pc.cloak = null;
            if (pc.bracelets.Length>0) {
                for (int i=0; i<pc.bracelets.Length; i++) if (pc.bracelets[i] == itemUpdater.item) pc.bracelets[i] = null;
            }
            if (!pc.inventory.items.Contains(itemUpdater.item) && itemUpdater.item != null) pc.inventory.items.Add(itemUpdater.item);
            StartCoroutine(pc.inventory.RefreshInABit());
        }
    }
}