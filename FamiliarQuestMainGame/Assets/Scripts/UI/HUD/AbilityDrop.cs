using UnityEngine;

public class AbilityDrop : MonoBehaviour {

    private Ability ability = null;

    public void Initialize(Ability ability) {
        GetComponent<AbilityScreenIcon>().Initialize(ability);
        this.ability = ability;
    }

    public void OnClick() {
        if (PlayerCharacter.localPlayer.GetComponent<SpiritUser>().overflowAbilities.Contains(ability)) DropsArea.OpenAbilities();
        DropsArea.ClearDrops();
    }
}