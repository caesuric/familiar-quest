using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HomingProjectile : MonoBehaviour {

    public GameObject player = null;
    public NavMeshAgent agent = null;
    public float duration = 18f;
	
	// Update is called once per frame
	void Update () {
        if (player != null && Vector3.Distance(player.transform.position, agent.destination) > 2 && !agent.pathPending) agent.destination = player.transform.position;
        duration -= Time.deltaTime;
        if (duration <= 0) Destroy(gameObject);
	}

    public void Initialize(Character character, AttackAbility ability, float damage, GameObject player) {
        this.player = player;
        agent = GetComponent<NavMeshAgent>();
        agent.destination = player.transform.position;
        agent.speed = 20;
        agent.angularSpeed = 120;
        var rhdd = GetComponentInChildren<RangedHitboxDealDamage>();
        rhdd.Initialize(character, (int)damage, ability.attributes, 0, 0, 0, "Enemy", ability);
    }
}
