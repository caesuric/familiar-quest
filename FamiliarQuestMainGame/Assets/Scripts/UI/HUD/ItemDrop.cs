using System.Collections;
using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public void Initialize(Equipment item, Inventory inventory) {
        StartCoroutine(InitializeInABit(item, inventory));
    }

    public IEnumerator InitializeInABit(Equipment item, Inventory inventory) {
        yield return new WaitForSeconds(0.2f);
        var itemUpdater = GetComponent<InventoryItemUpdater>();
        var configObj = GameObject.FindGameObjectWithTag("ConfigObject");
        itemUpdater.Initialize(item, inventory);
    }


    public void OnClick() {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Inventory>().Refresh();
        DropsArea.OpenInventory();
        DropsArea.ClearDrops();
    }
}
