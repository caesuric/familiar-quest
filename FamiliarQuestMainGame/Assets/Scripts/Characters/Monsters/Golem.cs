﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Golem : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Basic attack.",
                damage = 1.5f,
                element = Element.slashing,
                baseStat = BaseStat.strength,
                hitEffect = 1
            });
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, 50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, 50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.bashing, 50));
        }

    }
}
