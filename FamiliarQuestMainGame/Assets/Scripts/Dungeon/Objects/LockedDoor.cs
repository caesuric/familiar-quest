using UnityEngine;

public class LockedDoor : MonoBehaviour {
    public GameObject floatingTextPrefab;
    public float hp = 50;
    public bool reverseHinge = true;

    public void Unlock(GameObject unlocker) {
        unlocker.GetComponent<AudioGenerator>().PlaySoundByName("sfx_unlock2");
        //if (!NetworkServer.active) return;
        var door = gameObject.AddComponent<Door>();
        //door.reverseHinge = reverseHinge;
        Destroy(this);
    }

    public void TakeDamage(float amount, GameObject attacker) {
        //if (!NetworkServer.active) return;
        hp -= amount;
        if (hp <= 0) {
            if (attacker != null && attacker.GetComponent<AudioGenerator>() != null) attacker.GetComponent<AudioGenerator>().PlaySoundByName("sfx_wood_smash1");
            Destroy(gameObject);
        }
    }
}
