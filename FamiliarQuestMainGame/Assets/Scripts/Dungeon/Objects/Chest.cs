using UnityEngine;

public class Chest : MonoBehaviour {

    public void Use() {
        //if (!NetworkServer.active) return;
        var pc = GetComponent<UsableObject>().user;
        if (pc != null && GetComponent<LockedChest>() == null) {
            GetComponent<RewardGiver>().DropLoot(pc.GetComponent<Character>(), guaranteed: true);
            pc.GetComponent<AudioGenerator>().PlaySoundByName("sfx_chest_open3");
            Destroy(gameObject);
        }
    }
}