using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LobbyManager : MonoBehaviour { // : NetworkManager {

    public static new LobbyManager singleton;
    public GameObject gameTypeScreen;
    public GameObject internetGameScreen;
    public GameObject lanGameScreen;
    public GameObject matchCreationScreen;
    public GameObject searchScreen;
    public GameObject worldScreen;
    public GameObject newWorldScreen;
    public GameObject hud;
    public Text hudStatusText;
    public GameObject hudDisconnect;
    public GameObject hudEndGame;
    public InputField matchNameInput;
    public InputField worldNameInput;
    public RectTransform serverListBox;
    public RectTransform worldListBox;
    public GameObject serverEntryPrefab;
    public GameObject worldEntryPrefab;
    public bool loaded = false;
    public byte[] worldByteArray = null;
    private GAMETYPE gameType = GAMETYPE.SINGLEPLAYER;
    private List<GameObject> worldButtons = new List<GameObject>();

    private enum GAMETYPE {
        SINGLEPLAYER = 0,
        INTERNET = 1,
        LAN = 2
    }
    
    private void Start() {
        if (singleton != null) Destroy(gameObject);
        else {
            singleton = this;
            DontDestroyOnLoad(gameObject);
            AddResources(new List<string>() { "AbilityAttributeEffects", "AbilityAttributeEffects/DDEs", "AbilityAttributeEffects/DOTs", "Addons", "AOEs", "DamageZones", "Dungeon", "Dungeon/Traps", "Hits", "Monsters", "Player", "Projectiles", "StatusEffects" });
            StartSingleplayer();
        }
    }

    private void Update() {
        if (CharacterSelectScreen.characterByteArray == null) loaded = false;
        if (!loaded && CharacterSelectScreen.characterByteArray != null) {
            loaded = true;
            //StartSingleplayer(); //added to avoid multiplayer options
        }
    }

    private void AddResources(List<string> folders) {
        foreach (var folder in folders) {
            var resources = Resources.LoadAll("Prefabs/" + folder);
            //foreach (var prefab in resources) if (((GameObject)prefab).GetComponent<NetworkIdentity>() != null) spawnPrefabs.Add((GameObject)prefab);
        }
    }

    public void StartSingleplayer() {
        gameType = GAMETYPE.SINGLEPLAYER;
        gameTypeScreen.SetActive(false);
        worldScreen.SetActive(true);
        WorldGenerator.FetchWorlds();
        SetupWorldList();
    }

    public void ClickInternetGame() {
        gameTypeScreen.SetActive(false);
        internetGameScreen.SetActive(true);
    }

    public void ClickLanGame() {
        gameTypeScreen.SetActive(false);
        lanGameScreen.SetActive(true);
    }

    public void Cancel() {
        SceneManager.LoadScene("Character Selection");
        Destroy(gameObject);
    }

    public void HostInternetGame() {
        gameType = GAMETYPE.INTERNET;
        internetGameScreen.SetActive(false);
        worldScreen.SetActive(true);
        WorldGenerator.FetchWorlds();
        SetupWorldList();
    }

    public void FindInternetGame() {
        //StartMatchMaker();
        internetGameScreen.SetActive(false);
        searchScreen.SetActive(true);
        //matchMaker.ListMatches(0, 100, "", true, 0, 0, LoadMatchListGUI);
    }

    public void LoadMatchListGUI(bool success, string extendedInfo, List<MatchInfoSnapshot> matches) {
        foreach (Transform t in serverListBox) Destroy(t.gameObject);
        if (matches.Count == 0) return;
        foreach (var match in matches) {
            var obj = Instantiate(serverEntryPrefab, serverListBox);
            obj.GetComponent<LobbyServerItem>().Initialize(match.name, match.currentSize, match.networkId, this);
        }
    }

    public void CancelInternetGame() {
        internetGameScreen.SetActive(false);
        gameTypeScreen.SetActive(true);
    }

    public void CreateInternetGame() {
        //StartMatchMaker();
        if (matchNameInput.text == "" || matchNameInput == null) matchNameInput.text = "default";
        //matchMaker.CreateMatch(matchNameInput.text, (uint)10, true, "", "", "", 0, 0, OnMatchCreate);
        matchCreationScreen.SetActive(false);
        ActivateHud(true);
        //ServerChangeScene(onlineScene);
    }

    public void CancelCreateInternetGame() {
        matchCreationScreen.SetActive(false);
        internetGameScreen.SetActive(false);
    }

    public void SetupWorldList() {
        foreach (var world in WorldGenerator.worlds) {
            var obj = Instantiate(worldEntryPrefab, worldListBox);
            obj.GetComponent<WorldItem>().Initialize(world, this);
            worldButtons.Add(obj);
        }
    }

    public void CreateNewWorld() {
        worldScreen.SetActive(false);
        newWorldScreen.SetActive(true);
        worldNameInput.text = "";
    }

    public void FinalizeCreateWorld() {
        if (worldNameInput.text!="") {
            WorldGenerator.CreateWorld(worldNameInput.text);
            ClearWorldButtons();
            WorldGenerator.FetchWorlds();
            SetupWorldList();
            newWorldScreen.SetActive(false);
            worldScreen.SetActive(true);
        }
    }

    public void ClearWorldButtons() {
        foreach (var obj in worldButtons) Destroy(obj);
        worldButtons.Clear();
    }

    public void EnterWorld(string worldName) {
        worldByteArray = File.ReadAllBytes(Application.persistentDataPath + "/worlds/" + worldName + ".world");
        switch (gameType) {
            case GAMETYPE.INTERNET:
                worldScreen.SetActive(false);
                matchCreationScreen.SetActive(true);
                break;
            case GAMETYPE.SINGLEPLAYER:
            default:
                //StartHost();
                worldScreen.SetActive(false);
                //ServerChangeScene(onlineScene);
                break;
        }
    }

    public void ActivateHud(bool isServer) {
        hud.SetActive(true);
        hudStatusText.text = matchNameInput.text;
        if (isServer) {
            hudDisconnect.SetActive(false);
            hudEndGame.SetActive(true);
        }
        else {
            hudDisconnect.SetActive(true);
            hudEndGame.SetActive(false);
        }
    }

    public void HudDisconnect() {
        SaveCharacter();
        StartCoroutine(HudDisconnectSub());
    }

    public IEnumerator HudDisconnectSub() {
        yield return new WaitForSeconds(1);
        //StopMatchMaker();
        //NetworkManager.singleton.StopClient();
        SceneManager.LoadScene("Lobby");
        Destroy(gameObject);
    }

    public void HudEndGame() {
        SaveCharacter();
        StartCoroutine(HudEndGameSub());
    }

    public IEnumerator HudEndGameSub() {
        yield return new WaitForSeconds(1);
        //matchMaker.DestroyMatch(matchInfo.networkId, 0, null);
        //StopMatchMaker();
        //NetworkManager.singleton.StopHost();
        SceneManager.LoadScene("Lobby");
        Destroy(gameObject);
    }

    public void SaveCharacter() {
        //foreach (var player in PlayerCharacter.players) if (player.isLocalPlayer) player.GetComponent<AutoSaver>().SaveCharacter();
        foreach (var player in PlayerCharacter.players) if (player.isMe) player.GetComponent<AutoSaver>().SaveCharacter();
    }

    public void OnStopHost() {
        StartCoroutine(OnStopHostSub());

    }

    public IEnumerator OnStopHostSub() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Character Selection");
        Destroy(gameObject);
    }

}
