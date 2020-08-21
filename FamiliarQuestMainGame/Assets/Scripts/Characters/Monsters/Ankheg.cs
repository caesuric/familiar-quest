using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ankheg : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
            name = "Attack",
            description = "Basic attack.",
            damage = 1.5f,
            element = Element.piercing,
            baseStat = BaseStat.strength,
            hitEffect = 2
        },
        new AttackAbility {
            name = "Acid Breath",
            description = "Ankheg breath.",
            damage = 1.75f,
            element = Element.acid,
            baseStat = BaseStat.strength,
            isRanged = true,
            rangedProjectile = 2,
            cooldown = 15f
        });
	}
}
