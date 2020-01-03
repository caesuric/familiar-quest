using UnityEngine;

//[NetworkSettings(sendInterval = 0.01f)]
public class ProjectileMove : MonoBehaviour {
    private readonly float speed = 20.0f;
    //[SyncVar]
    private bool move;

    // Update is called once per frame
    void Update() {
        //if (!NetworkServer.active) return;
        if (GetComponentInChildren<RangedHitboxDealDamage>() && GetComponentInChildren<RangedHitboxDealDamage>().struck) move = false;
        else move = true;
        if (move) transform.position += transform.forward * Time.deltaTime * speed;
    }
}
