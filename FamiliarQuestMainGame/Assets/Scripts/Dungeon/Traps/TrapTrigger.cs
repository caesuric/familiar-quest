using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TrapTrigger : MonoBehaviour {

    public string trapType;
    public GameObject trap;
    public Vector3 trapPosition;
    public int damage = 0;
    public bool repeatingTrigger = false;
    private float cooldown = 0;

	// Update is called once per frame
	void Update () {
        //if (!NetworkServer.active) return;
        if (cooldown>0) cooldown -= Time.deltaTime;
	}

    private void OnTriggerStay(Collider other) {
        //if (!NetworkServer.active) return;
        var pc = other.GetComponent<PlayerCharacter>();
        if (pc==null || cooldown>0) return;
        var methods = GetMethods(pc);
        methods[trapType]();
        pc.GetComponent<AudioGenerator>().PlaySoundByName("sfx_trap_trigger2");
        if (!repeatingTrigger) {
            Destroy(gameObject);
            Destroy(this);
        }
        else cooldown = 2f;
    }

    private Dictionary<string, Action> GetMethods(PlayerCharacter pc)
    {
        return new Dictionary<string, Action>() {
            {"spike", SpawnBasicTrap},
            {"lava", SpawnBasicTrap},
            {"arrow", () => SpawnFacingTrap(pc)},
            {"fireball", () => SpawnFacingTrap(pc)},
            {"arrowSpread", () => SpawnFacingTrap(pc)},
            {"pit", SpawnDestinationTrap<PitTrap>},
            {"teleport", SpawnDestinationTrap<TeleportTrap>}
        };
    }

    private void SpawnBasicTrap()
    {
        GameObject obj = Instantiate(trap, transform.position, transform.rotation);
        if (obj.GetComponent<TrapDamage>() != null) obj.GetComponent<TrapDamage>().damage = damage;
        //NetworkServer.Spawn(obj);
    }

    private void SpawnFacingTrap(PlayerCharacter pc)
    {
        Quaternion trapRotation;
        trapRotation = Quaternion.LookRotation(pc.transform.position - trapPosition);
        var obj = Instantiate(trap, trapPosition, trapRotation);
        obj.GetComponent<TrapDamage>().damage = damage;
    }

    private void SpawnDestinationTrap<T>() where T: DestinationTrap
    {
        GameObject obj = Instantiate(trap, transform.position, transform.rotation);
        if (obj.GetComponent<TrapDamage>()!=null) obj.GetComponent<TrapDamage>().damage = damage;
        var trapComponent = obj.GetComponent<T>();
        trapComponent.destination = trapPosition;
    }
}
