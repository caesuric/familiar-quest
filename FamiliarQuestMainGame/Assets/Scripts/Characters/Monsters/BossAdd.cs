using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class BossAdd : MonoBehaviour {
    public Boss boss;

    private void OnDestroy() {
        if (boss.adds.Contains(gameObject)) boss.adds.Remove(gameObject);
        if (boss.addType == "boostsOnDeath") boss.LevelUp();
    }
}
