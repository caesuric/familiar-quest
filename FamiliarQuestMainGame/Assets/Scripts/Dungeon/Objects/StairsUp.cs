using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StairsUp : MonoBehaviour {

    bool onWayUpstairs = false;
    AutoSaver saveBlocker = null;
    public bool used = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (onWayUpstairs && !saveBlocker.currentlySaving) {
            PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_downstairs");
            //foreach (var player in PlayerCharacter.players) player.ready = false;
            LevelGen.instance.goingBack = true;
            LevelGen.instance.MoveFloorBackwards();
            //LobbyManager.singleton.ServerChangeScene("Generated Level");
            //SceneManager.sceneLoaded += SceneInitializer.instance.OnSceneLoaded;
            onWayUpstairs = false;
        }
	}
    //private void OnTriggerEnter(Collider other) {
    //    if (!NetworkServer.active) return;
    //    var otherPlayerCharacter = other.GetComponent<PlayerCharacter>();
    //    if (otherPlayerCharacter != null) {
    //        otherPlayerCharacter.configLoaded = false;
    //        //InitializeLevel.targetLevel -= 1;
    //        InitializeLevel.currentFloor -= 1;
    //        InitializeLevel.instance.CmdSetTargetLevel(InitializeLevel.targetLevel);
    //        InitializeLevel.instance.CmdSetCurrentFloor(InitializeLevel.currentFloor);
    //        InitializeLevel.SetEndingPosition();
    //        InitializeLevel.goingDown = true;
    //        InitializeLevel.ClearObjectLists();
    //        otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
    //        character = otherPlayerCharacter;
    //        saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
    //        onWayUpstairs = true;
    //    }
    //}

    public void Use() {
        //if (!NetworkServer.active) return;
        if (used) return;
        used = true;
        //var otherPlayerCharacter = PlayerCharacter.localPlayer;
        //otherPlayerCharacter.configLoaded = false;
        ////InitializeLevel.targetLevel -= 1;
        //InitializeLevel.currentFloor -= 1;
        //InitializeLevel.instance.CmdSetTargetLevel(InitializeLevel.targetLevel);
        //InitializeLevel.instance.CmdSetCurrentFloor(InitializeLevel.currentFloor);
        //InitializeLevel.SetEndingPosition();
        //InitializeLevel.goingDown = true;
        //InitializeLevel.ClearObjectLists();
        PlayerCharacter.localPlayer.GetComponent<AutoSaver>().SaveCharacter();
        saveBlocker = PlayerCharacter.localPlayer.GetComponent<AutoSaver>();
        onWayUpstairs = true;
    }
}
