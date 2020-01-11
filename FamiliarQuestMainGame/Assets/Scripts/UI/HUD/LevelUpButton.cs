using UnityEngine;

public class LevelUpButton : MonoBehaviour {

    private PlayerCharacter player = null;
    public GameObject button;
    public GameObject levelUpMenu;

    // Update is called once per frame
    void Update() {
        if (player == null) {
            button.SetActive(false);
            var players = PlayerCharacter.players;
            foreach (var item in players) {
                //if (item.GetComponent<NetworkIdentity>().isLocalPlayer)
                if (item.GetComponent<PlayerCharacter>().isMe) {
                    player = item.GetComponent<PlayerCharacter>();
                }
            }
        }
        else {
            button.SetActive(player.GetComponent<ExperienceGainer>().sparePoints > 0);
        }
    }

    public void Click() {
        levelUpMenu.SetActive(true);
        levelUpMenu.GetComponent<LevelUpMenu>().Initialize(player.GetComponent<Character>());
    }
}
