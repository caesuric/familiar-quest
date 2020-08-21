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
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Mug",
                description = "Does damage and steals an item.",
                damage = 1.5f,
                element = Element.slashing,
                baseStat = BaseStat.strength,
                hitEffect = 1,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "steal"
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.bashing, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, 50));
        }
    }
}
