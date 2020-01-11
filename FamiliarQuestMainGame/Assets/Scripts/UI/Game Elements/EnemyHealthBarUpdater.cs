using UnityEngine;

public class EnemyHealthBarUpdater : MonoBehaviour {

    public Character character;

    // Update is called once per frame
    void Update() {
        if (character != null && character.GetComponent<Health>() != null) {
            var hp = Mathf.Max(character.GetComponent<Health>().hp, 0);
            var maxHp = Mathf.Max(character.GetComponent<Health>().maxHP, 1);
            transform.localScale = new Vector3(0.2f * hp / maxHp, 1, 0.06f);
            transform.localPosition = new Vector3((-0.5f + hp / maxHp / 2.0f) * 2f, 0, -0.08f);
        }
    }
}
