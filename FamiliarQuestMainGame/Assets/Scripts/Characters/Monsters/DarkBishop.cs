using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DarkBishop : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Spell attack.",
                damage = 1f,
                element = Element.dark,
                baseStat = BaseStat.intelligence,
                hitEffect = 7,
                rangedProjectile = 7,
                isRanged = true
            },
            new UtilityAbility {
                name = "Cure",
                description = "Cure wounds.",
                baseStat = BaseStat.wisdom,
                mpUsage = 20,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "heal",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "degree",
                                value = 1.125f
                            }
                        }
                    }
                }
            });
        }
    }
}