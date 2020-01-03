using UnityEngine;

public class NewXPBarUpdater : MonoBehaviour {

    public Character character = null;
    private MOBAEnergyBar bar;
    // Use this for initialization
    void Start() {
        bar = GetComponent<MOBAEnergyBar>();
    }

    // Update is called once per frame
    void Update() {
        if (character == null) {
            var players = PlayerCharacter.players;
            foreach (var item in players) if (item.isMe) character = item.GetComponent<Character>();
        }
        else bar.Value = character.GetComponent<ExperienceGainer>().xpPercentage * 100;
    }
}
