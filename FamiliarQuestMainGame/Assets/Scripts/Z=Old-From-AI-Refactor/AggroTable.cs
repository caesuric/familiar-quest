//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AggroTable : MonoBehaviour {

//    public static List<AggroTable> tables = new List<AggroTable>();
//    public List<AggroEntry> aggroEntries = new List<AggroEntry>();
//    private float raycastTimer = 0;
//	// Use this for initialization
//	void Start () {
//        tables.Add(this);
//	}

//    private void OnDestroy() {
//        tables.Remove(this);
//    }

//    // Update is called once per frame
//    void Update () {
//        PruneTable();
//        raycastTimer += Time.deltaTime;
//        if (raycastTimer >= 1f) {
//            raycastTimer = 0f;
//            AddAndUpdateEntries();
//        }
//    }

//    private void PruneTable()
//    {
//        List<AggroEntry> prunelist = new List<AggroEntry>();
//        foreach (var entry in aggroEntries)
//        {
//            entry.time += Time.deltaTime;
//            if (entry.time > 30) prunelist.Add(entry);
//            if (entry.obj == null || entry.obj.GetComponent<Health>().hp <= 0) prunelist.Add(entry);
//        }
//        foreach (var entry in prunelist) aggroEntries.Remove(entry);
//    }

//    private void AddAndUpdateEntries()
//    {
//        foreach (var player in PlayerCharacter.players) {
//            CheckEntryBasedOnLos(player.gameObject);
//            //RaycastHit hit;
//            //Vector3 rayDirection = player.transform.position - transform.position;
//            //int layerMask = 1 << 8;
//            //layerMask = ~layerMask;
//            //if (Physics.Raycast(transform.position, rayDirection, out hit, maxDistance: Mathf.Infinity, layerMask: layerMask)) {
//            //    if (hit.transform == player.transform) {
//            //        var obj = hit.transform.gameObject;
//            //        if (FindEntry(obj) == null && obj.GetComponent<Health>().hp > 0) aggroEntries.Add(new AggroEntry(obj, 1));
//            //        else if (obj.GetComponent<Health>().hp > 0) FindEntry(obj).Touch();
//            //    }
//            //}
//        }
//    }

//    private void CheckEntryBasedOnLos(GameObject player) {
//        if (CanSeeSpecificPlayer(player, MonsterCombatant.sightRange)) { // if (CanSeeSpecificPlayer(player, Mathf.Infinity)) {
//            if (FindEntry(player) == null && player.GetComponent<Health>().hp > 0) aggroEntries.Add(new AggroEntry(player, 1));
//            else if (player.GetComponent<Health>().hp > 0) FindEntry(player).Touch();
//        }
//    }

//    private bool CanSeeSpecificPlayer(GameObject player, float range) {
//        if (RaycastCheck(player, transform.position, range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(-0.1f, 0, -0.1f), range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(-0.1f, 0, 0.1f), range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(0.1f, 0, -0.1f), range)) return true;
//        if (RaycastCheck(player, transform.position + new Vector3(0.1f, 0, 0.1f), range)) return true;
//        return false;
//    }

//    private bool RaycastCheck(GameObject player, Vector3 position, float range) {
//        var rayDirection = player.transform.position - position;
//        var hits = Physics.RaycastAll(position, rayDirection, range);
//        foreach (var hit in hits) {
//            if (hit.transform.gameObject.CompareTag("Wall") && Vector3.Distance(hit.transform.position, position) < Vector3.Distance(player.transform.position, position)) return false;
//        }
//        return true;
//    }

//    public AggroEntry FindEntry(GameObject obj) {
//        foreach (var item in aggroEntries) {
//            if (item.obj == obj) return item;
//        }
//        return null;
//    }

//    public AggroEntry FindTarget() {
//        float highest = 0;
//        AggroEntry highestObj = null;
//        foreach (var item in aggroEntries) {
//            if (item.value>highest) {
//                highest = item.value;
//                highestObj = item;
//            }
//        }
//        return highestObj;
//    }

//    public void IncreaseAggro(GameObject obj, float amount) {
//        var entry = FindEntry(obj);
//        if (entry==null) aggroEntries.Add(new AggroEntry(obj, amount));
//        else entry.value += amount;
//    }

//    public static void IncreaseAggroWithAll(GameObject obj, float amount) {
//        foreach (var table in tables) {
//            if (table.aggroEntries.Count == 0) continue;
//            var entry = table.FindEntry(obj);
//            if (entry == null) table.aggroEntries.Add(new AggroEntry(obj, amount));
//            else entry.value += amount;
//        }
//    }
//}

//public class AggroEntry {
//    public GameObject obj;
//    public float value;
//    public float time;

//    public AggroEntry(GameObject obj, float value) {
//        this.obj = obj;
//        this.value = value;
//        time = 0;
//    }
//    public void Touch() {
//        time = 0;
//    }
//}