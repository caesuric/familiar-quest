﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MirrorMage : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Spell attack.",
                damage = 1f,
                element = Element.fire,
                baseStat = BaseStat.intelligence,
                hitEffect = 3,
                rangedProjectile = 1,
                isRanged = true
            },
            new UtilityAbility {
                name = "Mirror Image",
                description = "Creates mirror images.",
                cooldown = 300f,
                attributes = new List<AbilityAttribute> {
                    new AbilityAttribute {
                        type = "mirrorImage"
                    }
                }
            });
        }
    }
}
