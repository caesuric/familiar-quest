using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class OchreJelly : MonoBehaviour {

    public GameObject prefab;
    Character character = null;
    Monster mob = null;
    public int generation = 0;
    private float splitCooldown = 0.5f;
    private bool initialized = false;



    // Update is called once per frame
    void Update () {
        //if (!NetworkServer.active) return;
        if (!initialized) {
            initialized = true;
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.fire, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.bashing, -50));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.acid, 100));
            GetComponent<Monster>().elementalAffinities.Add(new ElementalAffinity(Element.piercing, 50));
        }
        if (character==null) character = GetComponent<Character>();
        if (mob == null) mob = GetComponent<Monster>();
        if (splitCooldown > 0) splitCooldown -= Time.deltaTime;
        //if (character!=null && character.GetComponent<Health>().hp < character.GetComponent<Health>().maxHP) Split();
	}

    public void Split() {
        if (splitCooldown > 0) return;
        if (character.GetComponent<Health>().hp > 1 && generation < 4) {
            GetComponent<AudioGenerator>().PlaySoundByName("sfx_ochjelly_split");
            var obj = Instantiate(prefab, transform.position, transform.rotation);
            //NetworkServer.Spawn(obj);
            NerfSpawn(obj);
        }
    }

    private void NerfSpawn(GameObject obj) {
        var newCharacter = obj.GetComponent<Character>();
        character.GetComponent<Health>().maxHP = character.GetComponent<Health>().hp = character.GetComponent<Health>().hp / 2;
        newCharacter.GetComponent<Health>().maxHP = newCharacter.GetComponent<Health>().hp = character.GetComponent<Health>().hp;
        newCharacter.GetComponent<SpiritUser>().spirits.Clear();
        obj.GetComponent<RewardGiver>().xpValue = Mathf.Max(mob.GetComponent<RewardGiver>().xpValue / 2, 1);
        mob.GetComponent<RewardGiver>().xpValue = Mathf.Max(mob.GetComponent<RewardGiver>().xpValue / 2, 1);
        obj.GetComponent<OchreJelly>().generation = generation + 1;
        obj.GetComponent<RewardGiver>().generatedMonster = true;
        generation += 1;
    }
}
