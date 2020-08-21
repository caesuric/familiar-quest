using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bomber : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Explode",
                description = "Explode.",
                damage = 15f,
                element = Element.fire,
                baseStat = BaseStat.strength,
                hitEffect = 3,
                aoe = 3,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "selfDestruct"
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, -50));
        }
    }
}