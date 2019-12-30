using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireElemental : MonoBehaviour {

    private bool initialized = false;
	
	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.fire, BaseStat.strength, hitEffect: 3), new AttackAbility("Fire", "Fire", 1.6f, Element.fire, BaseStat.strength, cooldown: 30, radius: 4, aoe: 3, hitEffect: 3, isRanged: true));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, 100));
        }
	}
}
