using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RepeatedSpikeTrap : MonoBehaviour {

    // Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
	}

    private void OnTriggerStay(Collider other) {
        //if (!NetworkServer.active) return;
        var pc = other.GetComponent<PlayerCharacter>();
        if (pc == null) return;
        pc.GetComponent<Health>().TakeDamageFromTrap(GetComponent<TrapDamage>().damage, Element.piercing);
        pc.GetComponent<ObjectSpawner>().CreateFloatingTrapText("SPIKE TRAP", "A spike trap has been sprung.");
        pc.GetComponent<AudioGenerator>().PlaySoundByName("sfx_spike_trap3");
        Destroy(gameObject);
        Destroy(this);
    }
}
