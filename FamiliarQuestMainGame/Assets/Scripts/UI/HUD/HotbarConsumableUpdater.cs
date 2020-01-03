using UnityEngine;
using UnityEngine.UI;

public class HotbarConsumableUpdater : MonoBehaviour {

    public PlayerCharacter pc;
    public Text text;
    public int slot;

    // Update is called once per frame
    void Update() {
        if (pc == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) {
                //if (item.GetComponent<NetworkIdentity>().isLocalPlayer) {
                if (item.GetComponent<PlayerCharacter>().isMe) {
                    pc = item.GetComponent<PlayerCharacter>();
                }
            }
        }
        else {
            if (pc.GetComponent<HotbarUser>().consumableCounts.Count > slot) {
                text.text = "x" + pc.GetComponent<HotbarUser>().consumableCounts[slot].ToString();
            }
            else {
                text.text = "x0";
            }
        }
    }
}
