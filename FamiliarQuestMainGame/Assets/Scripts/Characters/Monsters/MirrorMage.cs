using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MirrorMage : MonoBehaviour {

    private bool initialized = false;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Attack", "Spell attack.", 1f, Element.fire, BaseStat.intelligence, hitEffect: 3, rangedProjectile: 1, isRanged: true), new UtilityAbility("Mirror Image", "Creates mirror images.", cooldown: 300f, attributes: new AbilityAttribute("mirrorImage")));
        }
    }
}
