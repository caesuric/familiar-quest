using UnityEngine;

public class FireballTrap : MonoBehaviour {
    public GameObject projectile;

    // Use this for initialization
    void Start() {
        //if (!NetworkServer.active) return;
        var obj = Instantiate(projectile, transform.position, transform.rotation);
        //NetworkServer.Spawn(obj);
        var tadd = obj.GetComponentInChildren<TrapFireballDealDamage>();
        tadd.damage = GetComponent<TrapDamage>().damage;
        Destroy(gameObject, 2);
        Destroy(this, 2);
    }
}
