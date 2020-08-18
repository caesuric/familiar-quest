using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Cyclops : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Pushing Attack", "Pushing attack.", 1.125f, Element.bashing, BaseStat.strength, hitEffect: 1, attributes: new AbilityAttribute("knockback", new AbilityAttributeParameter("degree", DataType.floatType, floatVal: 5.0f))));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, -50));
        }
    }
}