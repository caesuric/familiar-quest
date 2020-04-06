using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FloorTextUpdater : MonoBehaviour {
    private Text text;

    // Update is called once per frame
    void Update() {
        text = GetComponent<Text>();
        if (SceneManager.GetActiveScene().name == "Overworld") text.text = "overworld";
        else if (LevelGen.instance == null || LevelGen.instance.layout == null) text.text = "wizard's lab";
        else text.text = LevelGen.instance.levelName.ToLower() + " floor " + (LevelGen.instance.floor + 1) + "/" + LevelGen.instance.layout.numFloors;
    }
}