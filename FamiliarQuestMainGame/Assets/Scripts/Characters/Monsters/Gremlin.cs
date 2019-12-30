using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Gremlin : MonoBehaviour {

    private bool initialized = false;
    public Item item = null;
    
    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Mug", "Steals an item.", 1.5f, Element.bashing, BaseStat.strength, hitEffect: 1, attributes: new AbilityAttribute("steal")));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.bashing, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, 50));
        }
    }
}
