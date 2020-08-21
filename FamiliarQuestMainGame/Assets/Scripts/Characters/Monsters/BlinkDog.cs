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
        if (!initialized) initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
            name = "Attack",
            description = "Basic attack.",
            damage = 1.5f,
            element = Element.piercing,
            baseStat = BaseStat.strength,
            hitEffect = 4
        });   
	}
}
