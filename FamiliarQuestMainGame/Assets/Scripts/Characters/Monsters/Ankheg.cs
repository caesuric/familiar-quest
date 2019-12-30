using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ankheg : MonoBehaviour {

    private bool initialized = false;

	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (!initialized) initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.piercing, BaseStat.strength, hitEffect: 2), new AttackAbility("Acid Breath", "Ankheg breath", 1.75f, Element.acid, BaseStat.strength, isRanged: true, rangedProjectile: 2, cooldown: 15));
	}
}
