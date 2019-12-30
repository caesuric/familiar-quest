using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArrowSpreadTrap : MonoBehaviour {

    public GameObject arrow;
    private float angle = 45;
    private int projectiles = 5;
	// Use this for initialization
	void Start () {
        //if (!NetworkServer.active) return;
        float startingAngle = 0 - angle;
        float increment = (angle * 2) / (projectiles - 1);
        for (int i = 0; i < projectiles; i++) {
            var currentAngle = startingAngle + (increment * i);
            FireArrow(currentAngle);
        }
        Destroy(gameObject, 2);
        Destroy(this, 2);
	}

    private void FireArrow(float currentAngle)
    {
        var obj = Instantiate(arrow, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.rotation);
        obj.transform.Rotate(0, currentAngle, 0);
        //NetworkServer.Spawn(obj);
        var tadd = obj.GetComponentInChildren<TrapArrowDealDamage>();
        tadd.damage = GetComponent<TrapDamage>().damage;
    }
}
