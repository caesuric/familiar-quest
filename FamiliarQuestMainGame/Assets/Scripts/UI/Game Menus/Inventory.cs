using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Inventory : MonoBehaviour {

    public GameObject content;
    public GameObject itemUI;
    public SharedInventory sharedInventory = null;
    public GameObject inventoryDetails = null;
    public Image inventoryDetailsBackground = null;
    public Text inventoryDetailsDescriptionText = null;
    public GameObject inventoryDetailsStatTextContainer = null;
    public GameObject inventoryDetailsStatResultsContainer = null;
    public List<GameObject> inventoryItemUpdaters = new List<GameObject>();
    public GameObject weaponBox = null;
    public GameObject armorBox = null;
    public GameObject beltBox = null;
    public GameObject bracelet1Box = null;
    public GameObject bracelet2Box = null;
    public GameObject bracelet3Box = null;
    public GameObject bracelet4Box = null;
    public GameObject cloakBox = null;
    public GameObject earringBox = null;
    public GameObject hatBox = null;
    public GameObject necklaceBox = null;
    public GameObject shoesBox = null;
    public Toggle upgradesOnlyToggle;
    private List<GameObject> itemObjects = new List<GameObject>();
    private PlayerCharacter player = null;
    
    // Update is called once per frame
    void Update() {
        if (sharedInventory == null) {
            var obj = GameObject.FindGameObjectWithTag("ConfigObject");
            if (obj != null) sharedInventory = obj.GetComponent<SharedInventory>();
            //inventoryDetails.SetActive(false);
        }
        if (player == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) player = item.GetComponent<PlayerCharacter>();
        }
        if (sharedInventory != null && sharedInventory.inventory.Count != sharedInventory.inventoryNames.Count) sharedInventory.CmdRefresh();
    }

    public void Refresh() {
        RefreshItems();
        RefreshEquipment();
    }

    private void RefreshItems() {
        Update();
        foreach (var obj in itemObjects) Destroy(obj);
        itemObjects.RemoveRange(0, itemObjects.Count);
        for (int i = 0; i < sharedInventory.inventoryNames.Count; i++) {
            if (FilteredOut(i)) continue;
            var obj = Instantiate(itemUI);
            obj.transform.SetParent(content.transform, false);
            itemObjects.Add(obj);
            var itemUpdater = obj.GetComponentInChildren<InventoryItemUpdater>();
            var attackPower = sharedInventory.inventoryMainStat[i];
            var subtype = sharedInventory.inventorySubtypes[i];
            itemUpdater.Initialize(sharedInventory.inventoryNames[i], sharedInventory.inventoryDescriptions[i], sharedInventory.inventoryTypes[i], this, i, sharedInventory.inventoryQualities[i], sharedInventory.inventoryIcons[i], attackPower: attackPower, subtype: subtype, displayType: sharedInventory.inventory[i].displayType, flavorText: sharedInventory.inventory[i].flavorText);
            obj.AddComponent<DraggableInventoryItem>();
        }
    }

    private void RefreshWeaponSlot(GameObject box, int index, EquipmentSyncer syncer) {
        var item = Instantiate(itemUI);
        inventoryItemUpdaters.Add(item);
        item.transform.SetParent(box.transform, false);
        var itemUpdater = item.GetComponentInChildren<InventoryItemUpdater>();
        itemUpdater.Initialize(syncer.names[index], syncer.descriptions[index], syncer.types[index], this, -1, syncer.qualities[index], syncer.icons[index], attackPower: syncer.attackPower, subtype: syncer.weaponSubtype);
        var eds = item.AddComponent<EquipDropSlot>();
        eds.slotType = syncer.types[index];
    }

    private void RefreshSlot(GameObject box, int index, EquipmentSyncer syncer) {
        if (syncer.qualities[index] == -1) return;
        var item = Instantiate(itemUI);
        inventoryItemUpdaters.Add(item);
        item.transform.SetParent(box.transform, false);
        var itemUpdater = item.GetComponentInChildren<InventoryItemUpdater>();
        itemUpdater.Initialize(syncer.names[index], syncer.descriptions[index], syncer.types[index], this, 0 - index, syncer.qualities[index], syncer.icons[index]);
        var eds = item.AddComponent<EquipDropSlot>();
        eds.slotType = syncer.types[index];
        if (index >= 3 && index <= 6) {
            eds.equipNumber = index - 3;
            item.AddComponent<DraggableInventoryItem>();
        }
    }

    private void RefreshEquipment() {
        foreach (var item in inventoryItemUpdaters) Destroy(item);
        inventoryItemUpdaters.Clear();
        var syncer = PlayerCharacter.localPlayer.GetComponent<EquipmentSyncer>();
        RefreshWeaponSlot(weaponBox, 0, syncer);
        RefreshSlot(armorBox, 1, syncer);
        RefreshSlot(beltBox, 2, syncer);
        RefreshSlot(bracelet1Box, 3, syncer);
        RefreshSlot(bracelet2Box, 4, syncer);
        RefreshSlot(bracelet3Box, 5, syncer);
        RefreshSlot(bracelet4Box, 6, syncer);
        RefreshSlot(cloakBox, 7, syncer);
        RefreshSlot(earringBox, 8, syncer);
        RefreshSlot(hatBox, 9, syncer);
        RefreshSlot(necklaceBox, 10, syncer);
        RefreshSlot(shoesBox, 11, syncer);
    }

    public void EquipItem(int number, int equipNumber = 0) {
        PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_equip2");
        player.CmdEquipItem(number, equipNumber);
        sharedInventory.CmdRefresh();
        StartCoroutine(RefreshInABit());
    }

    public IEnumerator RefreshInABit() {
        yield return new WaitForSeconds(0.2f);
        RefreshItems();
        RefreshEquipment();
    }

    private bool FilteredOut(int i) {
        if (!upgradesOnlyToggle.isOn) return false;
        var primaryStat = GetPrimaryStat(i);
        var comparisonStat = GetComparisonStat(i);
        if (comparisonStat > primaryStat) return false;
        return true;
    }

    private float GetPrimaryStat(int i) {
        var c = PlayerCharacter.localPlayer.GetComponent<Character>();
        var es = PlayerCharacter.localPlayer.GetComponent<EquipmentSyncer>();
        var type = sharedInventory.inventoryTypes[i];
        if (type == "bracelet") type = "bracelet1";
        var num = EquipmentSyncer.slotKeys[type];
        if (type == "weapon") return es.attackPower;
        else if (c.strength > c.dexterity && c.strength > c.intelligence) return es.strength[num];
        else if (c.dexterity > c.strength && c.dexterity > c.intelligence) return es.dexterity[num];
        else return es.intelligence[num];
    }

    private float GetComparisonStat(int i) {
        var c = PlayerCharacter.localPlayer.GetComponent<Character>();
        var type = sharedInventory.inventoryTypes[i];
        if (type == "weapon") return sharedInventory.inventoryMainStat[i];
        if (c.strength > c.dexterity && c.strength > c.intelligence) return sharedInventory.inventoryStr[i];
        else if (c.dexterity > c.strength && c.dexterity > c.intelligence) return sharedInventory.inventoryDex[i];
        else return sharedInventory.inventoryInt[i];
    }
}
