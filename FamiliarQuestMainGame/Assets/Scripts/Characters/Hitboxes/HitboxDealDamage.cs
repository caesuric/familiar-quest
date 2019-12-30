using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitboxDealDamage : MonoBehaviour
{

    public Character character;
    public string faction;

    private void OnTriggerStay(Collider other)
    {
        if (character == null) return;
        //if (!NetworkServer.active || !character.GetComponent<Attacker>().isAttacking) return;
        if (!character.GetComponent<Attacker>().isAttacking) return;
        Damage.MeleeAttack(character, other, faction);
    }
}
