//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Networking;

//public class OpensDoors : MonoBehaviour {

//    private float raycastTimer = 0f;
//	// Use this for initialization
//	void Start () {
		
//	}
	
//	// Update is called once per frame
//	void Update () {
//        //if (!NetworkServer.active) return;
//        raycastTimer += Time.deltaTime;
//        if (raycastTimer >= 1f) {
//            raycastTimer = 0f;
//            var obj = GetNearestUsableObject();
//            if (obj == null) return;
//            var door = obj.GetComponent<Door>();
//            if (door != null && !door.open) door.OpenDoor();
//        }
//	}

//    public UsableObject GetNearestUsableObject() {
//        var hits = Physics.OverlapSphere(transform.position, 2f);
//        UsableObject nearestObj = null;
//        float closestDistance = Mathf.Infinity;
//        foreach (var hit in hits) {
//            var usable = hit.gameObject.GetComponent<UsableObject>();
//            if (usable == null) continue;
//            var distance = Vector3.Distance(usable.transform.position, transform.position);
//            if (distance < closestDistance) {
//                closestDistance = distance;
//                nearestObj = usable;
//            }
//        }
//        return nearestObj;
//    }
//}
