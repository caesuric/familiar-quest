using UnityEngine;
using UnityEngine.UI;

public class XPTextUpdater : MonoBehaviour {

    public PlayerCharacter attr = null;
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
                //if (item.GetComponent<NetworkIdentity>().isLocalPlayer) {
                if (item.GetComponent<PlayerCharacter>().isMe) {
                    attr = item.GetComponent<PlayerCharacter>();
                }
            }
        }
        else {
            text.text = "Level: " + attr.GetComponent<ExperienceGainer>().level.ToString() + " | XP To Level: " + attr.GetComponent<ExperienceGainer>().xpToLevel.ToString();
        }
    }
}
