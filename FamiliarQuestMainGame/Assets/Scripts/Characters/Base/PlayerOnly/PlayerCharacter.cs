using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

//[NetworkSettings(sendInterval = 0.016f)]
public class PlayerCharacter : DependencyUser {

    public static List<PlayerCharacter> players = new List<PlayerCharacter>();
    public static PlayerCharacter localPlayer = null;

    //[SyncVar]
    public int gold;
    //[SyncVar]
    public uint target;
    public Weapon weapon = new MeleeWeapon();
    public Armor armor = null;
    public Necklace necklace = null;
    public Belt belt = null;
    public Bracelet[] bracelets = { null, null, null, null };
    public Cloak cloak = null;
    public Earring earring = null;
    public Hat hat = null;
    public Shoes shoes = null;
    public IconCache iconCache;
    public List<Consumable> consumables = new List<Consumable>();
    public string selectedClass;
    public List<GameObject> effectIconObjects = new List<GameObject>();
    public bool configLoaded = false;
    public GameObject playerPrefab;
    private readonly bool positionSet = false;
    public Inventory inventory;
    public bool ready = false;
    public GameObject lightObj;
    public bool litArea = false;
    public GameObject losSource;
    public string characterId = null;
    public bool isMe = false;
    private bool loaded = false;

    // Use this for initialization
    void Start() {
        dependencies = new List<string>() { "Character", "HotbarUser", "ConfigGrabber" };
        Dependencies.Check(this);
        players.Add(this);
        if (SceneManager.GetActiveScene().name != "Dungeon") StartCoroutine(StartSub());
        OnStartServer();
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator StartSub() {
        yield return new WaitForSeconds(0.5f);
        transform.position = new Vector3(0, 145, 0);
    }

    private void Update() {
        if (!isMe) return;
        if (localPlayer==null) localPlayer = this;
        if (!ready && SharedInventory.instance != null) { // && GetComponent<ConfigGrabber>().si!=null
            //GetComponent<Mapper>().Reset();
            //GetComponent<ConfigGrabber>().si.character = gameObject;
            //if (GetComponent<ConfigGrabber>().il != null && InitializeLevel.currentFloor > 0) {
            //    SceneInitializer.instance.inside = true;
            //}
            //GetComponent<ConfigGrabber>().si.currentCharPosition = SceneInitializer.charPosition;
            //GetComponent<ConfigGrabber>().si.CmdSetCharacterPosition(gameObject);
            if (GetComponent<NetworkCharacterSyncer>() == ProtoClient.localPlayer) OnStartLocalPlayer();
            SharedInventory.instance.CmdRefresh();
            var lobbyCurtain = GameObject.FindGameObjectWithTag("LobbyCurtain");
            if (lobbyCurtain != null) lobbyCurtain.SetActive(false);
            ready = true;
        }
        else if (!ready && GetComponent<ConfigGrabber>().overworldInitializer!=null) {
            GetComponent<ConfigGrabber>().overworldInitializer.CmdSetCharacterPosition(gameObject);
            SharedInventory.instance.CmdRefresh();
            GameObject.FindGameObjectWithTag("LobbyCurtain").SetActive(false);
            ready = true;
        }
        //else if (!ready && GetComponent<ConfigGrabber>().initializeOverlordMode!=null) {
        //    if (NetworkServer.active) {
        //        GetComponent<ConfigGrabber>().initializeOverlordMode.SetupOverlord(gameObject);
        //    }
        //    else {
        //        GetComponent<ConfigGrabber>().initializeOverlordMode.CmdSetupCharacter(gameObject);
        //        GetComponent<ConfigGrabber>().initializeOverlordMode.SetupCharacter();
        //    }
        //    SharedInventory.instance.CmdRefresh();
        //    GameObject.FindGameObjectWithTag("LobbyCurtain").SetActive(false);
        //    ready = true;
        //}
    }

    public void OnStartServer() {
        MonsterScaler.ScaleToPlayers(players.Count);
        //target = netId.Value;
    }

    public void OnStartLocalPlayer() {
        //base.OnStartLocalPlayer();
        if (loaded) return;
        CmdLoadCharacter(CharacterSelectScreen.characterByteArray);
        GetComponent<Character>().CalculateAll();
        loaded = true;
        //var levelUpMenu = GameObject.FindGameObjectWithTag("LevelUpMenu");
        //levelUpMenu.SetActive(false);
    }

    void OnDestroy() {
        players.Remove(this);
        //if (NetworkServer.active) MonsterScaler.ScaleToPlayers(players.Count);
        MonsterScaler.ScaleToPlayers(players.Count);
    }

    public void GainGold(int amount) {
        if (GetComponent<SpiritUser>().HasPassive("goldBoost")) {
            foreach (var passive in GetComponent<SpiritUser>().spirits[0].passiveAbilities) {
                foreach (var attribute in passive.attributes) {
                    if (attribute.type == "goldBoost") amount = (int)(amount * (1f + attribute.FindParameter("degree").floatVal));
                }
            }
        }
        gold += amount;
    }

    public void EquipSpirit(int number, int slotNumber) {
        CmdEquipSpirit(number, slotNumber);
    }

    //[Command]
    public void CmdEquipSpirit(int number, int slotNumber) {
        if (GetComponent<SpiritUser>().spirits.Count > slotNumber) {
            SharedInventory.instance.spareSpirits.Add(GetComponent<SpiritUser>().spirits[slotNumber]);
            GetComponent<SpiritUser>().spirits[slotNumber] = SharedInventory.instance.spareSpirits[number];
        }
        else {
            GetComponent<SpiritUser>().spirits.Add(SharedInventory.instance.spareSpirits[number]);
        }
        SharedInventory.instance.spareSpirits.Remove(SharedInventory.instance.spareSpirits[number]);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    //[Command]
    public void CmdEquipItem(int number, int slotNumber) {
        Dictionary<string, Action> lookup = new Dictionary<string, Action>() {
            {"weapon", () => EquipWeapon(number) },
            {"armor", () => EquipArmor(number) },
            {"necklace", () => EquipNecklace(number) },
            {"belt", () => EquipBelt(number) },
            {"cloak", () => EquipCloak(number) },
            {"earring", () => EquipEarring(number) },
            {"hat", () => EquipHat(number) },
            {"shoes", () => EquipShoes(number) },
            {"bracelet", () => EquipBracelet(number, slotNumber) }
        };
        if (number < 0) EquipBracelet(number, slotNumber);
        else lookup[SharedInventory.instance.inventoryTypes[number]]();
        GetComponent<Character>().CalculateAll();
        SharedInventory.instance.CmdRefresh();
        if (inventory != null) inventory.RefreshInABit();
    }

    //[Command]
    public void CmdEquipBracelet(int number, int targetNumber) {
        EquipBracelet(number, targetNumber);
        GetComponent<Character>().CalculateAll();
        SharedInventory.instance.CmdRefresh();
        if (inventory != null) inventory.RefreshInABit();
    }

    private void EquipWeapon(int number) {
        SharedInventory.instance.inventory.Add(weapon);
        ModifyStats(weapon, SharedInventory.instance.inventory[number]);
        weapon = (Weapon)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipArmor(int number) {
        if (armor!=null) SharedInventory.instance.inventory.Add(armor);
        ModifyStats(armor, SharedInventory.instance.inventory[number]);
        armor = (Armor)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipNecklace(int number) {
        if (necklace!=null) SharedInventory.instance.inventory.Add(necklace);
        ModifyStats(necklace, SharedInventory.instance.inventory[number]);
        necklace = (Necklace)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipBelt(int number) {
        if (belt!=null) SharedInventory.instance.inventory.Add(belt);
        ModifyStats(belt, SharedInventory.instance.inventory[number]);
        belt = (Belt)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipBracelet(int number, int targetNumber) {
        if (number < 0) {
            SwapBracelets(0 - number - 3, targetNumber);
            return;
        }
        if (bracelets[targetNumber]!=null) SharedInventory.instance.inventory.Add(bracelets[targetNumber]);
        ModifyStats(bracelets[targetNumber], SharedInventory.instance.inventory[number]);
        bracelets[targetNumber] = (Bracelet)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void SwapBracelets(int number1, int number2) {
        var tmp = bracelets[number1];
        bracelets[number1] = bracelets[number2];
        bracelets[number2] = tmp;
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipCloak(int number) {
        if (cloak!=null) SharedInventory.instance.inventory.Add(cloak);
        ModifyStats(cloak, SharedInventory.instance.inventory[number]);
        cloak = (Cloak)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipHat(int number) {
        if (hat!=null) SharedInventory.instance.inventory.Add(hat);
        ModifyStats(hat, SharedInventory.instance.inventory[number]);
        hat = (Hat)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipShoes(int number) {
        if (shoes!=null) SharedInventory.instance.inventory.Add(shoes);
        ModifyStats(shoes, SharedInventory.instance.inventory[number]);
        shoes = (Shoes)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipEarring(int number) {
        if (earring!=null) SharedInventory.instance.inventory.Add(earring);
        ModifyStats(earring, SharedInventory.instance.inventory[number]);
        earring = (Earring)(SharedInventory.instance.inventory[number]);
        SharedInventory.instance.inventory.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void ModifyStats(Equipment oldEquip, Item newItem) {
        var newEquip = (Equipment)newItem;
        if (oldEquip != null) {
            //GetComponent<Character>().strength += newEquip.strength - oldEquip.strength;
            //GetComponent<Character>().dexterity += newEquip.dexterity - oldEquip.dexterity;
            //GetComponent<Character>().constitution += newEquip.constitution - oldEquip.constitution;
            //GetComponent<Character>().intelligence += newEquip.intelligence - oldEquip.intelligence;
            //GetComponent<Character>().wisdom += newEquip.wisdom - oldEquip.wisdom;
            //GetComponent<Character>().luck += newEquip.luck - oldEquip.luck;
            CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].ItemValue += newEquip.strength - oldEquip.strength;
            CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].ItemValue += newEquip.dexterity - oldEquip.dexterity;
            CharacterAttribute.attributes["constitution"].instances[GetComponent<Character>()].ItemValue += newEquip.constitution- oldEquip.constitution;
            CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].ItemValue += newEquip.intelligence - oldEquip.intelligence;
            CharacterAttribute.attributes["wisdom"].instances[GetComponent<Character>()].ItemValue += newEquip.wisdom - oldEquip.wisdom;
            CharacterAttribute.attributes["luck"].instances[GetComponent<Character>()].ItemValue += newEquip.luck - oldEquip.luck;
        }
        else {
            //GetComponent<Character>().strength += newEquip.strength;
            //GetComponent<Character>().dexterity += newEquip.dexterity;
            //GetComponent<Character>().constitution += newEquip.constitution;
            //GetComponent<Character>().intelligence += newEquip.intelligence;
            //GetComponent<Character>().wisdom += newEquip.wisdom;
            //GetComponent<Character>().luck += newEquip.luck;
            CharacterAttribute.attributes["strength"].instances[GetComponent<Character>()].ItemValue += newEquip.strength;
            CharacterAttribute.attributes["dexterity"].instances[GetComponent<Character>()].ItemValue += newEquip.dexterity;
            CharacterAttribute.attributes["constitution"].instances[GetComponent<Character>()].ItemValue += newEquip.constitution;
            CharacterAttribute.attributes["intelligence"].instances[GetComponent<Character>()].ItemValue += newEquip.intelligence;
            CharacterAttribute.attributes["wisdom"].instances[GetComponent<Character>()].ItemValue += newEquip.wisdom;
            CharacterAttribute.attributes["luck"].instances[GetComponent<Character>()].ItemValue += newEquip.luck;
        }
    }

    //[Command]
    public void CmdLoadCharacter(byte[] data) {
        CharacterSelectScreen.DeserializeCharacter(data);
        CharacterSelectScreen.loadedCharacter.ConvertTo(gameObject);
        MonsterScaler.ScaleToLevel();
    }

    //[ClientRpc]
    //public void RpcSetColor(int type) {
    //    var skins = GetComponent<CacheGrabber>().kittenFurCache;
    //    if (type != -1 && skins.Count > 0) {
    //        var renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    //        foreach (var renderer in renderers) {
    //            if (renderer.gameObject.CompareTag("KittenBody")) {
    //                renderer.material = skins[type];
    //                break;
    //            }
    //        }
    //    }
    //}

    //[ClientRpc]
    public void RpcRefreshInventory() {
        if (inventory != null) StartCoroutine(inventory.RefreshInABit());
    }

    //[ClientRpc]
    public void RpcClearReady() {
        ready = false;
    }

    public void GainSpirits(List<Spirit> spirits) {
        if (spirits.Count == 1) GetComponent<ObjectSpawner>().CreateFloatingStatusText("FOUND AN ABILITY!", "Found an ability!");
        else if (spirits.Count > 1) GetComponent<ObjectSpawner>().CreateFloatingStatusText("FOUND " + spirits.Count.ToString() + " ABILITIES!", "Found " + spirits.Count.ToString() + " abilities!");
        //foreach (var spirit in spirits) SharedInventory.instance.spareSpirits.Add(spirit);
        //foreach (var spirit in spirits) GetComponent<SpiritUser>().spirits[0].activeAbilities.Add(spirit.activeAbilities[0]);
        if (spirits.Count > 0) {
            if (spirits[0].activeAbilities.Count>0) {
                spirits[0].activeAbilities[0].currentCooldown = 0;
                GetComponent<SpiritUser>().spirits[0].activeAbilities.Add(spirits[0].activeAbilities[0]);
                DropsArea.AddAbilityDrop(spirits[0].activeAbilities[0]);
            }
            else if (spirits[0].passiveAbilities.Count>0) {
                GetComponent<SpiritUser>().overflowAbilities.Add(spirits[0].passiveAbilities[0]);
                DropsArea.AddAbilityDrop(spirits[0].passiveAbilities[0]);
            }
            
        }
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    public static int GetAverageLevel() {
        float total = 0;
        foreach (var player in players) total += player.GetComponent<ExperienceGainer>().level;
        total /= (players.Count);
        return (int)total;
    }
}
