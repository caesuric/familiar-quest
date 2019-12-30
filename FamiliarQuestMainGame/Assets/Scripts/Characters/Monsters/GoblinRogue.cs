using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoblinRogue : MonoBehaviour {

    private bool initialized = false;
    public bool hidden = true;
    
    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.slashing, BaseStat.strength, hitEffect: 0));
        }
        //if (GetComponent<MonsterCombatant>().InCombat()) hidden = false;
        else hidden = true;
    }
}
