using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spider : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update()
    {
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
                name = "Spider Poisoning",
                description = "Spider poisoning.",
                damage = 0f,
                element = Element.acid,
                baseStat = BaseStat.strength,
                dotDamage = 3.9375f,
                dotTime = 6f,
                cooldown = 6f,
                hitEffect = 2
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, -50));
        }
    }
}
