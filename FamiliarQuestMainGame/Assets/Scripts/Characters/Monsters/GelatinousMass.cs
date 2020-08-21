using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GelatinousMass : MonoBehaviour {

    private bool initialized = false;
    private Character character = null;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Spitting Attack",
                description = "Spitting attack.",
                damage = 0f,
                element = Element.acid,
                baseStat = BaseStat.strength,
                hitEffect = 5,
                dotDamage = 0.75f,
                dotTime = 8f,
                rangedProjectile = 2,
                isRanged = true,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "speed-",
                        parameters = new List<AbilityAttributeParameter> {
                            new AbilityAttributeParameter {
                                name = "degree",
                                value = 0.75f
                            },
                            new AbilityAttributeParameter {
                                name = "duration",
                                value = 8f
                            }
                        }
                    }
                }
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.acid, 100));
        }
        if (character == null) character = GetComponent<Character>();
        if (character != null) character.GetComponent<Health>().hp = Mathf.Min(character.GetComponent<Health>().maxHP, character.GetComponent<Health>().hp + (Time.deltaTime * character.GetComponent<Health>().maxHP / 5));
    }
}
