using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetUpdater : MonoBehaviour {

    public Text strText;
    public Text dexText;
    public Text conText;
    public Text intText;
    public Text wisText;
    public Text lucText;
    public Text levelText;
    public Text mpRegenText;
    public Text hpRegenText;
    public Text hpRegenOutOfCombatText;

    // Update is called once per frame
    void Update() {
        if (gameObject.activeSelf && PlayerCharacter.players.Count > 0 && PlayerCharacter.localPlayer != null) {
            var player = PlayerCharacter.localPlayer.GetComponent<Character>();
            levelText.text = player.GetComponent<ExperienceGainer>().level.ToString();
            strText.text = player.strength.ToString();
            dexText.text = player.dexterity.ToString();
            conText.text = player.constitution.ToString();
            intText.text = player.intelligence.ToString();
            wisText.text = player.wisdom.ToString();
            lucText.text = player.luck.ToString();
            mpRegenText.text = CalculateMpRegen().ToString() + " / sec.";
            hpRegenText.text = CalculateHpRegen().ToString() + " / sec.";
            hpRegenOutOfCombatText.text = CalculateHpRegenOutOfCombat().ToString() + " / sec.";
        }
    }

    private float CalculateMpRegen() {
        var total = 0f;
        foreach (var effect in PlayerCharacter.localPlayer.GetComponent<StatusEffectHost>().statusEffects) {
            if (effect.type == "mpOverTime") total += (effect.degree / effect.duration);
        }
        return (float)Math.Round(total, 2);
    }

    private float CalculateHpRegen() {
        var total = 0f;
        foreach (var effect in PlayerCharacter.localPlayer.GetComponent<StatusEffectHost>().statusEffects) if (effect.type == "hot") total += effect.degree;
        if (AutoHealer.OutOfCombat()) total += CalculateHpRegenOutOfCombat();
        return (float)Math.Round(total, 2);
    }

    private float CalculateHpRegenOutOfCombat() {
        var total = 60f * 0.375f * PlayerCharacter.localPlayer.GetComponent<Health>().maxHP / 290f;
        return (float)Math.Round(total, 2);
    }
}
