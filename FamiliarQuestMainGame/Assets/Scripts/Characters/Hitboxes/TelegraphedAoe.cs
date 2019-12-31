using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TelegraphedAoe : MonoBehaviour {

    public float timeLeft = 2f;
    private AttackAbility ability = null;
    private float damage = 0;
    private List<GameObject> touching = new List<GameObject>();
    public GameObject creator = null;
	
	// Update is called once per frame
	void Update () {
        timeLeft -= Time.deltaTime;
        if (timeLeft<=0 && ability!=null) {
            foreach (var hit in touching) {
                if (hit != null && hit.GetComponent<PlayerCharacter>() != null && creator != null) hit.GetComponent<Health>().TakeDamage(damage, ability.element, creator.GetComponent<Character>());
            }
            //NetworkServer.Destroy(gameObject);
            Destroy(gameObject);
        }
	}

    public void Initialize(AttackAbility ability, float damage, GameObject creator) {
        this.ability = ability;
        this.damage = damage;
        this.creator = creator;
    }

    private void OnTriggerEnter(Collider other) {
        if (!touching.Contains(other.gameObject)) touching.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other) {
        if (touching.Contains(other.gameObject)) touching.Remove(other.gameObject);
    }
}
