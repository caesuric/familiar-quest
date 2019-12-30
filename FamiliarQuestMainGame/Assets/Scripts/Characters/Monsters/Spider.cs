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
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.piercing, BaseStat.strength, hitEffect: 2), new AttackAbility("Spider Poisoning", "Spider Poisoning", 0f, Element.acid, BaseStat.strength, dotDamage: 3.9375f, dotTime: 6f, cooldown: 6f, hitEffect: 2));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, -50));
        }
    }
}
