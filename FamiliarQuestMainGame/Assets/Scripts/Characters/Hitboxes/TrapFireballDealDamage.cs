using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrapFireballDealDamage : MonoBehaviour {

    // Use this for initialization
    public GameObject projectile;
    public GameObject fireball;
    public int damage = -1;
    public bool armed = true;
    private float fxTimer = 1f;

    private void Update() {
        if (!armed) fxTimer -= Time.deltaTime;
        if (fxTimer <= 0) Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other) {
        if (!armed) return;
        //if (!NetworkServer.active) return;
        var otherCharacter = other.gameObject.GetComponent<Character>();
        if (otherCharacter != null && !otherCharacter.CompareTag("Enemy")) CreateExplosion(otherCharacter);
        else if (other.gameObject.CompareTag("Wall")) {
            var attr = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            CreateExplosion(attr);
            armed = false;
        }
    }

    private void CreateExplosion(Character character)
    {
        var obj = Instantiate(fireball, transform.position, transform.rotation);
        obj.transform.localScale *= 2;
        var aoedd = obj.GetComponent<TrapAOE>();
        aoedd.damage = damage;
        aoedd.radius = 2;
        character.GetComponent<SimulatedNoiseGenerator>().CmdMakeNoise(transform.position, 32);
        Destroy(projectile);
    }
}
