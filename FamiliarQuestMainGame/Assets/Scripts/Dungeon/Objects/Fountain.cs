using UnityEngine;

public class Fountain : Hideable {

    private bool fountainUsed = false;
    // Use this for initialization
    void Start() {
        items.Add(this);
    }

    public void Use() {
        //if (!NetworkServer.active) return;
        if (fountainUsed) return;
        var otherPlayerCharacter = GetComponent<UsableObject>().user;
        if (otherPlayerCharacter != null) {
            fountainUsed = true;
            int roll = Random.Range(0, 2);
            if (roll == 0) {
                otherPlayerCharacter.GetComponent<StatusEffectHost>().AddStatusEffect("poison", 9f);
                otherPlayerCharacter.GetComponent<ObjectSpawner>().CreateFloatingStatusText("POISONED BY FOUNTAIN!", "Poisoned by fountain!");
            }
            else {
                otherPlayerCharacter.GetComponent<Health>().hp = otherPlayerCharacter.GetComponent<Health>().maxHP;
                otherPlayerCharacter.GetComponent<ObjectSpawner>().CreateFloatingStatusText("HEALED TO FULL BY FOUNTAIN!", "Healed to full by fountain!");
            }
        }
    }
}