using UnityEngine;

public class LavaTrap : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        //if (!NetworkServer.active) return;
        var pc = other.GetComponent<PlayerCharacter>();
        if (pc == null) return;
        pc.GetComponent<ObjectSpawner>().CreateFloatingTrapText("LAVA TRAP", "A lava trap has been sprung.");
    }

    private void OnTriggerStay(Collider other) {
        //if (!NetworkServer.active) return;
        var pc = other.GetComponent<PlayerCharacter>();
        if (pc == null) return;
        pc.GetComponent<Health>().TakeDamageFromTrap(120 * Time.deltaTime, Element.fire, silent: true);
    }
}