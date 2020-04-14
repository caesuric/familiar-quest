using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public GameObject content;
    public GameObject itemUI;
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
    public PlayerCharacter player = null;
    public List<Item> items = new List<Item>();

    public static Dictionary<string, int> slotKeys = new Dictionary<string, int>() {
        {"weapon", 0},
        {"armor", 1},
        {"belt", 2},
        {"bracelet1", 3},
        {"bracelet2", 4},
        {"bracelet3", 5},
        {"bracelet4", 6},
        {"cloak", 7},
        {"earring", 8},
        {"hat", 9},
        {"necklace", 10},
        {"shoes", 11}
    };

    // Update is called once per frame
    void Update() {
        if (player == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) player = item.GetComponent<PlayerCharacter>();
            PlayerCharacter.localPlayer = player;
            player.inventory = this;
            if (player!= null && LobbyManager.singleton.worldByteArray != null) {
                BinaryFormatter bf = new BinaryFormatter();
                var loadedWorld = (SavedWorld)bf.Deserialize(new MemoryStream(LobbyManager.singleton.worldByteArray));
                loadedWorld.ConvertTo(GameObject.FindGameObjectWithTag("ConfigObject"));
            }
        }
    }

    public void Refresh() {
        RefreshItems();
        RefreshEquipment();
    }

    private void RefreshItems() {
        Update();
        foreach (var obj in itemObjects) Destroy(obj);
        itemObjects.RemoveRange(0, itemObjects.Count);
        foreach (var item in items) {
            //if (FilteredOut(item)) continue;
            var obj = Instantiate(itemUI);
            obj.transform.SetParent(content.transform, false);
            itemObjects.Add(obj);
            var itemUpdater = obj.GetComponentInChildren<InventoryItemUpdater>();
            itemUpdater.Initialize(item, this);
            obj.AddComponent<DraggableInventoryItem>();
        }
    }

    private void RefreshWeaponSlot(GameObject box) {
        var subtype = "";
        if (player.weapon is MeleeWeapon) subtype = "strength";
        else if (player.weapon is RangedWeapon && ((RangedWeapon)player.weapon).usesInt) subtype = "intelligence";
        else subtype = "dexterity";
        var item = Instantiate(itemUI);
        inventoryItemUpdaters.Add(item);
        item.transform.SetParent(box.transform, false);
        var itemUpdater = item.GetComponentInChildren<InventoryItemUpdater>();
        itemUpdater.Initialize(player.weapon, this);
        var eds = item.AddComponent<EquipDropSlot>();
        eds.slotType = "weapon";
    }

    private void RefreshSlot(GameObject box, int index) {
        Equipment equipment = null;
        var type = "";
        switch (index) {
            case 1:
            default:
                equipment = player.armor;
                type = "armor";
                break;
            case 2:
                equipment = player.belt;
                type = "belt";
                break;
            case 3:
                equipment = player.bracelets[0];
                type = "bracelet";
                break;
            case 4:
                equipment = player.bracelets[1];
                type = "bracelet";
                break;
            case 5:
                equipment = player.bracelets[2];
                type = "bracelet";
                break;
            case 6:
                equipment = player.bracelets[3];
                type = "bracelet";
                break;
            case 7:
                equipment = player.cloak;
                type = "cloak";
                break;
            case 8:
                equipment = player.earring;
                type = "earring";
                break;
            case 9:
                equipment = player.hat;
                type = "hat";
                break;
            case 10:
                equipment = player.necklace;
                type = "necklace";
                break;
            case 11:
                equipment = player.shoes;
                type = "shoes";
                break;
        }
        if (equipment == null) return;
        if (equipment.quality == -1) return;
        var item = Instantiate(itemUI);
        inventoryItemUpdaters.Add(item);
        item.transform.SetParent(box.transform, false);
        var itemUpdater = item.GetComponentInChildren<InventoryItemUpdater>();
        itemUpdater.Initialize(equipment, this);
        var eds = item.AddComponent<EquipDropSlot>();
        eds.slotType = type;
        if (index >= 3 && index <= 6) {
            eds.equipNumber = index - 3;
        }
        if (index != 0) item.AddComponent<DraggableInventoryItem>();
    }

    private void RefreshEquipment() {
        foreach (var item in inventoryItemUpdaters) Destroy(item);
        inventoryItemUpdaters.Clear();
        RefreshWeaponSlot(weaponBox);
        RefreshSlot(armorBox, 1);
        RefreshSlot(beltBox, 2);
        RefreshSlot(bracelet1Box, 3);
        RefreshSlot(bracelet2Box, 4);
        RefreshSlot(bracelet3Box, 5);
        RefreshSlot(bracelet4Box, 6);
        RefreshSlot(cloakBox, 7);
        RefreshSlot(earringBox, 8);
        RefreshSlot(hatBox, 9);
        RefreshSlot(necklaceBox, 10);
        RefreshSlot(shoesBox, 11);
    }

    public void EquipItem(int number, int equipNumber = 0) {
        PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_equip2");
        player.CmdEquipItem(number, equipNumber);
        StartCoroutine(RefreshInABit());
    }

    public void SwapBracelet(Item item, int equipNumber) {
        var bracelet = item as Bracelet;
        var targetBracelet = PlayerCharacter.localPlayer.bracelets[equipNumber];
        int braceletIndex = 0;
        for (int i=0; i<PlayerCharacter.localPlayer.bracelets.Length; i++) {
            if (PlayerCharacter.localPlayer.bracelets[i] == bracelet) braceletIndex = i;
        }
        PlayerCharacter.localPlayer.bracelets[braceletIndex] = targetBracelet;
        PlayerCharacter.localPlayer.bracelets[equipNumber] = bracelet;
        StartCoroutine(RefreshInABit());
    }

    public IEnumerator RefreshInABit() {
        yield return new WaitForSeconds(0.2f);
        RefreshItems();
        RefreshEquipment();
    }
}
