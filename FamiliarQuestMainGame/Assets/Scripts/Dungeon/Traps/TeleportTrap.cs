using UnityEngine;

public class TeleportTrap : DestinationTrap {

    private void OnTriggerEnter(Collider other) {
        //if (!NetworkServer.active) return;
        var pc = other.GetComponent<PlayerCharacter>();
        if (pc == null) return;
        pc.transform.position = destination;
        pc.GetComponent<ObjectSpawner>().CreateFloatingTrapText("TELEPORT TRAP", "A teleport trap has been sprung.");
        Destroy(gameObject);
        Destroy(this);
    }
}
