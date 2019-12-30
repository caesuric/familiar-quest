using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DamageZoneDealDamage : MonoBehaviour {
    public AttackAbility ability = null;
    public string faction = "";
    public Character character = null;
    private float timeLeft = -100;
    //[SyncVar]
    public float size = 1;
    private bool initialized = false;

	// Update is called once per frame
	void Update () {
        if (!initialized && size!=1) {
            transform.localScale *= size;
            initialized = true;
        }
        //if (NetworkServer.active) {
            if (timeLeft == -100 && ability != null) timeLeft = ability.dotTime;
            else if (ability != null) {
                timeLeft -= Time.deltaTime;
                if (timeLeft <= 0) Destroy(gameObject);
            }
        //}
	}

    private void OnTriggerStay(Collider other) {
        //if (NetworkServer.active) {
            if (character == null || faction == "" || ability == null || timeLeft == -100) return;
            var otherCharacter = other.gameObject.GetComponent<Character>();
            if (otherCharacter != null && !otherCharacter.CompareTag(faction)) otherCharacter.GetComponent<Health>().TakeDamageFromDot(ability.CalculateDotDamage(character) * Time.deltaTime, ability.element, character, silent: true);
            else if (other.gameObject.GetComponent<LockedDoor>() != null) other.gameObject.GetComponent<LockedDoor>().TakeDamage(ability.CalculateDotDamage(character) * Time.deltaTime, other.gameObject);
            else if (other.gameObject.GetComponent<LockedChest>() != null) other.gameObject.GetComponent<LockedChest>().TakeDamage(ability.CalculateDotDamage(character) * Time.deltaTime, other.gameObject);
        //}
    }

    public void Initialize(AttackAbility ability, Character character, string faction) {
        this.ability = ability;
        this.character = character;
        this.faction = faction;
    }
}
