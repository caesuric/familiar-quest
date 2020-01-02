using UnityEngine;

public class StairsDown : MonoBehaviour {
    bool onWayDownstairs = false;
    AutoSaver saveBlocker = null;
    public bool used = false;

    // Update is called once per frame
    void Update() {
        if (onWayDownstairs && !saveBlocker.currentlySaving) {
            PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_downstairs");
            //foreach (var player in PlayerCharacter.players) player.ready = false;
            LevelGen.instance.goingBack = false;
            LevelGen.instance.MoveFloorForward();
            //LobbyManager.singleton.ServerChangeScene("Generated Level");
            //SceneManager.sceneLoaded += SceneInitializer.instance.OnSceneLoaded;
            onWayDownstairs = false;

        }
    }

    public void Use() {
        //if (!NetworkServer.active) return;
        if (used) return;
        used = true;
        //PlayerCharacter.localPlayer.configLoaded = false;
        //InitializeLevel.currentFloor += 1;
        //InitializeLevel.instance.CmdSetTargetLevel(InitializeLevel.targetLevel);
        //InitializeLevel.instance.CmdSetCurrentFloor(InitializeLevel.currentFloor);
        //InitializeLevel.SetStartingPosition();
        //InitializeLevel.goingDown = true;
        //InitializeLevel.ClearObjectLists();
        PlayerCharacter.localPlayer.GetComponent<AutoSaver>().SaveCharacter();
        saveBlocker = PlayerCharacter.localPlayer.GetComponent<AutoSaver>();
        onWayDownstairs = true;
    }
}
