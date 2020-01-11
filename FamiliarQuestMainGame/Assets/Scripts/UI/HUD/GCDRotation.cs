using UnityEngine;
using UnityEngine.UI;

public class GCDRotation : MonoBehaviour {

    private GameObject player = null;
    private Character character = null;
    private PlayerCharacter pc = null;
    public Image GCDMask;
    public ActiveAbility ability = null;
    public int number;

    // Update is called once per frame
    void Update() {
        if (player == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) player = item.gameObject;
        }
        else {
            if (character == null) {
                character = player.GetComponent<Character>();
                pc = player.GetComponent<PlayerCharacter>();
            }
            if (pc.GetComponent<HotbarUser>().currentCooldownPercentages.Count > number) GCDMask.fillAmount = pc.GetComponent<HotbarUser>().currentCooldownPercentages[number];
            else GCDMask.fillAmount = character.GetComponent<AbilityUser>().GCDTime / AbilityUser.maxGCDTime;
        }
    }
}
