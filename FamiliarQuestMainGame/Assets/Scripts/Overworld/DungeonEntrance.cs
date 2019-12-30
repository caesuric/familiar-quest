using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour {

    bool onWayDown = false;
    AutoSaver saveBlocker = null;
    public int dungeonLevel = 1;
    public bool used = false;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (onWayDown && !saveBlocker.currentlySaving) {
            PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_downstairs");
            //foreach (var player in PlayerCharacter.players) player.ready = false;
            //LobbyManager.singleton.ServerChangeScene("Generated Level");
            //LobbyManager.singleton.ServerChangeScene("Dungeon");
            SceneManager.LoadScene("Dungeon");
            //SceneManager.sceneLoaded += SceneInitializer.instance.OnSceneLoaded;
            onWayDown = false;
        }
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (!NetworkServer.active) return;
    //    var otherPlayerCharacter = other.GetComponent<PlayerCharacter>();
    //    if (otherPlayerCharacter != null) {
    //        InitializeLevel.targetLevel = dungeonLevel;
    //        InitializeLevel.currentFloor = 1;
    //        InitializeLevel.instance.CmdSetTargetLevel(dungeonLevel);
    //        InitializeLevel.instance.CmdSetCurrentFloor(1);
    //        otherPlayerCharacter.configLoaded = false;
    //        SceneInitializer.instance.inside = true;
    //        otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
    //        character = otherPlayerCharacter;
    //        saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
    //        onWayDown = true;
    //    }
    //}

    public void Use() {
        //if (!NetworkServer.active) return;
        if (used) return;
        used = true;
        var otherPlayerCharacter = PlayerCharacter.localPlayer;
        //InitializeLevel.targetLevel = dungeonLevel;
        //InitializeLevel.currentFloor = 1;
        //InitializeLevel.instance.CmdSetTargetLevel(dungeonLevel);
        //InitializeLevel.instance.CmdSetCurrentFloor(1);
        //InitializeLevel.SetStartingPosition();
        //otherPlayerCharacter.configLoaded = false;
        SceneInitializer.instance.inside = true;
        LevelGen.targetLevel = dungeonLevel;
        LevelGen.settingLevel = true;
        otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
        saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
        otherPlayerCharacter.litArea = true;
        onWayDown = true;
    }
}
