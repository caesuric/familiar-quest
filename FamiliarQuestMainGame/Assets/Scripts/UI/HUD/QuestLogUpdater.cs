using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestLogUpdater : MonoBehaviour {

    QuestLog questLog;
    bool initialized = false;
    public Text text;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!initialized && PlayerCharacter.players.Count > 0 && PlayerCharacter.localPlayer.GetComponent<ConfigGrabber>().questLog != null) {
            questLog = PlayerCharacter.localPlayer.GetComponent<ConfigGrabber>().questLog;
            text.text = questLog.quests[0].text;
            initialized = true;
        }
        if (initialized) text.enabled = !questLog.quests[0].completed;
    }
}
