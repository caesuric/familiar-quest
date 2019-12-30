//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEngine;
//using UnityEngine.Networking;

//class MonsterIdler : MonoBehaviour {
//    public MonsterIdleBehavior idleBehavior;
//    void Start() {
//        //if (NetworkServer.active) {
//        if (GetComponent<Boss>() != null) {
//            idleBehavior = new BossIdleBehavior();
//            return;
//        }
//        var idleBehaviorRoll = Random.Range(0, 100);
//        if (idleBehaviorRoll < 75) idleBehavior = new DefaultIdleBehavior();
//        else if (idleBehaviorRoll < 95) idleBehavior = new PatrollerBehavior();
//        else if (idleBehaviorRoll < 98) idleBehavior = new WandererBehavior();
//        else idleBehavior = new ExplorerBehavior();
//    }
//    //}
//}
