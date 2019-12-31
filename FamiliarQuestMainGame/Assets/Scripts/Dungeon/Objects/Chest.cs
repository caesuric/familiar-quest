using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Chest : Hideable {

	// Use this for initialization
	void Start () {
        items.Add(this);
	}

    public void Use() {
        //if (!NetworkServer.active) return;
        var pc = GetComponent<UsableObject>().user;
        if (pc != null && GetComponent<LockedChest>()==null) {
            prune = true;
            GetComponent<RewardGiver>().DropLoot(pc.GetComponent<Character>(), guaranteed: true);
            pc.GetComponent<AudioGenerator>().PlaySoundByName("sfx_chest_open3");
            Destroy(gameObject);
        }
    }
}
