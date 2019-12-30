using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class BlinkDog : MonoBehaviour {

    private bool initialized = false;
	
	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (!initialized) initialized = MonsterInitializer.Initialize(gameObject, abilities: new ActiveAbility[] {new AttackAbility("Attack", "Basic attack.", 1.5f, Element.piercing, BaseStat.strength, hitEffect: 4)});
	}
}
