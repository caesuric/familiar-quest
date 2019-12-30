using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LandSquid : MonoBehaviour {

    private bool initialized = false;
    // Use this for initialization
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Basic attack.", 1.5f, Element.bashing, BaseStat.strength, hitEffect: 1), new AttackAbility("Squid Ink", "Squid Ink", 1.75f, Element.bashing, BaseStat.strength, rangedProjectile: 7, cooldown: 30f, hitEffect: 1, isRanged: true, attributes: new AbilityAttribute("blind", new AbilityParameter("duration", DataType.floatType, floatVal: 6))));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, 50));
        }
    }
}
