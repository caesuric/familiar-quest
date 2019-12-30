using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AOEDealDamage : MonoBehaviour {

    public int damage;
    public float radius;
    public string faction = "Player";
    public Character character;
    public List<AbilityAttribute> attributes;
    public AttackAbility ability;
    private float duration = 0.5f;
    private float fxDuration = 1.5f;
    private bool armed = true;
    //[SyncVar]
    public float size = 1;
    private bool initialized = false;

	// Update is called once per frame
	void Update () {
        if (!initialized && size != 1) {
            transform.localScale *= size;
            initialized = true;
        }
        //if (NetworkServer.active) {
            duration -= Time.deltaTime;
            fxDuration -= Time.deltaTime;
            if (duration < 0) armed = false;
            if (fxDuration <= 0) Destroy(gameObject);
        //}
	}

    private void OnTriggerEnter(Collider other) {
        //if (NetworkServer.active && armed) {
        if (armed) {
            var otherCharacter = other.gameObject.GetComponent<Character>();
            if (otherCharacter != null && !otherCharacter.CompareTag(faction)) otherCharacter.GetComponent<Health>().TakeDamage(damage, ability.element, character, ability: ability);
            else if (other.gameObject.GetComponent<LockedDoor>() != null) other.gameObject.GetComponent<LockedDoor>().TakeDamage(damage, other.gameObject);
            else if (other.gameObject.GetComponent<LockedChest>() != null) other.gameObject.GetComponent<LockedChest>().TakeDamage(damage, other.gameObject);
        }
    }

    public void Initialize(int damage, float radius, List<AbilityAttribute> attributes, string faction, Character character, AttackAbility ability)
    {
        this.damage = damage;
        this.radius = radius;
        this.attributes = attributes;
        this.faction = faction;
        this.character = character;
        this.ability = ability;
    }
}
