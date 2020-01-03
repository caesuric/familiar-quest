using UnityEngine;
using UnityEngine.UI;

public class GoldTextUpdater : MonoBehaviour {

    Text text;
    // Use this for initialization
    void Start() {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        if (PlayerCharacter.players.Count <= 0) return;
        text.text = PlayerCharacter.players[0].gold.ToString();
    }
}
