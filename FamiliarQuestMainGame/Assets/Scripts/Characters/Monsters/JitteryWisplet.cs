using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class JitteryWisplet : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1f, Element.light, BaseStat.intelligence, rangedProjectile: 6, hitEffect: 6, isRanged: true));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.acid, 50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, 50));
        }
    }
}
