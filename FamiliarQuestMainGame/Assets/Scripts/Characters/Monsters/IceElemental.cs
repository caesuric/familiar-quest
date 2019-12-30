using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class IceElemental : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.ice, BaseStat.strength, hitEffect: 4), new AttackAbility("Ice", "Ice", 1.6f, Element.ice, BaseStat.strength, rangedProjectile: 5, cooldown: 30, radius: 4, aoe: 4, hitEffect: 4, isRanged: true));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, 100));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, -50));
        }
    }
}
