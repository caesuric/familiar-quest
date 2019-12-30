using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterBaseAbilities : MonoBehaviour {

    public List<ActiveAbility> baseAbilities = new List<ActiveAbility>();

    void Update() {
        foreach (ActiveAbility ability in baseAbilities) {
            ability.currentCooldown -= Time.deltaTime;
            if (ability.currentCooldown < 0) ability.currentCooldown = 0;
        }
    }
}
