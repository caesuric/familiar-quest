using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonExit : MonoBehaviour {

    public bool onWayUp = false;
    AutoSaver saveBlocker = null;
    bool used = false;

    // Update is called once per frame
    void Update() {
        if (onWayUp && !saveBlocker.currentlySaving) {
            PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_downstairs");
            foreach (var player in PlayerCharacter.players) player.ready = false;
            var ao = SceneManager.LoadSceneAsync("Starting Area");
            LoadingProgressBar.StartLoad(ao, 0);
            //LobbyManager.singleton.ServerChangeScene("Temp Start Area");
            SceneManager.sceneLoaded += SceneInitializer.instance.OnNexusSceneLoaded;
            onWayUp = false;
        }
    }

    public void Use() {
        //if (!NetworkServer.active) return;
        if (used) return;
        used = true;
        var otherPlayerCharacter = PlayerCharacter.localPlayer;
        //InitializeLevel.currentFloor = 0;
        //InitializeLevel.instance.CmdSetCurrentFloor(0);
        otherPlayerCharacter.configLoaded = false;
        SceneInitializer.instance.inside = true;
        otherPlayerCharacter.litArea = false;
        otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
        saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
        onWayUp = true;
    }
}