using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArrowTrap : MonoBehaviour {

    public GameObject arrow;
    
	// Use this for initialization
	void Start () {
        //if (!NetworkServer.active) return;
        var obj = Instantiate(arrow, transform.position, transform.rotation);
        //NetworkServer.Spawn(obj);
        var tadd = obj.GetComponentInChildren<TrapArrowDealDamage>();
        tadd.damage = GetComponent<TrapDamage>().damage;
        tadd.projectile = obj;
        Destroy(gameObject, 2);
        Destroy(this, 2);
	}
}
