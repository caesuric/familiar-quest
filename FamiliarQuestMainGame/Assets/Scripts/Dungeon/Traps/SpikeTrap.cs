using UnityEngine;

public class SpikeTrap : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
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
