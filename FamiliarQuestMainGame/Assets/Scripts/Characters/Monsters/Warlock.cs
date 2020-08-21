using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Warlock : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Spell attack.",
                damage = 0.25f,
                element = Element.dark,
                baseStat = BaseStat.intelligence,
                hitEffect = 7,
                rangedProjectile = 7,
                isRanged = true,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "inflictVulnerability",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "degree",
                                value = 100f
                            },
                            new AbilityAttributeParameter {
                                name = "duration",
                                value = 10f
                            }
                        }
                    }
                }
            });
        }
    }
}

