using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoundArchon : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Basic Attack",
                description = "Basic attack.",
                damage = 1.5f,
                element = Element.slashing,
                hitEffect = 0
            },
            new UtilityAbility {
                name = "Cure All",
                description = "Heals all allies",
                baseStat = BaseStat.wisdom,
                cooldown = 5,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "healAll",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "degree",
                                value = 3.5f
                            }
                        }
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, 100));
        }
    }
}

