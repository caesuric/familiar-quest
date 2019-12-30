using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DarkKnight : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Pulling Attack", "Pulling attack.", 1.4f, Element.dark, BaseStat.strength, cooldown: 3, hitEffect: 7, isRanged: true, rangedProjectile: 7, attributes: new AbilityAttribute("pullTowards", new AbilityParameter("degree", DataType.floatType, floatVal: 5.0f))), new AttackAbility("Basic Attack", "Basic attack.", 1.5f, Element.slashing, BaseStat.strength, hitEffect: 0));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, 50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.bashing, 50));
        }
    }
}
    
