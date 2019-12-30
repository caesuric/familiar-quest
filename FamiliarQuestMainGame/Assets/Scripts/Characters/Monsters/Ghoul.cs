using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ghoul : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update()
    {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.piercing, BaseStat.strength), new AttackAbility("Ghoul Paralysis", "Ghoul paralysis", 3f, Element.dark, BaseStat.strength, cooldown: 10f, hitEffect: 1, attributes: new AbilityAttribute("paralyze", new AbilityParameter("duration", DataType.floatType, floatVal: 1))));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, 50));
        }
    }
}
