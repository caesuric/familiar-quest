using UnityEngine;

public class AbilityDrop : MonoBehaviour {

    private Ability ability = null;

    public void Initialize(Ability ability) {
        GetComponent<AbilityScreenIcon>().Initialize(ability);
        this.ability = ability;
    }

    public void OnClick() {
        if (ability is ActiveAbility activeAbility && PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemActivesOverflow.Contains(activeAbility)) DropsArea.OpenAbilities();
        else if (ability is PassiveAbility passiveAbility && PlayerCharacter.localPlayer.GetComponent<AbilityUser>().soulGemPassivesOverflow.Contains(passiveAbility)) DropsArea.OpenAbilities();
        DropsArea.ClearDrops();
    }
}