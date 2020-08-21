using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Minotaur : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Basic attack.",
                damage = 1.5f,
                element = Element.bashing,
                baseStat = BaseStat.strength,
                hitEffect = 1
            },
            new AttackAbility {
                name = "Charge",
                description = "Charge.",
                damage = 2.62f,
                element = Element.bashing,
                baseStat = BaseStat.strength,
                cooldown = 3f,
                hitEffect = 1,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "chargeTowards"
                    }
                }
            });
        }
    }
}