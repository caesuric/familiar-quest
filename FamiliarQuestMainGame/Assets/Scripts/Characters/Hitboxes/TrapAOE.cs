using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrapAOE : MonoBehaviour {

    public int damage;
    public float radius;
    public string faction = "Enemy";
    private float duration = 0.5f;
	
	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        duration -= Time.deltaTime;
        if (duration <= 0) Destroy(this);
	}

    private void OnTriggerEnter(Collider other) {
        //if (NetworkServer.active) {
            var otherCharacter = other.gameObject.GetComponent<Character>();
            if (otherCharacter != null && !otherCharacter.CompareTag(faction)) otherCharacter.GetComponent<Health>().TakeDamageFromTrap(damage, Element.fire);
            else if (other.gameObject.GetComponent<LockedDoor>() != null) other.gameObject.GetComponent<LockedDoor>().TakeDamage(damage, other.gameObject);
            else if (other.gameObject.GetComponent<LockedChest>() != null) other.gameObject.GetComponent<LockedChest>().TakeDamage(damage, other.gameObject);
        //}
    }
}

