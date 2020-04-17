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
    public bool diedToPlayer = true;
    public float fadeOutIntensity = 0f;
    public bool fading = false;
    public Shader shader;
    public Texture texture;
    public Texture gradientTexture;

    void Update() {
        if (!fading) return;
        fadeOutIntensity += Time.deltaTime / 0.1f;
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) {
            foreach (var material in renderer.materials) {
                material.SetFloat("_DissolveCutoff", fadeOutIntensity);
            }
        }
    }

    public void OnDeath() {
        if (died) return;
        died = true;
        fading = true;
        if (diedToPlayer) {
            foreach (var player in PlayerCharacter.players) {
                player.GetComponent<ExperienceGainer>().GainXP(GetComponent<RewardGiver>().xpValue / PlayerCharacter.players.Count);
                player.GainGold(GetComponent<RewardGiver>().goldValue / PlayerCharacter.players.Count);
            }
            if (PlayerCharacter.players.Count > 0 && killer != null) {
                var pc = killer.GetComponent<PlayerCharacter>();
                if (pc != null) {
                    pc.GainSpirits(GetComponent<SpiritUser>().spirits);
                    GetComponent<RewardGiver>().DropLoot(pc.GetComponent<Character>());
                }
            }
        }
        GetComponent<AudioGenerator>().CreateDeathSound();
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers) {
            foreach (var material in renderer.materials) {
                material.shader = shader;
                material.SetFloat("_DissolveCutoff", 0);
                material.SetFloat("_DissolveAlphaSource", 1);
                material.SetTexture("_DissolveMap1", texture);
                material.SetColor("_DissolveEdgeColor", new Color(1, 0.7f, 0, 1));
                material.SetFloat("_DissolveEdgeWidth", 0.125f);
                material.SetFloat("_DissolveEdgeShape", 1);
                material.SetFloat("_DissolveEdgeColorIntensity", 1);
                material.SetFloat("_DissolveEdgeTextureSource", 1);
                material.SetTexture("_DissolveEdgeTexture", gradientTexture);
                material.SetFloat("_SmoothnessTextureChannel", 1);
            }
        }
        Destroy(gameObject, 0.1f);
    }

    public void CreateDeathSound() {
        GetComponent<ObjectSpawner>().CmdSpawnWithPosition(deathSound, transform.position, transform.rotation);
    }
}
