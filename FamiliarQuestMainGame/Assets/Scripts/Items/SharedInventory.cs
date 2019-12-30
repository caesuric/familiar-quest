using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class SharedInventory : MonoBehaviour {

    public static SharedInventory instance;
    //public SyncListString spareSpiritNames = new SyncListString();
    //public SyncListString spareSpiritDescriptions = new SyncListString();
    //public SyncListString inventoryNames = new SyncListString();
    //public SyncListString inventoryDescriptions = new SyncListString();
    //public SyncListString inventoryTypes = new SyncListString();
    //public SyncListString inventorySubtypes = new SyncListString();
    //public SyncListInt inventoryQualities = new SyncListInt();
    //public SyncListFloat inventoryMainStat = new SyncListFloat();
    //public SyncListFloat inventoryMainStat2 = new SyncListFloat();
    //public SyncListInt inventoryStr = new SyncListInt();
    //public SyncListInt inventoryDex = new SyncListInt();
    //public SyncListInt inventoryCon = new SyncListInt();
    //public SyncListInt inventoryInt = new SyncListInt();
    //public SyncListInt inventoryWis = new SyncListInt();
    //public SyncListInt inventoryLuc = new SyncListInt();
    //public SyncListString inventoryIcons = new SyncListString();
    public List<string> spareSpiritNames = new List<string>();
    public List<string> spareSpiritDescriptions = new List<string>();
    public List<string> inventoryNames = new List<string>();
    public List<string> inventoryDescriptions = new List<string>();
    public List<string> inventoryTypes = new List<string>();
    public List<string> inventorySubtypes = new List<string>();
    public List<int> inventoryQualities = new List<int>();
    public List<float> inventoryMainStat = new List<float>();
    public List<float> inventoryMainStat2 = new List<float>();
    public List<int> inventoryStr = new List<int>();
    public List<int> inventoryDex = new List<int>();
    public List<int> inventoryCon = new List<int>();
    public List<int> inventoryInt = new List<int>();
    public List<int> inventoryWis = new List<int>();
    public List<int> inventoryLuc = new List<int>();
    public List<string> inventoryIcons = new List<string>();
    public List<Item> inventory = new List<Item>();
    public List<Spirit> spareSpirits = new List<Spirit>();

    void Start() {
        if (instance != null) {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        else instance = this;
        if (LobbyManager.singleton.worldByteArray != null) {
            BinaryFormatter bf = new BinaryFormatter();
            var loadedWorld = (SavedWorld)bf.Deserialize(new MemoryStream(LobbyManager.singleton.worldByteArray));
            loadedWorld.ConvertTo(gameObject);
        }
    }

    //[Command]
    public void CmdRefresh() {
        RefreshSharedInventory();
        RefreshPlayerGear();
    }

    private void RefreshSharedInventory()
    {
        inventoryNames.Clear();
        inventoryDescriptions.Clear();
        inventoryTypes.Clear();
        inventorySubtypes.Clear();
        inventoryQualities.Clear();
        inventoryMainStat.Clear();
        inventoryMainStat2.Clear();
        inventoryStr.Clear();
        inventoryDex.Clear();
        inventoryCon.Clear();
        inventoryInt.Clear();
        inventoryWis.Clear();
        inventoryLuc.Clear();
        inventoryIcons.Clear();
        foreach (var item in inventory)
        {
            inventoryNames.Add(item.name);
            inventoryDescriptions.Add(item.description);
            if (item is Weapon) {
                inventoryTypes.Add("weapon");
                if (item is MeleeWeapon) inventorySubtypes.Add("strength");
                else if (item is RangedWeapon && ((RangedWeapon)item).usesInt) inventorySubtypes.Add("intelligence");
                else inventorySubtypes.Add("dexterity");
                inventoryMainStat.Add(((Weapon)item).attackPower);
                inventoryMainStat2.Add(0);
            }
            else if (item is Armor) {
                inventoryTypes.Add("armor");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(((Armor)item).armor);
                inventoryMainStat2.Add(0);
            }
            else if (item is Necklace) {
                inventoryTypes.Add("necklace");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(0);
                inventoryMainStat2.Add(0);
            }
            else if (item is Belt) {
                inventoryTypes.Add("belt");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(0);
                inventoryMainStat2.Add(0);
            }
            else if (item is Bracelet) {
                inventoryTypes.Add("bracelet");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(0);
                inventoryMainStat2.Add(0);
            }
            else if (item is Cloak) {
                inventoryTypes.Add("cloak");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(0);
                inventoryMainStat2.Add(0);
            }
            else if (item is Earring) {
                inventoryTypes.Add("earring");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(0);
                inventoryMainStat2.Add(0);
            }
            else if (item is Hat) {
                inventoryTypes.Add("hat");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(((Hat)item).armor);
                inventoryMainStat2.Add(0);
            }
            else if (item is Shoes) {
                inventoryTypes.Add("shoes");
                inventorySubtypes.Add("");
                inventoryMainStat.Add(((Shoes)item).armor);
                inventoryMainStat2.Add(0);
            }
            if (item is Equipment) inventoryQualities.Add(((Equipment)item).quality);
            else inventoryQualities.Add(0);
            inventoryStr.Add(((Equipment)item).strength);
            inventoryDex.Add(((Equipment)item).dexterity);
            inventoryCon.Add(((Equipment)item).constitution);
            inventoryInt.Add(((Equipment)item).intelligence);
            inventoryWis.Add(((Equipment)item).wisdom);
            inventoryLuc.Add(((Equipment)item).luck);
            inventoryIcons.Add(((Equipment)item).icon);
        }
    }

    private void RefreshPlayerGear()
    {
        foreach (var player in PlayerCharacter.players)
        {
            //player.GetComponent<MenuUser>().weaponName = player.weapon.name;
            //if (player.armor != null) player.GetComponent<MenuUser>().armorName = player.armor.name;
            //else player.GetComponent<MenuUser>().armorName = "";
            //if (player.necklace != null) player.GetComponent<MenuUser>().accessoryName = player.necklace.name;
            //else player.GetComponent<MenuUser>().accessoryName = "";
            //player.GetComponent<MenuUser>().weaponDescription = player.weapon.description;
            //if (player.armor != null) player.GetComponent<MenuUser>().armorDescription = player.armor.description;
            //else player.GetComponent<MenuUser>().armorDescription = "";
            //if (player.necklace != null) player.GetComponent<MenuUser>().accessoryDescription = player.necklace.description;
            //else player.GetComponent<MenuUser>().accessoryDescription = "";
            player.RpcRefreshInventory();
        }
    }
}
