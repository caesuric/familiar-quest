using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class YoungDragon : MonoBehaviour {

    private bool initialized = false;
	
	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Basic attack.",
                damage = 1.5f,
                element = Element.piercing,
                baseStat = BaseStat.strength,
                hitEffect = 2
            },
            new AttackAbility {
                name = "Fire Breath",
                description = "Dragon breath.",
                damage = 0.58f,
                element = Element.fire,
                baseStat = BaseStat.strength,
                isRanged = true,
                rangedProjectile = 1,
                cooldown = 3,
                radius = 4,
                aoe = 3
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, 50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, -50));
        }
	}
}
