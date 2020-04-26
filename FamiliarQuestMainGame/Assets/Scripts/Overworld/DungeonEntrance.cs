using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour {

    bool onWayDown = false;
    AutoSaver saveBlocker = null;
    public int dungeonLevel = 1;
    public OverworldDungeon dungeonData = null;
    public bool used = false;

    // Update is called once per frame
    void Update() {
        if (onWayDown && !saveBlocker.currentlySaving) {
            PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_downstairs");
            var ao = SceneManager.LoadSceneAsync("Dungeon");
            LoadingProgressBar.StartLoad(ao, 0);
            onWayDown = false;
        }
    }

    public void Use() {
        if (used) return;
        used = true;
        var otherPlayerCharacter = PlayerCharacter.localPlayer;
        SceneInitializer.instance.inside = true;
        LevelGen.targetLevel = dungeonLevel;
        LevelGen.settingLevel = true;
        LevelGen.dungeonData = dungeonData;
        otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
        saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
        otherPlayerCharacter.litArea = true;
        onWayDown = true;
    }
}
