using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoundArchon : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Basic Attack", "Basic attack.", 1.5f, Element.slashing, BaseStat.strength, hitEffect: 0), new UtilityAbility("Cure All", "Heals all allies.", BaseStat.wisdom, cooldown: 5, attributes: new AbilityAttribute("healAll", new AbilityParameter("degree", DataType.floatType, floatVal: 3.5f))));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, 100));
        }
    }
}

