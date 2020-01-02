using UnityEngine;
using UnityEngine.SceneManagement;

public class PitTrap : DestinationTrap {
    public bool tripped = false;
    public bool onWayDownstairs = false;
    public PlayerCharacter character = null;
    public AutoSaver saveBlocker = null;

    public void Update() {
        if (onWayDownstairs && !saveBlocker.currentlySaving) {
            character.GetComponent<AudioGenerator>().PlaySoundByName("sfx_downstairs");
            //foreach (var player in PlayerCharacter.players) player.ready = false;
            //LobbyManager.singleton.ServerChangeScene("Generated Level");
            SceneManager.LoadScene("Generated Level");
            SceneInitializer.instance.currentCharPosition = destination;
            SceneManager.sceneLoaded += SceneInitializer.instance.OnSceneLoaded;
            onWayDownstairs = false;
        }
    }

    //private void OnTriggerEnter(Collider other) {
        //if (!NetworkServer.active) return;
        //var otherPlayerCharacter = other.GetComponent<PlayerCharacter>();
        //if (otherPlayerCharacter != null && !tripped) {
        //    tripped = true;
        //    otherPlayerCharacter.configLoaded = false;
        //    if (InitializeLevel.currentFloor + 1 > InitializeLevel.maps.Count) {
        //        Destroy(this.gameObject);
        //        return;
        //    }
        //    InitializeLevel.currentFloor += 1;
        //    InitializeLevel.instance.CmdSetTargetLevel(InitializeLevel.targetLevel);
        //    InitializeLevel.instance.CmdSetCurrentFloor(InitializeLevel.currentFloor);
        //    InitializeLevel.goingDown = true;
        //    InitializeLevel.ClearObjectLists();
        //    otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
        //    character = otherPlayerCharacter;
        //    saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
        //    onWayDownstairs = true;
        //}
    //}
}
