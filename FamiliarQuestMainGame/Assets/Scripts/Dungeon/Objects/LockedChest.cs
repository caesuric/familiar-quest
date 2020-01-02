using UnityEngine;

public class LockedChest : Hideable {

    public GameObject floatingText;
    public float hp = 30;
    // Use this for initialization
    void Start() {
        items.Add(this);
    }

    public void Unlock(GameObject unlocker) {
        if (hp > 0) unlocker.GetComponent<AudioGenerator>().PlaySoundByName("sfx_unlock2");
        //if (!NetworkServer.active) return;
        GetComponent<UsableObject>().hide = false;
        items.Remove(this);
        Destroy(this);
    }

    public void TakeDamage(float amount, GameObject attacker) {
        //if (!NetworkServer.active) return;
        hp -= amount;
        if (hp <= 0) {
            attacker.GetComponent<AudioGenerator>().PlaySoundByName("sfx_wood_smash1");
            Unlock(attacker);
        }
    }
}