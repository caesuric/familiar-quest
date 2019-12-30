using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DarkBishop : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Spell attack.", 1f, Element.dark, BaseStat.intelligence, hitEffect: 7, rangedProjectile: 7, isRanged: true), new UtilityAbility("Cure", "Cure wounds.", BaseStat.wisdom, mpUsage: 20, attributes: new AbilityAttribute("heal", new AbilityParameter("degree", DataType.floatType, floatVal: 1.125f))));
        }
    }
}