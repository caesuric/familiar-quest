using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ogre : MonoBehaviour {

    private bool initialized = false;
	
	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (!initialized) initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
            name = "Attack",
            description = "Basic attack.",
            damage = 1f,
            element = Element.bashing,
            baseStat = BaseStat.strength,
            hitEffect = 1,
            radius = 2,
            aoe = 2
        });
	}
}
