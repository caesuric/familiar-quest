using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DarkKnight : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Pulling Attack",
                description = "Pulling attack.",
                damage = 1.4f,
                element = Element.dark,
                baseStat = BaseStat.strength,
                cooldown = 3,
                hitEffect = 7,
                rangedProjectile = 7,
                isRanged = true,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "pullTowards",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "degree",
                                value = 5f
                            }
                        }
                    }
                }
            },
            new AttackAbility {
                name = "Basic Attack",
                description = "Basic attack.",
                damage = 1.5f,
                element = Element.slashing,
                baseStat = BaseStat.strength,
                hitEffect = 0
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, 50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.bashing, 50));
        }
    }
}
    
