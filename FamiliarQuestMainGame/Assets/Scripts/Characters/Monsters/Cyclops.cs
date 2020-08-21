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
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Pushing Attack",
                description = "Pushing attack.",
                damage = 1.125f,
                element = Element.bashing,
                baseStat = BaseStat.strength,
                hitEffect = 1,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "knockback",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "degree",
                                value = 5f
                            }
                        }
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, -50));
        }
    }
}