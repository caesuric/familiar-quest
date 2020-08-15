using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ManaPotionTracker : MonoBehaviour {
    public Text textObj;
    private PlayerCharacter pc = null;

    // Update is called once per frame
    void Update() {
        if (pc == null) pc = PlayerCharacter.localPlayer;
        if (pc == null) return;
        int count = 0;
        foreach (var item in pc.consumables) if (item != null && item.type == ConsumableType.mana) count = item.quantity;
        if (count == 0) textObj.text = "";
        else textObj.text = count.ToString() + "x";
    }
}
