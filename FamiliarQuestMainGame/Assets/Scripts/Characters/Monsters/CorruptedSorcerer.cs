using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CorruptedSorcerer : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Spell attack.",
                damage = 0.3f,
                element = Element.fire,
                baseStat = BaseStat.intelligence,
                hitEffect = 3,
                rangedProjectile = 1,
                isRanged = true,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "projectileSpread"
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, 100));
        }
    }
}