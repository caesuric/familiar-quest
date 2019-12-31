using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyHealthBarUpdater : MonoBehaviour {

    public Character attr;
	
	// Update is called once per frame
	void Update () {
        if (attr != null && attr.GetComponent<Health>() != null) {
            var hp = Mathf.Max(attr.GetComponent<Health>().hp, 0);
            var maxHp = Mathf.Max(attr.GetComponent<Health>().maxHP, 1);
            transform.localScale = new Vector3(0.2f * hp / maxHp, 1, 0.06f);
            transform.localPosition = new Vector3((-0.5f + hp / maxHp / 2.0f) * 2f, 0, -0.08f);
        }
	}
}
