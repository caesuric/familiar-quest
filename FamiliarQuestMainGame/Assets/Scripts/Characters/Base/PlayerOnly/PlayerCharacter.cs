﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

//[NetworkSettings(sendInterval = 0.016f)]
[RequireComponent(typeof(Character))]
[RequireComponent(typeof(HotbarUser))]
[RequireComponent(typeof(ConfigGrabber))]
public class PlayerCharacter : MonoBehaviour {

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
        if (!ready && GetComponent<NetworkCharacterSyncer>() == ProtoClient.localPlayer) {
            OnStartLocalPlayer();
            var lobbyCurtain = GameObject.FindGameObjectWithTag("LobbyCurtain");
            if (lobbyCurtain != null) lobbyCurtain.SetActive(false);
            ready = true;
        }
        //else if (!ready && GetComponent<ConfigGrabber>().overworldInitializer!=null) {
        //    GetComponent<ConfigGrabber>().overworldInitializer.CmdSetCharacterPosition(gameObject);
        //    GameObject.FindGameObjectWithTag("LobbyCurtain").SetActive(false);
        //    ready = true;
        //}
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
        if (GetComponent<AbilityUser>().HasPassive("goldBoost")) {
            foreach (var attribute in GetComponent<AbilityUser>().soulGemPassive.attributes) {
                if (attribute.type == "goldBoost") amount = (int)(amount * (1f + (float)attribute.FindParameter("degree").value));
            }
        }
        gold += amount;
    }

    //[Command]
    public void CmdEquipItem(int number, int slotNumber) {
        if (number < 0 || number >= inventory.items.Count) return;
        var item = inventory.items[number] as Equipment;
        if (item is Weapon) EquipWeapon(number);
        else if (item is Armor) EquipArmor(number);
        else if (item is Necklace) EquipNecklace(number);
        else if (item is Belt) EquipBelt(number);
        else if (item is Cloak) EquipCloak(number);
        else if (item is Earring) EquipEarring(number);
        else if (item is Hat) EquipHat(number);
        else if (item is Shoes) EquipShoes(number);
        else EquipBracelet(number, slotNumber);
        GetComponent<Character>().CalculateAll();
        if (inventory != null) StartCoroutine(inventory.RefreshInABit());
    }

    //[Command]
    public void CmdEquipBracelet(int number, int targetNumber) {
        EquipBracelet(number, targetNumber);
        GetComponent<Character>().CalculateAll();
        if (inventory != null) StartCoroutine(inventory.RefreshInABit());
    }

    private void EquipWeapon(int number) {
        if (weapon!=null) inventory.items.Add(weapon);
        ModifyStats(weapon, inventory.items[number]);
        weapon = (Weapon)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipArmor(int number) {
        if (armor!=null) inventory.items.Add(armor);
        ModifyStats(armor, inventory.items[number]);
        armor = (Armor)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipNecklace(int number) {
        if (necklace!=null) inventory.items.Add(necklace);
        ModifyStats(necklace, inventory.items[number]);
        necklace = (Necklace)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipBelt(int number) {
        if (belt!=null) inventory.items.Add(belt);
        ModifyStats(belt, inventory.items[number]);
        belt = (Belt)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipBracelet(int number, int targetNumber) {
        if (number < 0) {
            SwapBracelets(0 - number - 3, targetNumber);
            return;
        }
        if (bracelets[targetNumber]!=null) inventory.items.Add(bracelets[targetNumber]);
        ModifyStats(bracelets[targetNumber], inventory.items[number]);
        bracelets[targetNumber] = (Bracelet)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void SwapBracelets(int number1, int number2) {
        var tmp = bracelets[number1];
        bracelets[number1] = bracelets[number2];
        bracelets[number2] = tmp;
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipCloak(int number) {
        if (cloak!=null) inventory.items.Add(cloak);
        ModifyStats(cloak, inventory.items[number]);
        cloak = (Cloak)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipHat(int number) {
        if (hat!=null) inventory.items.Add(hat);
        ModifyStats(hat, inventory.items[number]);
        hat = (Hat)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipShoes(int number) {
        if (shoes!=null) inventory.items.Add(shoes);
        ModifyStats(shoes, inventory.items[number]);
        shoes = (Shoes)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    private void EquipEarring(int number) {
        if (earring!=null) inventory.items.Add(earring);
        ModifyStats(earring, inventory.items[number]);
        earring = (Earring)(inventory.items[number]);
        inventory.items.RemoveAt(number);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    public void ModifyStats(Equipment oldEquip, Item newItem) {
        var newEquip = (Equipment)newItem;
        if (oldEquip != null) {
            foreach (var kvp in oldEquip.stats) CharacterAttribute.attributes[kvp.Key].instances[GetComponent<Character>()].ItemValue -= kvp.Value;
        }
        foreach (var kvp in newEquip.stats) CharacterAttribute.attributes[kvp.Key].instances[GetComponent<Character>()].ItemValue += kvp.Value;
    }

    //[Command]
    public void CmdLoadCharacter(byte[] data) {
        if (data == null) {
            players.Remove(this);
            localPlayer = null;
            return;
        }
        CharacterSelectScreen.DeserializeCharacter(data);
        CharacterSelectScreen.loadedCharacter.ConvertTo(gameObject);
        var au = GetComponent<AbilityUser>();
        au.AddPassive(au.soulGemPassive);
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

    public void GainSoulGem(Ability ability) {
        GetComponent<ObjectSpawner>().CreateFloatingStatusText("FOUND A SOUL GEM!", "Found a soul gem!");
        if (ability is ActiveAbility activeAbility) {
            activeAbility.currentCooldown = 0;
            GetComponent<AbilityUser>().soulGemActives.Add(activeAbility);
        }
        else {
            GetComponent<AbilityUser>().soulGemPassivesOverflow.Add((PassiveAbility)ability);
        }
        DropsArea.AddAbilityDrop(ability);
        GetComponent<HotbarUser>().CmdRefreshAbilityInfo();
    }

    public static int GetAverageLevel() {
        float total = 0;
        foreach (var player in players) total += player.GetComponent<ExperienceGainer>().level;
        total /= (players.Count);
        return (int)total;
    }
}
