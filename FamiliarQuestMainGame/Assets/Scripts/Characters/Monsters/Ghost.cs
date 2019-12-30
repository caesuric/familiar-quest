using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ghost : MonoBehaviour {

    private Monster mob = null;

    private void Start() {
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.light, -50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.dark, 50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.ice, 50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, 50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.slashing, 50));
        GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.bashing, 50));
    }

    // Update is called once per frame
    void Update () {
        //if (NetworkServer.active) {
            if (mob == null) mob = GetComponent<Monster>();
            //if (mob != null && mob.GetComponent<MonsterCombatant>().player != null && Vector3.Distance(transform.position, mob.GetComponent<MonsterCombatant>().player.transform.position) < 15) transform.position = Vector3.Lerp(transform.position, mob.GetComponent<MonsterCombatant>().player.transform.position, Time.deltaTime / 2f);
        //}
	}
}
