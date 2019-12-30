using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Warlock : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Spell attack.", 0.25f, Element.dark, BaseStat.intelligence, hitEffect: 7, rangedProjectile: 7, isRanged: true, attributes: new AbilityAttribute("inflictVulnerability", new AbilityParameter("degree", DataType.floatType, floatVal: 100), new AbilityParameter("duration", DataType.floatType, floatVal: 10f))));
        }
    }
}

