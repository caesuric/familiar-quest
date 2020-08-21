using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ghoul : MonoBehaviour {

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
                element = Element.piercing,
                baseStat = BaseStat.strength,
                hitEffect = 0
            },
            new AttackAbility {
                name = "Ghoul Paralysis",
                description = "Ghoul paralysis.",
                damage = 3f,
                element = Element.dark,
                baseStat = BaseStat.strength,
                cooldown = 10f,
                hitEffect = 1,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "paralyze",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "duration",
                                value = 1f
                            }
                        }
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, 50));
        }
    }
}
