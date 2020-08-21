using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MirrorImage : MonoBehaviour {

    private bool initialized = false;
    private Health health;
    public GameObject creator = null;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility {
                name = "Attack",
                description = "Spell attack.",
                damage = 0.01f,
                element = Element.fire,
                baseStat = BaseStat.intelligence,
                hitEffect = 3,
                rangedProjectile = 1,
                isRanged = true
            });
            health = GetComponent<Health>();
        }
        if (creator==null) {
            GetComponent<MonsterMortal>().diedToPlayer = false;
            GetComponent<MonsterMortal>().OnDeath();
        }
    }
}
