using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityDrop : MonoBehaviour {

    public void Initialize(Ability ability) {
        GetComponent<AbilityScreenIcon>().Initialize(ability);
    }

    public void OnClick() {
        DropsArea.OpenAbilities();
        DropsArea.ClearDrops();
    }
}
