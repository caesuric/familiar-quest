using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

class MonsterMortal : MonoBehaviour {
    public GameObject deathSound;
    public bool deathSoundCreated = false;
    public bool died = false;
    public GameObject killer = null;

    public void OnDeath() {
        if (died) return;
        died = true;
        foreach (var player in PlayerCharacter.players) {
            player.GetComponent<ExperienceGainer>().GainXP(GetComponent<RewardGiver>().xpValue / PlayerCharacter.players.Count);
            player.GainGold(GetComponent<RewardGiver>().goldValue / PlayerCharacter.players.Count);
        }
        if (PlayerCharacter.players.Count > 0 && killer!=null) {
            var pc = killer.GetComponent<PlayerCharacter>();
            if (pc!=null) {
                pc.GainSpirits(GetComponent<SpiritUser>().spirits);
                GetComponent<RewardGiver>().DropLoot(pc.GetComponent<Character>());
            }
        }
        GetComponent<AudioGenerator>().CreateDeathSound();
        Destroy(gameObject, 0.1f);
    }

    public void CreateDeathSound() {
        GetComponent<ObjectSpawner>().CmdSpawnWithPosition(deathSound, transform.position, transform.rotation);
    }
}
