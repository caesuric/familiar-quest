using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnergyWisplet : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject);
            var monster = GetComponent<Monster>();
            monster.elementalAffinities.Add(new ElementalAffinity(Element.piercing, 75));
            monster.elementalAffinities.Add(new ElementalAffinity(Element.bashing, 75));
            monster.elementalAffinities.Add(new ElementalAffinity(Element.slashing, 75));
            monster.elementalAffinities.Add(new ElementalAffinity(Element.fire, 75));
            monster.elementalAffinities.Add(new ElementalAffinity(Element.ice, 75));
            monster.elementalAffinities.Add(new ElementalAffinity(Element.light, 75));
            monster.elementalAffinities.Add(new ElementalAffinity(Element.dark, 75));
            monster.elementalAffinities.Add(new ElementalAffinity(Element.acid, 75));
            var i = Random.Range(0, monster.elementalAffinities.Count);
            monster.elementalAffinities[i].amount = -50;
        }
    }
}
