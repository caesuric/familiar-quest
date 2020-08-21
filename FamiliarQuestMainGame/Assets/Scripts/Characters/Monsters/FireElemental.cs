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
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Fire",
                description = "Fire.",
                damage = 1.6f,
                element = Element.fire,
                baseStat = BaseStat.strength,
                cooldown = 30,
                radius = 4,
                aoe = 3,
                hitEffect = 3,
                rangedProjectile = 0,
                isRanged = true
            },
            new AttackAbility {
                name = "Basic Attack",
                description = "Basic attack.",
                damage = 1.5f,
                element = Element.fire,
                baseStat = BaseStat.strength,
                hitEffect = 3
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, 100));
        }
	}
}
