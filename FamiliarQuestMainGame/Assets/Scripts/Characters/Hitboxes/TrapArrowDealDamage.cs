using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrapArrowDealDamage : MonoBehaviour {

    // Use this for initialization
    public GameObject projectile;
    public int damage = -1;
    public bool armed = true;
    private float fxTimer = 1f;

    private void Update() {
        if (!armed) {
            fxTimer -= Time.deltaTime;
            if (fxTimer <= 0) Destroy(projectile);
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!armed) return;
        //if (!NetworkServer.active) return;
        Damage.TrapAttack(other, damage, projectile);
    }
}
