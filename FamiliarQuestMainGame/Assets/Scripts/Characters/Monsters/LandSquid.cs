using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LandSquid : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update()
    {
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
                name = "Squid Ink",
                description = "Squid ink.",
                damage = 1.75f,
                element = Element.bashing,
                baseStat = BaseStat.strength,
                rangedProjectile = 7,
                cooldown = 30f,
                hitEffect = 1,
                isRanged = true,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "blind",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "duration",
                                value = 6f
                            }
                        }
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, 50));
        }
    }
}
