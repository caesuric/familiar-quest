using UnityEngine;
using UnityEngine.UI;

public class CharacterItemLevelUpdater : MonoBehaviour {

    private PlayerCharacter pc = null;
    private Text text;
    private float timer = 0f;

    // Use this for initialization
    void Start() {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update() {
        if (PlayerCharacter.players.Count == 0) return;
        if (pc == null) pc = PlayerCharacter.players[0];
        timer += Time.deltaTime;
        if (timer >= 1) {
            timer = 0;
            CalculateItemLevel();
        }
    }

    private void CalculateItemLevel() {
        var results = 0;
        if (pc.weapon != null) results += pc.weapon.level;
        if (pc.hat != null) results += pc.hat.level;
        if (pc.necklace != null) results += pc.necklace.level;
        if (pc.armor != null) results += pc.armor.level;
        if (pc.cloak != null) results += pc.cloak.level;
        if (pc.belt != null) results += pc.belt.level;
        if (pc.shoes != null) results += pc.shoes.level;
        if (pc.earring != null) results += pc.earring.level;
        foreach (var bracelet in pc.bracelets) if (bracelet != null) results += bracelet.level;
        results = Mathf.RoundToInt(results / 12f);
        text.text = "AVG. ITEM LEVEL: " + results.ToString();
    }
}
