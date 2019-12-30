using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Imp : MonoBehaviour {

    private bool initialized = false;
    private bool hasSummoned = false;
    public GameObject impPrefab;
	
	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = MonsterInitializer.Initialize(gameObject, new AttackAbility("Fireball", "Fireball", 1.0f, Element.fire, BaseStat.intelligence, isRanged: true, rangedProjectile: 1));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, 50));
        }
        if (!hasSummoned) {
            var mob = GetComponent<Monster>();
            //if (GetComponent<Monster>()!=null && mob.GetComponent<MonsterCombatant>().InCombat()) Summon();
        }
    }

    private void Summon()
    {
        hasSummoned = true;
        int roll = Random.Range(0, 2);
        if (roll == 1)
        {
            Instantiate(impPrefab, transform.position, transform.rotation);
            var character = GetComponent<Character>();
            character.GetComponent<ObjectSpawner>().CreateFloatingStatusText("SUMMONED!", "Imp summoned another imp!");
        }
    }
}
