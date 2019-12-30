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
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Spitting attack", "Spitting attack.", 0f, Element.acid, BaseStat.strength, hitEffect: 5, dotDamage: 0.75f, dotTime: 8, isRanged: true, rangedProjectile: 2, attributes: new AbilityAttribute("speed-", new AbilityParameter("degree", DataType.floatType, floatVal: 0.75f), new AbilityParameter("duration", DataType.floatType, floatVal: 8f))));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.acid, 100));
        }
        if (character == null) character = GetComponent<Character>();
        if (character != null) character.GetComponent<Health>().hp = Mathf.Min(character.GetComponent<Health>().maxHP, character.GetComponent<Health>().hp + (Time.deltaTime * character.GetComponent<Health>().maxHP / 5));
    }
}
