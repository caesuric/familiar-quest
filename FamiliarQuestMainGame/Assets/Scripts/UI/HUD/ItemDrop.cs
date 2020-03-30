using System.Collections;
using UnityEngine;

public class ItemDrop : MonoBehaviour {

    Equipment item = null;

    public void Initialize(Equipment item, Inventory inventory) {
        StartCoroutine(InitializeInABit(item, inventory));
        this.item = item;
    }

    public IEnumerator InitializeInABit(Equipment item, Inventory inventory) {
        yield return new WaitForSeconds(0.2f);
        var itemUpdater = GetComponent<InventoryItemUpdater>();
        var configObj = GameObject.FindGameObjectWithTag("ConfigObject");
        itemUpdater.Initialize(item, inventory);
    }


    public void OnClick() {
        if (PlayerCharacter.localPlayer.inventory.items.Contains(item)) {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Inventory>().Refresh();
            DropsArea.OpenInventory();
        }
        DropsArea.ClearDrops();
    }
}
