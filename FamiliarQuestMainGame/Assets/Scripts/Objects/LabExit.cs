using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabExit : MonoBehaviour
{
    bool onWayOut = false;
    AutoSaver saveBlocker = null;
    public bool used = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (onWayOut && !saveBlocker.currentlySaving) {
            PlayerCharacter.localPlayer.GetComponent<AudioGenerator>().PlaySoundByName("sfx_upstairs");
            var ao = SceneManager.LoadSceneAsync("Overworld");
            LoadingProgressBar.StartLoad(ao, 1);
            onWayOut = false;
        }
    }

    public void Use() {
        if (used) return;
        used = true;
        var otherPlayerCharacter = PlayerCharacter.localPlayer;
        SceneInitializer.instance.inside = false;
        otherPlayerCharacter.GetComponent<AutoSaver>().SaveCharacter();
        saveBlocker = otherPlayerCharacter.GetComponent<AutoSaver>();
        otherPlayerCharacter.litArea = true;
        onWayOut = true;
    }
}
