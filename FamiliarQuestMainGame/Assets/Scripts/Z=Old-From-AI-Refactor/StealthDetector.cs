//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class StealthDetector : MonoBehaviour {

//    private new CapsuleCollider collider;
//    public GameObject visual;
//    public float stealthCounter = 0;
//    public float stealthInvestigateTolerance = 0.25f;
//    public float stealthAlarmTolerance = 1;
//    public bool detectingStealth = false;
//    private GameObject monster = null;

//    private void Start() {
//        monster = transform.parent.gameObject;
//        if (monster.name == "Animated Statue(Clone)" || monster.name == "Mimic(Clone)" || monster.name == "Phantom Fungus(Clone)") visual.SetActive(false);
//        collider = GetComponent<CapsuleCollider>();
//    }

//    private void Update() {
//        if (PlayerCharacter.players.Count == 0) return;
//        var isStealthyPlayer = false;
//        foreach (var player in PlayerCharacter.players) if (player.GetComponent<InputController>().stealthy) isStealthyPlayer = true;
//        if (isStealthyPlayer && collider.enabled == false) collider.enabled = true;
//        else if (!isStealthyPlayer && collider.enabled == true) collider.enabled = false;
//        if (detectingStealth) stealthCounter += Time.deltaTime;
//        else stealthCounter -= Time.deltaTime;
//        if (stealthCounter < 0) stealthCounter = 0;
//        detectingStealth = false;
//    }


//    private void OnTriggerStay(Collider collider) {
//        //if (!NetworkServer.active) return;
//        var player = collider.gameObject.GetComponent<PlayerCharacter>();
//        if (player==null || !player.GetComponent<StatusEffectHost>().CheckForEffect("stealth")) return;
//        RaycastHit hit;
//        Vector3 rayDirection = player.transform.position - transform.parent.transform.position;
//        int layerMask = 1 << 8;
//        layerMask = ~layerMask;
//        var checkRaycast = (Physics.Raycast(transform.position, rayDirection, out hit, maxDistance: MonsterCombatant.sightRange, layerMask: layerMask) && hit.transform == player.transform);
//        if (checkRaycast && stealthCounter >= stealthAlarmTolerance) player.GetComponent<StatusEffectHost>().RemoveEffectByName("stealth");
//        else if (checkRaycast && stealthCounter >= stealthInvestigateTolerance) monster.GetComponent<MonsterSenses>().HearNoise(player.transform.position);
//        else if (checkRaycast) detectingStealth = true;
//    }
//}

