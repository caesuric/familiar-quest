using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class SceneInitializer : MonoBehaviour {

    public static SceneInitializer instance;
    public static Vector3 charPosition;
    public GameObject character = null;
    //[SyncVar]
    public Vector3 currentCharPosition = new Vector3();
    public bool inside = false;

    void Start() {
        if (instance != null) {
            Destroy(gameObject);
            Destroy(this);
            return;
        }
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() {
        if (MusicController.instance == null) return;
        if (LevelGen.instance == null && SceneManager.GetActiveScene().name == "Starting Area") {
            MusicController.instance.PlayMusic(MusicController.instance.townMusic);
            return;
        }
        else if (LevelGen.instance == null && SceneManager.GetActiveScene().name == "Character Selection") {
            MusicController.instance.PlayMusic(MusicController.instance.menuMusic);
            return;
        }
        else if (LevelGen.instance == null && SceneManager.GetActiveScene().name == "Overworld") {
            MusicController.instance.PlayMusic(MusicController.instance.overworldMusic);
            return;
        }
        if (LevelGen.instance.dungeonType == null) return;
        var envType = LevelGen.instance.dungeonType.environmentType;
        if (LevelGen.instance.bossFightActive) MusicController.instance.PlayMusic(MusicController.instance.bossMusic);
        else if (LevelGen.instance.resettled && LevelGen.instance.dungeonType.settingType == DungeonSetting.DESIGNED) MusicController.instance.PlayMusic(MusicController.instance.dungeonMusic);
        else if (envType == "castle" || envType == "dungeon") MusicController.instance.PlayMusic(MusicController.instance.fortressMusic);
        else if (envType == "mine" || envType == "cave") MusicController.instance.PlayMusic(MusicController.instance.cavesAndMinesMusic);
        else if (envType == "sewer") MusicController.instance.PlayMusic(MusicController.instance.sewersMusic);
        else if (envType == "temple") MusicController.instance.PlayMusic(MusicController.instance.templeMusic);
        else if (envType == "vault") MusicController.instance.PlayMusic(MusicController.instance.treasureMusic);
        else if (envType == "tomb") MusicController.instance.PlayMusic(MusicController.instance.tombMusic);
    }

    //[Command]
    public void CmdSetCharacterPosition(GameObject obj) {
        if (Physics.CheckBox(new Vector3(currentCharPosition.x, currentCharPosition.y + 0.5f, currentCharPosition.z), new Vector3(0.4f, 0.4f, 0.4f))) {
            currentCharPosition = new Vector3(currentCharPosition.x + UnityEngine.Random.Range(-2, 2), currentCharPosition.y, currentCharPosition.z + UnityEngine.Random.Range(-2, 2));
            CmdSetCharacterPosition(obj);
        }
        else {
            obj.transform.position = currentCharPosition;
            RpcMoveCharacter(obj, currentCharPosition.x, currentCharPosition.z);
        }
    }

    //[ClientRpc]
    public void RpcMoveCharacter(GameObject obj, float x, float y) {
        obj.transform.position = new Vector3(x, 145, y);
    }

    public IEnumerator SetPositionInABit(GameObject obj) {
        yield return new WaitForSeconds(0.25f);
        CmdSetCharacterPosition(obj);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        //InitializeLevel.SetStartingPosition();
        SceneLoadedShared(scene, mode);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnNexusSceneLoaded(Scene scene, LoadSceneMode mode) {
        currentCharPosition = new Vector3(28, 0, 27);
        SceneLoadedShared(scene, mode);
        SceneManager.sceneLoaded -= OnNexusSceneLoaded;
    }

    public void OnPitTrapSceneLoaded(Scene scene, LoadSceneMode mode) {
        SceneLoadedShared(scene, mode);
        SceneManager.sceneLoaded -= OnPitTrapSceneLoaded;
    }

    public void SceneLoadedShared(Scene scene, LoadSceneMode mode) {
        foreach (var player in PlayerCharacter.players) StartCoroutine(SceneInitializer.instance.SetPositionInABit(player.gameObject));
    }
}
