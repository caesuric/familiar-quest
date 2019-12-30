using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Minotaur : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.bashing, BaseStat.strength, hitEffect: 1), new AttackAbility("Charge", "Charge", 2.62f, Element.bashing, BaseStat.strength, cooldown: 3, hitEffect: 1, attributes: new AbilityAttribute("chargeTowards")));
        }
    }
}