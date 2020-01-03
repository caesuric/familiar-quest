using UnityEngine;

public class ManaBarUpdater : MonoBehaviour {

    public Character attr = null;
    private RectTransform rectTransform;
    // Use this for initialization
    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {
        if (attr == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) {
                //if (item.GetComponent<NetworkIdentity>().isLocalPlayer) {
                if (item.GetComponent<PlayerCharacter>().isMe) {
                    attr = item.GetComponent<Character>();
                }
            }
        }
        else {
            rectTransform.localScale = new Vector3(attr.GetComponent<Mana>().mp / attr.GetComponent<Mana>().maxMP * 4, 0.25f, 1);
        }
    }
}
