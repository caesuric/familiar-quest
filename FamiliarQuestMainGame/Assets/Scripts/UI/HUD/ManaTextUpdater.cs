using UnityEngine;
using UnityEngine.UI;

public class ManaTextUpdater : MonoBehaviour {

    public Character attr = null;
    private Text text;
    // Use this for initialization
    void Start() {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        if (attr == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) {
                if (item.isMe) {
                    attr = item.GetComponent<Character>();
                }
            }
        }
        else {
            text.text = Mathf.FloorToInt(attr.GetComponent<Mana>().mp).ToString() + " / " + Mathf.FloorToInt(attr.GetComponent<Mana>().maxMP).ToString();
        }
    }
}
