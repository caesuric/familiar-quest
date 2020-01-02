using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour {

    bool onWayDown = false;
    AutoSaver saveBlocker = null;
    public int dungeonLevel = 1;
    public bool used = false;

    // Update is called once per frame
    void Update() {
        if (onWayDown && !saveBlocker.currentlySaving) {
            PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_downstairs");
            SceneManager.LoadScene("Dungeon");
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
        otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
        saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
        otherPlayerCharacter.litArea = true;
        onWayDown = true;
    }
}
