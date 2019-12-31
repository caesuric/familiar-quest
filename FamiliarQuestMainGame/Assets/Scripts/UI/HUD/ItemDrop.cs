using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {

    public void Initialize(Equipment item, Inventory inventory) {
        StartCoroutine(InitializeInABit(item, inventory));
    }

    public IEnumerator InitializeInABit(Equipment item, Inventory inventory) {
        yield return new WaitForSeconds(0.2f);
        var itemUpdater = GetComponent<InventoryItemUpdater>();
        var configObj = GameObject.FindGameObjectWithTag("ConfigObject");
        SharedInventory sharedInventory;
        sharedInventory = configObj.GetComponent<SharedInventory>();
        var i = sharedInventory.inventoryMainStat.Count - 1;
        var attackPower = sharedInventory.inventoryMainStat[i];
        var subtype = sharedInventory.inventorySubtypes[i];
        itemUpdater.Initialize(sharedInventory.inventoryNames[i], sharedInventory.inventoryDescriptions[i], sharedInventory.inventoryTypes[i], inventory, i, sharedInventory.inventoryQualities[i], sharedInventory.inventoryIcons[i], attackPower: attackPower, subtype: subtype);
    }


    public void OnClick() {
        SharedInventory.instance.CmdRefresh();
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Inventory>().Refresh();
        DropsArea.OpenInventory();
        DropsArea.ClearDrops();
    }
}
